using AutoMapper;
using MoreLinq;
using DocGen.Api.Core.Templates;
using DocGen.Shared.Framework;
using DocGen.Shared.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocGen.Shared.Core.Dynamic;
using Microsoft.CSharp.RuntimeBinder;

namespace DocGen.Api.Core.Documents
{
    public class DocumentService
    {
        private readonly IMapper _mapper;
        private readonly IRemoteIpAddressAccessor _remoteIpAddressAccessor;
        private readonly IDocumentRepository _documentRepository;
        private readonly ITemplateRepository _templateRepository;
        private readonly IEnumerable<IDocumentGenerator> _documentGenerators;

        public DocumentService(
            IMapper mapper,
            IRemoteIpAddressAccessor remoteIpAddressAccessor,
            //IDocumentRepository documentRepository,
            ITemplateRepository templateRepository,
            IEnumerable<IDocumentGenerator> documentGenerators)
        {
            _mapper = mapper;
            _remoteIpAddressAccessor = remoteIpAddressAccessor;
            //_documentRepository = documentRepository;
            _templateRepository = templateRepository;
            _documentGenerators = documentGenerators;
        }

        public async Task<string> CreateDocumentAsync(DocumentCreate create, DocumentGenerationMode generationMode)
        {
            Validator.ValidateNotNull(create, nameof(create));
            Validator.Validate(create);

            Template template = null;
            try
            {
                template = await _templateRepository.GetTemplateAsync(create.TemplateId);
            }
            catch (EntityNotFoundException ex)
            {
                ThrowEntityNotFoundAsClientModelValidation(ex, nameof(create.TemplateId));
            }

            ValidateDocumentAgainstTemplate(create, template);

            return await _documentGenerators.Single(d => d.GenerationMode == generationMode).GenerateAsync(template);
        }

        private void ThrowEntityNotFoundAsClientModelValidation(EntityNotFoundException ex, string member)
        {
            throw new ClientModelValidationException($"{ex.Entity} with ID {ex.EntityId} was not found", member);
        }

        private void ValidateDocumentAgainstTemplate(DocumentCreate create, Template template)
        {
            var inputValueErrors = new ModelErrorDictionary();

            var validInputValues = new Dictionary<string, dynamic>();

            template.Steps.ForEach(templateStep =>
            {
                if (templateStep.ConditionType == TemplateComponentConditionType.EqualsPreviousInputValue)
                {
                    var expectedPreviousInputValue = DynamicUtility.Unwrap<string>(() => templateStep.ConditionTypeData.PreviousInputValue);
                    var previousInputReference = DynamicUtility.Unwrap<string>(() => templateStep.ConditionTypeData.PreviousInputReference);

                    if (string.IsNullOrEmpty(expectedPreviousInputValue) || string.IsNullOrEmpty(previousInputReference))
                    {
                        throw new Exception("Internal error: invalid template");
                    }

                    if (!validInputValues.ContainsKey(previousInputReference) || !expectedPreviousInputValue.Equals(validInputValues[previousInputReference]))
                    {
                        // Skip this step
                        return;
                    }
                }

                var templateStepId = templateStep.Id;

                var defaultTemplateStepInput = templateStep.Inputs.SingleOrDefault(i => string.IsNullOrEmpty(i.Id));
                if (defaultTemplateStepInput != null)
                {
                    var result = ValidateTemplateStepInput(templateStep, defaultTemplateStepInput, create.InputValues, inputValueErrors, isDefaultInput: true);
                    if (result.success)
                    {
                        validInputValues.Add(result.templateStepInputId, result.inputValueDynamic);
                    }
                }

                templateStep.Inputs
                    .Where(templateStepInput => !string.IsNullOrEmpty(templateStepInput.Id))
                    .ForEach(templateStepInput =>
                    {
                        var result = ValidateTemplateStepInput(templateStep, templateStepInput, create.InputValues, inputValueErrors, isDefaultInput: false);
                        if (result.success)
                        {
                            validInputValues.Add(result.templateStepInputId, result.inputValueDynamic);
                        }
                    });
            });

            inputValueErrors.AssertValid();
        }

        private (bool success, string templateStepInputId, dynamic inputValueDynamic) ValidateTemplateStepInput(
            TemplateStep templateStep,
            TemplateStepInput templateStepInput,
            IDictionary<string, dynamic> inputValues,
            ModelErrorDictionary errors,
            bool isDefaultInput)
        {
            var templateStepInputId = templateStep.Id;
            if (!isDefaultInput)
            {
                templateStepInputId += Templates.Constants.TemplateComponentReferenceSeparator + templateStepInput.Id;
            }

            if (inputValues.TryGetValue(templateStepInputId, out dynamic inputValueDynamic)) // Id is a reference to a step
            {
                if (templateStepInput.Type == TemplateStepInputType.Text)
                {
                    try
                    {
                        var x = (string)inputValueDynamic;
                    }
                    catch (RuntimeBinderException)
                    {
                        AddInputValueError(errors, "Expected string", templateStepInputId);
                    }
                    return (true, templateStepInputId, inputValueDynamic);
                }
                else if (templateStepInput.Type == TemplateStepInputType.Radio)
                {
                    try
                    {
                        var radioValue = (string)inputValueDynamic;
                        try
                        {
                            var radioOptions = (IEnumerable<dynamic>)(templateStepInput.TypeData);
                            if (!radioOptions.Any(r => r.Value == radioValue))
                            {
                                AddInputValueError(errors, $"Unexpected value; must be from the set of values {string.Join(", ", radioOptions.Select(r => $"\"{r.Value}\""))}", templateStepInputId);
                            }
                            return (true, templateStepInputId, inputValueDynamic);
                        }
                        catch (RuntimeBinderException ex)
                        {
                            throw new Exception("Internal error: invalid template", ex);
                        }
                    }
                    catch (RuntimeBinderException)
                    {
                        AddInputValueError(errors, "Expected string", templateStepInputId);
                    }
                }
            }
            else
            {
                if (isDefaultInput)
                {
                    AddInputValueError(errors, $"Key invalid insufficient as template step {templateStep.Name} did not have a default input", templateStepInputId);
                }
                else
                {
                    AddInputValueError(errors, "Input is required", templateStepInputId);
                }
            }
            return (false, templateStepInputId, null);
        }

        private void AddInputValueError(ModelErrorDictionary errors, string error, string templateStepInputId)
        {
            errors.Add(error, $"{nameof(DocumentCreate.InputValues)}[\"{templateStepInputId}\"]");
        }
    }
}

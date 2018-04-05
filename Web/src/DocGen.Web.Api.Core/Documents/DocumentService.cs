using AutoMapper;
using MoreLinq;
using DocGen.Web.Api.Core.Templates;
using DocGen.Shared.Framework;
using DocGen.Shared.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocGen.Shared.Core.Dynamic;
using Microsoft.CSharp.RuntimeBinder;
using DocGen.Templating.Rendering;

namespace DocGen.Web.Api.Core.Documents
{
    public class DocumentService
    {
        private readonly ITemplateRepository _templateRepository;
        private readonly IDocumentRenderer _documentRenderer;
        private readonly IDocumentExportsFactory _documentExportsFactory;

        public DocumentService(
            ITemplateRepository templateRepository,
            IDocumentRenderer documentRenderer,
            IDocumentExportsFactory documentExportsFactory)
        {
            _templateRepository = templateRepository;
            _documentRenderer = documentRenderer;
            _documentExportsFactory = documentExportsFactory;
        }

        public Task<SerializableDocument> CreateSerializableDocumentAsync(DocumentCreate create)
            => CreateDocumentAsync<SerializableDocument>(create);

        public Task<TextDocument> CreateTextDocumentAsync(DocumentCreate create)
            => CreateDocumentAsync<TextDocument>(create);

        public Task<HtmlDocument> CreateHtmlDocumentAsync(DocumentCreate create)
            => CreateDocumentAsync<HtmlDocument>(create);


        #region Helpers

        private async Task<TDocument> CreateDocumentAsync<TDocument>(DocumentCreate create)
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

            return await _documentRenderer.RenderAsync<TDocument>(
                template.Markup,
                template.MarkupVersion,
                new DocumentRenderModel()
                {
                    Items = create.InputValues.Select(kvp => new DocumentRenderModelItem()
                    {
                        Reference = kvp.Key,
                        Value = ((object)kvp.Value).ToString()
                    }),
                    Sign = template.IsSignable ? create.GetIsSigned() : false,
                    Exports = _documentExportsFactory.Create(template, create.InputValues)
                });
        }

        private void ThrowEntityNotFoundAsClientModelValidation(EntityNotFoundException ex, string member)
        {
            throw new ClientModelValidationException($"{ex.Entity} with ID {ex.EntityId} was not found", member);
        }

        private void ValidateDocumentAgainstTemplate(DocumentCreate create, Template template)
        {
            var inputValueErrors = new ModelErrorDictionary();

            // Store the traversed values so we can easily analyse conditional inputs. It's fine to just store them as strings as we
            // have already validated their types and value comparison will work for stringified booleans and numbers.
            var validInputStringValues = new Dictionary<string, string>();

            template.Steps.ForEach(templateStep =>
            {
                var allConditionsMet = templateStep.Conditions.All(condition =>
                {
                    if (condition.Type == TemplateComponentConditionType.EqualsPreviousInputValue)
                    {
                        var expectedPreviousInputValue = DynamicUtility.Unwrap<string>(() => condition.TypeData.PreviousInputValue);
                        var previousInputId = DynamicUtility.Unwrap<string>(() => condition.TypeData.PreviousInputId);

                        if (string.IsNullOrEmpty(expectedPreviousInputValue) || string.IsNullOrEmpty(previousInputId))
                        {
                            throw new Exception("Internal error: invalid template");
                        }

                        return validInputStringValues.ContainsKey(previousInputId) && expectedPreviousInputValue.Equals(validInputStringValues[previousInputId]);
                    }
                    else if (condition.Type == TemplateComponentConditionType.IsDocumentSigned)
                    {
                        // Skip this step if the contract won't be signed
                        return create.GetIsSigned();
                    }
                    return false;
                });

                if (!allConditionsMet)
                {
                    return;
                }

                var templateStepId = templateStep.Id;

                var defaultTemplateStepInput = templateStep.Inputs.SingleOrDefault(i => string.IsNullOrEmpty(i.Key));
                if (defaultTemplateStepInput != null)
                {
                    var result = ValidateTemplateStepInput(templateStep, defaultTemplateStepInput, create.InputValues, inputValueErrors, isDefaultInput: true);
                    if (result.success)
                    {
                        validInputStringValues.Add(result.templateStepInputId, result.inputValueString);
                    }
                }

                templateStep.Inputs
                    .Where(templateStepInput => !string.IsNullOrEmpty(templateStepInput.Key))
                    .ForEach(templateStepInput =>
                    {
                        var result = ValidateTemplateStepInput(templateStep, templateStepInput, create.InputValues, inputValueErrors, isDefaultInput: false);
                        if (result.success)
                        {
                            validInputStringValues.Add(result.templateStepInputId, result.inputValueString);
                        }
                    });
            });

            inputValueErrors.AssertValid();
        }

        private (bool success, string templateStepInputId, string inputValueString) ValidateTemplateStepInput(
            TemplateStep templateStep,
            TemplateStepInput templateStepInput,
            IDictionary<string, dynamic> inputValues,
            ModelErrorDictionary errors,
            bool isDefaultInput)
        {
            var templateStepInputId = templateStep.Id;
            if (!isDefaultInput)
            {
                templateStepInputId += Templates.Constants.TemplateComponentReferenceSeparator + templateStepInput.Key;
            }

            if (inputValues.TryGetValue(templateStepInputId, out dynamic inputValueDynamic)) // Id is a reference to a step
            {
                if (templateStepInput.Type == TemplateStepInputType.Text)
                {
                    try
                    {
                        var textValue = (string)inputValueDynamic;
                        return (true, templateStepInputId, textValue);
                    }
                    catch (RuntimeBinderException)
                    {
                        AddInputValueError(errors, $"Expected string for \"{templateStepInput.Name ?? templateStep.Name}\"", templateStepInputId);
                    }
                }
                else if (templateStepInput.Type == TemplateStepInputType.Radio)
                {
                    try
                    {
                        var radioValue = (string)inputValueDynamic;
                        try
                        {
                            var radioOptions = (IEnumerable<dynamic>)(templateStepInput.TypeData);
                            if (radioOptions.Any(r => r.Value == radioValue))
                            {
                                return (true, templateStepInputId, inputValueDynamic);
                            }
                            else
                            {
                                AddInputValueError(errors, $"Unexpected value; must be from the set of values {string.Join(", ", radioOptions.Select(r => $"\"{r.Value}\""))}", templateStepInputId);
                            }
                        }
                        catch (RuntimeBinderException ex)
                        {
                            throw new Exception("Internal error: invalid template", ex);
                        }
                    }
                    catch (RuntimeBinderException)
                    {
                        AddInputValueError(errors, $"Expected string for \"{templateStepInput.Name ?? templateStep.Name}\"", templateStepInputId);
                    }
                }
                else if (templateStepInput.Type == TemplateStepInputType.Checkbox)
                {
                    try
                    {
                        var checkboxValue = (bool)(bool.Parse(inputValueDynamic));
                        return (true, templateStepInputId, checkboxValue.ToString());
                    }
                    catch (RuntimeBinderException)
                    {
                        AddInputValueError(errors, $"Expected boolean for \"{templateStepInput.Name ?? templateStep.Name}\"", templateStepInputId);
                    }
                }
            }
            else
            {
                AddInputValueError(errors, $"Input \"{templateStepInput.Name ?? templateStep.Name}\" is required", templateStepInputId);
            }
            return (false, templateStepInputId, null);
        }

        private void AddInputValueError(ModelErrorDictionary errors, string error, string templateStepInputId)
        {
            errors.Add(error, $"{nameof(DocumentCreate.InputValues)}[\"{templateStepInputId}\"]");
        }

        #endregion
    }
}

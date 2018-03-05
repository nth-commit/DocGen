using AutoMapper;
using MoreLinq;
using DocGen.Shared.Core.Dynamic;
using DocGen.Shared.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp.RuntimeBinder;
using DocGen.Shared.Framework;
using DocGen.Templating.Validation;

namespace DocGen.Api.Core.Templates
{
    public class TemplateService
    {
        private readonly IMapper _mapper;
        private readonly ITemplateRepository _templateRepository;
        private readonly ITemplateValidator _templateMarkupValidator;

        public TemplateService(
            IMapper mapper,
            ITemplateRepository templateRepository,
            ITemplateValidator templateMarkupValidator)
        {
            _mapper = mapper;
            _templateRepository = templateRepository;
            _templateMarkupValidator = templateMarkupValidator;
        }

        public async Task<Template> CreateTemplateAsync(TemplateCreate create, bool dryRun = false)
        {
            Validator.ValidateNotNull(create, nameof(create));
            Validator.Validate(create);
            ValidateTemplateSteps(create.Steps);

            var template = _mapper.Map<Template>(create);
            await ValidateTemplateHasUniqueIdAsync(template);

            ValidateTemplateMarkup(template);

            if (dryRun)
            {
                return template;
            }
            else
            {
                return await _templateRepository.CreateTemplateAsync(template);
            }
        }

        private async Task ValidateTemplateHasUniqueIdAsync(Template template)
        {
            try
            {
                await _templateRepository.GetTemplateAsync(template.Id);
                throw new ClientModelValidationException("A template with that name already exists", nameof(TemplateCreate.Name));
            }
            catch (EntityNotFoundException) { }
        }

        private void ValidateTemplateSteps(IEnumerable<TemplateStepCreate> steps)
        {
            // TODO: Validate allowed characters in name

            steps
                .Where(step => step.ParentReference?.Split(Constants.TemplateComponentReferenceSeparator).Count() > 1)
                .ForEach((step) =>
                {
                    throw new NotImplementedException("Only one level of step nesting is supported");
                });

            var stepsByIdLookup = steps.ToIndexedLookup(s => s.Id);

            stepsByIdLookup
                .Where(g => g.Count() > 1)
                .Select(g => g.Skip(1).First())
                .ForEach(indexedStep =>
                {
                    throw new ClientModelValidationException(
                        $"A step with the name {indexedStep.Element.Name} already exists (case-insensitive)",
                        $"{nameof(TemplateCreate.Steps)}[{indexedStep.Index}]"); // TODO: Make exception take params of object and resolve member name
                });

            var stepErrors = new ModelErrorDictionary();
            var stepsById = stepsByIdLookup.ToDictionary(g => g.Key, g => g.Single());
            stepsById.ForEach(kvp => ValidateTemplateStep(kvp.Key, stepsById, stepErrors));

            if (stepErrors.HasErrors)
            {
                throw new ClientModelValidationException(stepErrors);
            }
        }

        private void ValidateTemplateStep(string stepId, Dictionary<string, IndexedElement<TemplateStepCreate>> stepsById, ModelErrorDictionary stepErrors)
        {
            var indexedStep = stepsById[stepId];
            var step = indexedStep.Element;
            var stepIndex = indexedStep.Index;

            var stepErrorPath = new object[] { nameof(TemplateCreate.Steps), stepIndex };

            var isParentStep = stepsById.Any(kvp => kvp.Key.StartsWith(stepId) && kvp.Value.Index != stepIndex);
            if (isParentStep && step.Inputs.Count() > 1)
            {
                // Parent step is allowed to have one input, this is useful for branching to child steps.
                stepErrors.Add("A step that has sub-steps must not contain any inputs", stepErrorPath);
            }
            else if (!isParentStep && !step.Inputs.Any())
            {
                // Steps that do not have child steps must have inputs.
                // FUTURE: Maybe we want to add "informational" steps, with no inputs.
                stepErrors.Add("Step must have at least one inputs", stepErrorPath);
            }

            if (step.ConditionType == TemplateComponentConditionType.EqualsPreviousInputValue)
            {
                var stepConditionErrorPath = stepErrorPath.Concat(nameof(TemplateStepCreate.ConditionTypeData));
                var previousInputIdErrorPath = stepConditionErrorPath.Concat(nameof(TemplateStepConditionTypeData_EqualsPreviousInputValue.PreviousInputId));
                var previousInputValueErrorPath = stepConditionErrorPath.Concat(nameof(TemplateStepConditionTypeData_EqualsPreviousInputValue.PreviousInputValue));

                var previousInputId = DynamicUtility.Unwrap<string>(() => step.ConditionTypeData.PreviousInputId);
                if (string.IsNullOrEmpty(previousInputId))
                {
                    stepErrors.Add("Must have a greater than than or equal to 1", previousInputIdErrorPath);
                }
                else
                {
                    if (TryGetStepFromInputReference(previousInputId, stepsById, out IndexedElement<TemplateStepCreate> indexedPreviousStep))
                    {
                        if (indexedPreviousStep.Index >= stepIndex)
                        {
                            stepErrors.Add("Must reference a previous step", previousInputIdErrorPath);
                        }
                        else
                        {
                            var previousInput = GetTemplateStepInput(previousInputId, indexedPreviousStep.Element);
                            if (previousInput == null)
                            {
                                stepErrors.Add("Could not find input from given path", previousInputIdErrorPath);
                            }
                            else
                            {
                                if (previousInput.Type == TemplateStepInputType.Checkbox)
                                {
                                    try
                                    {
                                        DynamicUtility.UnwrapValue<bool>(() => step.ConditionTypeData.PreviousInputValue);
                                    }
                                    catch (RuntimeBinderException)
                                    {
                                        stepErrors.Add("Expected boolean", previousInputValueErrorPath);
                                    }
                                }
                                else if (previousInput.Type == TemplateStepInputType.Radio)
                                {
                                    try
                                    {
                                        DynamicUtility.Unwrap<string>(() => step.ConditionTypeData.PreviousInputValue);
                                    }
                                    catch (RuntimeBinderException)
                                    {
                                        throw;
                                    }
                                }
                                else
                                {
                                    stepErrors.Add("Type of previous input is not supported for conditions", previousInputValueErrorPath);
                                }
                            }
                        }
                    }
                    else
                    {
                        stepErrors.Add("Could not find a step from given path", previousInputIdErrorPath);
                    }
                }
            }

            ValidateTemplateStepInputs(step, stepErrors, stepErrorPath);
        }

        private void ValidateTemplateStepInputs(TemplateStepCreate step, ModelErrorDictionary stepErrors, IEnumerable<object> stepErrorPath)
        {
            var stepInputsErrorPath = stepErrorPath.Concat(nameof(TemplateStepCreate.Inputs));

            step.Inputs
                .ToIndexedLookup(i => i.Key)
                .Where(g => g.Count() > 1)
                .SelectMany(g => g.AsEnumerable())
                .ForEach(indexedStepInput =>
                {
                    stepErrors.Add(
                        $"An input with the id {indexedStepInput.Element.Key} already exists in this step (case-insensitive)",
                        stepInputsErrorPath.Concat(new object[] { indexedStepInput.Index }));
                });

            var isOnlyInput = step.Inputs.Count() == 1;
            step.Inputs.ForEach((stepInput, stepInputIndex) => ValidateTemplateStepInput(
                stepInput,
                stepErrors,
                stepInputsErrorPath.Concat(stepInputIndex),
                isOnlyInput));
        }

        private void ValidateTemplateStepInput(TemplateStepInputCreate stepInput, ModelErrorDictionary stepErrors, IEnumerable<object> stepInputErrorPath, bool isOnlyInput)
        {
            if (isOnlyInput)
            {
                if (!string.IsNullOrEmpty(stepInput.Name))
                {
                    stepErrors.Add("Must be empty if there is only one input for the step", stepInputErrorPath.Concat(nameof(TemplateStepInputCreate.Name)));
                }

                if (!string.IsNullOrEmpty(stepInput.Description))
                {
                    stepErrors.Add("Must be empty if there is only one input for the step", stepInputErrorPath.Concat(nameof(TemplateStepInputCreate.Description)));
                }
            }
            else
            {
                if (string.IsNullOrEmpty(stepInput.Key))
                {
                    stepErrors.Add("Required if there is more than one input for the step", stepInputErrorPath.Concat(nameof(TemplateStepInputCreate.Key)));
                }

                if (string.IsNullOrEmpty(stepInput.Name))
                {
                    stepErrors.Add("Required if there is more than one input for the step", stepInputErrorPath.Concat(nameof(TemplateStepInputCreate.Name)));
                }

                if (string.IsNullOrEmpty(stepInput.Description))
                {
                    stepErrors.Add("Required if there is more than one input for the step", stepInputErrorPath.Concat(nameof(TemplateStepInputCreate.Description)));
                }
            }
        }

        private bool TryGetStepFromInputReference(string reference, Dictionary<string, IndexedElement<TemplateStepCreate>> stepsById, out IndexedElement<TemplateStepCreate> step)
        {
            if (stepsById.TryGetValue(reference, out step))
            {
                // The input reference is to the step (the input itself is the default input for the step)
                return true;
            }

            var referencePath = reference.Split(Constants.TemplateComponentReferenceSeparator);
            var stepReference = string.Join(Constants.TemplateComponentReferenceSeparator.ToString(), referencePath.Take(referencePath.Count() - 1));
            return stepsById.TryGetValue(stepReference, out step);
        }

        private TemplateStepInputCreate GetTemplateStepInput(string inputReference, TemplateStepCreate step)
        {
            if (string.IsNullOrEmpty(inputReference)) { throw new ArgumentException(nameof(inputReference)); }

            var inputReferencePath = inputReference.Split(Constants.TemplateComponentReferenceSeparator);

            var stepParentReferencePath = step.ParentReference?.Split(Constants.TemplateComponentReferenceSeparator) ?? Enumerable.Empty<string>();
            if (!stepParentReferencePath.SequenceEqual(inputReferencePath.Take(stepParentReferencePath.Count())))
            {
                throw new Exception("Step's parent reference did not match input's reference");
            }

            var inputReferenceFromParentStep = inputReferencePath.Skip(stepParentReferencePath.Count());

            var stepId = inputReferenceFromParentStep.First();
            if (stepId != step.Id)
            {
                throw new Exception("Step id did not match id resolved from input reference");
            }

            if (inputReferenceFromParentStep.Count() == 1) // Reference to step
            {
                if (step.Inputs.Count() == 1)
                {
                    var input = step.Inputs.First();
                    if (string.IsNullOrEmpty(input.Key))
                    {
                        return input;
                    }
                    else
                    {
                        throw new Exception("Could not find default input from step reference (expected input's id to be empty)");
                    }
                }
                else
                {
                    throw new Exception("Input reference did not contain an input id, expected one step");
                }
            }
            else if (inputReferenceFromParentStep.Count() == 2) // Reference to input
            {
                var inputId = inputReferenceFromParentStep.Skip(1).Single();
                return step.Inputs.SingleOrDefault(i => i.Key == inputId);
            }
            else
            {
                throw new ArgumentException(nameof(inputReference));
            }
        }

        private void ValidateTemplateMarkup(Template template)
        {
            var references = template.Steps.SelectMany(s => s.Inputs.Select(i =>
            {
                var inputId = s.Id;
                if (!string.IsNullOrEmpty(i.Key))
                {
                    inputId += $".{i.Key}";
                }

                if (i.Type == TemplateStepInputType.Text)
                {
                    return ReferenceDefinition.String(inputId);
                }
                else if (i.Type == TemplateStepInputType.Checkbox)
                {
                    return ReferenceDefinition.Boolean(inputId);
                }
                else if (i.Type == TemplateStepInputType.Radio)
                {
                    var values = DynamicUtility.Unwrap(() => (IEnumerable<dynamic>)i.TypeData)
                        .Select(d => DynamicUtility.Unwrap<string>(() => d.Value));
                    return ReferenceDefinition.StringFrom(inputId, values);
                }
                else
                {
                    throw new Exception($"Unknown {nameof(TemplateStepInputType)}");
                }
            }));

            _templateMarkupValidator.Validate(template.Markup, template.MarkupVersion, references);
        }
    }
}

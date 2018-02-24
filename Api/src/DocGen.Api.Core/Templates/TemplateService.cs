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

namespace DocGen.Api.Core.Templates
{
    public class TemplateService
    {
        private readonly ITemplateRepository _templateRepository;
        private readonly IMapper _mapper;

        public TemplateService(
            ITemplateRepository templateRepository,
            IMapper mapper)
        {
            _templateRepository = templateRepository;
            _mapper = mapper;
        }

        public async Task<Template> CreateTemplateAsync(TemplateCreate create, bool dryRun = false)
        {
            Validator.ValidateNotNull(create, nameof(create));
            Validator.Validate(create);
            ValidateTemplateSteps(create.Steps);

            // TOOO: Consider changing the internal model for template, to make it easier to validate document info/generate docs?
            var template = _mapper.Map<Template>(create);

            return await _templateRepository.CreateTemplateAsync(template);
        }

        private void ValidateTemplateSteps(IEnumerable<TemplateStep> steps)
        {
            // TODO: Validate allowed characters in name

            steps
                .Where(step => step.ParentNames.Count() > 1)
                .ForEach((step) =>
                {
                    throw new NotImplementedException("Only one level of step nesting is supported");
                });

            var stepsByIdLookup = steps.ToIndexedLookup(s => ResolveId(s));

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

        private void ValidateTemplateStep(string stepId, Dictionary<string, IndexedElement<TemplateStep>> stepsById, ModelErrorDictionary stepErrors)
        {
            var indexedStep = stepsById[stepId];
            var step = indexedStep.Element;
            var stepIndex = indexedStep.Index;

            var stepErrorPath = new object[] { nameof(TemplateCreate.Steps), stepIndex };

            var isParentStep = stepsById.Any(kvp => kvp.Key.StartsWith(stepId) && kvp.Value.Index != stepIndex);
            if (isParentStep && step.Inputs.Any())
            {
                stepErrors.Add("A step that has sub-steps must not contain any inputs", stepErrorPath);
            }
            else if (!isParentStep && !step.Inputs.Any())
            {
                stepErrors.Add("Step must have at least one inputs", stepErrorPath);
            }

            if (step.ConditionType == TemplateComponentConditionType.EqualsPreviousInputValue)
            {
                var stepConditionErrorPath = stepErrorPath.Concat(nameof(TemplateStep.ConditionData));
                var previousInputPathErrorPath = stepConditionErrorPath.Concat(nameof(TemplateStepConditionTypeData_EqualsPreviousInputValue.PreviousInputPath));
                var previousInputValueErrorPath = stepConditionErrorPath.Concat(nameof(TemplateStepConditionTypeData_EqualsPreviousInputValue.PreviousInputValue));

                var previousInputPath = DynamicUtility.Unwrap<IEnumerable<string>>(() => step.ConditionData.PreviousInputPath);
                if (previousInputPath.Count() < 2)
                {
                    stepErrors.Add("Must have a greater than than or equal to 2", previousInputPathErrorPath);
                }
                else
                {
                    var previousStepId = ResolveId(previousInputPath.Take(previousInputPath.Count() - 1));
                    if (stepsById.TryGetValue(previousStepId, out IndexedElement<TemplateStep> indexedPreviousStep))
                    {
                        if (indexedPreviousStep.Index >= stepIndex)
                        {
                            stepErrors.Add("Must reference a previous step", previousInputPathErrorPath);
                        }
                        else
                        {
                            var previousInputName = previousInputPath.TakeLast(1).First();
                            TemplateStepInput previousInput = previousInputName == "{{default}}" ?
                                indexedPreviousStep.Element.Inputs.FirstOrDefault() :
                                indexedPreviousStep.Element.Inputs.Where(i => i.Name == previousInputName).FirstOrDefault();

                            if (previousInput == null)
                            {
                                stepErrors.Add("Could not find input from given path", previousInputPathErrorPath);
                            }
                            else
                            {
                                if (previousInput.Type == TemplateStepInputType.Checkbox)
                                {
                                    try
                                    {
                                        DynamicUtility.UnwrapValue<bool>(() => step.ConditionData.PreviousInputValue);
                                    }
                                    catch (RuntimeBinderException)
                                    {
                                        stepErrors.Add("Expected boolean", previousInputValueErrorPath);
                                    }
                                }
                                //else if (previousInput.Type == TemplateStepInputType.Radio)
                                //{
                                //    try
                                //    {
                                //        DynamicUtility.Unwrap<string>(() => step.ConditionData.PreviousInputValue);
                                //    }
                                //    catch (RuntimeBinderException)
                                //    {
                                //        throw;
                                //    }
                                //}
                                else
                                {
                                    stepErrors.Add("Type of previous input is not supported for conditions", previousInputValueErrorPath);
                                }
                            }
                        }
                    }
                    else
                    {
                        stepErrors.Add("Could not find a step from given path", previousInputPathErrorPath);
                    }
                }
            }

            if (!isParentStep)
            {
                ValidateTemplateStepInputs(step, stepErrors, stepErrorPath);
            }
        }

        private void ValidateTemplateStepInputs(TemplateStep step, ModelErrorDictionary stepErrors, IEnumerable<object> stepErrorPath)
        {
            var stepInputsErrorPath = stepErrorPath.Concat(nameof(TemplateStep.Inputs));

            step.Inputs
                .ToIndexedLookup(i => ResolveId(step, i))
                .Where(g => g.Count() > 1)
                .SelectMany(g => g.AsEnumerable())
                .ForEach(indexedStepInput =>
                {
                    stepErrors.Add(
                        $"An input with the name {indexedStepInput.Element.Name} already exists in this step (case-insensitive)",
                        stepInputsErrorPath.Concat(new object[] { indexedStepInput.Index }));
                });

            var isOnlyInput = step.Inputs.Count() == 1;
            step.Inputs.ForEach((stepInput, stepInputIndex) => ValidateTemplateStepInput(
                stepInput,
                stepErrors,
                stepInputsErrorPath.Concat(stepInputIndex),
                isOnlyInput));
        }

        private void ValidateTemplateStepInput(TemplateStepInput stepInput, ModelErrorDictionary stepErrors, IEnumerable<object> stepInputErrorPath, bool isOnlyInput)
        {
            if (isOnlyInput)
            {
                if (!string.IsNullOrEmpty(stepInput.Name))
                {
                    stepErrors.Add("Must be empty if there is only one input for the step", stepInputErrorPath.Concat(nameof(TemplateStepInput.Name)));
                }

                if (!string.IsNullOrEmpty(stepInput.Description))
                {
                    stepErrors.Add("Must be empty if there is only one input for the step", stepInputErrorPath.Concat(nameof(TemplateStepInput.Description)));
                }
            }
            else
            {
                if (string.IsNullOrEmpty(stepInput.Name))
                {
                    stepErrors.Add("Required if there is more than one input for the step", stepInputErrorPath.Concat(nameof(TemplateStepInput.Name)));
                }

                if (string.IsNullOrEmpty(stepInput.Description))
                {
                    stepErrors.Add("Required if there is more than one input for the step", stepInputErrorPath.Concat(nameof(TemplateStepInput.Description)));
                }
            }
        }

        private string ResolveId(TemplateStep step) => ResolveId(step.ParentNames.Concat(step.Name));

        private string ResolveId(TemplateStep step, TemplateStepInput stepInput) => ResolveId(step.ParentNames.Concat(step.Name).Concat(stepInput.Name));

        private string ResolveId(IEnumerable<string> path) => string.Join(".", path.Select(n => n.ToLowerInvariant().Replace(' ', '_')));
    }
}

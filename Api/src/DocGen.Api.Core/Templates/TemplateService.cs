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

            var template = _mapper.Map<Template>(create);

            return await _templateRepository.CreateTemplateAsync(template);
        }

        private void ValidateTemplate(TemplateCreate template)
        {
            Validator.Validate(template);
            ValidateTemplateSteps(template.Steps);

            // Validate order of conditions
            //var stepGroupErrors = new ModelErrorDictionary();
            //var stepGroupsPath = new object[] { nameof(Template.StepGroups) };
            //var stepGroups = template.StepGroups.ToList();

            //stepGroups.ForEach((stepGroup, stepGroupIndex) =>
            //{
            //    var stepsPath = stepGroupsPath.Concat(stepGroupIndex, nameof(StepGroup.Steps));

            //    stepGroup.Steps.ForEach((step, stepIndex) =>
            //    {
            //        var stepPath = stepsPath.Concat(stepIndex);
            //        var stepConditionTypeDataPath = stepPath.Concat(nameof(TemplateStep.ConditionTypeData));

            //        if (step.ConditionType == TemplateComponentConditionType.EqualsPreviousValue)
            //        {
            //            var requestedTargetStepGroupIndex = DynamicUtility.UnwrapNullableValue(() => (int?)step.ConditionTypeData.StepGroupIndex);
            //            if (requestedTargetStepGroupIndex.HasValue && requestedTargetStepGroupIndex > stepGroupIndex)
            //            {
            //                stepGroupErrors.Add("Invalid StepGroupIndex", stepConditionTypeDataPath.Concat(nameof(TemplateStepConditionTypeData_EqualsPreviousValue.StepGroupIndex)));
            //                return;
            //            }
            //            var targetStepGroupIndex = requestedTargetStepGroupIndex ?? stepGroupIndex;

            //            var targetStepIndex = DynamicUtility.UnwrapValue(() => (int)step.ConditionTypeData.StepIndex);
            //            if (targetStepGroupIndex == stepGroupIndex && targetStepIndex >= stepIndex)
            //            {
            //                stepGroupErrors.Add("Invalid StepIndex", stepConditionTypeDataPath.Concat(nameof(TemplateStepConditionTypeData_EqualsPreviousValue.StepIndex)));
            //            }

            //            var targetSteps = stepGroups[targetStepGroupIndex].Steps.ToList();
            //            if (targetStepIndex > targetSteps.Count - 1)
            //            {
            //                stepGroupErrors.Add("Invalid StepIndex - Out of range", stepConditionTypeDataPath.Concat(nameof(TemplateStepConditionTypeData_EqualsPreviousValue.StepIndex)));
            //            }

            //            var targetStep = targetSteps[targetStepIndex];
            //            var expectedStepType = (TemplateStepInputType)step.ConditionTypeData.ExpectedStepType;
            //            if (targetStep.Type != expectedStepType)
            //            {
            //                stepGroupErrors.Add("Previous step was an unexpected type", stepConditionTypeDataPath.Concat(nameof(TemplateStepConditionTypeData_EqualsPreviousValue.ExpectedStepType)));
            //            }

            //            if (expectedStepType == TemplateStepInputType.Checkbox)
            //            {
            //                var previousValue = DynamicUtility.UnwrapNullableValue<bool>(() => step.ConditionTypeData.PreviousValue);
            //                if (!previousValue.HasValue)
            //                {
            //                    stepGroupErrors.Add("Expected boolean", stepConditionTypeDataPath.Concat(nameof(TemplateStepConditionTypeData_EqualsPreviousValue.PreviousInputValue)));
            //                }
            //            }
            //            else if (expectedStepType == TemplateStepInputType.Radio)
            //            {
            //                var previousValue = DynamicUtility.UnwrapReference<string>(() => step.ConditionTypeData.PreviousValue);
            //                if (string.IsNullOrEmpty(previousValue))
            //                {
            //                    stepGroupErrors.Add("Expected string", stepConditionTypeDataPath.Concat(nameof(TemplateStepConditionTypeData_EqualsPreviousValue.PreviousInputValue)));
            //                }
            //            }
            //            else
            //            {
            //                stepGroupErrors.Add("Unexpected value type", stepConditionTypeDataPath.Concat(nameof(TemplateStepConditionTypeData_EqualsPreviousValue.PreviousInputValue)));
            //            }
            //        }
            //    });
            //});

            //stepGroupErrors.AssertValid();
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

            var stepsByIdLookup = steps.ToLookup(
                (step, i) => ResolveId(step),
                (step, i) => new TemplateStep_Index()
                {
                    Step = step,
                    StepIndex = i
                });

            stepsByIdLookup
                .Where(g => g.Count() > 1)
                .Select(g => g.Skip(1).First())
                .ForEach((step_index) =>
                {
                    throw new ClientModelValidationException(
                        $"A step with the name {step_index.Step.Name} already exists (case-insensitive)",
                        $"{nameof(TemplateCreate.Steps)}[{step_index.StepIndex}]"); // TODO: Make exception take params of object and resolve member name
                });

            var stepErrors = new ModelErrorDictionary();
            Dictionary<string, TemplateStep_Index> stepsById = stepsByIdLookup.ToDictionary(g => g.Key, g => g.Single());
            stepsById.ForEach(kvp => ValidateTemplateStep(kvp.Key, stepsById, stepErrors));

            if (stepErrors.HasErrors)
            {
                throw new ClientModelValidationException(stepErrors);
            }
        }

        private void ValidateTemplateStep(string stepId, Dictionary<string, TemplateStep_Index> stepsById, ModelErrorDictionary stepErrors)
        {
            var (step, stepIndex) = stepsById[stepId];
            var stepErrorPath = new object[] { nameof(TemplateCreate.Steps), stepIndex };

            var isParentStep = stepsById.Any(kvp => kvp.Key.StartsWith(stepId) && kvp.Value.StepIndex != stepIndex);
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
                    if (stepsById.TryGetValue(previousStepId, out TemplateStep_Index previousStep))
                    {
                        if (previousStep.StepIndex >= stepIndex)
                        {
                            stepErrors.Add("Must reference a previous step", previousInputPathErrorPath);
                        }
                        else
                        {
                            var previousInputName = previousInputPath.TakeLast(1).First();
                            TemplateStepInput previousInput = previousInputName == "{{default}}" ?
                                previousStep.Step.Inputs.FirstOrDefault() :
                                previousStep.Step.Inputs.Where(i => i.Name == previousInputName).FirstOrDefault();

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

            if (step.Inputs.Count() == 1)
            {
                ValidateTemplateStepInput(step.Inputs.Single(), 0, isOnlyInput: true);
            }
            else
            {
                step.Inputs.ForEach((stepInput, stepInputIndex) => ValidateTemplateStepInput(stepInput, stepInputIndex, isOnlyInput: false));
            }
        }

        private void ValidateTemplateStepInput(TemplateStepInput stepInput, int stepInputIndex, bool isOnlyInput)
        {

        }

        private class TemplateStep_Index
        {
            public TemplateStep Step { get; set; }

            public int StepIndex { get; set; }

            public void Deconstruct(out TemplateStep step, out int stepIndex)
            {
                step = Step;
                stepIndex = StepIndex;
            }
        }

        private string ResolveId(TemplateStep step) => ResolveId(step.ParentNames.Concat(step.Name));

        private string ResolveId(IEnumerable<string> stepPath) => string.Join(".", stepPath.Select(n => n.ToLowerInvariant().Replace(' ', '_')));
    }
}

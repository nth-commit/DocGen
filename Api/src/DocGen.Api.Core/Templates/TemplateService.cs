using AutoMapper;
using MoreLinq;
using DocGen.Shared.Core.Dynamic;
using DocGen.Shared.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            ValidateTemplate(create);

            var template = _mapper.Map<Template>(create);

            return await _templateRepository.CreateTemplateAsync(template);
        }

        private void ValidateTemplate(TemplateCreate template)
        {
            Validator.Validate(template);

            // Validate order of conditions
            var stepGroupErrors = new ModelErrorDictionary();
            var stepGroupsPath = new object[] { nameof(Template.StepGroups) };
            var stepGroups = template.StepGroups.ToList();

            stepGroups.ForEach((stepGroup, stepGroupIndex) =>
            {
                var stepsPath = stepGroupsPath.Concat(stepGroupIndex, nameof(StepGroup.Steps));

                stepGroup.Steps.ForEach((step, stepIndex) =>
                {
                    var stepPath = stepsPath.Concat(stepIndex);
                    var stepConditionTypeDataPath = stepPath.Concat(nameof(Step.ConditionTypeData));

                    if (step.ConditionType == StepConditionType.EqualsPreviousValue)
                    {
                        var requestedTargetStepGroupIndex = DynamicUtility.UnwrapNullableValue(() => (int?)step.ConditionTypeData.StepGroupIndex);
                        if (requestedTargetStepGroupIndex.HasValue && requestedTargetStepGroupIndex > stepGroupIndex)
                        {
                            stepGroupErrors.Add("Invalid StepGroupIndex", stepConditionTypeDataPath.Concat(nameof(StepConditionTypeData_EqualsPreviousValue.StepGroupIndex)));
                            return;
                        }
                        var targetStepGroupIndex = requestedTargetStepGroupIndex ?? stepGroupIndex;

                        var targetStepIndex = DynamicUtility.UnwrapValue(() => (int)step.ConditionTypeData.StepIndex);
                        if (targetStepGroupIndex == stepGroupIndex && targetStepIndex >= stepIndex)
                        {
                            stepGroupErrors.Add("Invalid StepIndex", stepConditionTypeDataPath.Concat(nameof(StepConditionTypeData_EqualsPreviousValue.StepIndex)));
                        }

                        var targetSteps = stepGroups[targetStepGroupIndex].Steps.ToList();
                        if (targetStepIndex > targetSteps.Count - 1)
                        {
                            stepGroupErrors.Add("Invalid StepIndex - Out of range", stepConditionTypeDataPath.Concat(nameof(StepConditionTypeData_EqualsPreviousValue.StepIndex)));
                        }

                        var targetStep = targetSteps[targetStepIndex];
                        var expectedStepType = (StepType)step.ConditionTypeData.ExpectedStepType;
                        if (targetStep.Type != expectedStepType)
                        {
                            stepGroupErrors.Add("Previous step was an unexpected type", stepConditionTypeDataPath.Concat(nameof(StepConditionTypeData_EqualsPreviousValue.ExpectedStepType)));
                        }

                        if (expectedStepType == StepType.Checkbox)
                        {
                            var previousValue = DynamicUtility.UnwrapNullableValue<bool>(() => step.ConditionTypeData.PreviousValue);
                            if (!previousValue.HasValue)
                            {
                                stepGroupErrors.Add("Expected boolean", stepConditionTypeDataPath.Concat(nameof(StepConditionTypeData_EqualsPreviousValue.PreviousValue)));
                            }
                        }
                        else if (expectedStepType == StepType.Radio)
                        {
                            var previousValue = DynamicUtility.UnwrapReference<string>(() => step.ConditionTypeData.PreviousValue);
                            if (string.IsNullOrEmpty(previousValue))
                            {
                                stepGroupErrors.Add("Expected string", stepConditionTypeDataPath.Concat(nameof(StepConditionTypeData_EqualsPreviousValue.PreviousValue)));
                            }
                        }
                        else
                        {
                            stepGroupErrors.Add("Unexpected value type", stepConditionTypeDataPath.Concat(nameof(StepConditionTypeData_EqualsPreviousValue.PreviousValue)));
                        }
                    }
                });
            });

            stepGroupErrors.AssertValid();
        }
    }
}

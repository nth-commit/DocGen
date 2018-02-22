using AutoMapper;
using DocGen.Shared.Core.Dynamic;
using DocGen.Shared.Validation;
using DocGen.Shared.WindowsAzure.Storage;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Api.Core.Templates
{
    public class TemplateService : ITemplateService
    {
        private readonly ICloudStorageClientFactory _cloudStorageClientFactory;
        private readonly IMapper _mapper;

        public TemplateService(
            ICloudStorageClientFactory cloudStorageClientFactory,
            IMapper mapper)
        {
            _cloudStorageClientFactory = cloudStorageClientFactory;
            _mapper = mapper;
        }

        async Task<Template> ITemplateService.CreateTemplateAsync(Template template, bool dryRun)
        {
            Validator.ValidateNotNull(template, nameof(template));
            ValidateTemplate(template);

            var templateRow = _mapper.Map<TemplateTableEntity>(template);

            var tableClient = _cloudStorageClientFactory.CreateTableClient();

            var table = tableClient.GetTableReference("templates");
            await table.CreateIfNotExistsAsync();

            await table.ExecuteAsync(TableOperation.Insert(templateRow));

            return template;
        }

        private void ValidateTemplate(Template template)
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

                    if (step.ConditionType == StepConditionType.EqualsPreviousValue)
                    {
                        var requestedTargetStepGroupIndex = DynamicUtility.UnwrapNullableValue(() => (int?)step.ConditionTypeData.StepGroupIndex);
                        if (requestedTargetStepGroupIndex.HasValue && requestedTargetStepGroupIndex > stepGroupIndex)
                        {
                            stepGroupErrors.Add("Invalid StepGroupIndex", stepPath.Concat(nameof(StepConditionTypeData_EqualsPreviousValue.StepGroupIndex)));
                            return;
                        }
                        var targetStepGroupIndex = requestedTargetStepGroupIndex ?? stepGroupIndex;

                        var targetStepIndex = DynamicUtility.UnwrapValue(() => (int)step.ConditionTypeData.StepIndex);
                        if (targetStepGroupIndex == stepGroupIndex && targetStepIndex >= stepIndex)
                        {
                            stepGroupErrors.Add("Invalid StepIndex", stepPath.Concat(nameof(StepConditionTypeData_EqualsPreviousValue.StepIndex)));
                        }

                        var targetSteps = stepGroups[targetStepGroupIndex].Steps.ToList();
                        if (targetStepIndex > targetSteps.Count - 1)
                        {
                            stepGroupErrors.Add("Invalid StepIndex - Out of range", stepPath.Concat(nameof(StepConditionTypeData_EqualsPreviousValue.StepIndex)));
                        }

                        var targetStep = targetSteps[targetStepIndex];
                        var expectedStepType = (StepType)step.ConditionTypeData.ExpectedStepType;
                        if (targetStep.Type != expectedStepType)
                        {
                            stepGroupErrors.Add("Previous step was an unexpected type", stepPath.Concat(nameof(StepConditionTypeData_EqualsPreviousValue.ExpectedStepType)));
                        }

                        if (expectedStepType == StepType.Checkbox)
                        {
                            var previousValue = DynamicUtility.UnwrapNullableValue<bool>(() => step.ConditionTypeData.PreviousValue);
                            if (!previousValue.HasValue)
                            {
                                stepGroupErrors.Add("Expected boolean", stepPath.Concat(nameof(StepConditionTypeData_EqualsPreviousValue.PreviousValue)));
                            }
                        }
                        else if (expectedStepType == StepType.Radio)
                        {
                            var previousValue = DynamicUtility.UnwrapReference<string>(() => step.ConditionTypeData.PreviousValue);
                            if (string.IsNullOrEmpty(previousValue))
                            {
                                stepGroupErrors.Add("Expected string", stepPath.Concat(nameof(StepConditionTypeData_EqualsPreviousValue.PreviousValue)));
                            }
                        }
                        else
                        {
                            stepGroupErrors.Add("Unexpected value type", stepPath.Concat(nameof(StepConditionTypeData_EqualsPreviousValue.PreviousValue)));
                        }
                    }
                });
            });
        }
    }
}

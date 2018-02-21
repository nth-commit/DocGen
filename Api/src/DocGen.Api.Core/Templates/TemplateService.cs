using AutoMapper;
using DocGen.Shared.Validation;
using DocGen.Shared.WindowsAzure.Storage;
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
            var (isValid, modelErrors) = Validator.IsValid(template);

            // Validate order of conditions
            var stepGroupErrors = new ModelErrorDictionary();
            var stepGroupsPath = new object[] { nameof(Template.StepGroups) };
            template.StepGroups.ForEach((stepGroup, stepGroupIndex) =>
            {
                var stepsPath = stepGroupsPath.Concat(stepGroupIndex, nameof(StepGroup.Steps));
                stepGroup.Steps.ForEach((step, stepIndex) =>
                {
                    var stepPath = stepsPath.Concat(stepIndex);
                    if (step.ConditionType == StepConditionType.PreviousCheckboxValue || step.ConditionType == StepConditionType.PreviousRadioValue)
                    {
                        if (step.ConditionTypeData.StepGroupIndex >= stepGroupIndex)
                        {
                            stepGroupErrors.Add("Invalid StepGroupIndex", stepPath.Concat(nameof(StepConditionTypeData_PreviousValue.StepGroupIndex)));
                        }
                        else if (step.ConditionTypeData.StepIndex >= stepIndex)
                        {
                            stepGroupErrors.Add("Invalid StepIndex", stepPath.Concat(nameof(StepConditionTypeData_PreviousValue.StepIndex)));
                        }

                        // TODO: Read target step and ensure that it matches the condition data
                    }
                });
            });
        }
    }
}

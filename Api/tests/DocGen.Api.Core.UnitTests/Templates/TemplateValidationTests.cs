using DocGen.Shared.Core.Dynamic;
using DocGen.Shared.Validation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DocGen.Api.Core.Templates
{
    public class TemplateValidationTests : TestsBase
    {
        [Fact]
        public async Task TestValidation_StepConditionReferencesFollowingStep_Fails()
        {
            await AssertTemplateInvalidAsync(
                CreateTemplate_OneStepGroup(new List<Step>()
                {
                    new Step()
                    {
                        Title = "A",
                        Description = "B",
                        Type = StepType.Text,
                        TypeData =  ExpandoObjectFactory.CreateDynamic(new Dictionary<string, object>()
                        {
                            { "Value", "C" }
                        }),
                        ConditionType = StepConditionType.EqualsPreviousValue,
                        ConditionTypeData = ExpandoObjectFactory.CreateDynamic(new Dictionary<string, object>()
                        {
                            { "StepGroupIndex", 0 },
                            { "StepIndex", 1 },
                            { "ExpectedStepType", StepType.Checkbox },
                            { "PreviousValue", true }
                        })
                    },
                    new Step()
                    {
                        Title = "A",
                        Description = "B",
                        Type = StepType.Checkbox
                    }
                }),
                "StepGroups[0].Steps[0].ConditionTypeData.StepIndex");
        }


        #region Helpers

        private async Task AssertTemplateInvalidAsync(TemplateCreate create, params string[] invalidMembers)
        {
            var templateService = ServiceProvider.GetRequiredService<TemplateService>();
            try
            {
                await templateService.CreateTemplateAsync(create);
                Assert.True(false, "Template was valid");
            }
            catch (ClientModelValidationException ex)
            {
                var actualInvalidMembers = ex.ModelErrors.Keys;
                var expectedInvalidMembersMissing = invalidMembers.Except(ex.ModelErrors.Keys);
                var expectedValidMembersPresent = ex.ModelErrors.Keys.Except(invalidMembers);

                Assert.True(0 == expectedInvalidMembersMissing.Count(), $"Members were valid, expected to be invalid: {string.Join(',', expectedInvalidMembersMissing)}");
                Assert.True(0 == expectedValidMembersPresent.Count(), $"Members were invalid, expected to be valid: {string.Join(',', expectedInvalidMembersMissing)}");
            }
        }

        private TemplateCreate CreateTemplate_OneStepGroup(IEnumerable<Step> steps)
        {
            return new TemplateCreate()
            {
                Name = "A",
                Text = "B",
                StepGroups = new List<StepGroup>()
                {
                    new StepGroup()
                    {
                        Title = "A",
                        Description = "B",
                        Steps = steps
                    }
                }
            };
        }

        #endregion
    }
}

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
                new TemplateCreate()
                {
                    Name = "A",
                    Text = "A",
                    Steps = new List<TemplateStepCreate>()
                    {
                        new TemplateStepCreate()
                        {
                            Name = "A",
                            Description = "A",
                            ConditionType = TemplateComponentConditionType.EqualsPreviousInputValue,
                            ConditionData = ExpandoObjectFactory.CreateDynamic(new Dictionary<string, object>()
                            {
                                { "PreviousInputPath", new List<string>() { "B", "{{default}}" } },
                                { "PreviousInputValue", true }
                            }),
                            Inputs = new List<TemplateStepInputCreate>()
                            {
                                new TemplateStepInputCreate()
                                {
                                    Type = TemplateStepInputType.Text
                                }
                            }
                            
                        },
                        new TemplateStepCreate()
                        {
                            Name = "B",
                            Description = "B",
                            Inputs = new List<TemplateStepInputCreate>()
                            {
                                new TemplateStepInputCreate()
                                {
                                    Type = TemplateStepInputType.Checkbox
                                }
                            }
                        }
                        
                    }
                },
                "Steps[0].ConditionData.PreviousInputPath");
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

        #endregion
    }
}

using DocGen.Shared.Core.Dynamic;
using DocGen.Shared.Validation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DocGen.Web.Api.Core.Templates
{
    public class TemplateValidationTests : TestsBase
    {
        [Fact]
        public async Task TestValidation_StepConditionReferencesFollowingStep_Fails()
        {
            await AssertTemplateInvalidAsync(
                new List<TemplateStepCreate>()
                {
                    new TemplateStepCreate()
                    {
                        Id = "a",
                        Name = "A",
                        Description = "A",
                        Conditions = new List<TemplateStepCondition>()
                        {
                            new TemplateStepCondition()
                            {
                                Type = TemplateComponentConditionType.EqualsPreviousInputValue,
                                TypeData = ExpandoObjectFactory.CreateDynamic(new Dictionary<string, object>()
                                {
                                    { "PreviousInputId", "b" },
                                    { "PreviousInputValue", true }
                                })
                            }
                        },
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
                        Id = "b",
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
                },
                "Steps[0].Conditions[0].TypeData.PreviousInputId");
        }


        #region Helpers

        private async Task AssertTemplateInvalidAsync(IEnumerable<TemplateStepCreate> steps, params string[] invalidMembers)
        {
            var templateService = ServiceProvider.GetRequiredService<TemplateService>();
            try
            {
                await templateService.CreateTemplateAsync(new TemplateCreate()
                {
                    Name = "x",
                    Description = "x",
                    Markup = "x",
                    MarkupVersion = 1,
                    SigningType = TemplateSigningType.NotSigned,
                    Steps = steps
                });
                Assert.True(false, "Template was valid");
            }
            catch (ClientModelValidationException ex)
            {
                var actualInvalidMembers = ex.ModelErrors.Keys;
                var expectedValidMembersPresent = ex.ModelErrors.Keys.Except(invalidMembers);
                var expectedInvalidMembersMissing = invalidMembers.Except(ex.ModelErrors.Keys);

                Assert.True(0 == expectedValidMembersPresent.Count(), $"Members were invalid, expected to be valid: {string.Join(',', expectedInvalidMembersMissing)}");
                Assert.True(0 == expectedInvalidMembersMissing.Count(), $"Members were valid, expected to be invalid: {string.Join(',', expectedInvalidMembersMissing)}");
            }
        }

        #endregion
    }
}

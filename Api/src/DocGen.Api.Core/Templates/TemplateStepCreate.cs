using DocGen.Shared.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DocGen.Api.Core.Templates
{
    public class TemplateStepCreate : ITemplateComponent
    {
        [StringNotNullOrEmpty]
        [RegularExpression(Constants.TemplateComponentReferenceRegexPattern)]
        public string Id { get; set; }

        [StringNotNullOrEmpty]
        public string Name { get; set; }

        [StringNotNullOrEmpty]
        public string Description { get; set; }

        [ValidateEnumerableElements]
        public IEnumerable<TemplateStepCondition> Conditions { get; set; } = Enumerable.Empty<TemplateStepCondition>();

        [ValidateEnumerableElements]
        public IEnumerable<TemplateStepInputCreate> Inputs { get; set; } = Enumerable.Empty<TemplateStepInputCreate>(); // Can be empty if a parent step.

        [RegularExpression(Constants.TemplateComponentReferenceRegexPattern)]
        public string ParentReference { get; set; }
    }
}

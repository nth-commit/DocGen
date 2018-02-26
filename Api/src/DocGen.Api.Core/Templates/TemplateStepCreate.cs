﻿using DocGen.Shared.Validation;
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
        [RegularExpression(Constants.TemplateComponentIdRegexPattern)]
        public string Id { get; set; }

        [StringNotNullOrEmpty]
        public string Name { get; set; }

        [StringNotNullOrEmpty]
        public string Description { get; set; }

        [NotValue(TemplateComponentConditionType.Unknown)]
        public TemplateComponentConditionType? ConditionType { get; set; }

        [ValidateAgainstTypeIf(typeof(TemplateStepConditionTypeData_EqualsPreviousInputValue), nameof(ConditionType), TemplateComponentConditionType.EqualsPreviousInputValue)]
        public dynamic ConditionTypeData { get; set; }

        public IEnumerable<TemplateStepInputCreate> Inputs { get; set; } = Enumerable.Empty<TemplateStepInputCreate>(); // Can be empty if a parent step.

        [RegularExpression(Constants.TemplateComponentReferenceRegexPattern)]
        public string ParentReference { get; set; }
    }

    public class TemplateStepConditionTypeData { }

    public class TemplateStepConditionTypeData_EqualsPreviousInputValue : TemplateStepConditionTypeData
    {
        [RegularExpression(Constants.TemplateComponentIdRegexPattern)]
        public string PreviousInputReference { get; set; }

        [Required]
        public dynamic PreviousInputValue { get; set; }
    }
}

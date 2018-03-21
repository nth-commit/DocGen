using DocGen.Shared.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DocGen.Web.Api.Core.Templates
{
    public class TemplateStepCondition
    {
        [NotValue(TemplateComponentConditionType.Unknown)]
        public TemplateComponentConditionType? Type { get; set; }

        [ValidateAgainstTypeIf(typeof(TemplateStepConditionTypeData_EqualsPreviousInputValue), nameof(Type), TemplateComponentConditionType.EqualsPreviousInputValue)]
        [NullIf(nameof(Type), TemplateComponentConditionType.IsDocumentSigned)]
        public dynamic TypeData { get; set; }
    }

    public class TemplateStepConditionTypeData { }

    public class TemplateStepConditionTypeData_EqualsPreviousInputValue : TemplateStepConditionTypeData
    {
        [RegularExpression(Constants.TemplateStepInputKeyRegexPattern)]
        public string PreviousInputId { get; set; }

        [Required]
        public dynamic PreviousInputValue { get; set; }
    }
}

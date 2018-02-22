using DocGen.Shared.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DocGen.Api.Core.Templates
{
    public class Step
    {
        [StringNotNullOrEmpty]
        public string Title { get; set; }

        [StringNotNullOrEmpty]
        public string Description { get; set; }

        [NotValue(StepType.Unknown)]
        public StepType Type { get; set; }

        [ValidateAgainstTypeIf(typeof(StepTypeData_Text), nameof(Type), StepType.Text)]
        [NullIf(nameof(Type), StepType.Checkbox)]
        public dynamic TypeData { get; set; }

        [NotValue(StepConditionType.Unknown)]
        public StepConditionType? ConditionType { get; set; }

        [ValidateAgainstTypeIf(typeof(StepConditionTypeData_EqualsPreviousValue), nameof(ConditionType), StepConditionType.EqualsPreviousValue)]
        public dynamic ConditionTypeData { get; set; }
    }

    public class StepTypeData { }

    public class StepTypeData_Text : StepTypeData
    {
        [StringNotNullOrEmpty]
        public string Value { get; set; }
    }

    public class StepConditionTypeData { }

    public class StepConditionTypeData_EqualsPreviousValue : StepConditionTypeData
    {
        [Range(0, int.MaxValue)]
        public int? StepGroupIndex { get; set; }

        [Range(0, int.MaxValue)]
        public int StepIndex { get; set; }

        [NotValue(StepType.Unknown)]
        public StepType ExpectedStepType { get; set; }

        [Required]
        public dynamic PreviousValue { get; set; }
    }
}

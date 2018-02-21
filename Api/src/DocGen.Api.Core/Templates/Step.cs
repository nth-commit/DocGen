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

        [ValidateAgainstTypeIf(typeof(StepConditionTypeData_PreviousCheckboxValue), nameof(ConditionType), StepConditionType.PreviousCheckboxValue)]
        [ValidateAgainstTypeIf(typeof(StepConditionTypeData_PreviousRadioValue), nameof(ConditionType), StepConditionType.PreviousRadioValue)]
        public dynamic ConditionTypeData { get; set; }
    }

    public class StepTypeData { }

    public class StepTypeData_Text : StepTypeData
    {
        [StringNotNullOrEmpty]
        public string Value { get; set; }
    }

    public class StepConditionTypeData { }

    public class StepConditionTypeData_PreviousValue : StepConditionTypeData
    {
        [Range(0, int.MaxValue)]
        public int? StepGroupIndex { get; set; }

        [Range(0, int.MaxValue)]
        public int StepIndex { get; set; }
    }

    public class StepConditionTypeData_PreviousCheckboxValue : StepConditionTypeData_PreviousValue
    {
        [Required]
        public bool? Value { get; set; }
    }

    public class StepConditionTypeData_PreviousRadioValue : StepConditionTypeData_PreviousValue
    {
        [StringNotNullOrEmpty]
        public string Value { get; set; }
    }
}

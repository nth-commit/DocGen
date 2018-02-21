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

        [Required]
        [ValidateAgainstTypeIf(typeof(StepTypeTextData), nameof(Type), StepType.Text)]
        public dynamic TypeData { get; set; }

        [NotValue(StepConditionType.Unknown)]
        public StepConditionType? ConditionType { get; set; }

        public dynamic ConditionTypeData { get; set; }
    }

    public class StepTypeData { }

    public class StepTypeTextData
    {
        [StringNotNullOrEmpty]
        public string Value { get; set; }
    }
}

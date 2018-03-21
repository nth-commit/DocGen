using DocGen.Shared.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DocGen.Web.Api.Core.Templates
{
    public class TemplateStepInputCreate
    {
        [RegularExpression(Constants.TemplateStepInputKeyRegexPattern)]
        public string Key { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Hint { get; set; }

        [NotValue(TemplateStepInputType.Unknown)]
        public TemplateStepInputType Type { get; set; }

        [NullIf(nameof(Type), TemplateStepInputType.Text)]
        [NullIf(nameof(Type), TemplateStepInputType.Checkbox)]
        [ValidateAgainstTypeIf(typeof(IEnumerable<TemplateStepInputTypeData_Radio>), nameof(Type), TemplateStepInputType.Radio)]
        public dynamic TypeData { get; set; }
    }

    public class TemplateStepInputTypeData_Radio
    {
        [StringNotNullOrEmpty]
        public string Name { get; set; }

        [StringNotNullOrEmpty]
        public string Value { get; set; }
    }
}

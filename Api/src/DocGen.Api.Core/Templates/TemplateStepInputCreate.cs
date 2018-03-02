using DocGen.Shared.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DocGen.Api.Core.Templates
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
        public dynamic TypeData { get; set; }
    }
}

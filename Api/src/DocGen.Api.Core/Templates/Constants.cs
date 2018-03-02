using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Api.Core.Templates
{
    public static class Constants
    {
        public const char TemplateComponentReferenceSeparator = '.';

        public const string TemplateStepInputKeyRegexPattern = "[a-z_]+[a-z_0-9]*";

        public const string TemplateStepIdRegexPattern = "(" + TemplateStepInputKeyRegexPattern + ")(." + TemplateStepInputKeyRegexPattern + ")*";

        public const string TemplateComponentReferenceRegexPattern = "(" + TemplateStepInputKeyRegexPattern + ")(." + TemplateStepInputKeyRegexPattern + ")*";
    }
}

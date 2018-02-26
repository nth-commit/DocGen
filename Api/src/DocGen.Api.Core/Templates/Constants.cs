using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Api.Core.Templates
{
    public static class Constants
    {
        public const string TemplateComponentIdRegexPattern = "[a-z_]+[a-z_0-9]*";

        public const char TemplateComponentReferenceSeparator = '.';

        public const string TemplateComponentReferenceRegexPattern = "(" + TemplateComponentIdRegexPattern + ")(." + TemplateComponentIdRegexPattern + ")*";
    }
}

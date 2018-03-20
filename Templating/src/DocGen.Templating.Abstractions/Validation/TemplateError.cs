using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Templating.Validation
{
    public class TemplateError
    {
        public int LineNumber { get; set; }

        public int LinePosition { get; set; }

        public int Length { get; set; }

        public string Message { get; set; }

        public TemplateErrorLevel Level { get; set; }

        public TemplateErrorCode Code { get; set; }

        public string Target { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Templating.Validation
{
    public class TemplateSyntaxError
    {
        public int LineNumber { get; set; }

        public int LinePosition { get; set; }

        public int Length { get; set; }

        public string Message { get; set; }

        public TemplateSyntaxErrorLevel Level { get; set; }

        public TemplateSyntaxErrorCode Code { get; set; }
    }
}

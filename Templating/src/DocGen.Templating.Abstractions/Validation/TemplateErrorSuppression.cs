using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Templating.Validation
{
    public class TemplateErrorSuppression
    {
        public TemplateErrorCode Code { get; set; }

        public string Target { get; set; }
    }
}

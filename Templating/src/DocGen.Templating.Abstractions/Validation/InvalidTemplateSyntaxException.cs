using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocGen.Templating.Validation
{
    public class InvalidTemplateSyntaxException : Exception
    {
        public IEnumerable<TemplateError> Errors { get; set; }

        public InvalidTemplateSyntaxException(IEnumerable<TemplateError> errors) : base("Invalid template syntax")
        {
            Errors = errors.OrderBy(e => e.LineNumber).ThenBy(e => e.LinePosition);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocGen.Templating.Validation
{
    public class InvalidTemplateSyntaxException : Exception
    {
        public IEnumerable<TemplateSyntaxError> Errors { get; set; }

        public InvalidTemplateSyntaxException(IEnumerable<TemplateSyntaxError> errors) : base("Invalid template syntax")
        {
            Errors = errors.OrderBy(e => e.LineNumber).ThenBy(e => e.LinePosition);
        }
    }
}

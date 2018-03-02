using DocGen.Templating.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Api.Core.Templates
{
    public class AlwaysValidTemplateMarkupValidator : ITemplateMarkupValidator
    {
        public void Validate(string markup, int markupVersion, IEnumerable<ReferenceDefinition> references)
        {
        }
    }
}

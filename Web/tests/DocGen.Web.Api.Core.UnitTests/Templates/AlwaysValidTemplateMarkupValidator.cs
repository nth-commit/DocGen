using DocGen.Templating.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Web.Api.Core.Templates
{
    public class AlwaysValidTemplateMarkupValidator : ITemplateValidator
    {
        public void Validate(string markup, int markupVersion, IEnumerable<ReferenceDefinition> references, IEnumerable<TemplateErrorSuppression> errorSuppressions)
        {
        }
    }
}

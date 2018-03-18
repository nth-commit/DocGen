using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Templating.Validation
{
    public interface ITemplateValidator
    {
        void Validate(
            string markup,
            int markupVersion,
            IEnumerable<ReferenceDefinition> references,
            IEnumerable<TemplateErrorSuppression> errorSuppressions);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocGen.Templating.Validation
{
    public class TemplateMarkupValidator : ITemplateMarkupValidator
    {
        private readonly IEnumerable<ITemplateVersionedMarkupValidator> _templateVersionedMarkupValidators;

        public TemplateMarkupValidator(
            IEnumerable<ITemplateVersionedMarkupValidator> templateVersionedMarkupValidators)
        {
            _templateVersionedMarkupValidators = templateVersionedMarkupValidators;
        }

        public void Validate(string markup, int markupVersion)
        {
            var validator = _templateVersionedMarkupValidators.FirstOrDefault(v => v.MarkupVersion == markupVersion);

            if (validator == null)
            {
                throw new MarkupVersionNotSupportedException();
            }
            
            validator.Validate(markup);
        }
    }
}

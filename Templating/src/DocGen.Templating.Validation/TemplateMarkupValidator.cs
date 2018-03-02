using DocGen.Templating.Validation.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DocGen.Templating.Validation
{
    public class TemplateMarkupValidator : ITemplateValidator
    {
        private readonly IEnumerable<IVersionedTemplateValidator> _templateVersionedMarkupValidators;

        public TemplateMarkupValidator(
            IEnumerable<IVersionedTemplateValidator> templateVersionedMarkupValidators)
        {
            _templateVersionedMarkupValidators = templateVersionedMarkupValidators;
        }

        public void Validate(string markup, int markupVersion, IEnumerable<ReferenceDefinition> references)
        {
            var validator = _templateVersionedMarkupValidators.FirstOrDefault(v => v.MarkupVersion == markupVersion);

            if (validator == null)
            {
                throw new MarkupVersionNotSupportedException();
            }
            
            validator.Validate(markup, references);
        }
    }
}

using DocGen.Templating.Validation.Shared;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace DocGen.Templating.Validation.V1
{
    public class TemplateMarkupValidatorV1 : BaseVersionedTemplateMarkupValidator, IVersionedTemplateMarkupValidator
    {
        public override int MarkupVersion => 1;

        protected override void ValidateExpressions(XDocument document, IEnumerable<ReferenceDefinition> references)
        {
            throw new NotImplementedException();
        }
    }
}

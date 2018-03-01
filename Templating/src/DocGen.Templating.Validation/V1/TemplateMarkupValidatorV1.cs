using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Templating.Validation.V1
{
    public class TemplateMarkupValidatorV1 : ITemplateVersionedMarkupValidator
    {
        public int MarkupVersion => 1;

        public void Validate(string markup)
        {
            throw new NotImplementedException();
        }
    }
}

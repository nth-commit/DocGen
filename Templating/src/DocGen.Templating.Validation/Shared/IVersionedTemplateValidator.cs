using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Templating.Validation.Shared
{
    public interface IVersionedTemplateValidator
    {
        int MarkupVersion { get; }

        // TODO:
        // Decide whether to collect input references and validate them at another place or
        // pass in input names/types and validate here.
        void Validate(string markup, IEnumerable<ReferenceDefinition> references);
    }
}

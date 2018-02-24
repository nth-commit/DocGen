using DocGen.Shared.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Api.Core.Templates
{
    public class TemplateCreate
    {
        [StringNotNullOrEmpty]
        public string Name { get; set; }

        [StringNotNullOrEmpty]
        public string Description { get; set; }

        [EnumerableNotEmpty]
        [ValidateEnumerableElements]
        public IEnumerable<TemplateStepCreate> Steps { get; set; }
    }
}

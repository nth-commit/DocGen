using DocGen.Shared.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DocGen.Api.Core.Templates
{
    public class TemplateCreate
    {
        [StringNotNullOrEmpty]
        public string Name { get; set; }

        [StringNotNullOrEmpty]
        public string Description { get; set; }

        [StringNotNullOrEmpty]
        public string Markup { get; set; }

        [Range(1, int.MaxValue)]
        public int MarkupVersion { get; set; }

        [NotValue(TemplateSigningType.Unknown)]
        public TemplateSigningType SigningType { get; set; }

        [EnumerableNotEmpty]
        [ValidateEnumerableElements]
        public IEnumerable<TemplateStepCreate> Steps { get; set; }
    }
}

using DocGen.Shared.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DocGen.Api.Core.Templates
{
    public class Template
    {
        public string Id { get; set; }

        [StringNotNullOrEmpty]
        public string Name { get; set; }

        [StringNotNullOrEmpty]
        public string Description { get; set; }

        [EnumerableNotEmpty]
        [ValidateEnumerableElements]
        public IEnumerable<TemplateStep> Steps { get; set; }
    }
}

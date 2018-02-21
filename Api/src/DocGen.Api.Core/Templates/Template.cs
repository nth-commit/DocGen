using DocGen.Shared.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DocGen.Api.Core.Templates
{
    public class Template
    {
        [StringNotNullOrEmpty]
        public string Name { get; set; }

        [StringNotNullOrEmpty]
        public string Text { get; set; }

        [EnumerableNotEmpty]
        [EnumerableElementsValid]
        public IEnumerable<StepGroup> StepGroups { get; set; }
    }
}

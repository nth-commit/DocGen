using DocGen.Shared.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DocGen.Api.Core.Templates
{
    public class StepGroup
    {
        [StringNotNullOrEmpty]
        public string Title { get; set; }

        [StringNotNullOrEmpty]
        public string Description { get; set; }

        [EnumerableNotEmpty]
        [ValidateEnumerableElements]
        public IEnumerable<Step> Steps { get; set; }
    }
}

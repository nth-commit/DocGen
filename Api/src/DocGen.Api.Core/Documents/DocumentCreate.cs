using DocGen.Shared.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DocGen.Api.Core.Documents
{
    public class DocumentCreate
    {
        [StringNotNullOrEmpty]
        public string TemplateId { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int TemplateVersion { get; set; }

        [Required]
        public IDictionary<string, dynamic> InputValues { get; set; }

        public IDictionary<string, string> TemplatingOptions { get; set; } = new Dictionary<string, string>();
    }
}

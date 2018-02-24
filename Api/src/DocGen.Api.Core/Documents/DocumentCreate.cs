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

        [StringNotNullOrEmpty]
        public string TemplateVersion { get; set; }

        [Required]
        public IDictionary<int, IDictionary<int, object>> Values { get; set; }
    }
}

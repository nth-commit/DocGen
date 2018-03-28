using DocGen.Shared.Core.Dynamic;
using DocGen.Shared.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DocGen.Web.Api.Core.Documents
{
    public class DocumentCreate
    {
        [StringNotNullOrEmpty]
        public string TemplateId { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int TemplateVersion { get; set; }

        [Required]
        public Dictionary<string, dynamic> InputValues { get; set; }
    }

    public static class DocumentCreateExtensions
    {
        public static bool GetIsSigned(this DocumentCreate create) => DynamicUtility.UnwrapValue(() => 
            bool.Parse(((object)create.InputValues["document_signed"]).ToString()));
    }
}

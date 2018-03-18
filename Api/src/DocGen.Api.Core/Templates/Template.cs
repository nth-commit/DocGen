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

        public string Name { get; set; }

        public int Version { get; set; }

        public string Description { get; set; }

        public string Markup { get; set; }

        public int MarkupVersion { get; set; }

        public TemplateSigningType SigningType { get; set; }

        public IEnumerable<TemplateStep> Steps { get; set; }
    }
}

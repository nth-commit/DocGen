using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Web.Api.Core.Templates
{
    public class TemplateStep
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<TemplateStepCondition> Conditions { get; set; }

        public IEnumerable<TemplateStepInput> Inputs { get; set; }

        public string ParentReference { get; set; }
    }
}

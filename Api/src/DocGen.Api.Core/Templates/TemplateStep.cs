using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Api.Core.Templates
{
    public class TemplateStep
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        
        public TemplateComponentConditionType? ConditionType { get; set; }

        public dynamic ConditionData { get; set; }

        public IEnumerable<TemplateStepInput> Inputs { get; set; }

        public IEnumerable<string> ParentNames { get; set; }
    }
}

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

        public dynamic ConditionTypeData { get; set; }

        public IEnumerable<TemplateStepInput> Inputs { get; set; }

        public string ParentReference { get; set; }
    }
}

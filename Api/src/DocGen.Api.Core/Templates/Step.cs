using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Api.Core.Templates
{
    public class Step
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public StepType Type { get; set; }

        public dynamic TypeData { get; set; }

        public StepConditionType ConditionType { get; set; }

        public dynamic ConditionTypeData { get; set; }
    }
}

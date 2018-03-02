using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Api.Core.Templates
{
    public class TemplateStepInput
    {
        public string Key { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Hint { get; set; }

        public TemplateStepInputType Type { get; set; }

        public dynamic TypeData { get; set; }
    }
}

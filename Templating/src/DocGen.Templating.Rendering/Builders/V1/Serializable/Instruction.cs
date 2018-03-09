using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Templating.Rendering.Builders.V1.Serializable
{
    public class Instruction
    {
        public ElementType ElementType { get; set; }

        public WriteType WriteType { get; set; }

        public IEnumerable<string> Conditions { get; set; }

        public string Reference { get; set; }
    }
}

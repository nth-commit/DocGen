using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Templating.Rendering
{
    public class SerializableDocument
    {
        public IEnumerable<Instruction> Instructions { get; set; }
    }
}

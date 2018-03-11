using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Templating.Rendering
{
    public class SerializableDocument
    {
        public int MarkupVersion { get; set; }

        public IEnumerable<Instruction> Instructions { get; set; }
    }
}

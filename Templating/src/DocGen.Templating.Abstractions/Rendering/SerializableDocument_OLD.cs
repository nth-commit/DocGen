using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Templating.Rendering
{
    public class SerializableDocument_OLD
    {
        public int MarkupVersion { get; set; }

        public IEnumerable<Instruction_OLD> Instructions { get; set; }
    }
}

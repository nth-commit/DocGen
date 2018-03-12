using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Templating.Rendering
{
    public class TextInstructionBody
    {
        public string Text { get; set; }

        public string Reference { get; set; }
    }

    public class TextInstruction_OLD : Instruction_OLD<TextInstructionBody>
    {
        public TextInstruction_OLD(ElementType elementType, WriteType writeType, IEnumerable<string> conditions, string text, string reference = null)
            : base(elementType, writeType, conditions, new TextInstructionBody() { Text = text, Reference = reference }) { }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Templating.Rendering
{
    public class Instruction_OLD
    {
        public ElementType ElementType { get; private set; }

        public WriteType WriteType { get; private set; }

        public IEnumerable<string> Conditions { get; private set; }

        public Instruction_OLD(ElementType elementType, WriteType writeType, IEnumerable<string> conditions)
        {
            ElementType = elementType;
            WriteType = writeType;
            Conditions = conditions;
        }
    }

    public class Instruction_OLD<TBody> : Instruction_OLD
    {
        public TBody Body { get; private set; }

        public Instruction_OLD(ElementType elementType, WriteType writeType, IEnumerable<string> conditions, TBody body)
            : base(elementType, writeType, conditions)
        {
            Body = body;
        }
    }
}

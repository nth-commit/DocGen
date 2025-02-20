﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Templating.Rendering
{
    public abstract class Instruction
    {
        public InstructionType Type { get; set; }

#if DEBUG
        public string TypeName { get; set; }
#endif

        public Instruction(InstructionType type)
        {
            Type = type;
#if DEBUG
            TypeName = type.ToString();
#endif
        }
    }

    public class BeginWritePageInstruction : Instruction
    {
        public BeginWritePageInstruction() : base(InstructionType.BeginWritePage)
        {
        }
    }

    public class EndWritePageInstruction : Instruction
    {
        public EndWritePageInstruction() : base(InstructionType.EndWritePage)
        {
        }
    }

    public class BeginWriteListInstruction : Instruction
    {
        public BeginWriteListInstruction() : base(InstructionType.BeginWriteList)
        {
        }
    }

    public class EndWriteListInstruction : Instruction
    {
        public EndWriteListInstruction() : base(InstructionType.EndWriteList)
        {
        }
    }

    public class BeginWriteListItemInstruction : Instruction
    {
        public IEnumerable<int> IndexPath { get; set; }

        public BeginWriteListItemInstruction(IEnumerable<int> indexPath) : base(InstructionType.BeginWriteListItem)
        {
            IndexPath = indexPath;
        }
    }

    public class EndWriteListItemInstruction : Instruction
    {
        public EndWriteListItemInstruction() : base(InstructionType.EndWriteListItem)
        {
        }
    }

    public class WriteParagraphBreakInstruction : Instruction
    {
        public WriteParagraphBreakInstruction() : base(InstructionType.WriteParagraphBreak)
        {
        }
    }

    public class WriteTextInstruction : Instruction
    {
        public string Text { get; set; }

        public string Reference { get; set; }

        public IEnumerable<string> Conditions { get; set; }

        public WriteTextInstruction(
            string text,
            string reference,
            IEnumerable<string> conditions)
            : base(InstructionType.WriteText)
        {
            Text = text;
            Reference = reference;
            Conditions = conditions;
        }
    }
}

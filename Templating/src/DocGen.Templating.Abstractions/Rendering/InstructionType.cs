using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Templating.Rendering
{
    public enum InstructionType
    {
        Unknown = 0,

        BeginWritePage,

        EndWritePage,

        BeginWriteList,

        EndWriteList,

        BeginWriteListItem,

        EndWriteListItem,

        WriteParagraphBreak,

        // TODO
        // Currently unsupported by template
        // Need to differentiate between this and paragraph break. Break = shift + enter in word, paragrah break = just enter
        WriteBreak,

        WriteText,

        // TODO
        // Currently unsupported by template
        WriteSignaturePlaceholder
    }
}

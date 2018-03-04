using DocGen.Templating.Rendering.Builders;
using DocGen.Templating.Rendering.Instructions.V1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Templating.Rendering.Builders.V1
{
    public interface IDocumentBuilderV1 : IDocumentBuilder
    {
        Task BeginWriteDocumentAsync(DocumentInstructionContextV1 context);

        Task EndWriteDocumentAsync(DocumentInstructionContextV1 context);

        Task BeginWritePageAsync(DocumentInstructionContextV1 context);

        Task EndWritePageAsync(DocumentInstructionContextV1 context);

        Task BeginWriteBlockAsync(DocumentInstructionContextV1 context);

        Task EndWriteBlockAsync(DocumentInstructionContextV1 context);

        Task WriteInlineAsync(string text, DocumentInstructionContextV1 context);
    }

    public interface IDocumentBuilderV1<T> : IDocumentBuilder<T>, IDocumentBuilderV1 { }
}

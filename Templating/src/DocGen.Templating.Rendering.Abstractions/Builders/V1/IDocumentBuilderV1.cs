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
        Task BeginWriteDocumentAsync(DocumentRenderModel model, DocumentInstructionContextV1 context);

        Task EndWriteDocumentAsync(DocumentInstructionContextV1 context);

        Task BeginWritePageAsync(DocumentInstructionContextV1 context);

        Task EndWritePageAsync(DocumentInstructionContextV1 context);

        Task BeginWriteBlockAsync(DocumentInstructionContextV1 context);

        Task EndWriteBlockAsync(DocumentInstructionContextV1 context);

        Task BeginWriteListAsync(DocumentInstructionContextV1 context);

        Task EndWriteListAsync(DocumentInstructionContextV1 context);

        Task BeginWriteListItemAsync(IEnumerable<int> indexPath, DocumentInstructionContextV1 context);

        Task EndWriteListItemAsync(DocumentInstructionContextV1 context);

        Task BeginConditionalAsync(string expression, DocumentInstructionContextV1 context);

        Task EndCondititionalAsync(DocumentInstructionContextV1 context);

        Task WriteTextAsync(string text, string reference, DocumentInstructionContextV1 context);
    }

    public interface IDocumentBuilderV1<T> : IDocumentBuilder<T>, IDocumentBuilderV1 { }
}

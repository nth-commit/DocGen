using DocGen.Templating.Rendering.Builders.V1;
using DocGen.Templating.Rendering.Instructions.V1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Templating.Rendering.Pdf.V1
{
    public class PdfDocumentBuilderV1 : IDocumentBuilderV1<PdfDocumentResult>
    {
        public PdfDocumentResult Result => new PdfDocumentResult();

        public int MarkupVersion => 1;

        public Task BeginWriteDocumentAsync(DocumentInstructionContextV1 context)
        {
            return Task.CompletedTask;
        }

        public Task EndWriteDocumentAsync(DocumentInstructionContextV1 context)
        {
            return Task.CompletedTask;
        }

        public Task BeginWritePageAsync(DocumentInstructionContextV1 context)
        {
            return Task.CompletedTask;
        }

        public Task EndWritePageAsync(DocumentInstructionContextV1 context)
        {
            return Task.CompletedTask;
        }

        public Task BeginWriteBlockAsync(DocumentInstructionContextV1 context)
        {
            return Task.CompletedTask;
        }

        public Task EndWriteBlockAsync(DocumentInstructionContextV1 context)
        {
            return Task.CompletedTask;
        }

        public Task BeginWriteListAsync(DocumentInstructionContextV1 context)
        {
            return Task.CompletedTask;
        }

        public Task EndWriteListAsync(DocumentInstructionContextV1 context)
        {
            return Task.CompletedTask;
        }

        public Task BeginWriteListItemAsync(int index, DocumentInstructionContextV1 context)
        {
            return Task.CompletedTask;
        }

        public Task EndWriteListItemAsync(DocumentInstructionContextV1 context)
        {
            return Task.CompletedTask;
        }

        public Task BeginConditionalAsync(string expression, DocumentInstructionContextV1 context)
        {
            return Task.CompletedTask;
        }

        public Task EndCondititionalAsync(DocumentInstructionContextV1 context)
        {
            return Task.CompletedTask;
        }

        public Task WriteTextAsync(string text, string reference, DocumentInstructionContextV1 context)
        {
            return Task.CompletedTask;
        }
    }
}

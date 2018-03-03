using DocGen.Templating.Rendering.Instructions.V1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Templating.Rendering.Builders.V1.Text
{
    public class TextDocumentBuilderV1 : IDocumentBuilderV1<string>
    {
        private readonly StringBuilder _stringBuilder = new StringBuilder();
        private bool _isRendering = false;

        public int MarkupVersion => 1;

        public string Result => _stringBuilder.ToString();

        public Task BeginWriteDocumentAsync(DocumentInstructionContextV1 context)
        {
            if (_isRendering)
            {
                throw new InvalidOperationException("Document writing already started");
            }

            _isRendering = true;

            return Task.CompletedTask;
        }

        public Task EndWriteDocumentAsync(DocumentInstructionContextV1 context)
        {
            if (!_isRendering)
            {
                throw new InvalidOperationException("Document writing has not started");
            }

            _isRendering = false;

            return Task.CompletedTask;
        }

        public Task BeginWritePageAsync(DocumentInstructionContextV1 context)
        {
            throw new NotImplementedException();
        }

        public Task EndWritePageAsync(DocumentInstructionContextV1 context)
        {
            throw new NotImplementedException();
        }

        public Task BeginWriteParagraphAsync(DocumentInstructionContextV1 context)
        {
            throw new NotImplementedException();
        }

        public Task EndWriteParagraphAsync(DocumentInstructionContextV1 context)
        {
            throw new NotImplementedException();
        }

        public Task WriteParagraphAsync(DocumentInstructionContextV1 context)
        {
            throw new NotImplementedException();
        }

        public Task WriteTextAsync(string text, DocumentInstructionContextV1 context)
        {
            throw new NotImplementedException();
        }
    }
}

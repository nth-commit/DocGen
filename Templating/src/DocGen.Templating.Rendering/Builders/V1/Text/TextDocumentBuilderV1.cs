using DocGen.Templating.Rendering.Instructions.V1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Templating.Rendering.Builders.V1.Text
{
    public class TextDocumentBuilderV1 : IDocumentBuilderV1<TextDocumentResult>
    {
        private readonly StringBuilder _stringBuilder = new StringBuilder();
        private readonly List<int> _pageLocations = new List<int>();

        private bool _isRendering = false;
        private bool _isComplete = false;
        private int _lineCount = 0;

        public int MarkupVersion => 1;

        public TextDocumentResult Result
        {
            get
            {
                if (!_isComplete)
                {
                    throw new InvalidOperationException("Rendering is not complete");
                }

                return new TextDocumentResult()
                {
                    Body = _stringBuilder.ToString(),
                    PageLocations = _pageLocations
                };
            }
        }

        public Task BeginWriteDocumentAsync(DocumentInstructionContextV1 context)
        {
            if (_isRendering || _isComplete)
            {
                throw new InvalidOperationException("Document writing already started");
            }

            _isRendering = true;

            return Task.CompletedTask;
        }

        public Task EndWriteDocumentAsync(DocumentInstructionContextV1 context)
        {
            if (!_isRendering || _isComplete)
            {
                throw new InvalidOperationException("Document writing has not started");
            }

            _isRendering = false;
            _isComplete = true;

            return Task.CompletedTask;
        }

        public Task BeginWritePageAsync(DocumentInstructionContextV1 context)
        {
            if (IsParagraphElement(context.Previous))
            {
                Debug.Assert(context.Previous == "page");
                BeginParagraph();
            }

            _pageLocations.Add(_lineCount);

            return Task.CompletedTask;
        }

        public Task EndWritePageAsync(DocumentInstructionContextV1 context)
        {
            return Task.CompletedTask;
        }

        public Task BeginWriteBlockAsync(DocumentInstructionContextV1 context)
        {
            if (IsParagraphElement(context.Previous))
            {
                BeginParagraph();
            }

            return Task.CompletedTask;
        }

        public Task EndWriteBlockAsync(DocumentInstructionContextV1 context)
        {
            return Task.CompletedTask;
        }

        public Task BeginWriteListAsync(DocumentInstructionContextV1 context)
        {
            throw new NotImplementedException();
        }

        public Task EndWriteListAsync(DocumentInstructionContextV1 context)
        {
            return Task.CompletedTask;
        }

        public Task BeginWriteListItemAsync(int index, DocumentInstructionContextV1 context)
        {
            throw new NotImplementedException();
        }

        public Task EndWriteListItemAsync(DocumentInstructionContextV1 context)
        {
            return Task.CompletedTask;
        }

        public Task WriteInlineAsync(string text, DocumentInstructionContextV1 context)
        {
            if (IsParagraphElement(context.Previous))
            {
                BeginParagraph();
            }

            _stringBuilder.Append(text);

            return Task.CompletedTask;
        }

        private bool IsParagraphElement(string element)
        {
            return element == "page" || element == "block" || element == "list" || element == "list-item";
        }

        private void BeginParagraph(string value = null)
        {
            _stringBuilder.AppendLine(value);
            _stringBuilder.AppendLine(value);
            _lineCount += 2;
        }
    }
}

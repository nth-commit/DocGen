using DocGen.Templating.Rendering.Instructions.V1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Templating.Rendering.Builders.V1.Text
{
    public class TextDocumentBuilderV1 : IDocumentBuilderV1<TextDocument>
    {
        private readonly StringBuilder _stringBuilder = new StringBuilder();
        private readonly List<int> _pageLocations = new List<int>();

        private bool _isRendering = false;
        private bool _isComplete = false;
        private int _lineCount = 0;

        public int MarkupVersion => 1;

        public TextDocument Result
        {
            get
            {
                if (!_isComplete)
                {
                    throw new InvalidOperationException("Rendering is not complete");
                }

                return new TextDocument()
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
            if (!context.IsFirstChild)
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
            if (!context.IsFirstChild)
            {
                BeginParagraph();

                if (context.Parent == "list-item")
                {
                    AppendIndentation(context);
                }
            }

            return Task.CompletedTask;
        }

        public Task EndWriteBlockAsync(DocumentInstructionContextV1 context)
        {
            return Task.CompletedTask;
        }

        public Task BeginWriteListAsync(DocumentInstructionContextV1 context)
        {
            if (!context.IsFirstChild || context.Parent == "list-item")
            {
                BeginParagraph();
            }
            else
            {
                AppendIndentation(context);
            }

            return Task.CompletedTask;
        }

        public Task EndWriteListAsync(DocumentInstructionContextV1 context)
        {
            return Task.CompletedTask;
        }

        public Task BeginWriteListItemAsync(int index, DocumentInstructionContextV1 context)
        {
            if (!context.IsFirstChild)
            {
                BeginParagraph();
            }

            AppendListIndex(context);

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

        public Task WriteTextAsync(string text, string expression, DocumentInstructionContextV1 context)
        {
            if (IsParagraphElement(context.Previous))
            {
                BeginParagraph();
            }

            //if (context.Parent == "list-item" && !context.IsFirstChild)
            //{
            //    AppendIndentation(context);
            //}

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

        private void AppendIndentation(DocumentInstructionContextV1 context)
        {
            foreach (var listItemIndex in context.ListItemPath)
            {
                _stringBuilder.Append("\t");
            }
        }

        private void AppendListIndex(DocumentInstructionContextV1 context)
        {
            AppendIndentation(context);

            foreach (var listItemIndex in context.ListItemPath)
            {
                _stringBuilder.Append((listItemIndex + 1) + ".");
            }

            _stringBuilder.Append(" ");
        }
    }
}

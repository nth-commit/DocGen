using DocGen.Templating.Rendering.Instructions.V1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace DocGen.Templating.Rendering.Builders.V1.Serializable
{
    public class SerializableDocumentBuilderV1 : IDocumentBuilderV1<SerializableDocument>
    {
        private readonly List<Instruction> _instructions = new List<Instruction>();
        private readonly Stack<string> _currentConditionals = new Stack<string>();
        private readonly List<string> _pendingText = new List<string>();

        private bool _isRendering = false;
        private bool _isComplete = false;

        public int MarkupVersion => 1;

        public SerializableDocument Result
        {
            get
            {
                if (!_isComplete)
                {
                    throw new InvalidOperationException("Rendering is not complete");
                }

                return new SerializableDocument()
                {
                    MarkupVersion = MarkupVersion,
                    Instructions = _instructions
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
            DebugAssertTextFlushed();
            _instructions.Add(new BeginWritePageInstruction());
            return Task.CompletedTask;
        }

        public Task EndWritePageAsync(DocumentInstructionContextV1 context)
        {
            FlushText();
            _instructions.Add(new EndWritePageInstruction());
            return Task.CompletedTask;
        }

        public Task BeginWriteBlockAsync(DocumentInstructionContextV1 context)
        {
            FlushText();

            if (!context.IsFirstChild)
            {
                WriteParagraphBreak();
            }

            return Task.CompletedTask;
        }

        public Task EndWriteBlockAsync(DocumentInstructionContextV1 context)
        {
            FlushText();
            return Task.CompletedTask;
        }

        public Task BeginWriteListAsync(DocumentInstructionContextV1 context)
        {
            FlushText();

            if (!context.IsFirstChild)
            {
                WriteParagraphBreak();
            }

            _instructions.Add(new BeginWriteListInstruction());

            return Task.CompletedTask;
        }

        public Task EndWriteListAsync(DocumentInstructionContextV1 context)
        {
            DebugAssertTextFlushed(); // List cannot contain text elements
            _instructions.Add(new EndWriteListInstruction());
            return Task.CompletedTask;
        }

        public Task BeginWriteListItemAsync(int index, DocumentInstructionContextV1 context)
        {
            FlushText();

            if (!context.IsFirstChild)
            {
                WriteParagraphBreak();
            }

            _instructions.Add(new BeginWriteListItemInstruction());

            return Task.CompletedTask;
        }

        public Task EndWriteListItemAsync(DocumentInstructionContextV1 context)
        {
            FlushText();
            _instructions.Add(new EndWriteListItemInstruction());
            return Task.CompletedTask;
        }

        public Task BeginConditionalAsync(string expression, DocumentInstructionContextV1 context)
        {
            FlushText();
            _currentConditionals.Push(expression);
            return Task.CompletedTask;
        }

        public Task EndCondititionalAsync(DocumentInstructionContextV1 context)
        {
            FlushText();
            _currentConditionals.Pop();
            return Task.CompletedTask;
        }

        public Task WriteTextAsync(string text, string reference, DocumentInstructionContextV1 context)
        {
            if (IsBlockElement(context.Previous))
            {
                // Text should flow to new paragraph if it follows a block element.
                WriteParagraphBreak();
            }

            if (string.IsNullOrEmpty(reference))
            {
                _pendingText.Add(text);
            }
            else
            {
                // If this is from a reference, then we want to generate an instruction which contains only this reference.
                FlushText();
                _instructions.Add(new WriteTextInstruction(text, reference, _currentConditionals.AsEnumerable()));
            }

            return Task.CompletedTask;
        }

        private void WriteParagraphBreak()
        {
            _instructions.Add(new WriteParagraphBreakInstruction());
        }

        private bool IsBlockElement(string element)
        {
            return element == "block" || element == "list" || element == "list-item";
        }

        private void FlushText()
        {
            if (_pendingText.Any())
            {
                _instructions.Add(new WriteTextInstruction(
                    string.Join(string.Empty, _pendingText),
                    null,
                    _currentConditionals.AsEnumerable()));

                _pendingText.Clear();
            }
        }

        private void DebugAssertTextFlushed() => Debug.Assert(!_pendingText.Any());
    }
}

using DocGen.Templating.Rendering.Instructions.V1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Templating.Rendering.Builders.V1.Serializable
{
    public class SerializableDocumentBuilderV1_OLD : IDocumentBuilderV1<SerializableDocument_OLD>
    {
        private readonly List<Instruction_OLD> _instructions = new List<Instruction_OLD>();
        private readonly Stack<string> _currentConditionals = new Stack<string>();

        private bool _isRendering = false;
        private bool _isComplete = false;

        public int MarkupVersion => 1;

        public SerializableDocument_OLD Result
        {
            get
            {
                if (!_isComplete)
                {
                    throw new InvalidOperationException("Rendering is not complete");
                }

                return new SerializableDocument_OLD()
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
            if (!context.IsFirstChild)
            {
                Debug.Assert(context.Previous == "page");
            }

            AddInstruction(ElementType.Page, WriteType.BeginWrite);

            return Task.CompletedTask;
        }

        public Task EndWritePageAsync(DocumentInstructionContextV1 context)
        {
            AddInstruction(ElementType.Page, WriteType.EndWrite);

            return Task.CompletedTask;
        }

        public Task BeginWriteBlockAsync(DocumentInstructionContextV1 context)
        {
            AddInstruction(ElementType.Block, WriteType.BeginWrite);

            return Task.CompletedTask;
        }

        public Task EndWriteBlockAsync(DocumentInstructionContextV1 context)
        {
            AddInstruction(ElementType.Block, WriteType.EndWrite);

            return Task.CompletedTask;
        }

        public Task BeginWriteListAsync(DocumentInstructionContextV1 context)
        {
            AddInstruction(ElementType.List, WriteType.BeginWrite);

            return Task.CompletedTask;
        }

        public Task EndWriteListAsync(DocumentInstructionContextV1 context)
        {
            AddInstruction(ElementType.List, WriteType.EndWrite);

            return Task.CompletedTask;
        }

        public Task BeginWriteListItemAsync(int index, DocumentInstructionContextV1 context)
        {
            AddInstruction(ElementType.ListItem, WriteType.BeginWrite);

            return Task.CompletedTask;
        }

        public Task EndWriteListItemAsync(DocumentInstructionContextV1 context)
        {
            AddInstruction(ElementType.ListItem, WriteType.EndWrite);

            return Task.CompletedTask;
        }

        public Task BeginConditionalAsync(string expression, DocumentInstructionContextV1 context)
        {
            _currentConditionals.Push(expression);

            return Task.CompletedTask;
        }

        public Task EndCondititionalAsync(DocumentInstructionContextV1 context)
        {
            _currentConditionals.Pop();

            return Task.CompletedTask;
        }

        public Task WriteTextAsync(string text, string reference, DocumentInstructionContextV1 context)
        {
            _instructions.Add(new TextInstruction_OLD(
                ElementType.Text,
                WriteType.Write,
                _currentConditionals.ToArray(),
                text,
                reference));

            return Task.CompletedTask;
        }

        private void AddInstruction(ElementType elementType, WriteType writeType)
        {
            _instructions.Add(new Instruction_OLD(elementType, writeType, _currentConditionals.ToArray()));
        }
    }
}

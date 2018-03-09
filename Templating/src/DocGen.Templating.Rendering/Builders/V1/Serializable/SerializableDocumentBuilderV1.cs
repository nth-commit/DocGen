using DocGen.Templating.Rendering.Instructions.V1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Templating.Rendering.Builders.V1.Serializable
{
    public class SerializableDocumentBuilderV1 : IDocumentBuilderV1<SerializableDocument>
    {
        // TODO:
        // Track conditions and references, include them in instructions themselves. Will have to have a collection of conditions, for nested ones

        private readonly Stack<string> _currentConditionals = new Stack<string>();

        private string _currentExpression;


        public int MarkupVersion => 1;

        public SerializableDocument Result => throw new NotImplementedException();

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

using DocGen.Templating.Rendering.Instructions.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DocGen.Templating.Rendering.Builders.V1.Serializable
{
    public class BalancingTests
    {
        [Fact]
        public async Task TestBalanced_1()
        {
            var result = await GetResultAsync(
                @"<document><page><inline>1<data>reference</data></inline></page></document>",
                new DocumentRenderModel()
                {
                    Items = new List<DocumentRenderModelItem>()
                    {
                        new DocumentRenderModelItem()
                        {
                            Reference = "reference",
                            Value = "2"
                        }
                    }
                });

            var elementTypeStack = new Stack<ElementType>();
            foreach (var instruction in result.Instructions)
            {
                if (instruction.WriteType == WriteType.BeginWrite)
                {
                    elementTypeStack.Push(instruction.ElementType);
                }
                else if (instruction.WriteType == WriteType.EndWrite)
                {
                    var expectedElementType = elementTypeStack.Pop();
                    Assert.Equal(expectedElementType, instruction.ElementType);
                }
            }
        }

        private async Task<SerializableDocument> GetResultAsync(string markup, DocumentRenderModel model = null)
        {
            var builder = new SerializableDocumentBuilderV1();

            await new DocumentInstructorV1().InstructRenderingAsync(
                @"<document><page><inline>1<data>reference</data></inline></page></document>",
                model ?? new DocumentRenderModel() { Items = Enumerable.Empty<DocumentRenderModelItem>() },
                builder);

            return builder.Result;
        }
    }
}

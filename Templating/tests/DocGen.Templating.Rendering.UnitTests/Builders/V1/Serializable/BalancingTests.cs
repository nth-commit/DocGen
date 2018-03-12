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
        private static readonly List<InstructionType> __beginInstructionTypes = new List<InstructionType>()
        {
            InstructionType.BeginWritePage, InstructionType.BeginWriteList, InstructionType.BeginWriteListItem
        };

        private static readonly List<InstructionType> __endInstructionTypes = new List<InstructionType>()
        {
            InstructionType.EndWritePage, InstructionType.EndWriteList, InstructionType.EndWriteListItem
        };

        private static readonly Dictionary<InstructionType, string> __instructionNamesByType = new Dictionary<InstructionType, string>()
        {
            { InstructionType.BeginWritePage, "page" },
            { InstructionType.EndWritePage, "page" },
            { InstructionType.BeginWriteList, "list" },
            { InstructionType.EndWriteList, "list" },
            { InstructionType.BeginWriteListItem, "list-item" },
            { InstructionType.EndWriteListItem, "list-item" },
        };

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

            var elementTypeStack = new Stack<string>();
            foreach (var instruction in result.Instructions)
            {
                if (__beginInstructionTypes.Contains(instruction.Type))
                {
                    elementTypeStack.Push(__instructionNamesByType[instruction.Type]);
                }
                else if (__endInstructionTypes.Contains(instruction.Type))
                {
                    var expectedElementType = elementTypeStack.Pop();
                    Assert.Equal(expectedElementType, __instructionNamesByType[instruction.Type]);
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

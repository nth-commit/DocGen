using DocGen.Templating.Rendering.Builders.V1;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DocGen.Templating.Rendering.Instructions.V1
{
    public class WriteInlineAggregationTests
    {
        [Fact]
        public async Task TestWriteInlineAggregation_Inlines()
        {
            var writeInlines = new List<string>();
            var builderMock = new Mock<IDocumentBuilderV1>();
            builderMock
                .Setup(x => x.WriteInlineAsync(It.IsAny<string>(), It.IsAny<DocumentInstructionContextV1>()))
                .Callback<string, DocumentInstructionContextV1>((text, context) => writeInlines.Add(text))
                .Returns(Task.CompletedTask);

            await new DocumentInstructorV1().InstructRenderingAsync(
                @"<document><page><inline>1</inline><inline>2</inline></page></document>",
                new DocumentRenderModel() { Items = Enumerable.Empty<DocumentRenderModelItem>() },
                builderMock.Object);

            Assert.Equal(new string[] { "1 2" }, writeInlines);
        }

        [Fact]
        public async Task TestWriteInlineAggregation_InlineAndText()
        {
            var writeInlines = new List<string>();
            var builderMock = new Mock<IDocumentBuilderV1>();
            builderMock
                .Setup(x => x.WriteInlineAsync(It.IsAny<string>(), It.IsAny<DocumentInstructionContextV1>()))
                .Callback<string, DocumentInstructionContextV1>((text, context) => writeInlines.Add(text))
                .Returns(Task.CompletedTask);

            await new DocumentInstructorV1().InstructRenderingAsync(
                @"<document><page><inline>1</inline>2</page></document>",
                new DocumentRenderModel() { Items = Enumerable.Empty<DocumentRenderModelItem>() },
                builderMock.Object);

            Assert.Equal(new string[] { "1 2" }, writeInlines);
        }

        [Fact]
        public async Task TestWriteInlineAggregation_InlineAndConditionalInline()
        {
            var writeInlines = new List<string>();
            var builderMock = new Mock<IDocumentBuilderV1>();
            builderMock
                .Setup(x => x.WriteInlineAsync(It.IsAny<string>(), It.IsAny<DocumentInstructionContextV1>()))
                .Callback<string, DocumentInstructionContextV1>((text, context) => writeInlines.Add(text))
                .Returns(Task.CompletedTask);

            await new DocumentInstructorV1().InstructRenderingAsync(
                @"<document><page><inline>1</inline><inline if='reference = value'>2</inline></page></document>",
                new DocumentRenderModel()
                {
                    Items = new List<DocumentRenderModelItem>()
                    {
                        new DocumentRenderModelItem()
                        {
                            Reference = "reference",
                            Value = "value"
                        }
                    }
                },
                builderMock.Object);

            Assert.Equal(new string[] { "1 2" }, writeInlines);
        }

        [Fact]
        public async Task TestWriteInlineAggregation_InlineAndData()
        {
            var writeInlines = new List<string>();
            var builderMock = new Mock<IDocumentBuilderV1>();
            builderMock
                .Setup(x => x.WriteInlineAsync(It.IsAny<string>(), It.IsAny<DocumentInstructionContextV1>()))
                .Callback<string, DocumentInstructionContextV1>((text, context) => writeInlines.Add(text))
                .Returns(Task.CompletedTask);

            await new DocumentInstructorV1().InstructRenderingAsync(
                @"<document><page><inline>1</inline><data>reference</data></page></document>",
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
                },
                builderMock.Object);

            Assert.Equal(new string[] { "1 2" }, writeInlines);
        }
    }
}

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
    public class WriteTextAggregationTests
    {
        [Fact]
        public async Task TestWriteTextAggregation_Inlines()
        {
            var writeTexts = new List<string>();
            var builderMock = new Mock<IDocumentBuilderV1>();
            builderMock
                .Setup(x => x.WriteTextAsync(It.IsAny<string>(), It.IsAny<DocumentInstructionContextV1>()))
                .Callback<string, DocumentInstructionContextV1>((text, context) => writeTexts.Add(text))
                .Returns(Task.CompletedTask);

            await new DocumentInstructorV1().InstructRenderingAsync(
                @"<document><page><inline>1</inline><inline>2</inline></page></document>",
                new DocumentRenderModel() { Items = Enumerable.Empty<DocumentRenderModelItem>() },
                builderMock.Object);

            Assert.Equal(new string[] { "1 2" }, writeTexts);
        }

        [Fact]
        public async Task TestWriteTextAggregation_InlineAndText()
        {
            var writeTexts = new List<string>();
            var builderMock = new Mock<IDocumentBuilderV1>();
            builderMock
                .Setup(x => x.WriteTextAsync(It.IsAny<string>(), It.IsAny<DocumentInstructionContextV1>()))
                .Callback<string, DocumentInstructionContextV1>((text, context) => writeTexts.Add(text))
                .Returns(Task.CompletedTask);

            await new DocumentInstructorV1().InstructRenderingAsync(
                @"<document><page><inline>1</inline>2</page></document>",
                new DocumentRenderModel() { Items = Enumerable.Empty<DocumentRenderModelItem>() },
                builderMock.Object);

            Assert.Equal(new string[] { "1 2" }, writeTexts);
        }

        [Fact]
        public async Task TestWriteTextAggregation_InlineAndConditionalInline()
        {
            var writeTexts = new List<string>();
            var builderMock = new Mock<IDocumentBuilderV1>();
            builderMock
                .Setup(x => x.WriteTextAsync(It.IsAny<string>(), It.IsAny<DocumentInstructionContextV1>()))
                .Callback<string, DocumentInstructionContextV1>((text, context) => writeTexts.Add(text))
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

            Assert.Equal(new string[] { "1 2" }, writeTexts);
        }

        [Fact]
        public async Task TestWriteTextAggregation_InlineAndData()
        {
            var writeTexts = new List<string>();
            var builderMock = new Mock<IDocumentBuilderV1>();
            builderMock
                .Setup(x => x.WriteTextAsync(It.IsAny<string>(), It.IsAny<DocumentInstructionContextV1>()))
                .Callback<string, DocumentInstructionContextV1>((text, context) => writeTexts.Add(text))
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

            Assert.Equal(new string[] { "1 2" }, writeTexts);
        }
    }
}

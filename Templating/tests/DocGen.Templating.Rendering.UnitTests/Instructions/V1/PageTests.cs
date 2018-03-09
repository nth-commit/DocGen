using DocGen.Templating.Rendering.Builders.V1;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DocGen.Templating.Rendering.Instructions.V1
{
    public class PageTests
    {
        [Fact]
        public async Task TestPage_Conditional_ConditionFails()
        {
            var builderMock = new Mock<IDocumentBuilderV1>();

            var pageBeginCount = 0;
            builderMock
                .Setup(x => x.BeginWritePageAsync(It.IsAny<DocumentInstructionContextV1>()))
                .Callback<DocumentInstructionContextV1>(context => pageBeginCount++)
                .Returns(Task.CompletedTask);

            var pageEndCount = 0;
            builderMock
                .Setup(x => x.EndWritePageAsync(It.IsAny<DocumentInstructionContextV1>()))
                .Callback<DocumentInstructionContextV1>(context => pageEndCount++)
                .Returns(Task.CompletedTask);

            await new DocumentInstructorV1().InstructRenderingAsync(
                @"<document><page></page><page if='reference = 2'></page></document>",
                new DocumentRenderModel()
                {
                    Items = new List<DocumentRenderModelItem>()
                    {
                        new DocumentRenderModelItem()
                        {
                            Reference = "reference",
                            Value = "1"
                        }
                    }
                },
                builderMock.Object);

            Assert.Equal(1, pageBeginCount);
            Assert.Equal(pageBeginCount, pageEndCount);
        }

        [Fact]
        public async Task TestPage_DoubleConditional_FirstConditionPasses_SecondConditionFails()
        {
            var builderMock = new Mock<IDocumentBuilderV1>();

            var pageBeginCount = 0;
            builderMock
                .Setup(x => x.BeginWritePageAsync(It.IsAny<DocumentInstructionContextV1>()))
                .Callback<DocumentInstructionContextV1>(context => pageBeginCount++)
                .Returns(Task.CompletedTask);

            var pageEndCount = 0;
            builderMock
                .Setup(x => x.EndWritePageAsync(It.IsAny<DocumentInstructionContextV1>()))
                .Callback<DocumentInstructionContextV1>(context => pageEndCount++)
                .Returns(Task.CompletedTask);

            await new DocumentInstructorV1().InstructRenderingAsync(
                @"<document><page if='reference = 1'></page><page if='reference = 2'></page></document>",
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

            Assert.Equal(1, pageBeginCount);
            Assert.Equal(pageBeginCount, pageEndCount);
        }
    }
}

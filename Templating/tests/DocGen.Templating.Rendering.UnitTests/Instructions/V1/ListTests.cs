using DocGen.Templating.Rendering.Builders;
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
    public class ListTests
    {
        [Fact]
        public async Task TestList_ListItemCount_Reset()
        {
            var listItemPaths = new List<IEnumerable<int>>();
            var builderMock = new Mock<IDocumentBuilderV1>();
            builderMock
                .Setup(x => x.BeginWriteListItemAsync(It.IsAny<ListIndexPath>(), It.IsAny<DocumentInstructionContextV1>()))
                .Callback<IEnumerable<int>, DocumentInstructionContextV1>((indexPath, context) => listItemPaths.Add(indexPath))
                .Returns(Task.CompletedTask);

            await new DocumentInstructorV1().InstructRenderingAsync(
                PageTemplate(@"
                    <list>
                        <list-item>1</list-item>
                        <list-item>2</list-item>
                    </list>
                    <list>
                        <list-item>1</list-item>
                        <list-item>2</list-item>
                    </list>"),
                new DocumentRenderModel() { Items = Enumerable.Empty<DocumentRenderModelItem>() },
                builderMock.Object);

            Assert.Equal(
                new List<IEnumerable<int>>()
                {
                    new int[] { 0 },
                    new int[] { 1 },
                    new int[] { 0 },
                    new int[] { 1 }
                },
                listItemPaths);
        }

        [Fact]
        public async Task TestList_ListItemCount_Continuation()
        {
            var listItemPaths = new List<IEnumerable<int>>();
            var builderMock = new Mock<IDocumentBuilderV1>();
            builderMock
                .Setup(x => x.BeginWriteListItemAsync(It.IsAny<ListIndexPath>(), It.IsAny<DocumentInstructionContextV1>()))
                .Callback<IEnumerable<int>, DocumentInstructionContextV1>((indexPath, context) => listItemPaths.Add(indexPath))
                .Returns(Task.CompletedTask);

            await new DocumentInstructorV1().InstructRenderingAsync(
                PageTemplate(@"
                    <list>
                        <list-item>1</list-item>
                        <list-item>2</list-item>
                    </list>
                    <list start='continue'>
                        <list-item>1</list-item>
                        <list-item>2</list-item>
                    </list>"),
                new DocumentRenderModel() { Items = Enumerable.Empty<DocumentRenderModelItem>() },
                builderMock.Object);

            Assert.Equal(
                new List<IEnumerable<int>>()
                {
                    new int[] { 0 },
                    new int[] { 1 },
                    new int[] { 2 },
                    new int[] { 3 }
                },
                listItemPaths);
        }

        [Fact]
        public async Task TestList_ListItemCount_Nested()
        {
            var listItemPaths = new List<IEnumerable<int>>();
            var builderMock = new Mock<IDocumentBuilderV1>();
            builderMock
                .Setup(x => x.BeginWriteListItemAsync(It.IsAny<ListIndexPath>(), It.IsAny<DocumentInstructionContextV1>()))
                .Callback<IEnumerable<int>, DocumentInstructionContextV1>((indexPath, context) => listItemPaths.Add(indexPath))
                .Returns(Task.CompletedTask);

            await new DocumentInstructorV1().InstructRenderingAsync(
                PageTemplate(@"
                    <list>
                        <list-item>
                            <list>
                                <list-item>1</list-item>
                                <list-item>2</list-item>
                            </list>
                        </list-item>
                        <list-item>
                            <list>
                                <list-item>1</list-item>
                                <list-item>2</list-item>
                            </list>
                        </list-item>
                    </list>
                    <list>
                        <list-item>
                            <list>
                                <list-item>1</list-item>
                                <list-item>2</list-item>
                            </list>
                        </list-item>
                        <list-item>
                            <list>
                                <list-item>1</list-item>
                                <list-item>2</list-item>
                            </list>
                        </list-item>
                    </list>"),
                new DocumentRenderModel() { Items = Enumerable.Empty<DocumentRenderModelItem>() },
                builderMock.Object);

            Assert.Equal(
                new List<IEnumerable<int>>()
                {
                    new int[] { 0 },
                    new int[] { 0, 0 },
                    new int[] { 0, 1 },
                    new int[] { 1 },
                    new int[] { 1, 0 },
                    new int[] { 1, 1 },
                    new int[] { 0 },
                    new int[] { 0, 0 },
                    new int[] { 0, 1 },
                    new int[] { 1 },
                    new int[] { 1, 0 },
                    new int[] { 1, 1 }
                },
                listItemPaths);
        }

        private string PageTemplate(string markup) => $"<document><page>{markup}</page></document>";
    }
}

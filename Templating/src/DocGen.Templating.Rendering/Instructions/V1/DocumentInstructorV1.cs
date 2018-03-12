using DocGen.Shared.Core.Dynamic;
using DocGen.Templating.Rendering.Builders.V1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DocGen.Templating.Rendering.Instructions.V1
{
    public class DocumentInstructorV1 : IDocumentInstructor, IDocumentInstructor<IDocumentBuilderV1>
    {
        private DocumentInstructionContextV1 _context;
        private IDocumentBuilderV1 _builder;
        private DocumentRenderModel _model;
        private bool _includeMetadata = true;
        private Dictionary<string, string> _valuesByReference;
        private Dictionary<int, int> _listItemIndexContinueOffsetByNestingLevel;

        public int MarkupVersion => 1;

        public async Task InstructRenderingAsync(string markup, DocumentRenderModel model, IDocumentBuilderV1 builder)
        {
            if (_context != null)
            {
                throw new InvalidOperationException();
            }

            _context = new DocumentInstructionContextV1();
            _builder = builder;
            _model = model;
            _valuesByReference = _model.Items.ToDictionary(i => i.Reference, i => i.Value);
            _listItemIndexContinueOffsetByNestingLevel = new Dictionary<int, int>();

            XDocument document = null;
            using (var sr = new StringReader(markup))
            {
                document = XDocument.Load(sr);
            }

            var root = document.Root;

            _context = _context.BeforeBegin(document.Root.Name.LocalName);
            await builder.BeginWriteDocumentAsync(_context);
            _context = _context.AfterBegin();

            foreach (var page in root.Elements())
            {
                await WriteConditionalElementAsync(page, async () =>
                {
                    await InstructPageRenderingAsync(page);
                });
            }

            _context = _context.BeforeEnd();
            await builder.EndWriteDocumentAsync(_context);
            _context = _context.AfterEnd();
        }

        private async Task InstructPageRenderingAsync(XElement page)
        {
            AssertElementName(page, "page");

            _context = _context.BeforeBegin(page.Name.LocalName);
            await _builder.BeginWritePageAsync(_context);
            _context = _context.AfterBegin();

            await TraverseContainerElementAsync(page);

            _context = _context.BeforeEnd();
            await _builder.EndWritePageAsync(_context);
            _context = _context.AfterEnd();
        }

        private async Task InstructBlockRenderingAsync(XElement block)
        {
            AssertElementName(block, "block");

            _context = _context.BeforeBegin(block.Name.LocalName);
            await _builder.BeginWriteBlockAsync(_context);
            _context = _context.AfterBegin();

            await TraverseContainerElementAsync(block);

            _context = _context.BeforeEnd();
            await _builder.EndWriteBlockAsync(_context);
            _context = _context.AfterEnd();
        }

        private async Task InstructListRenderingAsync(XElement list)
        {
            AssertElementName(list, "list");

            _context = _context.BeforeBegin(list.Name.LocalName);
            await _builder.BeginWriteListAsync(_context);
            _context = _context.AfterBegin();

            var listNestingLevel = _context.ListNestingLevel;
            if (!_listItemIndexContinueOffsetByNestingLevel.TryGetValue(listNestingLevel, out int listItemIndexContinueOffset))
            {
                _listItemIndexContinueOffsetByNestingLevel[listNestingLevel] = listItemIndexContinueOffset = 0;
            }

            var startAttribute = list.Attributes().FirstOrDefault(a => a.Name == "start");
            if (startAttribute == null || startAttribute.Value != "continue")
            {
                _listItemIndexContinueOffsetByNestingLevel[listNestingLevel] = listItemIndexContinueOffset = 0;
            }

            var listItems = list.Elements().ToList();
            var conditionallyExcludedListItems = 0;
            for (var i = 0; i < listItems.Count; i++)
            {
                var listItem = listItems[i];
                AssertElementName(listItem, "list-item");

                await WriteConditionalElementAsync(
                    listItem,
                    async () =>
                    {
                        int continuedListIndex = listItemIndexContinueOffset + i - conditionallyExcludedListItems;
                        _context = _context.BeforeBeginListItem(continuedListIndex);
                        await _builder.BeginWriteListItemAsync(continuedListIndex, _context);
                        _context = _context.AfterBegin();

                        await TraverseContainerElementAsync(listItem);

                        _context = _context.BeforeEnd();
                        await _builder.EndWriteListItemAsync(_context);
                        _context = _context.AfterEndListItem();
                    },
                    () => conditionallyExcludedListItems++);
            }

            _context = _context.BeforeEnd();
            await _builder.EndWriteListAsync(_context);
            _context = _context.AfterEnd();

            _listItemIndexContinueOffsetByNestingLevel[listNestingLevel] += listItems.Count;
        }

        private async Task TraverseContainerElementAsync(XElement container)
        {
            foreach (var node in container.Nodes())
            {
                if (node is XText)
                {
                    await InstructWriteTextAsync((XText)node);
                }
                else if (node is XElement)
                {
                    var element = (XElement)node;

                    await WriteConditionalElementAsync(element, async () =>
                    {
                        if (element.Name.LocalName == "inline")
                        {
                            await InstructWriteInlineAsync(element);
                        }
                        else if (element.Name.LocalName == "data")
                        {
                            await InstructWriteDataAsync(element);
                        }
                        else if (element.Name.LocalName == "block")
                        {
                            await InstructBlockRenderingAsync(element);
                        }
                        else if (element.Name.LocalName == "list")
                        {
                            await InstructListRenderingAsync(element);
                        }
                    });
                }
            }
        }

        private async Task InstructWriteInlineAsync(XElement inline)
        {
            AssertElementName(inline, "inline");

            foreach (var node in inline.Nodes())
            {
                if (node is XText)
                {
                    await InstructWriteTextAsync((XText)node);
                }
                else
                {
                    var element = node as XElement;
                    if (element != null && element.Name.LocalName == "data")
                    {
                        await InstructWriteDataAsync(element);
                    }
                    else
                    {
                        throw new Exception("Only data elements or text can be inside an inline element");
                    }
                }
            }
        }

        private async Task InstructWriteDataAsync(XElement data)
        {
            AssertElementName(data, "data");

            var reference = ((XText)data.FirstNode).Value;
            await InstructWriteTextAsync(_valuesByReference[reference], reference);
        }

        private async Task InstructWriteTextAsync(XText text, string reference = null)
        {
            await InstructWriteTextAsync(text.Value, reference);
        }

        private async Task InstructWriteTextAsync(string text, string reference = null)
        {
            if (Regex.Match(text, @"^\s").Success)
            {
                if (_context.IsFirstChild || _context.IsPreviousSiblingBlockLike)
                {
                    text = text.TrimStart();
                }
                else
                {
                    text = " " + text.TrimStart();
                }
            }

            if (Regex.Match(text, @"\s$").Success)
            {
                text =  text.TrimEnd() + " ";
            }

            _context = _context.BeforeBegin("text");
            await _builder.WriteTextAsync(text, _includeMetadata ? reference : null, _context);
            _context = _context.AfterBegin().BeforeEnd().AfterEnd();
        }

        private async Task WriteConditionalElementAsync(XElement conditionalElement, Func<Task> writeAction, Action onConditionFailed = null)
        {
            var (conditionalResult, conditionalExpression) = GetElementConditionalValue(conditionalElement);
            if (!string.IsNullOrEmpty(conditionalExpression))
            {
                if (conditionalResult)
                {
                    if (_includeMetadata)
                    {
                        await _builder.BeginConditionalAsync(conditionalExpression, _context);
                    }
                }
                else
                {
                    // Don't call the write action, this element should not be rendered.
                    onConditionFailed?.Invoke();
                    return;
                }
            }

            await writeAction();

            if (!string.IsNullOrEmpty(conditionalExpression) && _includeMetadata)
            {
                await _builder.EndCondititionalAsync(_context);
            }
        }

        private (bool conditional, string reference) GetElementConditionalValue(XElement element)
        {
            var ifAttribute = element.Attributes().FirstOrDefault(a => a.Name == "if");
            if (ifAttribute != null)
            {
                var ifExpressionSplit = ifAttribute.Value.Split('=').Select(s => s.Trim()).ToArray();
                var reference = ifExpressionSplit[0];
                return (_valuesByReference[reference] == ifExpressionSplit[1], reference);
            }
            return (false, null);
        }

        private void AssertElementName(XElement element, string name)
        {
            Debug.Assert(element.Name.LocalName == name);
        }
    }
}

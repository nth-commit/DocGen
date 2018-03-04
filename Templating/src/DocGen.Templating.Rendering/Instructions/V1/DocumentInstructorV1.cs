using DocGen.Templating.Rendering.Builders.V1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DocGen.Templating.Rendering.Instructions.V1
{
    public class DocumentInstructorV1 : IDocumentInstructor, IDocumentInstructor<IDocumentBuilderV1>
    {
        private DocumentInstructionContextV1 _context;
        private IDocumentBuilderV1 _builder;
        private DocumentRenderModel _model;
        private Dictionary<string, string> _valuesByReference;
        private List<string> _pendingText;
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
                if (!GetElementConditionalValue(page))
                {
                    continue;
                }

                await InstructPageRenderingAsync(page);
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

            var nestingLevel = _context.ListNestingLevel;

            if (!_listItemIndexContinueOffsetByNestingLevel.TryGetValue(nestingLevel, out int listItemIndexContinueOffset))
            {
                _listItemIndexContinueOffsetByNestingLevel[nestingLevel] = listItemIndexContinueOffset = 0;
            }

            var startAttribute = list.Attributes().FirstOrDefault(a => a.Name == "start");
            if (startAttribute == null || startAttribute.Value != "continue")
            {
                _listItemIndexContinueOffsetByNestingLevel[nestingLevel] = listItemIndexContinueOffset = 0;
            }

            var listItems = list.Elements().ToList();
            for (var i = 0; i < listItems.Count; i++)
            {
                var listItem = listItems[i];
                AssertElementName(listItem, "list-item");

                int continuedListIndex = listItemIndexContinueOffset + i;
                _context = _context.BeforeBeginListItem(continuedListIndex);
                await _builder.BeginWriteListItemAsync(continuedListIndex, _context);
                _context = _context.AfterBegin();

                await TraverseContainerElementAsync(listItem);

                _context = _context.BeforeEnd();
                await _builder.EndWriteListItemAsync(_context);
                _context = _context.AfterEndListItem();
            }

            _context = _context.BeforeEnd();
            await _builder.EndWriteListAsync(_context);
            _context = _context.AfterEnd();

            _listItemIndexContinueOffsetByNestingLevel[nestingLevel] += listItems.Count;
        }

        private async Task TraverseContainerElementAsync(XElement container)
        {
            foreach (var node in container.Nodes())
            {
                if (node is XText)
                {
                    AddPendingText((XText)node);
                }
                else if (node is XElement)
                {
                    var element = (XElement)node;

                    if (!GetElementConditionalValue(element))
                    {
                        continue;
                    }

                    if (element.Name.LocalName == "inline")
                    {
                        AddPendingText((XText)element.FirstNode);
                    }
                    else if (element.Name.LocalName == "data")
                    {
                        AddPendingText(_valuesByReference[((XText)element.FirstNode).Value]);
                    }
                    else if (element.Name.LocalName == "block")
                    {
                        await InstructWritePendingInlineAsync();
                        await InstructBlockRenderingAsync(element);
                    }
                    else if (element.Name.LocalName == "list")
                    {
                        await InstructWritePendingInlineAsync();
                        await InstructListRenderingAsync(element);
                    }
                }
            }

            await InstructWritePendingInlineAsync();
        }

        private async Task InstructWritePendingInlineAsync()
        {
            if (HasPendingText())
            {
                _context = _context.BeforeBegin("inline");
                await _builder.WriteInlineAsync(string.Join(" ", _pendingText), _context);
                _context = _context.AfterBegin().BeforeEnd().AfterEnd();

                _pendingText = null;
            }
        }

        private bool GetElementConditionalValue(XElement element)
        {
            var ifAttribute = element.Attributes().FirstOrDefault(a => a.Name == "if");
            if (ifAttribute != null)
            {
                var ifExpressionSplit = ifAttribute.Value.Split('=').Select(s => s.Trim()).ToArray();
                if (_valuesByReference[ifExpressionSplit[0]] != ifExpressionSplit[1])
                {
                    return false;
                }
            }
            return true;
        }

        private void AddPendingText(XText text)
        {
            AddPendingText(text.Value);
        }

        private void AddPendingText(string text)
        {
            if (!HasPendingText())
            {
                _pendingText = new List<string>();
            }
            _pendingText.Add(text.Trim());
        }

        private bool HasPendingText()
        {
            return _pendingText != null;
        }

        private void AssertElementName(XElement element, string name)
        {
            Debug.Assert(element.Name.LocalName == name);
        }
    }
}

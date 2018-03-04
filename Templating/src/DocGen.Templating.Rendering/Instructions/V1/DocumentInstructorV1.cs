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

            XDocument document = null;
            using (var sr = new StringReader(markup))
            {
                document = XDocument.Load(sr);
            }

            var root = document.Root;
            await builder.BeginWriteDocumentAsync(_context);

            foreach (var page in root.Elements())
            {
                await InstructPageRenderingAsync(page);
            }

            await builder.EndWriteDocumentAsync(_context);
        }

        private async Task InstructPageRenderingAsync(XElement page)
        {
            AssertElementName(page, "page");

            await _builder.BeginWritePageAsync(_context);
            await TraverseContainerElementAsync(page);
            await _builder.EndWritePageAsync(_context);
        }

        private async Task InstructBlockRenderingAsync(XElement block)
        {
            AssertElementName(block, "block");

            await _builder.BeginWriteParagraphAsync(_context);
            await TraverseContainerElementAsync(block);
            await _builder.EndWriteParagraphAsync(_context);
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

                    var ifAttribute = element.Attributes().FirstOrDefault(a => a.Name == "if");
                    if (ifAttribute != null)
                    {
                        var ifExpressionSplit = ifAttribute.Value.Split('=').Select(s => s.Trim()).ToArray();
                        if (_valuesByReference[ifExpressionSplit[0]] != ifExpressionSplit[1])
                        {
                            continue;
                        }
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
                        await InstructWritePendingTextAsync();
                        await InstructBlockRenderingAsync(element);
                    }
                }
            }

            await InstructWritePendingTextAsync();
        }

        private async Task InstructWritePendingTextAsync()
        {
            if (HasPendingText())
            {
                await _builder.WriteTextAsync(string.Join(" ", _pendingText), _context);
                _pendingText = null;
            }
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
            _pendingText.Add(text);
        }

        private bool HasPendingText()
        {
            return _pendingText != null;
        }

        private bool IsInlineElement(XElement element)
        {
            return element.Name.LocalName == "inline" || element.Name.LocalName == "data";
        }

        private void AssertElementName(XElement element, string name)
        {
            Debug.Assert(element.Name.LocalName == name);
        }
    }
}

using DocGen.Shared.Core.Dynamic;
using DocGen.Templating.Rendering.Builders.V1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
            _listItemIndexContinueOffsetByNestingLevel = new Dictionary<int, int>();

            XDocument document = null;
            using (var sr = new StringReader(markup))
            {
                document = XDocument.Load(sr);
            }

            var root = document.Root;

            var valuesByReference = _model.Items.ToDictionary(i => i.Reference, i => i.Value);

            _context = _context.BeforeBegin(document.Root.Name.LocalName);
            await builder.BeginWriteDocumentAsync(model, _context);
            _context = _context.AfterBegin();

            foreach (var page in root.Elements())
            {
                await WriteConditionalElementAsync(page, valuesByReference, async () =>
                {
                    await InstructPageRenderingAsync(page, valuesByReference);
                });
            }

            _context = _context.BeforeEnd();
            await builder.EndWriteDocumentAsync(_context);
            _context = _context.AfterEnd();
        }

        private async Task InstructPageRenderingAsync(XElement page, Dictionary<string, string> valuesByReference)
        {
            AssertElementName(page, "page");

            _context = _context.BeforeBegin(page.Name.LocalName);
            await _builder.BeginWritePageAsync(_context);
            _context = _context.AfterBegin();

            await TraverseContainerElementAsync(page, valuesByReference);

            _context = _context.BeforeEnd();
            await _builder.EndWritePageAsync(_context);
            _context = _context.AfterEnd();
        }

        private async Task InstructBlockRenderingAsync(XElement block, Dictionary<string, string> valuesByReference)
        {
            AssertElementName(block, "block");

            _context = _context.BeforeBegin(block.Name.LocalName);
            await _builder.BeginWriteBlockAsync(_context);
            _context = _context.AfterBegin();

            await TraverseContainerElementAsync(block, valuesByReference);

            _context = _context.BeforeEnd();
            await _builder.EndWriteBlockAsync(_context);
            _context = _context.AfterEnd();
        }

        private async Task InstructListRenderingAsync(XElement list, Dictionary<string, string> valuesByReference)
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
                    valuesByReference,
                    async () =>
                    {
                        int continuedListIndex = listItemIndexContinueOffset + i - conditionallyExcludedListItems;
                        _context = _context.BeforeBeginListItem(continuedListIndex);
                        await _builder.BeginWriteListItemAsync(_context.ListItemIndexPath, _context);
                        _context = _context.AfterBegin();

                        await TraverseContainerElementAsync(listItem, valuesByReference);

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

        private async Task TraverseContainerElementAsync(XElement container, Dictionary<string, string> valuesByReference)
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

                    await WriteConditionalElementAsync(element, valuesByReference, async () =>
                    {
                        if (element.Name.LocalName == "inline")
                        {
                            await InstructWriteInlineAsync(element, valuesByReference);
                        }
                        else if (element.Name.LocalName == "data")
                        {
                            await InstructWriteDataAsync(element, valuesByReference);
                        }
                        else if (element.Name.LocalName == "block")
                        {
                            await InstructBlockRenderingAsync(element, valuesByReference);
                        }
                        else if (element.Name.LocalName == "list")
                        {
                            await InstructListRenderingAsync(element, valuesByReference);
                        }
                        else if (element.Name.LocalName == "signature")
                        {
                            await InstructWriteSignatureAsync(element, valuesByReference);
                        }
                    });
                }
            }
        }

        private async Task InstructWriteInlineAsync(XElement inline, Dictionary<string, string> valuesByReference)
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
                        await InstructWriteDataAsync(element, valuesByReference);
                    }
                    else
                    {
                        throw new Exception("Only data elements or text can be inside an inline element");
                    }
                }
            }
        }

        private async Task InstructWriteDataAsync(XElement data, Dictionary<string, string> valuesByReference)
        {
            AssertElementName(data, "data");

            var reference = ((XText)data.FirstNode).Value;
            await InstructWriteTextAsync(valuesByReference[reference], reference);
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

        private async Task InstructWriteSignatureAsync(XElement signatureElement, Dictionary<string, string> valuesByReference)
        {
            var signatureValuesByReference = new Dictionary<string, string>()
            {
                { "sign", _model.Sign.ToString().ToLowerInvariant() }
            };

            var signerAttribute = signatureElement.Attributes().FirstOrDefault(a => a.Name == "signer");

            var representingAttribute = signatureElement.Attributes().FirstOrDefault(a => a.Name == "representing");
            var isRepresenting = representingAttribute != null;

            if (isRepresenting)
            {
                if (_model.Sign)
                {
                    signatureValuesByReference.Add("person", valuesByReference[signerAttribute.Value]);
                }

                signatureValuesByReference.Add("representing", true.ToString().ToLowerInvariant());
                signatureValuesByReference.Add("company", valuesByReference[representingAttribute.Value]);
            }
            else
            {
                signatureValuesByReference.Add("person", valuesByReference[signerAttribute.Value]);
                signatureValuesByReference.Add("representing", false.ToString().ToLowerInvariant());
            }

            await _builder.BeginWriteSigningAreaAsync(_context);
            await WritePartialAsync("signature", signatureValuesByReference);
            await _builder.EndWriteSigningAreaAsync(_context);
        }

        private async Task WritePartialAsync(string partialName, Dictionary<string, string> valuesByReference)
        {
            var assemblyDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var partialPath = Path.Combine(assemblyDir, "Instructions", $"V{MarkupVersion}", "Partials", $"{partialName}.partial.xml");

            XDocument document = null;
            using (var sr = new StringReader(File.ReadAllText(partialPath)))
            {
                document = XDocument.Load(sr);
            }

            await TraverseContainerElementAsync(document.Root.Elements().Single(), valuesByReference);
        }

        private async Task WriteConditionalElementAsync(XElement conditionalElement, Dictionary<string, string> valuesByReference, Func<Task> writeAction, Action onConditionFailed = null)
        {
            var (hasCondition, conditionResult, conditionalExpression) = GetElementConditionalValue(conditionalElement, valuesByReference);
            if (hasCondition)
            {
                if (conditionResult)
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

            if (hasCondition && conditionResult && _includeMetadata)
            {
                await _builder.EndCondititionalAsync(_context);
            }
        }

        private (bool hasCondition, bool conditionResult, string conditionalExpression) GetElementConditionalValue(XElement element, Dictionary<string, string> valuesByReference)
        {
            var ifAttribute = element.Attributes().FirstOrDefault(a => a.Name == "if");
            if (ifAttribute != null)
            {
                var ifExpressionSplit = ifAttribute.Value.Split('=').Select(s => s.Trim()).ToArray();
                var reference = ifExpressionSplit[0];
                return (true, valuesByReference[reference] == ifExpressionSplit[1], reference);
            }
            return (false, false, null);
        }

        private void AssertElementName(XElement element, string name)
        {
            Debug.Assert(element.Name.LocalName == name);
        }
    }
}

using DocGen.Templating.Rendering.Builders.V1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DocGen.Templating.Rendering.Instructions.V1
{
    public class DocumentInstructorV1 : IDocumentInstructor, IDocumentInstructor<IDocumentBuilderV1>
    {
        public int MarkupVersion => 1;

        public async Task InstructRenderingAsync(string markup, DocumentRenderModel model, IDocumentBuilderV1 builder)
        {
            XDocument document = null;
            using (var sr = new StringReader(markup))
            {
                document = XDocument.Load(sr);
            }

            var context = new DocumentInstructionContextV1();

            var root = document.Root;
            await builder.BeginWriteDocumentAsync(context);

            foreach (var page in root.Elements())
            {
                await InstructPageRenderingAsync(page, model, builder, context);
            }

            await builder.EndWriteDocumentAsync(context);
        }

        private async Task InstructPageRenderingAsync(XElement page, DocumentRenderModel model, IDocumentBuilderV1 builder, DocumentInstructionContextV1 context)
        {
            AssertElementName(page, "page");

            List<string> collectedInlines = null;
            void addInlineText(XText text)
            {
                collectedInlines = collectedInlines ?? new List<string>();
                collectedInlines.Add(text.Value);
            };

            foreach (var node in page.Nodes())
            {
                if (node is XText)
                {
                    addInlineText((XText)node);
                }
                else if (node is XElement)
                {
                    var element = (XElement)node;
                    if (IsInlineElement(element))
                    {
                        addInlineText((XText)element.FirstNode);
                    }
                    else if (element.Name.LocalName == "block")
                    {
                        if (collectedInlines != null)
                        {
                            await InstructWriteTextAsync(builder, context, collectedInlines);
                            collectedInlines = null;
                        }
                        await InstructBlockRenderingAsync(element, model, builder, context);
                    }
                }
            }

            if (collectedInlines != null)
            {
                await InstructWriteTextAsync(builder, context, collectedInlines);
                collectedInlines = null;
            }
        }

        private async Task InstructBlockRenderingAsync(XElement block, DocumentRenderModel model, IDocumentBuilderV1 builder, DocumentInstructionContextV1 context)
        {
            AssertElementName(block, "block");

            foreach (var child in block.Elements())
            {
                if (child.Name.LocalName == "inline")
                {
                    await InstructBlockRenderingAsync(child, model, builder, context);
                }
            }
        }

        private async Task InstructWriteTextAsync(IDocumentBuilderV1 builder, DocumentInstructionContextV1 context, IEnumerable<string> collectedInlines)
        {
            await builder.WriteTextAsync(string.Join(" ", collectedInlines), context);
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

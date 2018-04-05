using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DocGen.Templating.Rendering.Instructions.V1;

namespace DocGen.Templating.Rendering.Builders.V1.Html
{
    public class HtmlDocumentBuilderV1 : IDocumentBuilderV1<HtmlDocument>, IDisposable
    {
        private List<string> _pages;
        private bool _isComplete = false;

        private StringWriter _pageStringWriter;
        private XmlTextWriter _pageXmlTextWriter;

        public HtmlDocument Result {
            get
            {
                if (_pages == null)
                {
                    throw new InvalidOperationException("Rendering not started");
                }

                if (!_isComplete)
                {
                    throw new InvalidOperationException("Rendering not finished");
                }

                var cssPath = Path.Combine(
                    Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                    "Builders",
                    "V1",
                    "Html",
                    "styles.css");

                return new HtmlDocument()
                {
                    Pages = _pages,
                    Css = File.ReadAllText(cssPath)
                };
            }
        }

        public int MarkupVersion => 1;

        public Task BeginWriteDocumentAsync(DocumentRenderModel model, DocumentInstructionContextV1 context)
        {
            _pages = new List<string>();
            return Task.CompletedTask;
        }

        public Task EndWriteDocumentAsync(DocumentInstructionContextV1 context)
        {
            _isComplete = true;
            return Task.CompletedTask;
        }

        public Task BeginWritePageAsync(DocumentInstructionContextV1 context)
        {
            _pageStringWriter = new StringWriter();
            _pageXmlTextWriter = new XmlTextWriter(_pageStringWriter);
            _pageXmlTextWriter.Formatting = Formatting.Indented;
            _pageXmlTextWriter.Indentation = 4;

            _pageXmlTextWriter.WriteStartElement("div");
            WriteClass("page");

            return Task.CompletedTask;
        }

        public Task EndWritePageAsync(DocumentInstructionContextV1 context)
        {
            _pageXmlTextWriter.WriteFullEndElement();;

            _pages.Add(_pageStringWriter.ToString());

            _pageXmlTextWriter.Dispose();
            _pageStringWriter.Dispose();

            _pageXmlTextWriter = null;
            _pageStringWriter = null;

            return Task.CompletedTask;
        }

        public Task BeginWriteBlockAsync(DocumentInstructionContextV1 context)
        {
            _pageXmlTextWriter.WriteStartElement("div");

            var cssClassValue = "block";
            if (!context.HasContent)
            {
                cssClassValue += " empty";
            }
            WriteClass(cssClassValue);

            return Task.CompletedTask;
        }

        public Task EndWriteBlockAsync(DocumentInstructionContextV1 context)
        {
            _pageXmlTextWriter.WriteFullEndElement();;
            return Task.CompletedTask;
        }

        public Task BeginWriteListAsync(int startIndex, DocumentInstructionContextV1 context)
        {
            _pageXmlTextWriter.WriteStartElement("ol");
            _pageXmlTextWriter.WriteAttributeString("start", (startIndex + 1).ToString());

            return Task.CompletedTask;
        }

        public Task EndWriteListAsync(DocumentInstructionContextV1 context)
        {
            _pageXmlTextWriter.WriteFullEndElement();;
            return Task.CompletedTask;
        }

        public Task BeginWriteListItemAsync(ListIndexPath path, DocumentInstructionContextV1 context)
        {
            _pageXmlTextWriter.WriteStartElement("li");

            _pageXmlTextWriter.WriteStartElement("div");
            WriteClass("label");
            _pageXmlTextWriter.WriteString(path.Format());
            _pageXmlTextWriter.WriteFullEndElement();

            _pageXmlTextWriter.WriteStartElement("div");
            WriteClass("content");

            return Task.CompletedTask;
        }

        public Task EndWriteListItemAsync(DocumentInstructionContextV1 context)
        {
            _pageXmlTextWriter.WriteFullEndElement(); // div
            _pageXmlTextWriter.WriteFullEndElement(); // li
            return Task.CompletedTask;
        }

        public Task BeginConditionalAsync(string expression, DocumentInstructionContextV1 context)
        {
            // Write an attribute to the current element! Easy :)
            return Task.CompletedTask;
        }

        public Task EndCondititionalAsync(DocumentInstructionContextV1 context)
        {
            return Task.CompletedTask;
        }

        public Task BeginWriteSignatureAreaAsync(string signatoryId, DocumentInstructionContextV1 context)
        {
            _pageXmlTextWriter.WriteStartElement("div");
            WriteClass("signature");

            if (!string.IsNullOrEmpty(signatoryId))
            {
                _pageXmlTextWriter.WriteAttributeString("data-signatory-id", signatoryId);
            }

            return Task.CompletedTask;
        }

        public Task EndWriteSignatureAreaAsync(DocumentInstructionContextV1 context)
        {
            _pageXmlTextWriter.WriteEndElement();
            return Task.CompletedTask;
        }

        public Task WriteTextAsync(string text, string reference, DocumentInstructionContextV1 context)
        {
            _pageXmlTextWriter.WriteStartElement("span");
            _pageXmlTextWriter.WriteString(text);
            _pageXmlTextWriter.WriteEndElement();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            if (_pageXmlTextWriter != null)
            {
                _pageXmlTextWriter.Dispose();
                _pageXmlTextWriter = null;
            }
        }

        private void WriteClass(string cssClassValue)
        {
            _pageXmlTextWriter.WriteAttributeString("class", cssClassValue);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
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

                return new HtmlDocument()
                {
                    Pages = _pages
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

            BeginWriteElement("div", "page");

            return Task.CompletedTask;
        }

        public Task EndWritePageAsync(DocumentInstructionContextV1 context)
        {
            EndWriteElement();

            _pages.Add(_pageStringWriter.ToString());

            _pageXmlTextWriter.Dispose();
            _pageStringWriter.Dispose();

            _pageXmlTextWriter = null;
            _pageStringWriter = null;

            return Task.CompletedTask;
        }

        public Task BeginWriteBlockAsync(DocumentInstructionContextV1 context)
        {
            BeginWriteElement("div", "block");
            _pageXmlTextWriter.WriteRaw("&nbsp");
            return Task.CompletedTask;
        }

        public Task EndWriteBlockAsync(DocumentInstructionContextV1 context)
        {
            EndWriteElement();
            return Task.CompletedTask;
        }

        public Task BeginWriteListAsync(DocumentInstructionContextV1 context)
        {
            BeginWriteElement("ol");
            return Task.CompletedTask;
        }

        public Task EndWriteListAsync(DocumentInstructionContextV1 context)
        {
            EndWriteElement();
            return Task.CompletedTask;
        }

        public Task BeginWriteListItemAsync(IEnumerable<int> indexPath, DocumentInstructionContextV1 context)
        {
            BeginWriteElement("li");
            return Task.CompletedTask;
        }

        public Task EndWriteListItemAsync(DocumentInstructionContextV1 context)
        {
            EndWriteElement();
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

        public Task BeginWriteSigningAreaAsync(DocumentInstructionContextV1 context)
        {
            return Task.CompletedTask;
        }

        public Task EndWriteSigningAreaAsync(DocumentInstructionContextV1 context)
        {
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

        private void BeginWriteElement(string element, string cssClass = null)
        {
            _pageXmlTextWriter.WriteStartElement(element);
            if (!string.IsNullOrEmpty(cssClass))
            {
                _pageXmlTextWriter.WriteAttributeString("class", cssClass);
            }
        }

        private void EndWriteElement()
        {
            _pageXmlTextWriter.WriteFullEndElement();
        }
    }
}

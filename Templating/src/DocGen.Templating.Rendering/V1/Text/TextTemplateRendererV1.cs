using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Templating.Rendering.V1.Text
{
    public class TextTemplateRendererV1 : ITemplateRendererV1<string>
    {
        private readonly StringBuilder _stringBuilder = new StringBuilder();
        private bool _isRendering = false;

        public int MarkupVersion => 1;

        public string Result => _stringBuilder.ToString();

        public Task BeginWriteDocumentAsync(RenderContext context)
        {
            if (_isRendering)
            {
                throw new InvalidOperationException("Document writing already started");
            }

            _isRendering = true;

            return Task.CompletedTask;
        }

        public Task EndWriteDocumentAsync(RenderContext context)
        {
            if (!_isRendering)
            {
                throw new InvalidOperationException("Document writing has not started");
            }

            _isRendering = false;

            return Task.CompletedTask;
        }

        public Task BeginWritePageAsync(RenderContext context)
        {
            throw new NotImplementedException();
        }

        public Task EndWritePageAsync(RenderContext context)
        {
            throw new NotImplementedException();
        }

        public Task BeginWriteParagraphAsync(RenderContext context)
        {
            throw new NotImplementedException();
        }

        public Task EndWriteParagraphAsync(RenderContext context)
        {
            throw new NotImplementedException();
        }

        public Task WriteParagraphAsync(RenderContext context)
        {
            throw new NotImplementedException();
        }

        public Task WriteTextAsync(string text, RenderContext context)
        {
            throw new NotImplementedException();
        }
    }
}

using DocGen.Templating.Rendering.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Templating.Rendering.V1
{
    public interface ITemplateRendererV1 : IVersionedTemplateRenderer
    {
        Task BeginWriteDocumentAsync(RenderContext context);

        Task EndWriteDocumentAsync(RenderContext context);

        Task BeginWritePageAsync(RenderContext context);

        Task EndWritePageAsync(RenderContext context);

        Task BeginWriteParagraphAsync(RenderContext context);

        Task EndWriteParagraphAsync(RenderContext context);

        Task WriteParagraphAsync(RenderContext context);

        Task WriteTextAsync(string text, RenderContext context);
    }

    public interface ITemplateRendererV1<T> : IVersionedTemplateRenderer<T>, ITemplateRendererV1 { }
}

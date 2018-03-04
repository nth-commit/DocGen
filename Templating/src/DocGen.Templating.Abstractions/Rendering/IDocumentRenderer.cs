using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Templating.Rendering
{
    public interface IDocumentRenderer
    {
        Task<T> RenderAsync<T>(string markup, int markupVersion, DocumentRenderModel model);
    }
}

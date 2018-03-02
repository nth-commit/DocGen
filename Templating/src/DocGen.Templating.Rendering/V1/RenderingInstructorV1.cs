using DocGen.Templating.Rendering.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DocGen.Templating.Rendering.V1
{
    public class RenderingInstructorV1 : IVersionedRenderingInstructor, IVersionedRenderingInstructor<ITemplateRendererV1>
    {
        public int MarkupVersion => 1;

        public async Task InstructRenderingAsync(string markup, TemplateRenderModel model, ITemplateRendererV1 renderer)
        {
            var document = XDocument.Load(markup);
            var context = new RenderContext();

            await renderer.BeginWriteDocumentAsync(context);

            await renderer.EndWriteDocumentAsync(context);
        }
    }
}

using DocGen.Templating.Rendering.Builders;
using DocGen.Templating.Rendering.Instructions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Templating.Rendering
{
    public class DocumentRenderer : IDocumentRenderer
    {
        private readonly IServiceProvider _serviceProvider;

        public DocumentRenderer(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<T> RenderAsync<T>(string markup, int markupVersion, DocumentRenderModel model)
        {
            var renderer = _serviceProvider.GetRequiredService<IEnumerable<IDocumentBuilder<T>>>().FirstOrDefault(r => r.MarkupVersion == markupVersion);
            if (renderer == null)
            {
                throw new MarkupVersionNotSupportedException();
            }

            var instructor = _serviceProvider.GetRequiredService<IEnumerable<IDocumentInstructor>>().FirstOrDefault(i => i.MarkupVersion == markupVersion);
            if (instructor == null)
            {
                throw new MarkupVersionNotSupportedException();
            }

            // HACK!
            var method = instructor.GetType().GetMethod(nameof(IDocumentInstructor<IDocumentBuilder<T>>.InstructRenderingAsync));
            await (method.Invoke(instructor, new object[] { markup, model, renderer }) as Task);
            //var instructorVersioned = instructor as IVersionedRenderingInstructor<IVersionedTemplateRenderer<T>>;
            //await instructorVersioned.InstructRenderingAsync(markup, model, renderer);

            return renderer.Result;
        }
    }
}

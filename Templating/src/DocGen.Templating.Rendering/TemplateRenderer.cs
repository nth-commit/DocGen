using DocGen.Templating.Rendering.Shared;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Templating.Rendering
{
    public class TemplateRenderer : ITemplateRenderer
    {
        private readonly IServiceProvider _serviceProvider;

        public TemplateRenderer(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task<T> RenderAsync<T>(string markup, int markupVersion, TemplateRenderModel model)
        {
            var renderer = _serviceProvider.GetRequiredService<IEnumerable<IVersionedTemplateRenderer<T>>>().FirstOrDefault(r => r.MarkupVersion == markupVersion);
            if (renderer == null)
            {
                throw new MarkupVersionNotSupportedException();
            }

            var instructor = _serviceProvider.GetRequiredService<IEnumerable<IVersionedRenderingInstructor>>().FirstOrDefault(i => i.MarkupVersion == markupVersion);
            if (instructor == null)
            {
                throw new MarkupVersionNotSupportedException();
            }

            var rendererType = renderer.GetType();
            var renderingInstructorType = typeof(IVersionedRenderingInstructor<>);
            var renderingInstructorGenericType = renderingInstructorType.MakeGenericType(rendererType);

            var renderingInstructor = _serviceProvider.GetService(renderingInstructorGenericType);

            //var renderingInstructor = _versionedRenderingInstructors.FirstOrDefault(i => i.MarkupVersion == markupVersion);
            //if (renderingInstructor == null)
            //{
            //    throw new MarkupVersionNotSupportedException();
            //}



            //renderingInstructor.InstructRenderingAsync(markup, model, renderer);
            //return renderer;

            return renderer.Result;
        }
    }
}

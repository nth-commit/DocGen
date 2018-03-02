using DocGen.Templating.Rendering;
using DocGen.Templating.Rendering.Shared;
using DocGen.Templating.Rendering.V1;
using DocGen.Templating.Rendering.V1.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class TemplateRenderingServices
    {
        public static IServiceCollection AddTemplatingRenderingServices(
            this IServiceCollection services)
        {
            services.AddTransient<ITemplateRenderer, TemplateRenderer>();

            // V1
            services.AddTransient<IVersionedRenderingInstructor<ITemplateRendererV1>, RenderingInstructorV1>();
            services.AddTransient<IVersionedRenderingInstructor, RenderingInstructorV1>();
            services.AddTransient<IVersionedTemplateRenderer<string>, TextTemplateRendererV1>();

            return services;
        }
    }
}

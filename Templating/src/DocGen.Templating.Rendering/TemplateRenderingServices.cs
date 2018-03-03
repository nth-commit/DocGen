using DocGen.Templating.Rendering;
using DocGen.Templating.Rendering.Builders;
using DocGen.Templating.Rendering.Builders.V1;
using DocGen.Templating.Rendering.Builders.V1.Text;
using DocGen.Templating.Rendering.Instructions;
using DocGen.Templating.Rendering.Instructions.V1;
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
            services.AddTransient<IDocumentInstructor<IDocumentBuilderV1>, DocumentInstructorV1>();
            services.AddTransient<IDocumentInstructor, DocumentInstructorV1>();
            services.AddTransient<IDocumentBuilder<string>, TextDocumentBuilderV1>();

            return services;
        }
    }
}

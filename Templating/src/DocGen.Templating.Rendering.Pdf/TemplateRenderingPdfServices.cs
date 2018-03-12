using DocGen.Templating.Rendering.Builders;
using DocGen.Templating.Rendering.Builders.V1;
using DocGen.Templating.Rendering.Pdf;
using DocGen.Templating.Rendering.Pdf.V1;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class TemplateRenderingPdfServices
    {
        public static IServiceCollection AddTemplatingRenderingPdfServices(
            this IServiceCollection services)
        {
            services.AddTransient<IDocumentBuilder<PdfDocument>, PdfDocumentBuilderV1>();
            return services;
        }
    }
}

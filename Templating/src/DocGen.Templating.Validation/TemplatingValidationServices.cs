using DocGen.Templating.Validation;
using DocGen.Templating.Validation.V1;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class TemplatingValidationServices
    {
        public static IServiceCollection AddTemplatingValidationServices(
            this IServiceCollection services)
        {
            services.AddTransient<ITemplateMarkupValidator, TemplateMarkupValidator>();

            // V1
            services.AddTransient<ITemplateVersionedMarkupValidator, TemplateMarkupValidatorV1>();

            return services;
        }
    }
}

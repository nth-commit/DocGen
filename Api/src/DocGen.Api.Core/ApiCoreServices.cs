using DocGen.Api.Core.Templates;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApiCoreServices
    {
        public static IServiceCollection AddApiCoreServices(
            this IServiceCollection services)
        {
            services.AddTransient<TemplateService>();
            services.AddTransient<ITemplateRepository, TableStorageTemplateRepository>();
            services.AddTransient<ITemplateIdResolver, TemplateIdResolver>();

            return services;
        }
    }
}

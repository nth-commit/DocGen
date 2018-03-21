using Autofac;
using AutoMapper;
using DocGen.Web.Api.Core.Templates;
using DocGen.Templating.Validation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Web.Api.Core
{
    public class TestsBase : IDisposable
    {
        public TestsBase()
        {
            var services = new ServiceCollection();

            services.AddAutoMapper(conf =>
            {
                conf.AddApiCoreMappers();
            });

            services.AddApiCoreServices();
            services.AddTransient<ITemplateRepository, InMemoryTemplateRepository>();
            services.AddTransient<ITemplateValidator, AlwaysValidTemplateMarkupValidator>();

            LifetimeScope = services.BuildAutofacServiceProvider()
                .GetRequiredService<ILifetimeScope>()
                .BeginLifetimeScope();
        }

        protected ILifetimeScope LifetimeScope { get; private set; }

        protected IServiceProvider ServiceProvider => LifetimeScope.Resolve<IServiceProvider>();

        public void Dispose()
        {
            LifetimeScope.Dispose();
        }
    }
}

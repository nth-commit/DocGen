using AutoMapper;
using DocGen.Api.Core.Templates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DocGen.Tools.DataInitializer
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");
            var configuration = builder.Build();

            var services = new ServiceCollection();
            services.AddConfigurationProvider(configuration);
            services.AddWindowsAzureStorageServices();
            services.AddApiCoreServices();
            services.AddAutoMapper(conf =>
            {
                conf.AddApiCoreMappers();
            });

            var serviceProvider = services.BuildServiceProvider();
            var service = serviceProvider.GetRequiredService<ITemplateService>();
            service.CreateTemplateAsync(new Template()
            {
                Name = "Non Disclosure Agreement",
                Text = "Test"
            }).Wait();
        }
    }
}

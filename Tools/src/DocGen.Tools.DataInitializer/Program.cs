using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            services.AddLogging();

            services.AddConfigurationProvider(configuration);
            services.AddWindowsAzureStorageServices();
            services.AddApiCoreServices();
            services.AddAutoMapper(conf =>
            {
                conf.AddApiCoreMappers();
            });
            services.AddTemplatingValidationServices();

            services.AddTransient<IDataInitializer, TemplateDataInitializer>();

            var serviceProvider = services.BuildServiceProvider();
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            foreach (var dataIntializer in serviceProvider.GetRequiredService<IEnumerable<IDataInitializer>>())
            {
                dataIntializer.RunAsync(logger).GetAwaiter().GetResult();
            }
        }
    }
}

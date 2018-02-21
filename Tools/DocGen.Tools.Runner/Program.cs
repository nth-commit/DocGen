using AutoMapper;
using DocGen.Api.Core.Templates;
using DocGen.Shared.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DocGen.Tools.Runner
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

            try
            {
                service.CreateTemplateAsync(new Template()
                {
                    Name = "Non Disclosure Agreement",
                    Text = "Test",
                    StepGroups = new List<StepGroup>()
                {
                    new StepGroup()
                    {
                        Title = "Title 1",
                        Description = "Desc 1",
                        Steps = new List<Step>()
                        {
                            new Step()
                            {
                                Title = "Title 1.1",
                                Description = "Description 1.1"
                            }
                        }
                    }
                }
                }).GetAwaiter().GetResult();
            }
            catch (ClientModelValidationException ex)
            {
                ex.ModelErrors.ForEach(kvp =>
                {
                    kvp.Value.ForEach(error =>
                    {
                        Console.WriteLine($"{kvp.Key}: {error}");
                    });
                });
                Console.ReadLine();
            }
        }
    }
}

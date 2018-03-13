using AutoMapper;
using DocGen.Api.Core.Documents;
using DocGen.Api.Core.Templates;
using DocGen.Shared.Core.Dynamic;
using DocGen.Shared.Framework;
using DocGen.Shared.Validation;
using DocGen.Templating.Rendering;
using DocGen.Templating.Validation;
using DocGen.Templating.Validation.V1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoreLinq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
            services.AddTemplatingValidationServices();
            services.AddTemplatingRenderingServices();

            services.AddTransient<IRemoteIpAddressAccessor, StubbedRemoteIpAddressAccessor>();


            var serviceProvider = services.BuildServiceProvider();

            try
            {
                #region Create template
                //var service = serviceProvider.GetRequiredService<TemplateService>();
                //service.CreateTemplateAsync(new TemplateCreate()
                //{
                //    Name = "Non-Disclosure Agreement 2",
                //    Description = "Test",
                //    Markup = "x",
                //    MarkupVersion = 1,
                //    Steps = new List<TemplateStepCreate>()
                //    {
                //        new TemplateStepCreate()
                //        {
                //            Id = "title0",
                //            Name = "Title 0",
                //            Description = "Desc 0",
                //            Inputs = new List<TemplateStepInputCreate>()
                //            {
                //                new TemplateStepInputCreate()
                //                {
                //                    Type = TemplateStepInputType.Radio,
                //                    TypeData = new TemplateStepInputTypeData_Radio[]
                //                    {
                //                        new TemplateStepInputTypeData_Radio()
                //                        {
                //                            Name = "Person",
                //                            Value = "person"
                //                        },
                //                        new TemplateStepInputTypeData_Radio()
                //                        {
                //                            Name = "Company",
                //                            Value = "company"
                //                        }
                //                    },
                //                    Hint = "Enter some text yo!"
                //                }
                //            }
                //        },
                //        new TemplateStepCreate()
                //        {
                //            Id = "title1",
                //            Name = "Title 1",
                //            Description = "Desc 1",
                //            Inputs = new List<TemplateStepInputCreate>()
                //            {
                //                new TemplateStepInputCreate()
                //                {
                //                    Type = TemplateStepInputType.Text,
                //                    Hint = "Enter some text yo!"
                //                }
                //            }
                //        },
                //        new TemplateStepCreate()
                //        {
                //            Id = "title2",
                //            Name = "Title 2",
                //            Description = "Description 2",
                //            Inputs = new List<TemplateStepInputCreate>()
                //            {
                //                new TemplateStepInputCreate()
                //                {
                //                    Type = TemplateStepInputType.Checkbox,
                //                    Hint = "Some hint"
                //                }
                //            }
                //        },
                //        new TemplateStepCreate()
                //        {
                //            Id = "title3",
                //            Name = "Title 3",
                //            Description = "Description 3",
                //            ConditionType = TemplateComponentConditionType.EqualsPreviousInputValue,
                //            ConditionTypeData = ExpandoObjectFactory.CreateDynamic(new Dictionary<string, object>()
                //            {
                //                { "PreviousInputId", "title2" },
                //                { "PreviousInputValue", true }
                //            }),
                //            Inputs = new List<TemplateStepInputCreate>()
                //            {
                //                new TemplateStepInputCreate()
                //                {
                //                    Type = TemplateStepInputType.Text,
                //                    Hint = "Enter some text yo!"
                //                }
                //            }
                //        }
                //    }
                //},
                //dryRun: true).GetAwaiter().GetResult();
                #endregion

                #region Create document

                //var service = serviceProvider.GetRequiredService<DocumentService>();
                //var result = service.CreateDocumentAsync(
                //    new DocumentCreate()
                //    {
                //        TemplateId = "non-disclosure-agreement",
                //        TemplateVersion = 1,
                //        InputValues = new Dictionary<string, dynamic>()
                //        {
                //            { "organisation.name", "Automio Limited" },
                //            { "organisation.location", "New Plymouth" },
                //            { "organisation.description", "operates a carpet manufacturing factory in Stratford" },
                //            { "contractor.type", "company" },
                //            { "contractor.company.name", "Lava Lamps Limited" },
                //            { "contractor.company.location", "New Plymouth" },
                //            { "disclosure_reason", "To provide marketing services to the Organisation" },
                //            { "disclosure_access", true },
                //            { "disclosure_access.details.persons", "sub-contractors, board members" }
                //        }
                //    }).GetAwaiter().GetResult();

                //Console.WriteLine(result.Body);

                #endregion
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
            }

            #region Create template markup
            //try
            //    {
            //        var service1 = serviceProvider.GetRequiredService<ITemplateValidator>();
            //        service1.Validate(
            //            @"<document>
            //                <page>
            //                    <block if=""contractor.type = company"">
            //                        This is some conditionally displayed stuff.
            //                    </block>
            //                    <inline>Inline content</inline>
            //                    <data>contractor.company.name</data>
            //                </page>
            //            </document>",
            //            1,
            //            new List<ReferenceDefinition>()
            //            {
            //                ReferenceDefinition.StringFrom("contractor.type", new string[] { "company", "person", "god" }),
            //                ReferenceDefinition.String("contractor.company.name"),
            //                ReferenceDefinition.String("test")
            //            });
            //    }
            //    catch (InvalidTemplateSyntaxException ex)
            //    {
            //        ex.Errors.ForEach(e =>
            //        {
            //            var error = e.Message;
            //            if (e.LineNumber > -1)
            //            {
            //                error += $" {e.LineNumber}:{e.LinePosition}";
            //            }
            //            Console.WriteLine(error);
            //        });
            //        Console.ReadLine();
            //    }
            #endregion

            RenderTemplateAsync(serviceProvider).GetAwaiter().GetResult();

            Console.ReadLine();
        }

        private static async Task RenderTemplateAsync(IServiceProvider serviceProvider)
        {
            var templateRenderer = serviceProvider.GetRequiredService<IDocumentRenderer>();
            var result = await templateRenderer.RenderAsync<SerializableDocument>(
                @"<document>
                    <page>
                        List 1
                        <list>
                            <list-item>
                                <block if=""contractor.type = company"">
                                    This is a company.
                                </block>
                                <block if=""contractor.type = person"">
                                    This is a Person.
                                </block>
                                <inline>Inline content</inline>
                                <data>contractor.company.name</data>
                            </list-item>
                            <list-item>
                                <list>
                                    <list-item>A list item in a nested list.</list-item>
                                    <list-item>A second list item in a nested list.</list-item>
                                </list>
                            </list-item>
                            <list-item>
                                The title of a nested list which isn't the first child.
                                <list>
                                    <list-item>
                                        <block>Some text</block>
                                        A list item in a nested list
                                    </list-item>
                                </list>
                            </list-item>
                        </list>
                        Some stuff between this lists
                        <list start='continue'>
                            <list-item>
                                <block>Some text</block>
                                Some inline text
                            </list-item>
                            <list-item>
                                <list>
                                    <list-item>A list item in a nested list.</list-item>
                                    <list-item>A second list item in a nested list.</list-item>
                                </list>
                            </list-item>
                            <list-item>
                                The title of a nested list which isn't the first child.
                                <list>
                                    <list-item>
                                        <block>Some text</block>
                                        A list item in a nested list
                                    </list-item>
                                </list>
                            </list-item>
                        </list>
                    </page>
                </document>",
                1,
                new DocumentRenderModel()
                {
                    Items = new List<DocumentRenderModelItem>()
                    {
                        new DocumentRenderModelItem()
                        {
                            Reference = "contractor.type",
                            Value = "company"
                        },
                        new DocumentRenderModelItem()
                        {
                            Reference = "contractor.company.name",
                            Value = "Michael Fry-White LTD"
                        }
                    }
                });

            Console.WriteLine(JsonConvert.SerializeObject(result.Instructions, Formatting.Indented));
        }
    }
}

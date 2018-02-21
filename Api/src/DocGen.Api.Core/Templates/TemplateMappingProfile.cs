using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Api.Core.Templates
{
    public class TemplateMappingProfile : Profile
    {
        public TemplateMappingProfile()
        {
            CreateMap<Template, TemplateTableEntity>()
                .ForMember(dest => dest.RowKey, opts => opts.ResolveUsing(src => src.Name))
                .ForMember(dest => dest.PartitionKey, opts => opts.ResolveUsing(src => Regions.Constants.Names.NewZealand))
                .ForMember(dest => dest.DataJson, opts => opts.ResolveUsing(src => JsonConvert.SerializeObject(src)));
        }
    }
}

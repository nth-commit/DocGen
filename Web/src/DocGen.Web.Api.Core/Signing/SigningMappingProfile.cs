using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocGen.Web.Api.Core.Signing
{
    public class SigningMappingProfile : Profile
    {
        public SigningMappingProfile()
        {
            CreateMap<SigningRequest, SigningRequestTableEntity>()
                .ForMember(dest => dest.RowKey, opts => opts.ResolveUsing(src => src.Id))
                .ForMember(dest => dest.PartitionKey, opts => opts.ResolveUsing(src => Regions.Constants.Names.NewZealand))
                .ForMember(dest => dest.DataJson, opts => opts.ResolveUsing(src => JsonConvert.SerializeObject(src)));

            CreateMap<SigningRequestTableEntity, SigningRequest>()
                .ConvertUsing((src, dest) => JsonConvert.DeserializeObject<SigningRequest>(src.DataJson));
        }
    }
}

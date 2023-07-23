using AutoMapper;
using Microsoft.Build.Framework;
using PriceAggregator.Api.DTOs;
using PriceAggregator.Api.Models;
using System.Globalization;

namespace PriceAggregator.Api.Mapping
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Price, PriceDto>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Close.ToString("N", CultureInfo.CurrentCulture)))
                .ForMember(dest => dest.From, opt => opt.MapFrom(src => src.FromCurrency.ToString()))
                .ForMember(dest => dest.To, opt => opt.MapFrom(src => src.ToCurrency.ToString()))
                .ForMember(dest => dest.DateTime, opt => opt.MapFrom(src => src.StartDateTime));
               
        }
    }
}

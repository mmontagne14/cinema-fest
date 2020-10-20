using AutoMapper;
using CinemaFest.Application.Dtos;
using CinemaFest.Application.Features.Festival.Commands;
using CinemaFest.Domain.Entities;

namespace CinemaFest.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateFestivalCommand, Festival>();
            CreateMap<ContactDto, Contact>();
            CreateMap<AddressDto, Address>();
            CreateMap<LocalityDto, Locality>();
            CreateMap<ProvinceDto, Province>();
            CreateMap<CountryDto, Country>();
            CreateMap<LocationDto, Location>();
            CreateMap<ImageDto, FestivalImage>();
            CreateMap<UpdateFestivalCommand, Festival>();
            CreateMap<TaxonomyDto, Taxonomy>();
        }
    }
}

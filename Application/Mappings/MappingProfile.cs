using AutoMapper;
using CinemaFest.Application.Features.Festival.Commands;
using CinemaFest.Domain.Entities;

namespace CinemaFest.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateFestivalCommand, Festival>();
        }
    }
}

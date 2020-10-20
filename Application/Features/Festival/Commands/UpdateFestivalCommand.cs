namespace CinemaFest.Application.Features.Festival.Commands
{
    using CinemaFest.Application.Wrappers;
    using MediatR;
    using CinemaFest.Domain.Entities;
    using System.Threading.Tasks;
    using System.Threading;
    using CinemaFest.Application.Interfaces;
    using AutoMapper;
    using System;
    using CinemaFest.Application.Dtos;
    using System.Collections.Generic;

    public class UpdateFestivalCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string About { get; set; }
        public int FirstEditionYear { get; set; }
        public ContactDto Contact { get; set; }

        public ICollection<LocationDto> Locations { get; set; }

        public ICollection<ImageDto> Images { get; set; }

        public ICollection<TaxonomyDto> Taxonomies { get; set; }

        public class UpdateFestivalCommandHandler : IRequestHandler<UpdateFestivalCommand, Response<int>>
        {
            private readonly IUnitOfWork unitOfWork;
            private readonly IMapper mapper;
            private readonly IFileService fileService;

            public UpdateFestivalCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService)
            {
                this.unitOfWork = unitOfWork;
                this.mapper = mapper;
                this.fileService = fileService;
            }
            
            public async Task<Response<int>> Handle(UpdateFestivalCommand command, CancellationToken cancellationToken)
            {
                var festival = mapper.Map<Festival>(command);
                festival.ModifiedAt = DateTime.Now;
                festival.Images = fileService.GetBase64FromFestivalImagesStream(festival.Images);

                await unitOfWork.Festivals.UpdateFestivalWithImagesAndLocationsAsync(festival);
                unitOfWork.Commit();
                return new Response<int>(festival.Id);
            }
        }
    }
}

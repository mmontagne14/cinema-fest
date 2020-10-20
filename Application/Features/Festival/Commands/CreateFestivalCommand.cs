namespace CinemaFest.Application.Features.Festival.Commands
{
    using CinemaFest.Domain.Entities;
    using CinemaFest.Application.Interfaces;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;
    using CinemaFest.Application.Wrappers;
    using AutoMapper;
    using System;
    using System.IO;
    using CinemaFest.Application.Dtos;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using CinemaFest.Domain.Common;

    public class CreateFestivalCommand : IRequest<Response<int>>
    {
        public string Name { get; set; }
        public string About { get; set; }
        public int FirstEditionYear { get; set; }

        public ContactDto Contact { get; set; }

        public ICollection<LocationDto> Locations { get; set; }

        public ICollection<ImageDto> Images { get; set; }

        public ICollection<TaxonomyDto> Taxonomies { get; set; }
        //TODO change string to IFormFile and add it to Fluent Validator (size and format)

        public class CreateFestivalCommandHandler : IRequestHandler<CreateFestivalCommand, Response<int>>
        {
            private readonly IUnitOfWork unitOfwork;
            private readonly IMapper mapper;
            private readonly IFileService fileService;

            public CreateFestivalCommandHandler(IUnitOfWork unitOfwork, IMapper mapper, IFileService fileService)
            {
                this.unitOfwork = unitOfwork;
                this.mapper = mapper;
                this.fileService = fileService;
            }
            
            public async Task<Response<int>> Handle(CreateFestivalCommand command, CancellationToken cancellationToken)
            {
                var festival = mapper.Map<Festival>(command);
                festival.SetInitialProperties();

                festival.Images = fileService.GetBase64FromFestivalImagesStream(festival.Images);
                festival.Id = await unitOfwork.Festivals.CreateFestivalAsync(festival);
                unitOfwork.Commit();
                return new Response<int>(festival.Id);
            }

        }
    }
}

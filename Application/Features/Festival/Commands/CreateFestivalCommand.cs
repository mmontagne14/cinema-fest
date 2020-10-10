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

    public class CreateFestivalCommand : IRequest<Response<int>>
    {
        public string Name { get; set; }
        public string About { get; set; }
        public int FirstEditionYear { get; set; }

        public string ProfileImgFilePath { get; set; }
        public string CoverPageImgFilePath { get; set; }
        public ContactDto Contact { get; set; }

        public ICollection<AddressDto> Locations { get; set; }

        //TODO change string for IFormFile and add it to Fluent Validator (size and format)

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
            
            public async Task<Response<int>> Handle(CreateFestivalCommand request, CancellationToken cancellationToken)
            {
                var festival = mapper.Map<Festival>(request);
                festival.SetInitialProperties();

                festival.ProfileImg = request.ProfileImgFilePath != null ? fileService.GetBinaryFileFromStream(request.ProfileImgFilePath) : null;
                festival.CoverPageImg = request.CoverPageImgFilePath != null ? fileService.GetBinaryFileFromStream(request.CoverPageImgFilePath) : null;

                festival.Id = await unitOfwork.Festivals.AddFestivalAsync(festival);
                unitOfwork.Commit();
                return new Response<int>(festival.Id);
            }
        }
    }
}

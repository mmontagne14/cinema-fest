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

    public class CreateFestivalCommand : IRequest<Response<int>>
    {
        public string Name { get; set; }
        public string About { get; set; }
        public int FirstEditionYear { get; set; }

        public string ProfileImgFilePath { get; set; }
        public string CoverPageImgFilePath { get; set; }

        //TODO change string for IFormFile and add it to Fluent Validator (size and format)

        public class CreateFestivalCommandHandler : IRequestHandler<CreateFestivalCommand, Response<int>>
        {
            private readonly IUnitOfWork unitOfwork;
            private readonly IMapper mapper;

            public CreateFestivalCommandHandler(IUnitOfWork unitOfwork, IMapper mapper)
            {
                this.unitOfwork = unitOfwork;
                this.mapper = mapper;
            }
            
            public async Task<Response<int>> Handle(CreateFestivalCommand request, CancellationToken cancellationToken)
            {
                var festival = mapper.Map<Festival>(request);
                festival.CreatedAt = DateTime.Now;
                festival.ModifiedAt = DateTime.Now;
                festival.Active = true;

                festival.ProfileImg = GetBinaryFileFromStream(request.ProfileImgFilePath);
                festival.CoverPageImg = GetBinaryFileFromStream(request.CoverPageImgFilePath);

                festival.Id = await unitOfwork.Festivals.AddAsync(festival);
                unitOfwork.Commit();
                return new Response<int>(festival.Id);
            }


            private byte[] GetBinaryFileFromStream(string varFilePath)
            {
                byte[] file;
                using (var stream = new FileStream(varFilePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = new BinaryReader(stream))
                    {
                        file = reader.ReadBytes((int)stream.Length);
                    }
                }
                return file;
            }


        }
    }
}



namespace CinemaFest.Application.Features.Festival.Queries
{
    using AutoMapper;
    using CinemaFest.Application.Interfaces;
    using Domain.Entities;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetFestivalByIdQuery : IRequest<Festival> {
        public int Id { get; set; }

    }

    public class GetFestivalByIdQueryHandler : IRequestHandler<GetFestivalByIdQuery, Festival>
    {
        private readonly IUnitOfWork unitOfwork;
        private readonly IMapper mapper;

        public GetFestivalByIdQueryHandler(IUnitOfWork unitOfwork, IMapper mapper)
        {
            this.unitOfwork = unitOfwork;
            this.mapper = mapper;
        }

        public async Task<Festival> Handle(GetFestivalByIdQuery request, CancellationToken cancellationToken)
        {
            return await unitOfwork.Festivals.RetrieveFestivalWithImagesAndLocationsByIdAsync(request.Id);


        }
    }
}

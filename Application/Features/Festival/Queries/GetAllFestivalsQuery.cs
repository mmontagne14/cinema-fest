namespace CinemaFest.Application.Features.Festival.Queries
{
    using CinemaFest.Application.Interfaces;
    using CinemaFest.Domain.Entities;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetAllFestivalsQuery : IRequest<IEnumerable<Festival>> {
    }
    public class GetAllFestivalsQueryHandler : IRequestHandler<GetAllFestivalsQuery, IEnumerable<Festival>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAllFestivalsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        
        public async Task<IEnumerable<Festival>> Handle(GetAllFestivalsQuery request, CancellationToken cancellationToken)
        {
            return await unitOfWork.Festivals.GetAllAsync();
        }
    }
}

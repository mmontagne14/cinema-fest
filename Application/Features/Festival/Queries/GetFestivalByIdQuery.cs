

namespace CinemaFest.Application.Features.Festival.Queries
{
    using Domain.Entities;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetFestivalByIdQuery : IRequest<Festival> {
        public int Id { get; set; }

    }

    public class GetFestivalByIdQueryHandler : IRequestHandler<GetFestivalByIdQuery, Festival>
    {
        public async Task<Festival> Handle(GetFestivalByIdQuery request, CancellationToken cancellationToken)
        {
            return new Festival()

            { Name = "Canes Festival", About = "bla bla bla" };


        }
    }
}

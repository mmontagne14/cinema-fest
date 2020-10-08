

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

    public class UpdateFestivalCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string About { get; set; }
        public string Awards { get; set; }
        public int FirstEditionYear { get; set; }

        public bool Active { get; set; }

        public class UpdateFestivalCommandHandler : IRequestHandler<UpdateFestivalCommand, Response<int>>
        {
            private readonly IUnitOfWork unitOfWork;
            private readonly IMapper mapper;
 

            public UpdateFestivalCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                this.unitOfWork = unitOfWork;
                this.mapper = mapper;
            }
            
            public async Task<Response<int>> Handle(UpdateFestivalCommand command, CancellationToken cancellationToken)
            {
                var festival = await unitOfWork.Festivals.GetByIdAsync(command.Id);
                if(festival == null)
                    throw new Exception($"El festival no fue encontrado.");


                festival.Name = command.Name;
                festival.About = command.About;
                festival.Active = command.Active;

                await unitOfWork.Festivals.UpdateAsync(festival);
                return new Response<int>(festival.Id);
            }
        }
    }
}

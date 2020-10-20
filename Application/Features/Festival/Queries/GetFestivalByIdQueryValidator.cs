using CinemaFest.Application.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaFest.Application.Features.Festival.Queries
{
    public class GetFestivalByIdQueryValidator : AbstractValidator<GetFestivalByIdQuery>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetFestivalByIdQueryValidator(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;

            RuleFor(f => f.Id)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                    .Must(Id =>
                    {
                        return unitOfWork.Festivals.FestivalExistsById(Id);
                    })
                .WithMessage("*No existe el registro.");

        }



    }
}

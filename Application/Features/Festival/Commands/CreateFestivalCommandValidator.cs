using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaFest.Application.Features.Festival.Commands
{
    public class CreateFestivalCommandValidator : AbstractValidator<CreateFestivalCommand>
    {
        public CreateFestivalCommandValidator()
        {
            RuleFor(f => f.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("*Requerido")
                .MaximumLength(100);

            RuleFor(f => f.About)
                .NotEmpty().WithMessage("*Requerido")
                .MaximumLength(2500);

            RuleFor(f => f.FirstEditionYear)
                .NotEmpty().WithMessage("*Requerido")
                .LessThan(DateTime.Now.Year + 1);

        }
    }
}

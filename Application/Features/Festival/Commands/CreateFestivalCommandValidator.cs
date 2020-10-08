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
                .NotEmpty()
                .NotNull()
                .MaximumLength(50);

            RuleFor(f => f.About)
                .MaximumLength(250);

            RuleFor(f => f.FirstEditionYear)
                .NotEmpty();

        }
    }
}

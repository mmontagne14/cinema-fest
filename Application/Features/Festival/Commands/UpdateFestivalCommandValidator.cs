using CinemaFest.Application.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaFest.Application.Features.Festival.Commands
{
        public class UpdateFestivalCommandValidator : AbstractValidator<UpdateFestivalCommand>
        {
        private readonly IUnitOfWork unitOfWork;

        public UpdateFestivalCommandValidator(IUnitOfWork unitOfWork)
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

            RuleFor(f => f.Name)
                    .NotEmpty()//.WithMessage("*Requerido")
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

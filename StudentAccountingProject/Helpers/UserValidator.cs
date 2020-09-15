using FluentValidation;
using StudentAccountingProject.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAccountingProject.Helpers
{
    public class UserValidator : AbstractValidator<BaseProfile>
    {
        public UserValidator()
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().NotEmpty().WithMessage("{PropertyName} cannot be empty")
                .Length(2,20).WithMessage("{PropertyName} lenght must be greater than 2 and less than 20")
                .Matches(@"^[a-zA-Z-']*$").WithMessage("{PropertyName} incorrect");

            RuleFor(x => x.Surname)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotNull().NotEmpty().WithMessage("{PropertyName} cannot be empty")
               .Length(2, 20).WithMessage("{PropertyName} lenght must be greater than 2 and less than 20")
               .Matches(@"^[a-zA-Z-']*$").WithMessage("{PropertyName} incorrect");

            RuleFor(x => x.User.Email)
              .Cascade(CascadeMode.StopOnFirstFailure)
              .EmailAddress().WithMessage("{PropertyName} incorrect");

            RuleFor(x => x.Age)
              .Cascade(CascadeMode.StopOnFirstFailure)
              .InclusiveBetween("1","122").WithMessage("{PropertyName} incorrect");
        }
    }
}

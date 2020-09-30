using FluentValidation;
using StudentAccountingProject.MediatR.Course.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAccountingProject.Helpers
{
    public class AddingCourseValidation : AbstractValidator<NewCourseModel>
    {
        public AddingCourseValidation()
        {
            RuleFor(x => x.Name)
               .Cascade(CascadeMode.StopOnFirstFailure)
               .NotEmpty().WithMessage("{PropertyName} incorrect");

            RuleFor(x => x.Description)
              .Cascade(CascadeMode.StopOnFirstFailure)
              .NotEmpty().WithMessage("{PropertyName} incorrect");

            RuleFor(x => x.AuthorId)
              .Cascade(CascadeMode.StopOnFirstFailure)
              .NotEmpty().WithMessage("{PropertyName} incorrect");

            RuleFor(x => x.DateOfStart)
              .Cascade(CascadeMode.StopOnFirstFailure)
              .NotEmpty()
              .WithMessage("{PropertyName} incorrect");
            
            RuleFor(x => x.DateOfEnd)
              .Cascade(CascadeMode.StopOnFirstFailure)
              .NotEmpty().WithMessage("{PropertyName} incorrect");
        }
    }
}

using FluentValidation;
using StudentAccountingProject.MediatR.Student.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAccountingProject.Helpers
{
    public class GettingStudentsValidation : AbstractValidator<PagginationModel>
    {
        public GettingStudentsValidation(int studentsCount)
        {
            RuleFor(x => x.PageSize)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .GreaterThan(-1).WithMessage("Rows incorrect");

            RuleFor(x => x.CurrentPage)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .GreaterThan(0).WithMessage("Current page incorrect");
                //.LessThan(studentsCount + 1).WithMessage("{PropertyName} more than students count");
        }
    }
}

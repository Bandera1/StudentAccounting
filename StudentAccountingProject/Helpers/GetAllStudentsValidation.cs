using FluentValidation;
using StudentAccountingProject.MediatR.Student.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAccountingProject.Helpers
{
    public class GetAllStudentsValidation : AbstractValidator<GetAllStudentsDTO>
    {
        public GetAllStudentsValidation(int studentsCount)
        {
            RuleFor(x => x.From)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .GreaterThan(-1)
                .LessThan(x => x.To).WithMessage("{PropertyName} incorrect")
                .LessThan(studentsCount).WithMessage("{PropertyName} more than students count");
            
            RuleFor(x => x.To)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .GreaterThan(0).WithMessage("{PropertyName} incorrect")
                .LessThan(studentsCount+1).WithMessage("{PropertyName} more than students count");
        }
    }
}

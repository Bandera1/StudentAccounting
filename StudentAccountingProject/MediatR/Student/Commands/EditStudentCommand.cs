using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentAccountingProject.DB;
using StudentAccountingProject.DB.Entities;
using StudentAccountingProject.DB.Helpers;
using StudentAccountingProject.DB.IdentityModels;
using StudentAccountingProject.Helpers;
using StudentAccountingProject.MediatR.Student.DTO;
using StudentAccountingProject.MediatR.Student.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StudentAccountingProject.MediatR.Student.Commands
{
    public class EditStudentCommand : IRequest<BaseViewModel>
    {
        public EditStudentDTO DTO { get; set; }

        public class EditStudentCommandHandler : BaseMediator,IRequestHandler<EditStudentCommand, BaseViewModel>
        {
            public EditStudentCommandHandler(EFDbContext context) : base(context)
            {
            }

            public async Task<BaseViewModel> Handle(EditStudentCommand request, CancellationToken cancellationToken)
            {
                var student = Context.Users
                    .Include(x => x.BaseProfile)
                    .Where(x => x.BaseProfile.StudentProfile != null)
                    .FirstOrDefault(x => x.Id == request.DTO.Id);
                if (student == null || student.BaseProfile.IsDeleted)
                {
                    return new BaseViewModel
                    {
                        Status = false,
                        ErrorMessage = "Student doesn`t exist"
                    };
                }

                var newUser = new BaseProfile
                {
                    Name = request.DTO.Name,
                    Surname = request.DTO.Surname,
                    Age = request.DTO.Age,
                    User = new DbUser
                    {
                        Email = request.DTO.Email
                    }
                };
                var userValidator = new UserValidator();
                var validationResult = userValidator.Validate(newUser);
                if (validationResult.Errors.Count > 0)
                {
                    return new BaseViewModel
                    {
                        Status = false,
                        ErrorMessage = validationResult.Errors.First().ErrorMessage
                    };
                }

                student.BaseProfile.Name = newUser.Name;
                student.BaseProfile.Surname = newUser.Surname;
                student.BaseProfile.Age = newUser.Age;
                student.Email = newUser.User.Email;
                await Context.SaveChangesAsync();

                return new BaseViewModel { Status = true };
            }
        }

    }
}

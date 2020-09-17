using MediatR;
using Microsoft.AspNetCore.Identity;
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
    public class CreateStudentCommand : IRequest<BaseViewModel>
    {
        public CreateStudentDTO DTO { get; set; }

        public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, BaseViewModel>
        {
            private readonly EFDbContext Context;
            private readonly UserManager<DbUser> UserManager;

            public CreateStudentCommandHandler(EFDbContext context, UserManager<DbUser> userManager)
            {
                UserManager = userManager;
                Context = context;
            }
            
            public async Task<BaseViewModel> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
            {
                var roleName = ProjectRoles.STUDENT;
                var userRegisterister = Context.Users.FirstOrDefault(x => x.Email == request.DTO.Email);
                if (userRegisterister != null)
                {
                    return new BaseViewModel
                    {
                        Status = false,
                        ErrorMessage = ("Email alredy exist")
                    };
                }

                var student = new StudentProfile();
                var baseProfile = new BaseProfile
                {
                    Name = request.DTO.Name,
                    Surname = request.DTO.Surname,
                    RegisterDate = DateTime.Now,
                    Age = request.DTO.Age,
                    StudentProfile = student,
                    IsDeleted = false,
                };
                var dbClient = new DbUser
                {
                    Email = request.DTO.Email,
                    UserName = request.DTO.Email,
                    BaseProfile = baseProfile
                };
                baseProfile.UserId = dbClient.Id;
                baseProfile.User = dbClient;

                var userValidator = new UserValidator();
                var validationResult = userValidator.Validate(baseProfile);
                if (validationResult.Errors.Count > 0)
                {
                    return new BaseViewModel
                    {
                        Status = false,
                        ErrorMessage = validationResult.Errors.First().ErrorMessage
                    };
                }


                var result = await UserManager.CreateAsync(dbClient, "QWerty-1");
                if (!result.Succeeded)
                {
                    return new BaseViewModel
                    {
                        Status = false,
                        ErrorMessage = ("The password must contain 8 characters, at least one capital letter")
                    };
                }
                
                result = await UserManager.AddToRoleAsync(dbClient, roleName);
                if (result.Succeeded) return new BaseViewModel { Status = true };

                return new BaseViewModel { Status = false, ErrorMessage = "Error" };
            }
        }
    }
}

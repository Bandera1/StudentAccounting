using MediatR;
using Microsoft.AspNetCore.Identity;
using StudentAccountingProject.DB;
using StudentAccountingProject.DB.Entities;
using StudentAccountingProject.DB.IdentityModels;
using StudentAccountingProject.Helpers;
using StudentAccountingProject.MediatR.Account.DTO;
using StudentAccountingProject.MediatR.Account.ViewModels;
using StudentAccountingProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace StudentAccountingProject.MediatR.Account.Commands
{
    public class RegistrationCommand : IRequest<RegistrationViewModel>
    {
        public RegisterDTO RegisterDTO { get; set; }

        public class RegistrationCommandHandler : IRequestHandler<RegistrationCommand, RegistrationViewModel>
        {
            private readonly EFDbContext Context;
            private readonly UserManager<DbUser> UserManager;
            private readonly SignInManager<DbUser> SignInManager;
            private readonly IJwtTokenService IJwtTokenService;

            public RegistrationCommandHandler(EFDbContext context, UserManager<DbUser> userManager, SignInManager<DbUser> signInManager, IJwtTokenService _IJwtTokenService)
            {
                UserManager = userManager;
                Context = context;
                SignInManager = signInManager;
                IJwtTokenService = _IJwtTokenService;
            }

            public async Task<RegistrationViewModel> Handle(RegistrationCommand request, CancellationToken cancellationToken)
            {
                var roleName = ProjectRoles.STUDENT;
                var userRegisterister = Context.Users.FirstOrDefault(x => x.Email == request.RegisterDTO.Email);
                if (userRegisterister != null)
                {
                    return new RegistrationViewModel 
                    { 
                        Status = false,
                        ErrorMessage = ("Email alredy exist")
                    };
                }             

                var student = new StudentProfile();
                var baseProfile = new BaseProfile
                {
                    Name = request.RegisterDTO.Name,
                    Surname = request.RegisterDTO.Surname,
                    RegisterDate = DateTime.Now,
                    Age = request.RegisterDTO.Age,
                    StudentProfile = student,
                    IsDeleted = false,
                    IsFacebookAccount = request.RegisterDTO.Password.Equals("FacebookPassword88-") ? true : false
                };
                var dbClient = new DbUser
                {
                    Email = request.RegisterDTO.Email,
                    UserName = request.RegisterDTO.Email,
                    BaseProfile = baseProfile,
                    EmailConfirmed = request.RegisterDTO.Password.Equals("FacebookPassword88-") ? true : false
                };
                baseProfile.UserId = dbClient.Id;
                baseProfile.User = dbClient;

                var userValidator = new UserValidator();
                var validationResult = userValidator.Validate(baseProfile);
                if(validationResult.Errors.Count > 0)
                {
                    return new RegistrationViewModel
                    {
                        Status = false,
                        ErrorMessage = validationResult.Errors.First().ErrorMessage
                    };
                }


                var result = await UserManager.CreateAsync(dbClient, request.RegisterDTO.Password);
                if (!result.Succeeded)
                {
                    return new RegistrationViewModel 
                    { 
                        Status = false, 
                        ErrorMessage = ("The password must contain 8 characters, at least one capital letter")
                    };
                }
                result = await UserManager.AddToRoleAsync(dbClient, roleName);

                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(dbClient, isPersistent: false);
                    return new RegistrationViewModel 
                    { 
                        Status = true, 
                        Token = await IJwtTokenService.CreateToken(dbClient) 
                    };
                }
                return new RegistrationViewModel { Status = false, ErrorMessage = "Error" };
            }       
        }
    }
}

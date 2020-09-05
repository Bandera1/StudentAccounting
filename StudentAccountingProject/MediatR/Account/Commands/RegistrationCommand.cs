using MediatR;
using Microsoft.AspNetCore.Identity;
using StudentAccountingProject.DB;
using StudentAccountingProject.DB.Entities;
using StudentAccountingProject.DB.IdentityModels;
using StudentAccountingProject.Helpers;
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
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }

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
                var userRegisterister = Context.Users.FirstOrDefault(x => x.Email == request.Email);
                if (userRegisterister != null)
                {
                    return new RegistrationViewModel { Status = false, ErrorMessage = ("Email alredy exist") };
                }

                if (string.IsNullOrEmpty(request.Name))
                {
                    return new RegistrationViewModel { Status = false, ErrorMessage = ("Name cannot be empty") };
                }
                else
                {
                    var NameRegex = new Regex(@"[aA-Z-z0-9._()\[\]-]{3,15}$");

                    if (!NameRegex.IsMatch(request.Name))
                    {
                        return new RegistrationViewModel { Status = false, ErrorMessage = ("The name must be at least 3 characters long") };
                    }
                }

                if (string.IsNullOrEmpty(request.Surname))
                {
                    return new RegistrationViewModel { Status = false, ErrorMessage = ("Surname cannot be empty") };
                }
                else
                {
                    var nameANDsurnameREGEX = new Regex(@"[A-Za-z0-9._()\[\]-]{3,15}$");
                    if (!nameANDsurnameREGEX.IsMatch(request.Surname))
                    {
                        return new RegistrationViewModel { Status = false, ErrorMessage = ("The surname must be at least 3 characters long") };
                    }
                }
                if (string.IsNullOrEmpty(request.Email))
                {
                    return new RegistrationViewModel { Status = false, ErrorMessage = ("Вкажіть пошту.") };
                }
                else
                {
                    var testmail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                    if (!testmail.IsMatch(request.Email))
                    {
                        return new RegistrationViewModel { Status = false, ErrorMessage = ("Невірно вказана почта.") };
                    }
                }

                StudentProfile student = new StudentProfile();
                BaseProfile baseProfile = new BaseProfile
                {
                    Name = request.Name,
                    Surname = request.Surname,
                    StudentProfile = student
                };

                var dbClient = new DbUser
                {
                    Email = request.Email,
                    UserName = request.Email,
                    BaseProfile = baseProfile
                };

                var res = UserManager.CreateAsync(dbClient, request.Password).Result;
                if (!res.Succeeded)
                {
                    return new RegistrationViewModel { Status = false, ErrorMessage = ("The password must contain 8 characters, at least one capital letter") };
                }
                res = await UserManager.AddToRoleAsync(dbClient, roleName);

                if (res.Succeeded)
                {
                    await SignInManager.SignInAsync(dbClient, isPersistent: false);
                    return new RegistrationViewModel { Status = true, Token = await IJwtTokenService.CreateToken(dbClient) };
                }
                return new RegistrationViewModel { Status = false, ErrorMessage = string.Empty };
            }       
        }
    }
}

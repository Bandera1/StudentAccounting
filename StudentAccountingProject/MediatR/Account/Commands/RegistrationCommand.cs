﻿using MediatR;
using Microsoft.AspNetCore.Identity;
using StudentAccountingProject.DB;
using StudentAccountingProject.DB.Entities;
using StudentAccountingProject.DB.IdentityModels;
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
            private readonly EFDbContext _context;
            private readonly UserManager<DbUser> _userManager;
            private readonly SignInManager<DbUser> _signInManager;
            private readonly IJwtTokenService _IJwtTokenService;

            public RegistrationCommandHandler(EFDbContext context, UserManager<DbUser> userManager, SignInManager<DbUser> signInManager, IJwtTokenService IJwtTokenService)
            {
                _userManager = userManager;
                _context = context;
                _signInManager = signInManager;
                _IJwtTokenService = IJwtTokenService;
            }

            public async Task<RegistrationViewModel> Handle(RegistrationCommand request, CancellationToken cancellationToken)
            {
                var roleName = "Student";
                var userReg = _context.Users.FirstOrDefault(x => x.Email == request.Email);
                if (userReg != null)
                {
                    return new RegistrationViewModel { Status = false, ErrorMessage = ("Цей емейл вже зареєстровано.") };
                }

                if (request.Name == null)
                {
                    return new RegistrationViewModel { Status = false, ErrorMessage = ("Поле вводу імені пусте.") };
                }
                else
                {
                    var nameANDsurnameREGEX = new Regex(@"[A-Za-z0-9._()\[\]-]{3,15}$");

                    if (!nameANDsurnameREGEX.IsMatch(request.Name))
                    {
                        return new RegistrationViewModel { Status = false, ErrorMessage = ("Імя має мати мінімум 3 символи максимум 15 символів англійською мовою.") };
                    }
                }

                if (request.Surname == null)
                {
                    return new RegistrationViewModel { Status = false, ErrorMessage = ("Поле вводу прізвища пусте.") };
                }
                else
                {
                    var nameANDsurnameREGEX = new Regex(@"[A-Za-z0-9._()\[\]-]{3,15}$");
                    if (!nameANDsurnameREGEX.IsMatch(request.Surname))
                    {
                        return new RegistrationViewModel { Status = false, ErrorMessage = ("Прізвище має мати мінімум 3 символи максимум 15 символів англійською мовою.") };
                    }
                }
                if (request.Email == null)
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
             


                DbUser dbClient = new DbUser
                {
                    Email = request.Email,
                    UserName = request.Email,
                    BaseProfile = baseProfile
                };


                var res = _userManager.CreateAsync(dbClient, request.Password).Result;
                if (!res.Succeeded)
                {
                    return new RegistrationViewModel { Status = false, ErrorMessage = ("Код доступу має складатись з 8 символів, містити мінімум одну велику літеру! ") };
                }
                res = _userManager.AddToRoleAsync(dbClient, roleName).Result;

                if (res.Succeeded)
                {
                    //_context.StudentProfiles.Add(student);
                    //_context.SaveChanges();
                    await _signInManager.SignInAsync(dbClient, isPersistent: false);
                    return new RegistrationViewModel { Status = true, Token = _IJwtTokenService.CreateToken(dbClient) };
                }
                return new RegistrationViewModel { Status = false, ErrorMessage = ("") };
            }       
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentAccountingProject.DB;
using StudentAccountingProject.DB.IdentityModels;
using StudentAccountingProject.MediatR.Account.DTO;
using StudentAccountingProject.MediatR.Account.ViewModels;
using StudentAccountingProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StudentAccountingProject.MediatR.Account.Commands
{
    public class LoginCommand : IRequest<LoginViewModel>
    {
        public LoginDTO LoginDTO { get; set; }

        public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginViewModel>
        {
            private readonly EFDbContext Context;
            private readonly UserManager<DbUser> UserManager;
            private readonly SignInManager<DbUser> SignInManager;
            private readonly IJwtTokenService IJwtTokenService;

            public LoginCommandHandler(EFDbContext context, UserManager<DbUser> userManager, SignInManager<DbUser> signInManager, IJwtTokenService _IJwtTokenService)
            {
                UserManager = userManager;
                Context = context;
                SignInManager = signInManager;
                IJwtTokenService = _IJwtTokenService;
            }

            public async Task<LoginViewModel> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                var user = Context.Users.FirstOrDefault(x => x.Email == request.LoginDTO.Email);
                if (user == null)
                {
                    return new LoginViewModel
                    {
                        Status = false,
                        ErrorMessage = "Email or password is incorrect"
                    };
                }

                var result = await SignInManager
                .PasswordSignInAsync(user, request.LoginDTO.Password, false, false);
                if (!result.Succeeded)
                {
                    return new LoginViewModel
                    {
                        Status = false,
                        ErrorMessage = "Email or password is incorrect"
                    };
                }


                var token = await IJwtTokenService.CreateToken(user);
                await SignInManager.SignInAsync(user, isPersistent: false);
                return new LoginViewModel
                {
                    Status = true,
                    Token = token
                };
            }
        }
    }
}

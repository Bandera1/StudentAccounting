using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentAccountingProject.DB;
using StudentAccountingProject.DB.IdentityModels;
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
        public string Email { get; set; }
        public string Password { get; set; }


        public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginViewModel>
        {
            private readonly EFDbContext _context;
            private readonly UserManager<DbUser> _userManager;
            private readonly SignInManager<DbUser> _signInManager;
            private readonly IJwtTokenService _IJwtTokenService;

            public LoginCommandHandler(EFDbContext context, UserManager<DbUser> userManager, SignInManager<DbUser> signInManager, IJwtTokenService IJwtTokenService)
            {
                _userManager = userManager;
                _context = context;
                _signInManager = signInManager;
                _IJwtTokenService = IJwtTokenService;
            }

            public async Task<LoginViewModel> Handle(LoginCommand request, CancellationToken cancellationToken)
            {

                var user = _context.Users.FirstOrDefault(x => x.Email == request.Email);
                if (user == null)
                {
                    return new LoginViewModel
                    {
                        Status = false,
                        ErrorMessage = "Даний користувач не знайденний"
                    };
                }

                var token = await _IJwtTokenService.CreateToken(user);
                await _signInManager.SignInAsync(user, isPersistent: false);
                return new LoginViewModel
                {
                    Status = true,
                    Token = token
                };
            }
        }

    }
}

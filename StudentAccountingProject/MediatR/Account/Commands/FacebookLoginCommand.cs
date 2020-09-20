using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NLog.Fluent;
using StudentAccountingProject.DB;
using StudentAccountingProject.DB.Helpers;
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
    public class FacebookLoginCommand : IRequest<LoginViewModel>
    {
        public FacebookLoginDTO DTO { get; set; }

        public class FacebookLoginCommandHandler : IRequestHandler<FacebookLoginCommand, LoginViewModel>
        {
            private readonly EFDbContext Context;
            private IMediator Mediator;
            private readonly SignInManager<DbUser> SignInManager;
            private readonly IJwtTokenService IJwtTokenService;

            public FacebookLoginCommandHandler(EFDbContext context, IMediator mediator, SignInManager<DbUser> signInManager, IJwtTokenService _IJwtTokenService)
            {
                Mediator = mediator;
                Context = context;
                SignInManager = signInManager;
                IJwtTokenService = _IJwtTokenService;
            }

            public async Task<LoginViewModel> Handle(FacebookLoginCommand request, CancellationToken cancellationToken)
            {
                var searchStudent = Context.Users.FirstOrDefault(x => x.Email == request.DTO.Email);
                if (searchStudent == null)
                {
                    var registerResult = await Mediator.Send(new RegistrationCommand
                    {
                        RegisterDTO = new RegisterDTO
                        {
                            Name = request.DTO.Name,
                            Surname = request.DTO.Surname,
                            Email = request.DTO.Email,
                            Age = "1",
                            Password = "FacebookPassword88-"
                        }
                    });

                    return new LoginViewModel
                    {
                        Status = registerResult.Status,
                        ErrorMessage = registerResult.ErrorMessage,
                        Token = registerResult.Token
                    };
                }

                var token = await IJwtTokenService.CreateToken(searchStudent);
                await SignInManager.SignInAsync(searchStudent, isPersistent: false);
                return new LoginViewModel
                {
                    Status = true,
                    Token = token
                };
            }
        }
    }
}

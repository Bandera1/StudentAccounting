using MediatR;
using Microsoft.AspNetCore.Identity;
using StudentAccountingProject.DB;
using StudentAccountingProject.DB.Helpers;
using StudentAccountingProject.DB.IdentityModels;
using StudentAccountingProject.MediatR.Account.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StudentAccountingProject.MediatR.Account.Commands
{
    public class ConfirmEmailCommand : IRequest<bool>
    {
        public ConfirmEmailDTO DTO { get; set; }

        public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, bool>
        {
            private readonly UserManager<DbUser> UserManager;

            public ConfirmEmailCommandHandler (UserManager<DbUser> userManager)
            {
                UserManager = userManager;
            }

            public async Task<bool> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
            {
                var user = await UserManager.FindByIdAsync(request.DTO.UserId);

                user.EmailConfirmed = true;
                await UserManager.UpdateAsync(user);

                return true;
            }
        }
    }
}

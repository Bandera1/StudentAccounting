using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentAccountingProject.DB;
using StudentAccountingProject.DB.Helpers;
using StudentAccountingProject.DB.IdentityModels;
using StudentAccountingProject.MediatR.Student.DTO;
using StudentAccountingProject.MediatR.Student.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StudentAccountingProject.MediatR.Student.Commands
{
    public class ChangePasswordCommand : IRequest<BaseViewModel>
    {
        public NewPasswordModel Model { get; set; }

        public class ChangePasswordCommandHandler : BaseMediator, IRequestHandler<ChangePasswordCommand, BaseViewModel>
        {
            private readonly UserManager<DbUser> UserManager;

            public ChangePasswordCommandHandler(EFDbContext context, UserManager<DbUser> userManager) : base(context)
            {
                UserManager = userManager;
            }

            public async Task<BaseViewModel> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
            {
                var student = await UserManager.FindByIdAsync(request.Model.StudentId);
                var baseProfile = Context.BaseProfiles.FirstOrDefault(x => !x.IsDeleted && x.Id == student.Id);

                if (student == null || baseProfile == null || baseProfile.IsDeleted)
                {
                    new BaseViewModel
                    {
                        ErrorMessage = "Student doesn`t exist"
                    };
                }

                var changeResult = await UserManager.ChangePasswordAsync(student, request.Model.CurrentPassword, request.Model.NewPassword);
                if (!changeResult.Succeeded)
                {
                    return new BaseViewModel
                    {
                        ErrorMessage = "The password must contain 8 characters, at least one capital letter"
                    };
                }

                return new BaseViewModel();
            }
        }
    }
}

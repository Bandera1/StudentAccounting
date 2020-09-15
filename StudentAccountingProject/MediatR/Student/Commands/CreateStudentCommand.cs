using MediatR;
using Microsoft.AspNetCore.Identity;
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
    public class CreateStudentCommand : IRequest<BaseViewModel>
    {
        public CreateStudentDTO DTO { get; set; }

        public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, BaseViewModel>
        {
            private readonly EFDbContext Context;
            private readonly UserManager<DbUser> UserManager;
            private readonly SignInManager<DbUser> SignInManager;

            public CreateStudentCommandHandler(EFDbContext context, UserManager<DbUser> userManager, SignInManager<DbUser> signInManager)
            {
                UserManager = userManager;
                Context = context;
                SignInManager = signInManager;
            }
            

            public async Task<BaseViewModel> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}

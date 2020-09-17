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
    public class DeleteStudentCommand : IRequest<BaseViewModel>
    {
        public DeleteStudentDTO DTO { get; set; }

        public class DeleteStudentCommandHandler : BaseMediator,IRequestHandler<DeleteStudentCommand, BaseViewModel>
        {
            private readonly UserManager<DbUser> UserManager;

            public DeleteStudentCommandHandler(EFDbContext context, UserManager<DbUser> userManager) : base(context)
            {
                UserManager = userManager;
            }

            public async Task<BaseViewModel> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
            {
                if (request.DTO.students.Count.Equals(0)) return new BaseViewModel { Status = false, ErrorMessage = "Empty students list" };

                foreach (var student in request.DTO.students)
                {
                    if (Context.BaseProfiles
                        .Where(x => x.StudentProfile != null && !x.IsDeleted)
                        .FirstOrDefault(s => s.Id == student.StudentId) == null)
                    {
                        return new BaseViewModel
                        {
                            Status = false,
                            ErrorMessage = $"{student.StudentId} doesn`t exist"
                        };
                    }

                    var deletedUser = Context.Users
                        .Include(x => x.BaseProfile.StudentProfile)
                        .FirstOrDefault(x => x.Id == student.StudentId);

                    deletedUser.BaseProfile.IsDeleted = true;
                    Context.Courses
                        .Where(x => x.AuthorId == deletedUser.BaseProfile.Id)
                        .ToList()
                        .ForEach(x => x.IsDeleted = true);
                    Context.StudentsToCourses
                        .Where(x => x.CourseId == deletedUser.BaseProfile.Id)
                        .ToList()
                        .ForEach(x => x.Course.IsDeleted = true);
                    Context.SaveChanges();
                }

                return new BaseViewModel { Status = true };
            }
        }
    }
}

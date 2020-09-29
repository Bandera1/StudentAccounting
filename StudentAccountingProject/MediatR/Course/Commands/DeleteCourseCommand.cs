using MediatR;
using StudentAccountingProject.DB;
using StudentAccountingProject.DB.Helpers;
using StudentAccountingProject.MediatR.Course.DTO;
using StudentAccountingProject.MediatR.Student.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StudentAccountingProject.MediatR.Course.Commands
{
    public class DeleteCourseCommand : IRequest<BaseViewModel>
    {
        public DelCourseModel Model { get; set; }

        public class DeleteCourseCommandHandler : BaseMediator, IRequestHandler<DeleteCourseCommand, BaseViewModel>
        {
            public DeleteCourseCommandHandler(EFDbContext context) : base(context)
            {
            }

            public Task<BaseViewModel> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
            {
                if (request.Model.Courses.Count.Equals(0))
                {
                    return Task.FromResult(new BaseViewModel
                    { 
                        Status = false, ErrorMessage = "Empty students list" 
                    });
                }

                DB.Entities.Course realCourse = null;
                foreach (var course in request.Model.Courses)
                {
                    realCourse = Context.Courses.FirstOrDefault(x => !x.IsDeleted && x.Id.Equals(course.CourseId));
                    if (realCourse == null)
                    {
                        return Task.FromResult(new BaseViewModel 
                        {
                            ErrorMessage = $"{course.CourseId} doesn`t exist" 
                        });
                    }
                    realCourse.IsDeleted = true;
                }
                Context.SaveChanges();

                return Task.FromResult(new BaseViewModel());
            }
        }
    }
}

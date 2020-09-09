using MediatR;
using StudentAccountingProject.DB;
using StudentAccountingProject.DB.Helpers;
using StudentAccountingProject.MediatR.Course.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StudentAccountingProject.MediatR.Course.Queries
{
    public class GetAllCourseQuery : IRequest<ICollection<CourseViewModel>>
    {
        public class GetAllCourseQueryHandler : BaseMediator, IRequestHandler<GetAllCourseQuery, ICollection<CourseViewModel>>
        {
            public GetAllCourseQueryHandler(EFDbContext context) : base(context)
            {
            }

            public async Task<ICollection<CourseViewModel>> Handle(GetAllCourseQuery request, CancellationToken cancellationToken)
            {
                var courses = Context.Courses
                 .Select(x => new CourseViewModel
                 {
                     Id = x.Id,
                     Author = x.Author.Name + " " + x.Author.Surname,
                     Name = x.Name,
                     Description = x.Description,
                     DateOfStart = x.DateOfStart.ToString("dd MMMM yyyy"),
                     DateOfEnd = x.DateOfEnd.ToString("dd MMMM yyyy"),
                     Rating = Convert.ToInt32(x.Rating)
                 }).ToList();

                return courses;
            }
        }

    }
}

using MediatR;
using StudentAccountingProject.DB;
using StudentAccountingProject.DB.Helpers;
using StudentAccountingProject.MediatR.Course.DTO;
using StudentAccountingProject.MediatR.Course.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace StudentAccountingProject.MediatR.Course.Queries
{
    public class GetAllCourseQuery : IRequest<ICollection<CourseViewModel>>
    {
        public GetAllCoursesDTO DTO { get; set; }

        public class GetAllCourseQueryHandler : BaseMediator, IRequestHandler<GetAllCourseQuery, ICollection<CourseViewModel>>
        {
            public GetAllCourseQueryHandler(EFDbContext context) : base(context)
            {
            }

            public async Task<ICollection<CourseViewModel>> Handle(GetAllCourseQuery request, CancellationToken cancellationToken)
            {
                var courses = Context.Courses
                   .Where(x => !x.IsDeleted)
                 //.Where(x => Context.StudentsToCourses.Where(b => b.CourseId == x.Id)
                 //.Where(z => z.StudentId == request.DTO.StudentId).Count() == 0)
                 .Select(x => new CourseViewModel
                 {
                     Id = x.Id,
                     Author = x.Author.Name + " " + x.Author.Surname,
                     Name = x.Name,
                     Description = x.Description,
                     DateOfStart = x.DateOfStart.ToString("dd MMMM yyyy"),
                     DateOfEnd = x.DateOfEnd.ToString("dd MMMM yyyy"),
                     Rating = Convert.ToInt32(x.Rating),
                     IsSubscribe = Context.StudentsToCourses.Where(n => n.CourseId == x.Id)
                     .FirstOrDefault(b => b.StudentId == request.DTO.StudentId) == null ? false : true
                 }).ToList();

                return courses;
            }
        }

    }
}

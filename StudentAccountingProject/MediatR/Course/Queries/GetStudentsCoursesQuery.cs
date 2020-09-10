using MediatR;
using StudentAccountingProject.DB;
using StudentAccountingProject.DB.Helpers;
using StudentAccountingProject.MediatR.Course.DTO;
using StudentAccountingProject.MediatR.Course.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StudentAccountingProject.MediatR.Course.Queries
{
    public class GetStudentsCoursesQuery : IRequest<ICollection<StudentCoursesViewModels>>
    {
        public GetStudentsCoursesDTO DTO { get; set; }

        public class GetStudentsCoursesQueryHandler : BaseMediator, IRequestHandler<GetStudentsCoursesQuery, ICollection<StudentCoursesViewModels>>
        {
            public GetStudentsCoursesQueryHandler(EFDbContext context) : base(context)
            {
            }

            public async Task<ICollection<StudentCoursesViewModels>> Handle(GetStudentsCoursesQuery request, CancellationToken cancellationToken)
            {
                var courses = Context
                    .StudentsToCourses
                    .Where(x => x.StudentId == request.DTO.StudentId)
                    .Select(x => new StudentCoursesViewModels 
                    {
                        Id = x.Id,
                        Author = x.Course.Author.Name + " " + x.Course.Author.Surname,
                        Name = x.Course.Name,
                        Description = x.Course.Description,
                        DateOfStart = x.Course.DateOfStart.ToString("dd MMMM yyyy"),
                        DateOfEnd = x.Course.DateOfEnd.ToString("dd MMMM yyyy"),
                        Rating = Convert.ToInt32(x.Course.Rating),
                    })
                    .ToList();

               return courses;
            }
        }
    }
}

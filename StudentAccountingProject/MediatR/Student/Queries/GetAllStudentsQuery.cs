using MediatR;
using StudentAccountingProject.DB;
using StudentAccountingProject.DB.Helpers;
using StudentAccountingProject.MediatR.Student.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StudentAccountingProject.MediatR.Student.Queries
{
    public class GetAllStudentsQuery : IRequest<ICollection<GetAllStudentsViewModel>>
    {

        public class GetAllStudentsQueryHandler : BaseMediator,IRequestHandler<GetAllStudentsQuery, ICollection<GetAllStudentsViewModel>>
        {
            public GetAllStudentsQueryHandler(EFDbContext context) : base(context)
            {
            }

            public async Task<ICollection<GetAllStudentsViewModel>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
            {
                var students = Context.BaseProfiles.Where(x => x.StudentProfile != null && !x.IsDeleted)
                    .Select(x => new GetAllStudentsViewModel
                    {
                        HashId = x.Id,
                        Name = x.Name,
                        Surname = x.Surname,
                        Age = x.Age,
                        Email = x.User.Email,
                        RegisterDate = x.RegisterDate.ToString("dd MMMM yyyy")
                    }).ToList();

                for (int i = 0; i < students.Count; i++)
                {
                    if(Context.StudentsToCourses.Where(x => x.StudentId == students[i].Id).Count() > 0)
                    {
                        students[i].DateOfCourseStart = Context.StudentsToCourses
                            .Where(x => x.StudentId == students[i].Id)
                            .OrderBy(x => x.Course.DateOfStart)
                            .First()
                            .Course.DateOfStart.ToString("dd MMMM yyyy");                        
                    } else
                    {
                        students[i].DateOfCourseStart = DateTime.Now.ToString("dd MMMM yyyy");
                    }

                    students[i].Id = (i + 1).ToString();
                } 

                return students;
            }
        }


    }
}

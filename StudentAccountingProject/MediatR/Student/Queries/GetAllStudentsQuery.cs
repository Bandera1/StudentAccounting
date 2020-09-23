using MediatR;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using StudentAccountingProject.DB;
using StudentAccountingProject.DB.Helpers;
using StudentAccountingProject.Helpers;
using StudentAccountingProject.MediatR.Student.DTO;
using StudentAccountingProject.MediatR.Student.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StudentAccountingProject.MediatR.Student.Queries
{
    public class GetAllStudentsQuery : IRequest<GetAllStudentsViewModel>
    {
        public GetAllStudentsDTO DTO { get; set; }

        public class GetAllStudentsQueryHandler : BaseMediator,IRequestHandler<GetAllStudentsQuery, GetAllStudentsViewModel>
        {
            public GetAllStudentsQueryHandler(EFDbContext context) : base(context)
            {
            }

            public async Task<GetAllStudentsViewModel> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
            {
                var validationResult = StudentValidation(request.DTO);
                if (!validationResult.Status) return validationResult; 

                var students = GetStudents(request.DTO.From, request.DTO.To);
                AddIndex(students, request.DTO.From);

                return new GetAllStudentsViewModel 
                {
                    Status = true,
                    Students = students
                };
            }        

            private GetAllStudentsViewModel StudentValidation(GetAllStudentsDTO DTO)
            {
                var studentValidation = new GetAllStudentsValidation(Context.BaseProfiles
                   .Where(x => x.StudentProfile != null && !x.IsDeleted)
                   .Count());
                var validationResult = studentValidation.Validate(DTO);
                if (validationResult.Errors.Count > 0)
                {
                    return new GetAllStudentsViewModel
                    {
                        Status = false,
                        ErrorMessage = validationResult.Errors.First().ErrorMessage
                    };
                }
                return new GetAllStudentsViewModel { Status = true };
            }

            private List<GetAllStudentInner> GetStudents(int from,int to)
            {
                return Context.BaseProfiles
                   .Where(x => x.StudentProfile != null && !x.IsDeleted)
                   .Skip(from)
                   .Take(to - from)
                   .Select(x => new GetAllStudentInner
                   {
                       HashId = x.Id,
                       Name = x.Name,
                       Surname = x.Surname,
                       Age = x.Age,
                       Email = x.User.Email,
                       RegisterDate = x.RegisterDate.ToString("dd MMMM yyyy")
                   }).ToList();
            }

            private void AddIndex(List<GetAllStudentInner> students,int from)
            {
                int index = from;
                for (int i = 0; i < students.Count; i++)
                {
                    if (Context.StudentsToCourses.Where(x => x.StudentId == students[i].Id).Count() > 0)
                    {
                        students[i].DateOfCourseStart = Context.StudentsToCourses
                            .Where(x => x.StudentId == students[i].Id)
                            .OrderBy(x => x.Course.DateOfStart)
                            .First()
                            .Course.DateOfStart.ToString("dd MMMM yyyy");
                    }
                    else
                    {
                        students[i].DateOfCourseStart = DateTime.Now.ToString("dd MMMM yyyy");
                    }

                    students[i].Id = (index + 1).ToString();
                    index++;
                }
            }
        }


    }
}

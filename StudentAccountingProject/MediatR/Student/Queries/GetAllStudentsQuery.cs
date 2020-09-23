using MediatR;
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
                var userValidator = new GetAllStudentsValidation(Context.BaseProfiles
                    .Where(x => x.StudentProfile != null && !x.IsDeleted)
                    .Count());
                var validationResult = userValidator.Validate(request.DTO);
                if (validationResult.Errors.Count > 0)
                {
                    return new GetAllStudentsViewModel
                    {
                        Status = false,
                        ErrorMessage = validationResult.Errors.First().ErrorMessage
                    };
                }


                var students = Context.BaseProfiles
                    .Where(x => x.StudentProfile != null && !x.IsDeleted)
                    .Skip(request.DTO.From)
                    .Take(request.DTO.To - request.DTO.From)
                    .Select(x => new GetAllStudentInner
                    {
                        HashId = x.Id,
                        Name = x.Name,
                        Surname = x.Surname,
                        Age = x.Age,
                        Email = x.User.Email,
                        RegisterDate = x.RegisterDate.ToString("dd MMMM yyyy")
                    }).ToList();

                int index = request.DTO.From;
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

                    students[i].Id = (index + 1).ToString();
                    index++;
                } 

                return new GetAllStudentsViewModel 
                {
                    Status = true,
                    Students = students
                };
            }
        }


    }
}

using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using StudentAccountingProject.DB;
using StudentAccountingProject.DB.Entities;
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

            public Task<GetAllStudentsViewModel> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
            {
                var validationResult = StudentValidation(request.DTO);
                if (!validationResult.Status)
                {
                    return Task.FromResult(validationResult);
                }

                var students = GetStudents(request.DTO);
                AddIndex(students, GetSkip(request.DTO.CurrentPage, request.DTO.Rows));

                return Task.FromResult(new GetAllStudentsViewModel
                {
                    Status = true,
                    Students = students
                });
            }        

            private GetAllStudentsViewModel StudentValidation(GetAllStudentsDTO DTO)
            {
                var studentValidation = new GetAllStudentsValidation
                    (
                    Context.BaseProfiles
                   .Include(x => x.StudentProfile)
                   .Where(x => x.StudentProfile != null && !x.IsDeleted)                 
                   .Count()
                   );

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

            private List<GetAllStudentInner> GetStudents(GetAllStudentsDTO dto)
            {
                var filter = dto.Filter;

                return Context.BaseProfiles
                   .Include(x => x.StudentProfile)
                   .Where(x => x.StudentProfile != null && !x.IsDeleted)
                   .Where(x =>
                    x.Name.Equals(String.IsNullOrEmpty(filter.Name) || filter.Name.Equals("string") ? x.Name : dto.Filter.Name) &&
                    x.Surname.Equals(String.IsNullOrEmpty(filter.Surname) || filter.Surname.Equals("string") ? x.Surname : dto.Filter.Surname) &&
                    x.Age.Equals(String.IsNullOrEmpty(filter.Age) || filter.Age.Equals("string") ? x.Age : dto.Filter.Age)
                   )
                   .Skip(GetSkip(dto.CurrentPage,dto.Rows))
                   .Take(dto.Rows)
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

            //private string NormalizedFilter(string filter, string field)
            //{
            //    if (String.IsNullOrEmpty(filter) || filter.Equals("string"))
            //    {
            //        return field;
            //    }

            //    return filter;
            //}


            private int GetSkip(int currentPage,int rows)
            {             
                return (currentPage-1)*rows;
            }

            private void AddIndex(List<GetAllStudentInner> students,int from)
            {
                int index = from;
                for (int i = 0; i < students.Count; i++)
                {
                    if (Context.StudentsToCourses.Where(x => x.StudentId == students[i].Id).Count() > 0)
                    {
                        students[i].DateOfCourseStart = Context.StudentsToCourses
                            .Include(x => x.Course)
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

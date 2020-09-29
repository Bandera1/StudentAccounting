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
    public class GetAllStudentsQuery : IRequest<StudentsModel>
    {
        public PagginationModel paggination { get; set; }

        public class GetAllStudentsQueryHandler : BaseMediator,IRequestHandler<GetAllStudentsQuery, StudentsModel>
        {
            public GetAllStudentsQueryHandler(EFDbContext context) : base(context)
            {
            }

            public Task<StudentsModel> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
            {
                ValidatePagginationModel(request.paggination);               

                var students = GetStudents(request.paggination);
                AddData(students);

                return Task.FromResult(new StudentsModel
                {           
                    Students = students
                });
            }        

            private void ValidatePagginationModel(PagginationModel model)
            {
                var studentsCount = Context.BaseProfiles
                   .AsNoTracking()
                   .Include(x => x.StudentProfile)
                   .Where(x => x.StudentProfile != null && !x.IsDeleted)
                   .Count();
                var studentValidation = new GettingStudentsValidation(studentsCount);                   

                var validationResult = studentValidation.Validate(model);
                if (validationResult.Errors.Count > 0)
                {
                    throw new Exception(validationResult.Errors.First().ErrorMessage);                    
                }
            }

            private List<StudentItem> GetStudents(PagginationModel model)
            {
                var filter = model.Filter.SearchCriteria;
                var students = Context.BaseProfiles
                   .AsNoTracking()
                   .Include(x => x.StudentProfile)
                   .Where(x => x.StudentProfile != null && !x.IsDeleted);

                if(!String.IsNullOrEmpty(filter))
                {
                    students = students.Where(x => 
                    x.Name.ToLower().Contains(filter.ToLower()) 
                    || x.Surname.ToLower().Contains(filter.ToLower())
                    );
                }

                return students
                   .Skip(GetCurrentPage(model.CurrentPage, model.PageSize))
                   .Take(model.PageSize)
                   .Select(x => new StudentItem
                   {
                       Id = x.Id,
                       Name = x.Name,
                       Surname = x.Surname,
                       Age = x.Age,
                       Email = x.User.Email,
                       RegisterDate = x.RegisterDate.ToString("dd MMMM yyyy")
                   }).ToList();
            }

            private int GetCurrentPage(int currentPage,int PageSize)
            {             
                return (currentPage-1)*PageSize;
            }

            private void AddData(List<StudentItem> students) // delete
            {
                var studentsToCourse = Context.StudentsToCourses.AsNoTracking();

                for (int i = 0; i < students.Count; i++)
                {
                    if (studentsToCourse.Where(x => x.StudentId == students[i].Id).Count() > 0)
                    {
                        students[i].DateOfCourseStart = Context.StudentsToCourses
                            .AsNoTracking()
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
                }
            }
        }


    }
}

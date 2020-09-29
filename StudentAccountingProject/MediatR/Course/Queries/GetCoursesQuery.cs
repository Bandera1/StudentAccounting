using MediatR;
using Microsoft.EntityFrameworkCore;
using StudentAccountingProject.DB;
using StudentAccountingProject.DB.Helpers;
using StudentAccountingProject.Helpers;
using StudentAccountingProject.MediatR.Course.DTO;
using StudentAccountingProject.MediatR.Student.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StudentAccountingProject.MediatR.Course.Queries
{
    public class GetCoursesQuery : IRequest<CourseModel>
    {
        public PagginationModel paggination { get; set; }

        public class GetCoursesQueryHandler : BaseMediator, IRequestHandler<GetCoursesQuery, CourseModel>
        {
            public GetCoursesQueryHandler(EFDbContext context) : base(context)
            {
            }

            public Task<CourseModel> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
            {
                ValidatePagginationModel(request.paggination);

                var courses = GetCourses(request.paggination);

                return Task.FromResult(new CourseModel { Courses = courses });
            }

            private void ValidatePagginationModel(PagginationModel model)
            {
                var coursesCount = Context.Courses
                   .AsNoTracking()
                   .Where(x => !x.IsDeleted)
                   .Count();
                var studentValidation = new GettingCoursesValidation(coursesCount);

                var validationResult = studentValidation.Validate(model);
                if (validationResult.Errors.Count > 0)
                {
                    throw new Exception(validationResult.Errors.First().ErrorMessage);
                }
            }

            private List<CourseItem> GetCourses(PagginationModel model)
            {
                var filter = model.Filter.SearchCriteria;
                var courses = Context.Courses
                   .AsNoTracking()
                   .Include(x => x.Author)
                   .Where(x => !x.IsDeleted);

                if (!String.IsNullOrEmpty(filter))
                {
                    courses = courses.Where(x =>
                       x.Name.ToLower().Contains(filter.ToLower())
                    || x.Author.Name.ToLower().Contains(filter.ToLower())
                    || x.Author.Surname.ToLower().Contains(filter.ToLower())                  
                    );
                }

                return courses
                   .Skip(GetCurrentPage(model.CurrentPage, model.PageSize))
                   .Take(model.PageSize)
                   .Select(x => new CourseItem
                   {
                       Id = x.Id,
                       Author = new AuthorModel
                       {
                           Id = x.Author.Id,
                           Name = x.Author.Name,
                           Surname = x.Author.Surname
                       },
                       Description = x.Description,
                       Name = x.Name,
                       Rating = x.Rating,
                       PhotoPath = x.PhotoPath,
                       DateOfStart = x.DateOfStart.ToString("dd MMMM yyyy"),
                       DateOfEnd = x.DateOfEnd.ToString("dd MMMM yyyy"),
                   }).ToList();
            }

            private int GetCurrentPage(int currentPage, int PageSize)
            {
                return (currentPage - 1) * PageSize;
            }
        }


    }
}

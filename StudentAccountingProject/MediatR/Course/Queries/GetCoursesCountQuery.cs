using MediatR;
using Microsoft.EntityFrameworkCore;
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
    public class GetCoursesCountQuery : IRequest<CoursesCountViewModel>
    {
        public class GetCoursesCountQueryHandler : BaseMediator, IRequestHandler<GetCoursesCountQuery, CoursesCountViewModel>
        {
            public GetCoursesCountQueryHandler(EFDbContext context) : base(context)
            {
            }

            public Task<CoursesCountViewModel> Handle(GetCoursesCountQuery request, CancellationToken cancellationToken)
            {
                var count = Context.Courses
                    .AsNoTracking()
                    .Where(x => !x.IsDeleted)
                    .Count();

                return Task.FromResult(new CoursesCountViewModel { Count = count });
            }
        }

    }
}

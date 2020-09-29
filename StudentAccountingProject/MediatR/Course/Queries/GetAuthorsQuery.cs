using MediatR;
using Microsoft.EntityFrameworkCore;
using StudentAccountingProject.DB;
using StudentAccountingProject.DB.Helpers;
using StudentAccountingProject.MediatR.Course.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StudentAccountingProject.MediatR.Course.Queries
{
    public class GetAuthorsQuery : IRequest<List<AuthorModel>>
    {
        public class GetCoursesQueryHandler : BaseMediator, IRequestHandler<GetAuthorsQuery, List<AuthorModel>>
        {
            public GetCoursesQueryHandler(EFDbContext context) : base(context)
            {
            }

            public Task<List<AuthorModel>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
            {
                var authors = Context.BaseProfiles
                    .Include(x => x.AdminProfile)
                    .AsNoTracking()
                    .Where(x => !x.IsDeleted && x.AdminProfile == null)
                    .Select(x => new AuthorModel 
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Surname = x.Surname
                    }).ToList();

                return Task.FromResult(authors);
            }
        }
    }
}

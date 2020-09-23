using MediatR;
using StudentAccountingProject.DB;
using StudentAccountingProject.DB.Helpers;
using StudentAccountingProject.MediatR.Student.DTO;
using StudentAccountingProject.MediatR.Student.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StudentAccountingProject.MediatR.Student.Queries
{
    public class GetStudentsCountQuery : IRequest<GetStudentsCountViewModel>
    {
        public class GetStudentsCountQueryHandler : BaseMediator, IRequestHandler<GetStudentsCountQuery, GetStudentsCountViewModel>
        {
            public GetStudentsCountQueryHandler(EFDbContext context) : base(context)
            {
            }

            public async Task<GetStudentsCountViewModel> Handle(GetStudentsCountQuery request, CancellationToken cancellationToken)
            {
                var result = Context.BaseProfiles
                    .Where(x => x.StudentProfile != null && !x.IsDeleted)
                    .Count();


                return new GetStudentsCountViewModel { StudentsCount = result };
            }
        }

    }
}

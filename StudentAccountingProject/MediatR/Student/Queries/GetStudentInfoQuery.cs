using MediatR;
using Microsoft.EntityFrameworkCore;
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
    public class GetStudentInfoQuery : IRequest<StudentInfoModel>
    {
        public string StudentId { get; set; }

        public class GetStudentInfoQueryHandler : BaseMediator, IRequestHandler<GetStudentInfoQuery, StudentInfoModel>
        {
            public GetStudentInfoQueryHandler(EFDbContext context) : base(context)
            {
            }

            public Task<StudentInfoModel> Handle(GetStudentInfoQuery request, CancellationToken cancellationToken)
            {
                var student = Context.BaseProfiles
                    .AsNoTracking()
                    .Include(x => x.User)
                    .FirstOrDefault(x => !x.IsDeleted && x.Id == request.StudentId);

                return Task.FromResult(new StudentInfoModel
                {
                    PhotoPath = student.PhotoPath,
                    Name = student.Name,
                    Surname = student.Surname,
                    Email = student.User.Email,
                    Age = student.Age,
                    IsFacebookAccount = student.IsFacebookAccount
                });
            }
        }

    }
}

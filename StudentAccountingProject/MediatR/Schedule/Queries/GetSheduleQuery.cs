using MediatR;
using Microsoft.EntityFrameworkCore;
using StudentAccountingProject.DB;
using StudentAccountingProject.DB.Helpers;
using StudentAccountingProject.MediatR.Schedule.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StudentAccountingProject.MediatR.Schedule.Queries
{
    public class GetSheduleQuery : IRequest<ScheduleModel>
    {
        public string StudentId { get; set; }

        public class GetSheduleQueryHandler : BaseMediator,IRequestHandler<GetSheduleQuery, ScheduleModel>
        {
            public GetSheduleQueryHandler(EFDbContext context) : base(context)
            {
            }

            public Task<ScheduleModel> Handle(GetSheduleQuery request, CancellationToken cancellationToken)
            {
                var student = Context.BaseProfiles
                    .Include(x => x.StudentProfile)
                    .FirstOrDefault(x => !x.IsDeleted && x.StudentProfile != null);
                if(student == null)
                {
                    return null;
                }

                var schedules = Context.StudentsToCourses
                    .Include(x => x.Course)
                    .Where(x => !x.Course.IsDeleted)
                    .Where(x => x.StudentId == request.StudentId)
                    .Select(x => new ScheduleItem
                    {
                        Title = x.Course.Name,
                        Start = x.Course.DateOfStart.ToString("s"),
                        End = x.Course.DateOfEnd.ToString("s")
                    }).ToList();

                return Task.FromResult(new ScheduleModel { Items = schedules });
            }
        }

    }
}

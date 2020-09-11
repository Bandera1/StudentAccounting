using MediatR;
using Microsoft.AspNetCore.Http;
using StudentAccountingProject.DB;
using StudentAccountingProject.DB.Helpers;
using StudentAccountingProject.MediatR.Account.Commands;
using StudentAccountingProject.MediatR.Course.DTO;
using StudentAccountingProject.MediatR.Course.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StudentAccountingProject.MediatR.Course.Commands
{
    public class SubscribeToCourseCommand : IRequest<SubscribeCourseViewModel>
    {
        public SubscribeToCourseDTO DTO { get; set; }

        public class SubscribeToCourseCommandHandler : BaseMediator, IRequestHandler<SubscribeToCourseCommand, SubscribeCourseViewModel>
        {
            private readonly IMediator Mediator;

            public SubscribeToCourseCommandHandler(EFDbContext context, IMediator mediator) : base(context)
            {
                Mediator = mediator;
            }

            public async Task<SubscribeCourseViewModel> Handle(SubscribeToCourseCommand request, CancellationToken cancellationToken)
            {
                var course = Context.Courses.FirstOrDefault(x => x.Id == request.DTO.CourseId);
                if (course == null)
                {
                    return new SubscribeCourseViewModel
                    {
                        Status = false,
                        ErrorMessage = "Curse does not exist"
                    };
                }

                var student = Context.BaseProfiles.FirstOrDefault(x => x.Id == request.DTO.StudentId);
                if (student == null)
                {
                    return new SubscribeCourseViewModel
                    {
                        Status = false,
                        ErrorMessage = "Student does not exist"
                    };
                }
                if (!Context.Users.FirstOrDefault(x => x.Id == student.UserId).EmailConfirmed)
                {
                    await Mediator.Send(new SendConfirmEmailCommand
                    {
                        DTO = new Account.DTO.SendConfirmEmailDTO { UserId = student.UserId }
                    });
                    return new SubscribeCourseViewModel
                    {
                        Status = false,
                        ErrorMessage = "Your email is not verified. Go to the email for confirmation."
                    };

                }

                //if (student.Courses == null) student.Courses = new List<DB.Entities.Course>();

                Context.StudentsToCourses.Add(new DB.Entities.StudentToCourse
                {
                    CourseId = course.Id,
                    StudentId = student.Id
                });
                Context.SaveChanges();             
                return new SubscribeCourseViewModel { Status = true };
            }
        }
    }
}


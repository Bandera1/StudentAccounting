using Hangfire;
using Hangfire.Common;
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
using System.Linq.Expressions;
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
            IBackgroundJobClient BackgroundJobClient;

            public SubscribeToCourseCommandHandler(
                EFDbContext context, 
                IMediator mediator,
                IBackgroundJobClient backgroundJobClient) : base(context)
            {
                Mediator = mediator;
                BackgroundJobClient = backgroundJobClient;
            }

            public async Task<SubscribeCourseViewModel> Handle(SubscribeToCourseCommand request, CancellationToken cancellationToken)
            {
                var course = Context.Courses.FirstOrDefault(x => x.Id == request.DTO.CourseId && !x.IsDeleted);
                if (course == null)
                {
                    return new SubscribeCourseViewModel
                    {
                        Status = false,
                        ErrorMessage = "Curse does not exist"
                    };
                }

                var student = Context.Users.FirstOrDefault(x => x.Id == request.DTO.StudentId);
                if (student == null)
                {
                    return new SubscribeCourseViewModel
                    {
                        Status = false,
                        ErrorMessage = "Student does not exist"
                    };
                }
                if (!student.EmailConfirmed)
                {
                    await Mediator.Send(new SendConfirmEmailCommand
                    {
                        DTO = new Account.DTO.SendConfirmEmailDTO { UserId = student.Id }
                    });
                    return new SubscribeCourseViewModel
                    {
                        Status = false,
                        ErrorMessage = "Your email is not verified. Go to the email for confirmation."
                    };

                }

                Context.StudentsToCourses.Add(new DB.Entities.StudentToCourse
                {
                    CourseId = course.Id,
                    StudentId = student.Id
                });
                Context.SaveChanges();

                System.Action<string, string, int> act = async (courseName, email, days) => {
                    await Mediator.Send(new SendNotificationEmailCommand
                    {
                        DTO = new SendNotificationEmailDTO
                        {
                            CourseName = courseName,
                            UserEmail = email,
                            DaysCount = days
                        }
                    });
                };

                if ((course.DateOfStart - DateTime.Now).Days >= 30)
                {
                    var days = (course.DateOfStart - DateTime.Now).Days - 30;

                    BackgroundJobClient.Schedule(
                    () => act.Invoke(course.Name, student.Email, 30),
                    TimeSpan.FromDays(days));

                    BackgroundJobClient.Schedule(
                    () => act.Invoke(course.Name, student.Email, 7),
                    TimeSpan.FromDays(days + 23));

                    int hour = 0;
                    if(course.DateOfStart.Hour > 8)
                    {
                        if(DateTime.Now.Hour > 8)
                        {
                            days += 6;
                            hour = DateTime.Now.Hour - 8;
                        } 
                        else
                        {
                            days += 7;
                            hour = 8 - DateTime.Now.Hour;
                        }
                    } 
                    else
                    {
                        if (DateTime.Now.Hour > 8)
                        {
                            days += 5;
                            hour = DateTime.Now.Hour - 8;
                        }
                        else
                        {
                            days += 6;
                            hour = 8 - DateTime.Now.Hour;
                        }
                    }

                    BackgroundJobClient.Schedule(
                    () => act.Invoke(course.Name, student.Email, 7),
                    new TimeSpan(days,hour,0,0));
                }
                else
                {
                    var days = (course.DateOfStart - DateTime.Now).Days;

                    if(days > 7)
                    {
                        days -= 7;
                        BackgroundJobClient.Schedule(
                        () => act.Invoke(course.Name, student.Email, 7),
                        TimeSpan.FromDays(days));

                        int hour = 0;
                        if (course.DateOfStart.Hour > 8)
                        {
                            if (DateTime.Now.Hour > 8)
                            {
                                days += 6;
                                hour = DateTime.Now.Hour - 8;
                            }
                            else
                            {
                                days += 7;
                                hour = 8 - DateTime.Now.Hour;
                            }
                        }
                        else
                        {
                            if (DateTime.Now.Hour > 8)
                            {
                                days += 5;
                                hour = DateTime.Now.Hour - 8;
                            }
                            else
                            {
                                days += 6;
                                hour = 8 - DateTime.Now.Hour;
                            }
                        }

                        BackgroundJobClient.Schedule(
                        () => act.Invoke(course.Name, student.Email, 7),
                        new TimeSpan(days, hour, 0, 0));
                    } 
                    else
                    {
                        int hour = 0;
                        if (course.DateOfStart.Hour > 8)
                        {
                            if (DateTime.Now.Hour > 8)
                            {
                                if((course.DateOfStart - DateTime.Now).Days < 7)
                                {
                                    days = 6 + (course.DateOfStart - DateTime.Now).Days;
                                }
                                days += 6;
                                hour = DateTime.Now.Hour - 8;
                            }
                            else
                            {
                                if ((course.DateOfStart - DateTime.Now).Days < 7)
                                {
                                    days = 7 + (course.DateOfStart - DateTime.Now).Days;
                                }
                                hour = 8 - DateTime.Now.Hour;
                            }
                        }
                        else
                        {
                            if (DateTime.Now.Hour > 8)
                            {
                                days += 5;
                                hour = DateTime.Now.Hour - 8;
                            }
                            else
                            {
                                days += 6;
                                hour = 8 - DateTime.Now.Hour;
                            }
                        }

                        BackgroundJobClient.Schedule(
                        () => act.Invoke(course.Name, student.Email, 7),
                        new TimeSpan(days, hour, 0, 0));
                    }              
                }            

                return new SubscribeCourseViewModel { Status = true };
            }          
        }
    }
}


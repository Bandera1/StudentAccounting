using Hangfire;
using Hangfire.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using StudentAccountingProject.DB;
using StudentAccountingProject.DB.Helpers;
using StudentAccountingProject.MediatR.Account.Commands;
using StudentAccountingProject.MediatR.Course.DTO;
using StudentAccountingProject.MediatR.Course.ViewModels;
using StudentAccountingProject.MediatR.Student.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace StudentAccountingProject.MediatR.Course.Commands
{
    public class SubscribeToCourseCommand : IRequest<BaseViewModel>
    {
        public SubscribeModel model { get; set; }

        public class SubscribeToCourseCommandHandler : BaseMediator, IRequestHandler<SubscribeToCourseCommand, BaseViewModel>
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

            public async Task<BaseViewModel> Handle(SubscribeToCourseCommand request, CancellationToken cancellationToken)
            {
                //-----------GET DATA-------------
                var course = Context.Courses
                    .FirstOrDefault(x => x.Id == request.model.CourseId && !x.IsDeleted);
                var student = Context.Users
                    .FirstOrDefault(x => x.Id == request.model.StudentId);

                //-----------VALIDATION-------------
                var validationResult = Validate(course,student);
                if (!String.IsNullOrEmpty(validationResult.ErrorMessage))
                {
                    return validationResult;
                }

                var emailValidationResult = await ValidateEmailConfirmed(student);
                if(!String.IsNullOrEmpty(emailValidationResult.ErrorMessage))
                {
                    return emailValidationResult;
                }

                //-----------OTHER-------------
                AddStudentToCourse(Context, course.Id, student.Id);

                NotificationTree(course, student);

                return new BaseViewModel ();
            }          
        
            private BaseViewModel Validate(
                DB.Entities.Course course,
                DB.IdentityModels.DbUser student)
            {
                if (course == null)
                {
                    return new BaseViewModel
                    {
                        ErrorMessage = "Curse does not exist"
                    };
                }

                if (student == null)
                {
                    return new BaseViewModel
                    {
                        ErrorMessage = "Student does not exist"
                    };
                }

                return new BaseViewModel();
            }

            private async Task<BaseViewModel> ValidateEmailConfirmed(DB.IdentityModels.DbUser student)
            {
                if (!student.EmailConfirmed)
                {
                    await Mediator.Send(new SendConfirmEmailCommand
                    {
                        DTO = new Account.DTO.SendConfirmEmailDTO { UserId = student.Id }
                    });
                    return new BaseViewModel
                    {
                        ErrorMessage = "Your email is not verified. Go to the email for confirmation."
                    };
                }
                return new BaseViewModel();
            }
        
            private void AddStudentToCourse(EFDbContext context,string courseId,string studentId)
            {
                Context.StudentsToCourses.Add(new DB.Entities.StudentToCourse
                {
                    CourseId = courseId,
                    StudentId = studentId
                });
                Context.SaveChanges();
            }
        
            private void NotificationTree(DB.Entities.Course course, DB.IdentityModels.DbUser student)
            {
                const int MONTH = 30;
                const int WEEK = 7;
                const int DAY = 1;
                var days = 0;


                if ((course.DateOfStart - DateTime.Now).Days >= MONTH)
                {
                    MoreThanMonth();
                }
                else
                {
                    LessThanMonth();
                }

                void MoreThanMonth()
                {
                    days = (course.DateOfStart - DateTime.Now).Days - MONTH;

                    SendNotificationSchedule(
                        course.Name, 
                        student.Email, 
                        MONTH, 
                        TimeSpan.FromDays(days)); // Month
 
                    SendNotificationSchedule(
                        course.Name,
                        student.Email, 
                        WEEK,TimeSpan.FromDays(days + 23)); // Week

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

                    SendNotificationSchedule(
                        course.Name, 
                        student.Email, 
                        DAY, 
                        new TimeSpan(days, hour, 0, 0)); // Week
                }

                void LessThanMonth()
                {
                    days = (course.DateOfStart - DateTime.Now).Days;
                    
                    if(days > WEEK)
                    {
                        MoreThanWeek();
                    } else
                    {
                        LessThanWeek();
                    }



                    void MoreThanWeek()
                    {
                        days -= WEEK;

                        SendNotificationSchedule(
                            course.Name,
                            student.Email,
                            DAY,
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

                        SendNotificationSchedule(
                          course.Name,
                          student.Email,
                          DAY,
                          new TimeSpan(days, hour, 0, 0));
                    }

                    void LessThanWeek()
                    {
                        int hour = 0;
                        if (course.DateOfStart.Hour > 8)
                        {
                            if (DateTime.Now.Hour > 8)
                            {
                                if ((course.DateOfStart - DateTime.Now).Days < WEEK)
                                {
                                    days = 6 + (course.DateOfStart - DateTime.Now).Days;
                                }
                                days += 6;
                                hour = DateTime.Now.Hour - 8;
                            }
                            else
                            {
                                if ((course.DateOfStart - DateTime.Now).Days < WEEK)
                                {
                                    days = WEEK + (course.DateOfStart - DateTime.Now).Days;
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

                        SendNotificationSchedule(
                          course.Name,
                          student.Email,
                          DAY,
                          new TimeSpan(days, hour, 0, 0));
                    }
                }
            }   
        
            private void SendNotificationSchedule(
                string CourseName,
                string Email,
                int EmailDays,
                TimeSpan Schedule)
            {
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

                BackgroundJobClient.Schedule(
                    () => act.Invoke(CourseName, Email, EmailDays),
                    Schedule);           
            }
        }
    }
}


using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StudentAccountingProject.DB;
using StudentAccountingProject.DB.Entities;
using StudentAccountingProject.DB.Helpers;
using StudentAccountingProject.Helpers;
using StudentAccountingProject.MediatR.Course.DTO;
using StudentAccountingProject.MediatR.Student.ViewModels;
using StudentAccountingProject.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StudentAccountingProject.MediatR.Course.Commands
{
    public class CreateCourseCommand : IRequest<BaseViewModel>
    {
        public NewCourseModel Model { get; set; }

        public class NewCourseModelHandler : BaseMediator, IRequestHandler<CreateCourseCommand, BaseViewModel>
        {
            private readonly IConfiguration _configuration;
            private readonly IWebHostEnvironment _env;

            public NewCourseModelHandler(EFDbContext context, IConfiguration configuration,
                IWebHostEnvironment env) : base(context)
            {
                _configuration = configuration;
                _env = env;
            }

            public Task<BaseViewModel> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
            {
                var validationResult = Validate(request.Model);
                if (!String.IsNullOrEmpty(validationResult))
                {
                    return Task.FromResult(new BaseViewModel { ErrorMessage = validationResult });
                }

                var imagePath = SavePhoto(request.Model.PhotoBase64); // Save photo to server
                var newCourse =  CreateCourse(request.Model, imagePath); // Create course
                SetCourseImage(newCourse); // Set photo to course

                Context.Add(newCourse);
                Context.SaveChanges();

                return Task.FromResult(new BaseViewModel());
            }

            private string Validate(NewCourseModel model)
            {
                var user = Context.BaseProfiles
                    .AsNoTracking()
                    .Where(x => !x.IsDeleted && x.StudentProfile != null && x.Id == model.AuthorId);
                if(user == null)
                {
                    return "Incorrect user";
                }

                if(!String.IsNullOrEmpty(ValidateDate(model)))
                {
                    return ValidateDate(model);
                }

                var courseValidator = new AddingCourseValidation();
                var validationResult = courseValidator.Validate(model);
                if (validationResult.Errors.Count > 0)
                {
                    return validationResult.Errors.First().ErrorMessage;
                }
                
                return String.Empty;
            }
        
            private string ValidateDate(NewCourseModel model)
            {
                if (DateTime.Parse(model.DateOfEnd) < DateTime.Parse(model.DateOfStart))
                {
                    return "Incorrect date";
                } 
                if(DateTime.Parse(model.DateOfStart) < DateTime.Now)
                {
                    return "Incorrect date";
                }

                return String.Empty;
            }

            private DB.Entities.Course CreateCourse(NewCourseModel model,string photoPath)
            {
                var newCourse = new DB.Entities.Course
                {
                    Name = model.Name,
                    Description = model.Description,
                    Rating = 0,
                    AuthorId = model.AuthorId,
                    PhotoPath = photoPath,
                    DateOfStart = DateTime.Parse(model.DateOfStart),
                    DateOfEnd = DateTime.Parse(model.DateOfEnd)
                };

                return newCourse;
            }

            private string SavePhoto(string photoBase64)
            {
                var photoPath = string.Empty;

                string imageName = Guid.NewGuid().ToString() + ".jpg"; // 1. Create photo name
                string pathSaveImages = InitStaticFiles  // 2. Save photo
                   .CreateImageByFileName(_env, _configuration,
                   new string[] { "ImagesPath", "ImagesPathCourses" }, // 3. Send path
                   imageName, photoBase64); // 4. Send imageName and photo

                if (pathSaveImages != null) // 5. If photo is created
                {
                    photoPath = imageName;
                }                

                return photoPath;
            }

            private void SetCourseImage(DB.Entities.Course course)
            {
                const string defaultSize = "1280";

                string path = $"{_configuration.GetValue<string>("CoursesUrlImages")}/{defaultSize}_";
                string imagePath = !String.IsNullOrEmpty(course.PhotoPath) ? path + course.PhotoPath :
                        path + _configuration.GetValue<string>("DefaultCourseImage");

                course.PhotoPath = imagePath;
            }
        }

    }
}

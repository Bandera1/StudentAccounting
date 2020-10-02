using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StudentAccountingProject.DB;
using StudentAccountingProject.DB.Entities;
using StudentAccountingProject.DB.Helpers;
using StudentAccountingProject.MediatR.Account.DTO;
using StudentAccountingProject.MediatR.Student.ViewModels;
using StudentAccountingProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StudentAccountingProject.MediatR.Account.Commands
{
    public class ChangeUserImageCommand : IRequest<BaseViewModel>
    {
        public NewPhotoModel Model { get; set; }

        public class ChangUserImageCommandHandler : BaseMediator,IRequestHandler<ChangeUserImageCommand, BaseViewModel>
        {
            private readonly IConfiguration _configuration;
            private readonly IWebHostEnvironment _env;

            public ChangUserImageCommandHandler(EFDbContext context, IConfiguration configuration,
                IWebHostEnvironment env) : base(context)
            {
                _configuration = configuration;
                _env = env;
            }

            public Task<BaseViewModel> Handle(ChangeUserImageCommand request, CancellationToken cancellationToken)
            {
                var user = Context.BaseProfiles
                    .FirstOrDefault(x => !x.IsDeleted && x.Id == request.Model.UserId);

                if(user != null)
                {
                    if(CreateImage(user, request.Model))
                    {
                        SetUserImage(user);
                    }
                }

                return Task.FromResult(new BaseViewModel { Status = true });
            }

            private bool CreateImage(BaseProfile user, NewPhotoModel model)
            {
                string image = null;

                string imageName = Guid.NewGuid().ToString() + ".jpg"; // 1. Create photo name
                string pathSaveImages = InitStaticFiles  // 2. Save photo
                   .CreateImageByFileName(_env, _configuration,
                   new string[] { "ImagesPath", "ImagesPathUsers" }, // 3. Send path
                   imageName, model.ImageBase64); // 4. Send imageName and photo

                if (pathSaveImages != null) // 5. If photo is created
                {
                    image = imageName;
                    user.PhotoPath = image;
                    return true;
                }
                else
                {
                    image = user.PhotoPath;
                    return false;
                }
            }
        
            private void SetUserImage(BaseProfile user)
            {
                const string defaultSize = "250";

                string path = $"{_configuration.GetValue<string>("UserUrlImages")}/{defaultSize}_";
                string imagePath = user.PhotoPath != null ? path + user.PhotoPath :
                        path + _configuration.GetValue<string>("DefaultImage");

                user.PhotoPath = imagePath;

                Context.SaveChanges(); // 6. Add photo to user
            }
        }

    }
}

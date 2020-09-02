using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentAccountingProject.DB.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAccountingProject.DB
{
    public class SeederDB
    {
        public static void SeedRoles(EFDbContext context, UserManager<DbUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            if (context.Roles.Count() == 0)
            {
                var roleName = "Admin";
                var result = roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                }).Result;

                roleName = "Student";
                result = roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                }).Result;           
            }
        }


        public static void SeedData(IServiceProvider services, IWebHostEnvironment env, IConfiguration config)
        {
            using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var manager = scope.ServiceProvider.GetRequiredService<UserManager<DbUser>>();
                var managerRole = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var context = scope.ServiceProvider.GetRequiredService<EFDbContext>();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();


                // Roles: 2
               

                SeederDB.SeedRoles(context, manager, managerRole);
               
            }

        }


    }
}

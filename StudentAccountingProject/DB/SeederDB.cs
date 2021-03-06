﻿using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentAccountingProject.DB.Entities;
using StudentAccountingProject.DB.IdentityModels;
using StudentAccountingProject.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAccountingProject.DB
{
    public class SeederDB
    {
        public async static void SeedRoles(EFDbContext context, UserManager<DbUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            if (context.Roles.Count() == 0)
            {
                var roleName = ProjectRoles.ADMIN;
                await roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });

                roleName = ProjectRoles.STUDENT;
                await roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }
        }

        public async static void SeedStudents(EFDbContext context, UserManager<DbUser> userManager)
        {
            if (context.BaseProfiles.Where(x => x.StudentProfile != null).Count() == 0)
            {
                var role = ProjectRoles.STUDENT;

                //----------------#1-------------------
                var baseProfile = new BaseProfile
                {
                    Name = "Jordan",
                    Surname = "Montana",
                    Age = new Random().Next(18, 56).ToString(),
                    RegisterDate = DateTime.Now.AddDays(new Random().Next(5, 31)),
                    PhotoPath = "/1280_default-user.png"
                };

                var studentProfile = new StudentProfile
                {
                    BaseProfile = baseProfile
                };
                baseProfile.StudentProfile = studentProfile;

                var dbUser = new DbUser
                {
                    Email = "student1@gmail.com",
                    UserName = "student1@gmail.com",
                    BaseProfile = baseProfile
                };
                baseProfile.UserId = dbUser.Id;

                var result = userManager.CreateAsync(dbUser, "QWerty-1").Result;
                result = userManager.AddToRoleAsync(dbUser, role).Result;

                //----------------#2-------------------

                baseProfile = new BaseProfile
                {
                    Name = "Black",
                    Surname = "Water",
                    Age = new Random().Next(18, 56).ToString(),
                    RegisterDate = DateTime.Now.AddDays(new Random().Next(5, 31)),
                    PhotoPath = "/1280_default-user.png"
                };

                studentProfile = new StudentProfile
                {
                    BaseProfile = baseProfile
                };
                baseProfile.StudentProfile = studentProfile;

                dbUser = new DbUser
                {
                    Email = "student2@gmail.com",
                    UserName = "student2@gmail.com",
                    BaseProfile = baseProfile
                };
                baseProfile.UserId = dbUser.Id;

                result = userManager.CreateAsync(dbUser, "QWerty-1").Result;
                result = userManager.AddToRoleAsync(dbUser, role).Result;
            }
        }

        public async static void SeedAdmins(EFDbContext context, UserManager<DbUser> userManager)
        {
            if (context.BaseProfiles.Where(x => x.AdminProfile != null).Count() == 0)
            {
                var role = ProjectRoles.ADMIN;

                //----------------#1-------------------
                var baseProfile = new BaseProfile
                {
                    Name = "Admin",
                    Surname = "Adminovich",
                    Age = new Random().Next(18, 56).ToString(),
                    RegisterDate = DateTime.Now.AddDays(new Random().Next(5, 31)),
                    PhotoPath = "/1280_default-user.png"
                };

                var adminProfile = new AdminProfile
                {
                    BaseProfile = baseProfile,
                    IsSuperAdmin = true
                };
                baseProfile.AdminProfile = adminProfile;

                var dbUser = new DbUser
                {
                    Email = "admin@gmail.com",
                    UserName = "admin@gmail.com",
                    BaseProfile = baseProfile
                };
                baseProfile.UserId = dbUser.Id;

                var result = userManager.CreateAsync(dbUser, "QWerty-1").Result;
                result = userManager.AddToRoleAsync(dbUser, role).Result;
            }
        }

        public static void SeedCourses(EFDbContext context)
        {
            if (context.Courses.Count() == 0)
            {
                var courses = new List<Course>();
                courses.Add(new Course
                {
                    Name = "Python automatic course",
                    Description = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate",
                    AuthorId = context.BaseProfiles.Where(x => x.StudentProfile != null).First().Id,
                    DateOfStart = DateTime.Now.AddDays(new Random().Next(1, 25)),
                    DateOfEnd = DateTime.Now.AddDays(new Random().Next(25, 67)),
                    Rating = new Random().Next(1, 5),
                    PhotoPath = "CoursesImages/1280_default-course.png"
                });

                courses.Add(new Course
                {
                    Name = "Ruby automatic course",
                    Description = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate",
                    AuthorId = context.BaseProfiles.Where(x => x.StudentProfile != null).First().Id,
                    DateOfStart = DateTime.Now.AddDays(new Random().Next(1, 25)),
                    DateOfEnd = DateTime.Now.AddDays(new Random().Next(25, 67)),
                    Rating = new Random().Next(1, 5),
                    PhotoPath = "CoursesImages/1280_default-course.png"
                });

                courses.Add(new Course
                {
                    Name = "C hell course",
                    Description = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate",
                    AuthorId = context.BaseProfiles.Where(x => x.StudentProfile != null).First().Id,
                    DateOfStart = DateTime.Now.AddDays(new Random().Next(1, 25)),
                    DateOfEnd = DateTime.Now.AddDays(new Random().Next(25, 67)),
                    Rating = new Random().Next(1, 5),
                    PhotoPath = "CoursesImages/1280_default-course.png"
                });

                courses.Add(new Course
                {
                    Name = "JS Angular course",
                    Description = "Far far away, behind the word mountains, far from the countries Vokalia and Consonantia, there live the blind texts. Separated they live in Bookmarksgrove right at the coast of the Semantics, a large language ocean. A small river named Duden flows by their place and supplies it with the necessary",
                    Author = context.BaseProfiles.Where(x => x.StudentProfile != null).First(),
                    DateOfStart = DateTime.Now.AddDays(new Random().Next(1,25)),
                    DateOfEnd = DateTime.Now.AddDays(new Random().Next(25, 67)),
                    Rating = new Random().Next(1, 5),
                    PhotoPath = "CoursesImages/1280_default-course.png"
                });

                courses.Add(new Course
                {
                    Name = "React & Redux course",
                    Description = "Far far away, behind the word mountains, far from the countries Vokalia and Consonantia, there live the blind texts. Separated they live in Bookmarksgrove right at the coast of the Semantics, a large language ocean. A small river named Duden flows by their place and supplies it with the necessary",
                    Author = context.BaseProfiles.Where(x => x.StudentProfile != null).First(),
                    DateOfStart = DateTime.Now.AddDays(new Random().Next(1, 25)),
                    DateOfEnd = DateTime.Now.AddDays(new Random().Next(25, 67)),
                    Rating = new Random().Next(1, 5),
                    PhotoPath = "CoursesImages/1280_default-course.png"
                });

                courses.Add(new Course
                {
                    Name = "C++ course",
                    Description = "Far far away, behind the word mountains, far from the countries Vokalia and Consonantia, there live the blind texts. Separated they live in Bookmarksgrove right at the coast of the Semantics, a large language ocean. A small river named Duden flows by their place and supplies it with the necessary",
                    Author = context.BaseProfiles.Where(x => x.StudentProfile != null).First(),
                    DateOfStart = DateTime.Now.AddDays(new Random().Next(1, 25)),
                    DateOfEnd = DateTime.Now.AddDays(new Random().Next(25, 67)),
                    Rating = new Random().Next(1, 5)
                });

                courses.Add(new Course
                {
                    Name = "C# ASP.NET CORE for 7 day",
                    Description = "This is joke.You can never learn ASP.NET in 7 days",
                    Author = context.BaseProfiles.Where(x => x.StudentProfile != null).First(),
                    DateOfStart = DateTime.Now.AddDays(new Random().Next(1, 25)),
                    DateOfEnd = DateTime.Now.AddDays(new Random().Next(25, 67)),
                    Rating = new Random().Next(1, 5),
                    PhotoPath = "CoursesImages/1280_default-course.png"
                });

                context.Courses.AddRange(courses);
                context.SaveChanges();
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
                // Students: 2
                // Admin: 1 - SuperAdmin
                // Courses: 5

                SeederDB.SeedRoles(context, manager, managerRole);
                SeederDB.SeedStudents(context, manager);
                SeederDB.SeedAdmins(context, manager);
                SeederDB.SeedCourses(context);
            }

        }


    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentAccountingProject.DB.Entities;
using StudentAccountingProject.DB.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAccountingProject.DB
{
    public class EFDbContext : IdentityDbContext<DbUser, IdentityRole, string, IdentityUserClaim<string>,
    IdentityUserRole<string>, IdentityUserLogin<string>,
    IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public EFDbContext(DbContextOptions<EFDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<BaseProfile> BaseProfiles { get; set; }
        public DbSet<StudentProfile> StudentProfiles { get; set; }
        public DbSet<AdminProfile> AdminProfiles { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentToCourse> StudentsToCourses { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<BaseProfile>()
                .HasOne(x => x.AdminProfile)
                .WithOne(x => x.BaseProfile)
                .HasForeignKey<AdminProfile>(x => x.BaseProfileId);

            builder.Entity<BaseProfile>()
                .HasOne(x => x.StudentProfile)
                .WithOne(x => x.BaseProfile)
                .HasForeignKey<StudentProfile>(x => x.BaseProfileId);

            builder.Entity<StudentProfile>()
                .HasOne(x => x.BaseProfile)
                .WithOne(x => x.StudentProfile);

            //builder.Entity<BaseProfile>()
            //    .HasMany<Course>(x => x.Courses);

            //builder.Entity<Course>()
            //    .HasMany<BaseProfile>(x => x.Subscribers);

            builder.Entity<Course>()
                .HasOne<BaseProfile>(x => x.Author);
        }
    }
}

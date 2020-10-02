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
                .HasForeignKey<AdminProfile>(x => x.BaseProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BaseProfile>()
                .HasOne(x => x.StudentProfile)
                .WithOne(x => x.BaseProfile)
                .HasForeignKey<StudentProfile>(x => x.BaseProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<StudentProfile>()
                .HasOne(x => x.BaseProfile)
                .WithOne(x => x.StudentProfile)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BaseProfile>()
                .HasOne(x => x.User)
                .WithOne(x => x.BaseProfile)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Course>()
                .Property(b => b.IsDeleted)
                .HasDefaultValue(false);

            builder.Entity<BaseProfile>()
                .Property(b => b.IsDeleted)
                .HasDefaultValue(false);

            builder.Entity<BaseProfile>()
                .Property(b => b.IsFacebookAccount)
                .HasDefaultValue(false);

            builder.Entity<Course>()
                .HasOne<BaseProfile>(x => x.Author);
        }
    }
}

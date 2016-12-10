namespace ClubManager.Migrations.ApplicationMigrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using Models.ClubModels;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\ApplicationMigrations";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));


            context.Roles.AddOrUpdate(
                role => role.Id,
                new IdentityRole { Id = "1", Name = "Admin" },
                new IdentityRole { Id = "2", Name = "ClubAdmin" },
                new IdentityRole { Id = "3", Name = "ClubMember" });


            context.Users.AddOrUpdate(u => u.Email,
                new ApplicationUser
                {
                    StudentId = "S00157097",
                    Email = "S00157097@mail.itsligo.ie",
                    DateJoined = DateTime.Now,
                    UserName = "S00157097@mail.itsligo.ie",
                    PasswordHash = new PasswordHasher().HashPassword("Ss0157097$1"),
                    SecurityStamp = Guid.NewGuid().ToString(),
                },
                new ApplicationUser
                {
                    StudentId = "S00157098",
                    Email = "S00157098@mail.itsligo.ie",
                    DateJoined = DateTime.Now,
                    UserName = "S00157098@mail.itsligo.ie",
                    PasswordHash = new PasswordHasher().HashPassword("Ss00157098$1"),
                    SecurityStamp = Guid.NewGuid().ToString(),
                },
                new ApplicationUser
                {
                    StudentId = "S00157099",
                    Email = "S00157099@mail.itsligo.ie",
                    DateJoined = DateTime.Now,
                    UserName = "S00157099@mail.itsligo.ie",
                    PasswordHash = new PasswordHasher().HashPassword("Ss00157099$1"),
                    SecurityStamp = Guid.NewGuid().ToString(),
                });

            ApplicationUser admin = userManager.FindByEmail("S00157097@mail.itsligo.ie");
            if (admin != null)
                userManager.AddToRole(admin.Id, "Admin");


            ApplicationUser clubAdmin = userManager.FindByEmail("S00157098@mail.itsligo.ie");
            if (clubAdmin != null)
                userManager.AddToRole(clubAdmin.Id, "ClubAdmin");

            ApplicationUser clubMember = userManager.FindByEmail("S00157099@mail.itsligo.ie");
            if (clubMember != null)
                userManager.AddToRole(clubMember.Id, "ClubMember");

            //SeedStudents(context);
        }

        private void SeedStudents(ApplicationDbContext current)
        {
            List<Student> selectedStudents = new List<Student>();

            using (ClubDbContext ctx = new ClubDbContext())
            {
                var randomStudentSet = ctx.Students
                    .Select(s => new { s.StudentId, r = Guid.NewGuid() });

                List<string> subset = randomStudentSet
                    .OrderBy(s => s.r)
                    .Select(s => s.StudentId).Take(10)
                    .ToList();

                foreach (string s in subset)
                {
                    selectedStudents.Add(
                        ctx.Students.First(st => st.StudentId == s)
                        );
                }

                Club chosen = ctx.Clubs.First();

                foreach (Student s in selectedStudents)
                {
                    ctx.Members.AddOrUpdate(
                        m => m.StudentId,
                        new Member
                        {
                            ClubId = chosen.ClubId,
                            StudentId = s.StudentId
                        });
                }

                ctx.SaveChanges();
            }

            foreach (Student s in selectedStudents)
            {
                current.Users.AddOrUpdate(u => u.StudentId,
                    new ApplicationUser
                    {
                        StudentId = s.StudentId,
                        UserName = s.StudentId + "@mail.itsligo.ie",
                        Email = s.StudentId + "@mail.itsligo.ie",
                        EmailConfirmed = true,
                        DateJoined = DateTime.Now,
                        PasswordHash = new PasswordHasher().HashPassword(s.StudentId + "$1"),
                        SecurityStamp = Guid.NewGuid().ToString(),
                    });
            }

            current.SaveChanges();
        }
    }
}

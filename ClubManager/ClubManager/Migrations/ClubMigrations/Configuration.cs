namespace ClubManager.Migrations.ClubMigrations
{
    using CsvHelper;
    using Models.ClubModels;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    internal sealed class Configuration : DbMigrationsConfiguration<ClubManager.Models.ClubModels.ClubDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\ClubMigrations";
        }

        protected override void Seed(ClubManager.Models.ClubModels.ClubDbContext context)
        {

            context.Clubs.AddOrUpdate(c => c.ClubName,
                new Club
                {
                    ClubName = "The Tiddly Winks Club",
                    CreationDate = DateTime.Now,
                    clubEvents = new List<ClubEvent>()
                    {
                        new ClubEvent
                        {
                            StartDateTime = DateTime.Now.Subtract(new TimeSpan(5, 0, 0, 0, 0)),
                            EndDateTime = DateTime.Now.Subtract(new TimeSpan(5, 0, 0, 0, 0)),
                            Location = "Sligo",
                            Venue = "Arena"
                        },
                        new ClubEvent
                        {
                            StartDateTime = DateTime.Now.Subtract(new TimeSpan(3, 0, 0, 0, 0)),
                            EndDateTime = DateTime.Now.Subtract(new TimeSpan(3, 0, 0, 0, 0)),
                            Location = "Sligo",
                            Venue = "Main Canteen"
                        }
                    }
                });

            context.Clubs.AddOrUpdate(c => c.ClubName,
                new Club
                {
                    ClubName = "The Chess Club",
                    CreationDate = DateTime.Now,
                    clubEvents = new List<ClubEvent>()
                    {
                        new ClubEvent
                        {
                            StartDateTime = DateTime.Now.Subtract(new TimeSpan(5, 0, 0, 0, 0)),
                            EndDateTime = DateTime.Now.Subtract(new TimeSpan(5, 0, 0, 0, 0)),
                            Location = "Dublin",
                            Venue = "Arena"
                        },
                        new ClubEvent
                        {
                            StartDateTime = DateTime.Now.Subtract(new TimeSpan(3, 0, 0, 0, 0)),
                            EndDateTime = DateTime.Now.Subtract(new TimeSpan(3, 0, 0, 0, 0)),
                            Location = "Dublin",
                            Venue = "Main Canteen"
                        }
                    }
                });

            context.SaveChanges();

            Assembly assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream("ClubManager.Migrations.ClubMigrations.students.csv"))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    CsvReader csvReader = new CsvReader(reader);
                    csvReader.Configuration.HasHeaderRecord = false;
                    csvReader.Configuration.WillThrowOnMissingField = false;

                    Student[] testStudents = csvReader.GetRecords<Student>().ToArray();
                    context.Students.AddOrUpdate(s => s.StudentId, testStudents);
                }
            }
        }
    }
}

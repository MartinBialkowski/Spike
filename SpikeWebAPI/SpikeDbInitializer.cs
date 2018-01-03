using EFCoreSpike5.Models;
using Spike.Core.Entity;
using System;
using System.Linq;

namespace SpikeWebAPI
{
    public class SpikeDbInitializer
    {
        private static EFCoreSpikeContext context;
        public static void Initialize(IServiceProvider serviceProvider)
        {
            context = (EFCoreSpikeContext)serviceProvider.GetService(typeof(EFCoreSpikeContext));
            context.Database.EnsureCreated();
            InitilizeDatabase();
        }

        private static void InitilizeDatabase()
        {
            if(!context.Courses.Any())
            {
                context.Courses.Add(new Course
                {
                    Name = "Random"
                });

                context.Courses.Add(new Course
                {
                    Name = "New Course"
                });

                context.SaveChanges();
            }

            if(!context.Students.Any())
            {
                context.Students.Add(new Student
                {
                    Name = "Martin B",
                    CourseId = 1
                });

                context.Students.Add(new Student
                {
                    Name = "Witalian",
                    CourseId = 2
                });

                context.Students.Add(new Student
                {
                    Name = "SomeRandomStudent",
                    CourseId = 1
                });

                context.SaveChanges();
            }
        }
    }
}

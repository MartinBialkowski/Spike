using EFCoreSpike5.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Spike.Core.Entity;
using System;
using System.Linq;

namespace Spike.WebApi
{
    public class SpikeDbInitializer
    {
        private static EFCoreSpikeContext context;

        public static void Initialize(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                context = serviceScope.ServiceProvider.GetRequiredService<EFCoreSpikeContext>();
                context.Database.Migrate();
                InitilizeDatabase();
            }
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

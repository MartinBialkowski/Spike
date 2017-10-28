using EFCoreSpike5.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SpikeWebAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ControllersTest
{
    public class StudentControllerTest : IDisposable
    {
        private readonly TestServer server;
        private HttpClient client;
        private readonly EFCoreSpikeContext context;
        private string url = "api/students";
        private string studentName = "TestName";
        private string updatedName = "UpdatedName";
        private string contentType = "application/json";

        //https://github.com/aspnet/Mvc/issues/5562 https://github.com/aspnet/Home/issues/1558
        //dotnet core does not support System.Net.Http.Formatting yet. It works, but shows errors. Works in progress
        public StudentControllerTest()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..")))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var configuration = builder.Build();

            var optionsBuilder = new DbContextOptionsBuilder<EFCoreSpikeContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            context = new EFCoreSpikeContext(optionsBuilder.Options);

            server = new TestServer(new WebHostBuilder()
            .UseConfiguration(configuration)
            .UseStartup<Startup>());

        }

        public void Dispose()
        {
            CleanupCreatedStudent(studentName);
            CleanupCreatedStudent(updatedName);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task ShouldGetTwoElementsSortedByName()
        {
            // arrange
            var queryString = "?pageNumber=1&pageLimit=2&sort=Name";
            var request = url + queryString;
            HttpResponseMessage httpResponse;
            TestPagedResult<StudentTestResponse> response;
            List<StudentTestResponse> students = new List<StudentTestResponse>();
            string actual, expected;
            var studentsList = context.Students.Include(s => s.Course)
                .OrderBy(s => s.Name)
                .Skip(0).Take(2).ToList();

            foreach (var item in studentsList)
            {
                students.Add(CreateResponseStudent(item));
            }

            TestPagedResult<StudentTestResponse> expectedPagedResult = new TestPagedResult<StudentTestResponse>()
            {
                PageNumber = 1,
                PageSize = 2,
                TotalNumberOfPages = 2,
                TotalNumberOfRecords = 3,
                Results = students,
                FirstPageUrl = "/api/students?pageNumber=1&pageLimit=2&sort=Name",
                PreviousPageUrl = null,
                NextPageUrl = "/api/students?pageNumber=2&pageLimit=2&sort=Name",
                LastPageUrl = "/api/students?pageNumber=2&pageLimit=2&sort=Name",

            };
            // act
            using (client = server.CreateClient())
            {
                httpResponse = await client.GetAsync(request);
                response = await httpResponse.Content.ReadAsAsync<TestPagedResult<StudentTestResponse>>();
            }
            actual = JsonConvert.SerializeObject(response);
            expected = JsonConvert.SerializeObject(expectedPagedResult);
            // assert
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal(expected, actual);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task ShouldGetAllElementsSortedByName()
        {
            // arrange
            var queryString = "?sort=Name";
            var request = url + queryString;
            HttpResponseMessage httpResponse;
            List<Student> response, students;
            string actual, expected;
            // act
            using (client = server.CreateClient())
            {
                httpResponse = await client.GetAsync(request);
                response = await httpResponse.Content.ReadAsAsync<List<Student>>();
            }

            students = context.Students.OrderBy(s => s.Name).ToList();
            actual = JsonConvert.SerializeObject(response);
            expected = JsonConvert.SerializeObject(students);
            // assert
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal(expected, actual);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task ShouldGetFilteredStudentSortedByName()
        {
            // arrange
            var filterValue = "Martin";
            var queryString = $"?pageNumber=1&pageLimit=3&Name={filterValue}&sort=Name";
            var request = url + queryString;
            HttpResponseMessage httpResponse;
            TestPagedResult<StudentTestResponse> response;
            List<StudentTestResponse> students = new List<StudentTestResponse>();
            string actual, expected;
            var filteredStudents = context.Students.Include(s => s.Course)
                .Where(s => s.Name.Contains(filterValue))
                .OrderBy(s => s.Name)
                .Skip(0).Take(3).ToList();
            foreach (var item in filteredStudents)
            {
                students.Add(CreateResponseStudent(item));
            }

            TestPagedResult<StudentTestResponse> expectedPagedResult = new TestPagedResult<StudentTestResponse>()
            {
                PageNumber = 1,
                PageSize = 3,
                TotalNumberOfPages = 1,
                TotalNumberOfRecords = 1,
                Results = students,
                FirstPageUrl = $"/api/students?pageNumber=1&pageLimit=3&Name={filterValue}&sort=Name",
                PreviousPageUrl = null,
                NextPageUrl = null,
                LastPageUrl = $"/api/students?pageNumber=1&pageLimit=3&Name={filterValue}&sort=Name",

            };
            // act
            using (client = server.CreateClient())
            {
                httpResponse = await client.GetAsync(request);
                response = await httpResponse.Content.ReadAsAsync<TestPagedResult<StudentTestResponse>>();
            }
            actual = JsonConvert.SerializeObject(response);
            expected = JsonConvert.SerializeObject(expectedPagedResult);
            // assert
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal(expected, actual);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task ShouldGetSingleStudent()
        {
            // arrange
            int studentId = 1;
            var request = $"{url}/{studentId}";
            HttpResponseMessage httpResponse;
            Student student;
            StudentTestResponse response;
            string actual, expected;

            student = context.Students.Include(s => s.Course).FirstOrDefault(s => s.Id == studentId);
            var studentDTO = CreateResponseStudent(student);
            expected = JsonConvert.SerializeObject(studentDTO);
            // act
            using (client = server.CreateClient())
            {
                httpResponse = await client.GetAsync(request);
                response = await httpResponse.Content.ReadAsAsync<StudentTestResponse>();
            }
            actual = JsonConvert.SerializeObject(response);
            // assert
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal(expected, actual);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task ShouldAddNewStudent()
        {
            // arrange
            var newStudent = new StudentTestCreateRequest()
            {
                CourseId = 1,
                Name = studentName
            };

            string request = url;
            string studentJson = JsonConvert.SerializeObject(newStudent);
            var content = new StringContent(studentJson, Encoding.UTF8, contentType);

            HttpResponseMessage httpResponse;
            StudentTestResponse response;
            Student student;
            string actual, expected;

            // act
            using (client = server.CreateClient())
            {
                httpResponse = await client.PostAsync(request, content);
                response = await httpResponse.Content.ReadAsAsync<StudentTestResponse>();
            }
            actual = JsonConvert.SerializeObject(response);
            student = await GetStudentByName(studentName);
            var studentDTO = new StudentTestResponse()
            {
                Id = student.Id,
                Name = student.Name,
                Course = null
            };
            expected = JsonConvert.SerializeObject(studentDTO);
            // assert
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal(expected, actual);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task ShouldUpdateStudent()
        {
            // arrange
            int studentId = PrepareStudentForTest(studentName);
            var newStudent = new StudentTestUpdateRequest()
            {
                Id = studentId,
                CourseId = 1,
                Name = updatedName
            };

            var studentJson = JsonConvert.SerializeObject(newStudent);
            var content = new StringContent(studentJson, Encoding.UTF8, contentType);
            string request = $"{url}/{studentId}";

            HttpResponseMessage httpResponse;
            Student student;
            // act
            using (client = server.CreateClient())
            {
                httpResponse = await client.PutAsync(request, content);
            }
            student = await GetStudentByName(updatedName);
            // assert
            httpResponse.EnsureSuccessStatusCode();
            Assert.True(student != null);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task ShouldDeleteStudent()
        {
            // arrange
            int studentId = PrepareStudentForTest(updatedName);
            string request = $"{url}/{studentId}";
            HttpResponseMessage httpResponse;
            // act
            using (client = server.CreateClient())
            {
                httpResponse = await client.DeleteAsync(request);
            }
            var expectedStudent = await GetStudentByName(updatedName);
            // assert
            httpResponse.EnsureSuccessStatusCode();
            Assert.Equal(expectedStudent, null);
        }

        private async Task<Student> GetStudentByName(string name)
        {
            return await context.Students
                .Include(s => s.Course)
                .FirstOrDefaultAsync(x => x.Name == name);
        }

        private int PrepareStudentForTest(string name)
        {
            Student result;
            if ((result = context.Students.FirstOrDefault(x => x.Name == name)) != null)
            {
                return result.Id;
            }

            var student = new Student()
            {
                Name = name,
                CourseId = 1
            };

            context.Add(student);
            context.SaveChanges();

            return student.Id;
        }

        private void CleanupCreatedStudent(string name)
        {
            Student result;
            if ((result = context.Students.FirstOrDefault(x => x.Name == name)) != null)
            {
                context.Remove(result);
                context.SaveChanges();
            }
        }

        private StudentTestResponse CreateResponseStudent(Student student)
        {
            CourseTestResponse courseDTO = null;
            if (student.Course != null)
            {
                courseDTO = new CourseTestResponse()
                {
                    Name = student.Course.Name
                };
            }
            var studentDTO = new StudentTestResponse()
            {
                Id = student.Id,
                Name = student.Name,
                Course = courseDTO
            };

            return studentDTO;
        }
    }
}
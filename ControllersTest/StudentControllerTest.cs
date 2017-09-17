using EFCoreSpike5.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SpikeRepo.Repositories;
using SpikeWebAPI;
using System;
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
        private readonly HttpClient client;
        private readonly EFCoreSpikeContext context;
        private string url = "api/students";
        private string studentName = "TestName";
        private string updatedName = "UpdatedName";
        private string contentType = "application/json";
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
            client = server.CreateClient();
        }

        public void Dispose()
        {
            CleanupCreatedStudent(studentName);
            CleanupCreatedStudent(updatedName);
        }

        [Fact]
        public async Task ShouldGetThreeElementsSortedByName()
        {
            // arrange
            string expected = @"[{""Id"":1,""CourseId"":1,""Name"":""Martin B"",""Course"":null},{""Id"":3,""CourseId"":1,""Name"":""SomeRandomStudent"",""Course"":null},{""Id"":2,""CourseId"":1,""Name"":""Witalian"",""Course"":null}]";
            var queryString = "?pageNumber=1&pageLimit=3&sort=Name";
            var request = url + queryString;
            // act
            var response = await client.GetAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();
            // assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(expected, responseString);
        }

        [Fact]
        public async Task ShouldGetSingleStudent()
        {
            // arrange
            string expected = @"{""Id"":1,""CourseId"":1,""Name"":""Martin B"",""Course"":null}";
            string studentId = "/1";
            var request = url + studentId;
            // act
            var response = await client.GetAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();
            // assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(expected, responseString);
        }

        [Fact]
        public async Task ShouldAddNewStudent()
        {
            // arrange
            var student = new StudentCreateRequest()
            {
                CourseId = 1,
                Name = studentName
            };
            string studentJson = JsonConvert.SerializeObject(student);
            var content = new StringContent(studentJson, Encoding.UTF8, contentType);
            string request = url;
            // act
            var response = await client.PostAsync(request, content);
            var responseString = await response.Content.ReadAsStringAsync();
            // assert
            var expectedStudent = await GetStudentByName(studentName);
            var expected = JsonConvert.SerializeObject(expectedStudent);
            response.EnsureSuccessStatusCode();
            Assert.Equal(expected, responseString);
        }

        [Fact]
        public async Task ShouldUpdateStudent()
        {
            // arrange
            int studentId = PrepareStudentForTest(studentName);
            var student = new StudentUpdateRequest()
            {
                Id = studentId,
                CourseId = 1,
                Name = updatedName
            };
            var studentJson = JsonConvert.SerializeObject(student);
            var content = new StringContent(studentJson, Encoding.UTF8, contentType);
            string request = $"{url}/{studentId}";
            // act
            var response = await client.PutAsync(request, content);
            // assert
            response.EnsureSuccessStatusCode();
            var expected = JsonConvert.SerializeObject(student);
            var actualStudent = await GetStudentByName(updatedName);
            var result = JsonConvert.SerializeObject(actualStudent);
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task ShouldDeleteStudent()
        {
            // arrange
            int studentId = PrepareStudentForTest(updatedName);
            string request = $"{url}/{studentId}";
            // act
            var response = await client.DeleteAsync(request);
            // assert
            var expectedStudent = await GetStudentByName(updatedName);
            response.EnsureSuccessStatusCode();
            Assert.Equal(expectedStudent, null);
        }

        private async Task<Student> GetStudentByName(string name)
        {
            return await context.Students.FirstOrDefaultAsync(x => x.Name == name);
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
            }
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Spike.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Spike.WebApi.IntegrationTest
{
    public class StudentControllerTest : IDisposable, IClassFixture<ControllerFixture>
    {
        //private HttpClient client;
        private ControllerFixture fixture;
        private HttpResponseMessage httpResponse;
        string actual, expected;

        private string url = "api/students";
        private string studentName = "TestName";
        private string updatedName = "UpdatedName";
        private string contentType = "application/json";

        //https://github.com/aspnet/Mvc/issues/5562 https://github.com/aspnet/Home/issues/1558
        //dotnet core does not support System.Net.Http.Formatting yet. It works, but shows errors. Works in progress
        public StudentControllerTest(ControllerFixture fixture)
        {
            this.fixture = fixture;
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
            int pageSize = 2;
            var queryString = $"?pageNumber=1&pageLimit={pageSize}&sort=Name";
            var request = url + queryString;
            var students = PrepareStudentsResponse(pageSize);
            TestPagedResult<StudentTestResponse> response;
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
            using (var client = fixture.server.CreateClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.authenticationToken);
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
            List<Student> response;
            // act
            using (var client = fixture.server.CreateClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.authenticationToken);
                httpResponse = await client.GetAsync(request);
                response = await httpResponse.Content.ReadAsAsync<List<Student>>();
            }

            var students = fixture.context.Students.OrderBy(s => s.Name).ToList();
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
            var pageSize = 3;
            var filterValue = "Witalian";
            var queryString = $"?pageNumber=1&pageLimit={pageSize}&Name={filterValue}&sort=Name";
            var request = url + queryString;
            var students = PrepareStudentsResponse(pageSize, filterValue);
            TestPagedResult<StudentTestResponse> response;
            TestPagedResult<StudentTestResponse> expectedPagedResult = new TestPagedResult<StudentTestResponse>()
            {
                PageNumber = 1,
                PageSize = pageSize,
                TotalNumberOfPages = 1,
                TotalNumberOfRecords = 1,
                Results = students,
                FirstPageUrl = $"/api/students?pageNumber=1&pageLimit={pageSize}&Name={filterValue}&sort=Name",
                PreviousPageUrl = null,
                NextPageUrl = null,
                LastPageUrl = $"/api/students?pageNumber=1&pageLimit={pageSize}&Name={filterValue}&sort=Name",

            };
            // act
            using (var client = fixture.server.CreateClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.authenticationToken);
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
            StudentTestResponse response;

            var student = fixture.context.Students.Include(s => s.Course).FirstOrDefault(s => s.Id == studentId);
            var studentDTO = CreateResponseStudent(student);
            expected = JsonConvert.SerializeObject(studentDTO);
            // act
            using (var client = fixture.server.CreateClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.authenticationToken);
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

            StudentTestResponse response;

            // act
            using (var client = fixture.server.CreateClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.authenticationToken);
                httpResponse = await client.PostAsync(request, content);
                response = await httpResponse.Content.ReadAsAsync<StudentTestResponse>();
            }
            actual = JsonConvert.SerializeObject(response);
            var student = await GetStudentByName(studentName);
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
            int studentId = GetOrCreateStudentForTest(studentName);
            var newStudent = new StudentTestUpdateRequest()
            {
                Id = studentId,
                CourseId = 1,
                Name = updatedName
            };

            var studentJson = JsonConvert.SerializeObject(newStudent);
            var content = new StringContent(studentJson, Encoding.UTF8, contentType);
            string request = $"{url}/{studentId}";

            // act
            using (var client = fixture.server.CreateClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.authenticationToken);
                httpResponse = await client.PutAsync(request, content);
            }
            var student = await GetStudentByName(updatedName);
            // assert
            httpResponse.EnsureSuccessStatusCode();
            Assert.True(student != null);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task ShouldDeleteStudent()
        {
            // arrange
            int studentId = GetOrCreateStudentForTest(updatedName);
            string request = $"{url}/{studentId}";
            // act
            using (var client = fixture.server.CreateClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.authenticationToken);
                httpResponse = await client.DeleteAsync(request);
            }
            var expectedStudent = await GetStudentByName(updatedName);
            // assert
            httpResponse.EnsureSuccessStatusCode();
            Assert.Null(expectedStudent);
        }

        private async Task<Student> GetStudentByName(string name)
        {
            return await fixture.context.Students
                .Include(s => s.Course)
                .FirstOrDefaultAsync(x => x.Name == name);
        }

        private int GetOrCreateStudentForTest(string name)
        {
            Student result;
            if ((result = fixture.context.Students.FirstOrDefault(x => x.Name == name)) != null)
            {
                return result.Id;
            }

            var student = new Student()
            {
                Name = name,
                CourseId = 1
            };

            fixture.context.Add(student);
            fixture.context.SaveChanges();

            return student.Id;
        }

        private void CleanupCreatedStudent(string name)
        {
            Student result;
            if ((result = fixture.context.Students.FirstOrDefault(x => x.Name == name)) != null)
            {
                fixture.context.Remove(result);
                fixture.context.SaveChanges();
            }
        }

        private List<StudentTestResponse> PrepareStudentsResponse(int pageSize, string filterValue = "")
        {
            List<StudentTestResponse> studentsResult = new List<StudentTestResponse>();

            var students = LoadStudents(pageSize, filterValue);
            foreach (var item in students)
            {
                studentsResult.Add(CreateResponseStudent(item));
            }

            return studentsResult;
        }

        private List<Student> LoadStudents(int pageSize, string filterValue)
        {
            return fixture.context.Students.Include(s => s.Course)
                .Where(s => s.Name.Contains(filterValue))
                .OrderBy(s => s.Name)
                .Skip(0).Take(pageSize).ToList();
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
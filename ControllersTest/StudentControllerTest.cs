using FluentAssertions;
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
        private readonly ControllerFixture fixture;
        private HttpResponseMessage httpResponse;

	    private const string url = "api/students";
	    private const string studentName = "TestName";
	    private const string updatedName = "UpdatedName";
        private const string contentType = "application/json";

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
            const int pageSize = 2;
            var queryString = $"?pageNumber=1&pageLimit={pageSize}&sort=Name";
            var request = url + queryString;
            var students = PrepareStudentsResponse(pageSize);
            TestPagedResult<StudentTestResponse> response;
            var expectedPagedResult = new TestPagedResult<StudentTestResponse>()
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
            using (var client = fixture.Server.CreateClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.AuthenticationToken);
                httpResponse = await client.GetAsync(request);
                response = await httpResponse.Content.ReadAsAsync<TestPagedResult<StudentTestResponse>>();
            }
            // assert
            httpResponse.EnsureSuccessStatusCode();
            response.Should().BeEquivalentTo(expectedPagedResult);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task ShouldGetAllElementsSortedByName()
        {
            // arrange
            var queryString = "?sort=Name";
            var request = $"{url}/all{queryString}";
            List<Student> response;
            // act
            using (var client = fixture.Server.CreateClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.AuthenticationToken);
                httpResponse = await client.GetAsync(request);
                response = await httpResponse.Content.ReadAsAsync<List<Student>>();
            }

            var students = fixture.Context.Students.OrderBy(s => s.Name).ToList();
            // assert
            httpResponse.EnsureSuccessStatusCode();
            response.Should().BeEquivalentTo(students);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task ShouldGetFilteredStudentSortedByName()
        {
            // arrange
            const int pageSize = 3;
            const string filterValue = "Witalian";
            var queryString = $"?pageNumber=1&pageLimit={pageSize}&Name={filterValue}&sort=Name";
            var request = url + queryString;
            var students = PrepareStudentsResponse(pageSize, filterValue);
            TestPagedResult<StudentTestResponse> response;
            var expectedPagedResult = new TestPagedResult<StudentTestResponse>()
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
            using (var client = fixture.Server.CreateClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.AuthenticationToken);
                httpResponse = await client.GetAsync(request);
                response = await httpResponse.Content.ReadAsAsync<TestPagedResult<StudentTestResponse>>();
            }
            // assert
            httpResponse.EnsureSuccessStatusCode();
            response.Should().BeEquivalentTo(expectedPagedResult);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task ShouldGetSingleStudent()
        {
            // arrange
            const int studentId = 1;
            var request = $"{url}/{studentId}";
            StudentTestResponse response;

            var student = fixture.Context.Students.Include(s => s.Course).FirstOrDefault(s => s.Id == studentId);
            var studentDto = CreateResponseStudent(student);
            // act
            using (var client = fixture.Server.CreateClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.AuthenticationToken);
                httpResponse = await client.GetAsync(request);
                response = await httpResponse.Content.ReadAsAsync<StudentTestResponse>();
            }
            // assert
            httpResponse.EnsureSuccessStatusCode();
            response.Should().BeEquivalentTo(studentDto);
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

            const string request = url;
            var studentJson = JsonConvert.SerializeObject(newStudent);
            var content = new StringContent(studentJson, Encoding.UTF8, contentType);

            StudentTestResponse response;

            // act
            using (var client = fixture.Server.CreateClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.AuthenticationToken);
                httpResponse = await client.PostAsync(request, content);
                response = await httpResponse.Content.ReadAsAsync<StudentTestResponse>();
            }
            var student = await GetStudentByName(studentName);
            var studentDto = new StudentTestResponse()
            {
                Id = student.Id,
                Name = student.Name,
                Course = null
            };
            // assert
            httpResponse.EnsureSuccessStatusCode();
            response.Should().BeEquivalentTo(studentDto);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task ShouldUpdateStudent()
        {
            // arrange
            var studentId = GetOrCreateStudentForTest(studentName);
            var newStudent = new StudentTestUpdateRequest()
            {
                Id = studentId,
                CourseId = 1,
                Name = updatedName
            };

            var studentJson = JsonConvert.SerializeObject(newStudent);
            var content = new StringContent(studentJson, Encoding.UTF8, contentType);
            var request = $"{url}/{studentId}";

            // act
            using (var client = fixture.Server.CreateClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.AuthenticationToken);
                httpResponse = await client.PutAsync(request, content);
            }
            var student = await GetStudentByName(updatedName);
            // assert
            httpResponse.EnsureSuccessStatusCode();
            student.Should().BeOfType<Student>().And.NotBeNull();
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task ShouldDeleteStudent()
        {
            // arrange
            var studentId = GetOrCreateStudentForTest(updatedName);
            var request = $"{url}/{studentId}";
            // act
            using (var client = fixture.Server.CreateClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fixture.AuthenticationToken);
                httpResponse = await client.DeleteAsync(request);
            }
            var expectedStudent = await GetStudentByName(updatedName);
            // assert
            httpResponse.EnsureSuccessStatusCode();
            expectedStudent.Should().BeNull();
        }

        private async Task<Student> GetStudentByName(string name)
        {
            return await fixture.Context.Students
                .Include(s => s.Course)
                .FirstOrDefaultAsync(x => x.Name == name);
        }

        private int GetOrCreateStudentForTest(string name)
        {
            Student result;
            if ((result = fixture.Context.Students.FirstOrDefault(x => x.Name == name)) != null)
            {
                return result.Id;
            }

            var student = new Student()
            {
                Name = name,
                CourseId = 1
            };

            fixture.Context.Add(student);
            fixture.Context.SaveChanges();

            return student.Id;
        }

        private void CleanupCreatedStudent(string name)
        {
            Student result;
            if ((result = fixture.Context.Students.FirstOrDefault(x => x.Name == name)) != null)
            {
                fixture.Context.Remove(result);
                fixture.Context.SaveChanges();
            }
        }

        private List<StudentTestResponse> PrepareStudentsResponse(int pageSize, string filterValue = "")
        {
	        var students = LoadStudents(pageSize, filterValue);

	        return students.Select(CreateResponseStudent).ToList();
        }

        private IEnumerable<Student> LoadStudents(int pageSize, string filterValue)
        {
            return fixture.Context.Students.Include(s => s.Course)
                .Where(s => s.Name.Contains(filterValue))
                .OrderBy(s => s.Name)
                .Skip(0).Take(pageSize).ToList();
        }

        private static StudentTestResponse CreateResponseStudent(Student student)
        {
            CourseTestResponse courseDto = null;
            if (student.Course != null)
            {
                courseDto = new CourseTestResponse
                {
                    Name = student.Course.Name
                };
            }
            var studentDto = new StudentTestResponse
            {
                Id = student.Id,
                Name = student.Name,
                Course = courseDto
            };

            return studentDto;
        }
    }
}
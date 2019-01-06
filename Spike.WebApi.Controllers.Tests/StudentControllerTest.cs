using Xunit;
using Spike.Core.Interface;
using Moq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Spike.Core.Entity;
using Spike.WebApi.Types.DTOs;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using AutoSFaP.Models;

namespace Spike.WebApi.Controllers.Tests
{
    public class StudentControllerTest
    {
        private readonly Mock<IStudentRepository> repository;
        private readonly Mock<IMapper> mapper;
        private readonly Mock<ILogger<StudentsController>> logger;
        private readonly Mock<IAuthorizationService> authorizationService;
        public StudentControllerTest()
        {
            repository = new Mock<IStudentRepository>();
            mapper = new Mock<IMapper>();
            logger = new Mock<ILogger<StudentsController>>();
            authorizationService = new Mock<IAuthorizationService>();
        }

        [Fact]
        public async Task ShouldGetAllStudents()
        {
            // arrange
            mapper.Setup(x => x.Map<string, SortField<Student>[]>(It.IsAny<string>()))
                .Returns(new SortField<Student>[0]);
            mapper.Setup(x => x.Map<StudentFilterDto, FilterField<Student>[]>(It.IsAny<StudentFilterDto>()))
                .Returns(new FilterField<Student>[0]);
            repository.Setup(x => x.GetAsync(It.IsAny<SortField<Student>[]>(), It.IsAny<FilterField<Student>[]>()))
                .Returns(GetStudents());
            var controller = new StudentsController(repository.Object, mapper.Object, logger.Object, authorizationService.Object);
            // act
            var result = await controller.GetAllStudents(null, null);
            var okResult = result.Result.As<OkObjectResult>();
            var students = okResult.Value.As<List<Student>>();
            // assert
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            students.Should().HaveCount(await GetStudents().Count());
        }

        [Fact]
        public async Task ShouldReturnBadRequestWhenInvalidDataProvided()
        {
            // arrange
            mapper.Setup(x => x.Map<string, SortField<Student>[]>(It.IsAny<string>()))
                .Throws(new ArgumentException());
            mapper.Setup(x => x.Map<StudentFilterDto, FilterField<Student>[]>(It.IsAny<StudentFilterDto>()))
                .Throws(new ArgumentException());
            var controller = new StudentsController(repository.Object, mapper.Object, logger.Object, authorizationService.Object);
            // act
            var result = await controller.GetAllStudents(null, null);
            var badResult = result.Result.As<BadRequestObjectResult>();
            // assert
            badResult.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task ShouldReturnOneStudent()
        {
            // arrange
            repository.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .Returns(GetStudent());
            authorizationService.Setup(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<Student>(), It.IsAny<string>()))
                .ReturnsAsync(AuthorizationResult.Success());
            mapper.Setup(x => x.Map<Student, StudentResponseDataTransferObject>(It.IsAny<Student>()))
                .Returns(await GetStudentResponse());
            var controller = new StudentsController(repository.Object, mapper.Object, logger.Object, authorizationService.Object);
            // act
            var result = await controller.GetStudent(It.IsAny<int>());
            var okResult = result.Result.As<OkObjectResult>();
            var student = okResult.Value.As<StudentResponseDataTransferObject>();
            // assert
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            student.Should().BeEquivalentTo(await GetStudentResponse());
        }

        [Fact]
        public async Task ShouldReturnUrlForCreatedStudent()
        {
            // arrange
            mapper.Setup(x => x.Map<StudentCreateRequestDataTransferObject, Student>(It.IsAny<StudentCreateRequestDataTransferObject>()))
                .Returns(await GetStudent());
            mapper.Setup(x => x.Map<Student, StudentResponseDataTransferObject>(It.IsAny<Student>()))
                .Returns(await GetStudentResponse());

            var controller = new StudentsController(repository.Object, mapper.Object, logger.Object, authorizationService.Object);
            // act
            var result = await controller.PostStudent(It.IsAny<StudentCreateRequestDataTransferObject>());
            var createdResult = result.Result.As<CreatedAtActionResult>();
            // assert
            createdResult.StatusCode.Should().Be(StatusCodes.Status201Created);
            createdResult.ActionName.Should().Be("GetStudent");
        }

        private IAsyncEnumerable<Student> GetStudents()
        {
            return new Student[]
            {
                new Student { Id = 1, Name = "Test1", CourseId = 2 },
                new Student { Id = 2, Name = "Test2", CourseId = 1 }
            }.ToAsyncEnumerable();
        }

        private Task<Student> GetStudent()
        {
            return GetStudents().First();
        }

        private async Task<StudentResponseDataTransferObject> GetStudentResponse()
        {
            var student = await GetStudent();
            return new StudentResponseDataTransferObject
            {
                Id = student.Id,
                Name = student.Name
            };
        }
    }
}

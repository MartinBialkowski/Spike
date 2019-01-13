using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spike.Core.Interface;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Spike.Core.Entity;
using Spike.WebApi.Types.DTOs;
using Microsoft.Extensions.Logging;
using AutoSFaP.Models;

namespace Spike.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/students")]
    [ApiController]
    public class StudentsController : Controller
    {
        private readonly IStudentRepository studentRepository;
        private readonly IAuthorizationService authorizationService;
        private readonly IMapper mapper;
        private readonly ILogger<StudentsController> logger;
        public StudentsController(IStudentRepository studentRepository, IMapper mapper,
            ILogger<StudentsController> logger, IAuthorizationService authorizationService)
        {
            this.studentRepository = studentRepository;
            this.mapper = mapper;
            this.logger = logger;
            this.authorizationService = authorizationService;
        }

        // GET: /api/students?sort=CourseId,Name-
        [Authorize]
        [HttpGet("all")]
        [ProducesResponseType(typeof(Student), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 401)]
        [ProducesResponseType(typeof(void), 403)]
        public async Task<ActionResult<Student>> GetAllStudents([FromQuery] StudentFilterDto filterDto, [FromQuery] string sort = "Id")
        {
            SortField<Student>[] sortFields;
            FilterField<Student>[] filterFields;
            try
            {
                sortFields = mapper.Map<string, SortField<Student>[]>(sort);
                filterFields = mapper.Map<StudentFilterDto, FilterField<Student>[]>(filterDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(await studentRepository.GetAsync(sortFields, filterFields).ToList());
        }

        // GET: /api/students?pageNumber=1&pageLimit=3&Name=Martin&sort=CourseId,Name-
        [Authorize(Policy = "StudentDiscount")]
        [HttpGet]
        [ProducesResponseType(typeof(PagedResultDataTransferObject<StudentResponseDataTransferObject>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 401)]
        [ProducesResponseType(typeof(void), 403)]
        public async Task<ActionResult<PagedResultDataTransferObject<StudentResponseDataTransferObject>>> GetStudents([FromQuery] PagingDto pagingDto, [FromQuery] StudentFilterDto filterDto, [FromQuery] string sort = "Id")
        {
            SortField<Student>[] sortFields;
            FilterField<Student>[] filterFields;
            try
            {
                sortFields = mapper.Map<string, SortField<Student>[]>(sort);
                filterFields = mapper.Map<StudentFilterDto, FilterField<Student>[]>(filterDto);
            }
            catch (Exception ex) when (ex is ArgumentException)
            {
                return BadRequest(ex.Message);
            }
            var paging = mapper.Map<PagingDto, Paging>(pagingDto);
            var pagedResult = await studentRepository.GetAsync(paging, sortFields, filterFields);
            return Ok(CreatePagedResultDto<Student, StudentResponseDataTransferObject>(pagedResult, paging, sort, filterDto));
        }

        // GET: api/students/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(StudentResponseDataTransferObject), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 403)]
        public async Task<ActionResult<StudentResponseDataTransferObject>> GetStudent(int id)
        {
            var student = await studentRepository.GetByIdAsync(id);

            if (student == null)
            {
                return NotFound($"Student by Id: {id} not found.");
            }

            var authorizationResult = await authorizationService.AuthorizeAsync(User, student, "GetSelf");
            if (authorizationResult.Succeeded)
            {
                return Ok(mapper.Map<Student, StudentResponseDataTransferObject>(student));
            }
            if (User.Identity.IsAuthenticated)
            {
                return Forbid();
            }
            return Challenge();


        }

        // PUT: api/students/5
        [Authorize(Policy = "Master")]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> PutStudent(int id, StudentUpdateRequestDataTransferObject studentDto)
        {
            if (id != studentDto.Id)
            {
                return BadRequest($"Provided student Id: {studentDto.Id} not match id from url {id}.");
            }

            var student = mapper.Map<StudentUpdateRequestDataTransferObject, Student>(studentDto);
            studentRepository.Update(student);

            try
            {
                await studentRepository.CommitAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await studentRepository.GetByIdAsync(id) == null)
                {
                    return NotFound($"Student by id: {id} not exist.");
                }
                throw;
            }

            return NoContent();
        }

        // POST: api/students
        [HttpPost]
        [ProducesResponseType(typeof(StudentResponseDataTransferObject), 201)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<ActionResult<StudentResponseDataTransferObject>> PostStudent(StudentCreateRequestDataTransferObject studentDto)
        {
            var student = mapper.Map<StudentCreateRequestDataTransferObject, Student>(studentDto);
            studentRepository.Add(student);
            await studentRepository.CommitAsync();
            var studentResponse = mapper.Map<Student, StudentResponseDataTransferObject>(student);
            return CreatedAtAction("GetStudent", new { id = student.Id }, studentResponse);
        }

        // DELETE: api/students/5
        [Authorize(Policy = "Master")]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await studentRepository.GetByIdAsync(id);
            if (student == null)
            {
                return NotFound($"Student by id: {id} not exist.");
            }

            studentRepository.Delete(student);
            await studentRepository.CommitAsync();

            return NoContent();
        }

        private PagedResultDataTransferObject<TReturn> CreatePagedResultDto<T, TReturn>(PagedResult<T> result, Paging paging, string sort, StudentFilterDto filterDto)
        {
            const int firstPage = 1;
            return new PagedResultDataTransferObject<TReturn>()
            {
                PageNumber = paging.PageNumber,
                PageSize = paging.PageLimit,
                TotalNumberOfPages = result.TotalNumberOfPages,
                TotalNumberOfRecords = result.TotalNumberOfRecords,
                Results = mapper.Map<List<T>, List<TReturn>>(result.Results),
                FirstPageUrl = PrepareUrlPage(firstPage, paging.PageLimit, sort, filterDto),
                LastPageUrl = PrepareUrlPage(result.TotalNumberOfPages, paging.PageLimit, sort, filterDto),
                NextPageUrl = paging.PageNumber < result.TotalNumberOfPages ? PrepareUrlPage(paging.PageNumber + 1, paging.PageLimit, sort, filterDto) : null,
                PreviousPageUrl = paging.PageNumber > firstPage ? PrepareUrlPage(paging.PageNumber - 1, paging.PageLimit, sort, filterDto) : null
            };
        }

        private string PrepareUrlPage(int pageNumber, int pageLimit, string sortFields, StudentFilterDto filterDto)
        {
            return Url.Action("GetStudents", new
            {
                pageNumber,
                pageLimit,
                filterDto?.Id,
                filterDto?.Name,
                filterDto?.CourseId,
                sort = sortFields
            });
        }
    }
}
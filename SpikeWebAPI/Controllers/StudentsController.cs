using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spike.Core.Interface;
using SpikeWebAPI.DTOs;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Spike.Core.Model;
using Spike.Core.Entity;

namespace Spike.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/students")]
    public class StudentsController : Controller
    {
        private readonly IStudentRepository studentRepository;
        private readonly IMapper mapper;

        public StudentsController(IStudentRepository studentRepository, IMapper mapper)
        {
            this.studentRepository = studentRepository;
            this.mapper = mapper;
        }

        // GET: /api/students?pageNumber=1&pageLimit=3&Name=Martin&sort=CourseId,Name-
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetStudents(PagingDTO pagingDTO, StudentFilterDTO filterDTO, string sort = "Id")
        {
            SortField<Student>[] sortFields;
            FilterField<Student>[] filterFields;
            try
            {
                sortFields = mapper.Map<string, SortField<Student>[]>(sort);
                filterFields = mapper.Map<StudentFilterDTO, FilterField<Student>[]>(filterDTO);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is ArgumentNullException)
            {
                return BadRequest(ex.Message);
            }
            if (pagingDTO.PageNumber == null && pagingDTO.PageLimit == null)
            {
                return Ok(await studentRepository.GetAsync(sortFields, filterFields).ToList());
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Paging paging = mapper.Map<PagingDTO, Paging>(pagingDTO);
            var pagedResult = await studentRepository.GetAsync(paging, sortFields, filterFields);
            return Ok(CreatePagedResultDTO<Student, StudentResponseDataTransferObject>(pagedResult, paging, sort, filterDTO));
        }

        // GET: api/students/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var student = await studentRepository.GetByIdAsync(id);

            if (student == null)
            {
                return NotFound($"Student by Id: {id} not found.");
            }

            return Ok(mapper.Map<Student, StudentResponseDataTransferObject>(student));
        }

        // PUT: api/students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent([FromRoute] int id, [FromBody] StudentUpdateRequestDataTransferObject studentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != studentDTO.Id)
            {
                return BadRequest($"Provided student Id: {studentDTO.Id} not match id from url {id}.");
            }

            var student = mapper.Map<StudentUpdateRequestDataTransferObject, Student>(studentDTO);
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
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/students
        [HttpPost]
        public async Task<IActionResult> PostStudent([FromBody] StudentCreateRequestDataTransferObject studentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var student = mapper.Map<StudentCreateRequestDataTransferObject, Student>(studentDTO);
            studentRepository.Add(student);
            await studentRepository.CommitAsync();
            var studentResponse = mapper.Map<Student, StudentResponseDataTransferObject>(student);
            return CreatedAtAction("GetStudent", new { id = student.Id }, studentResponse);
        }

        // DELETE: api/students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var student = await studentRepository.GetByIdAsync(id);
            if (student == null)
            {
                return NotFound($"Student by id: {id} not exist.");
            }

            studentRepository.Delete(student);
            await studentRepository.CommitAsync();

            return NoContent();
        }

        private PagedResultDataTransferObject<TReturn> CreatePagedResultDTO<T, TReturn>(PagedResult<T> result, Paging paging, string sort, StudentFilterDTO filterDTO)
        {
            const int firstPage = 1;
            return new PagedResultDataTransferObject<TReturn>()
            {
                PageNumber = paging.PageNumber,
                PageSize = paging.PageLimit,
                TotalNumberOfPages = result.TotalNumberOfPages,
                TotalNumberOfRecords = result.TotalNumberOfRecords,
                Results = mapper.Map<List<T>, List<TReturn>>(result.Results),
                FirstPageUrl = PrepareUrlPage(firstPage, paging.PageLimit, sort, filterDTO),
                LastPageUrl = PrepareUrlPage(result.TotalNumberOfPages, paging.PageLimit, sort, filterDTO),
                NextPageUrl = paging.PageNumber < result.TotalNumberOfPages ? PrepareUrlPage(paging.PageNumber + 1, paging.PageLimit, sort, filterDTO) : null,
                PreviousPageUrl = paging.PageNumber > firstPage ? PrepareUrlPage(paging.PageNumber - 1, paging.PageLimit, sort, filterDTO) : null
            };
        }

        private string PrepareUrlPage(int pageNumber, int pageLimit, string sortFields, StudentFilterDTO filterDTO)
        {
            return Url.Action("GetStudents", new
            {
                pageNumber,
                pageLimit,
                filterDTO?.Id,
                filterDTO?.Name,
                filterDTO?.CourseId,
                sort = sortFields
            });
        }
    }
}
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFCoreSpike5.Models;
using SpikeRepo.Abstract;
using EFCoreSpike5.ConstraintsModels;
using SpikeWebAPI.DTOs;
using AutoMapper;
using System;
using System.Collections.Generic;

namespace SpikeWebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/students")]
    public class StudentsController : Controller
    {
        private readonly IStudentRepository studentRepository;

        public StudentsController(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
        }

        // GET: /api/students?pageNumber=1&pageLimit=3&sort=CourseId,Name-
        [HttpGet("", Name = "GetStudents")]
        public async Task<IActionResult> GetStudents(Paging paging, string sort = "Id")
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            SortField<Student>[] sortFields;
            try
            {
                sortFields = Mapper.Map<string, SortField<Student>[]>(sort);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            var pagedResult = await studentRepository.GetAsync(paging, sortFields);
            return Ok(CreatePagedResultDTO<Student, StudentResponseDataTransferObject>(pagedResult, paging, sort));
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

            return Ok(Mapper.Map<Student, StudentResponseDataTransferObject>(student));
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

            var student = Mapper.Map<StudentUpdateRequestDataTransferObject, Student>(studentDTO);
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

            var student = Mapper.Map<StudentCreateRequestDataTransferObject, Student>(studentDTO);
            studentRepository.Add(student);
            await studentRepository.CommitAsync();

            return CreatedAtAction("GetStudent", new { id = student.Id }, student);
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
            //return Ok(student);
        }

        private PagedResultDataTransferObject<TReturn> CreatePagedResultDTO<T, TReturn>(PagedResult<T> result, Paging paging, string sort)
        {
            const int firstPage = 1;
            return new PagedResultDataTransferObject<TReturn>()
            {
                PageNumber = paging.PageNumber,
                PageSize = paging.PageLimit,
                TotalNumberOfPages = result.TotalNumberOfPages,
                TotalNumberOfRecords = result.TotalNumberOfRecords,
                Results = Mapper.Map<List<T>, List<TReturn>>(result.Results),
                FirstPageUrl = PrepareUrlPage(firstPage, paging.PageLimit, sort),
                LastPageUrl = PrepareUrlPage(result.TotalNumberOfPages, paging.PageLimit, sort),
                NextPageUrl = paging.PageNumber < result.TotalNumberOfPages ? PrepareUrlPage(paging.PageNumber + 1, paging.PageLimit, sort) : null,
                PreviousPageUrl = paging.PageNumber > firstPage ? PrepareUrlPage(paging.PageNumber - 1, paging.PageLimit, sort) : null
            };
        }

        private string PrepareUrlPage(int pageNumber, int pageLimit, string sortFields)
        {
            return Url.Link("GetStudents", new
            {
                pageNumber = pageNumber,
                pageLimit = pageLimit,
                sort = sortFields
            });
        }
    }
}
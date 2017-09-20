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
        [HttpGet]
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

            return Ok(await studentRepository.GetAsync(paging, sortFields).ToList());
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
                string message = $"Student with id: {id} does not exist";
                return NotFound(message);
            }

            return Ok(student);
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
                string message = $"Mismatch, student id: {studentDTO.Id} does not equal id : {id}";
                return BadRequest(message);
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
                    string message = $"Student with id: {id} does not exist";
                    return NotFound(message);
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
                string message = $"Student with id: {id} does not exist";
                return NotFound(message);
            }

            studentRepository.Delete(student);
            await studentRepository.CommitAsync();

            return NoContent();
            //return Ok(student);
        }
    }
}
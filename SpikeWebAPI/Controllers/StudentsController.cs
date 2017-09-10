﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFCoreSpike5.Models;
using SpikeRepo.Abstract;
using EFCoreSpike5.ConstraintsModels;
using SpikeWebAPI.DTOs;

namespace SpikeWebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/students")]
    public class StudentsController : Controller
    {
        private readonly EFCoreSpikeContext _context;
        private readonly IStudentRepository studentRepository;

        public StudentsController(EFCoreSpikeContext context, IStudentRepository studentRepository)
        {
            _context = context;
            this.studentRepository = studentRepository;
        }

        // GET: /api/students?pageNumber=1&pageLimit=1&sort=CourseId,-Name
        [HttpGet]
        public async Task<ICollection<Student>> GetStudents(Paging paging, string sort = "Id")
        {
            var sortFields = new SortField<Student>[2];
            sortFields[0] = new SortField<Student>
            {
                PropertyName = "CourseId",
                SortOrder = EFCoreSpike5.CommonModels.SortOrder.Ascending
            };
            sortFields[1] = new SortField<Student>
            {
                PropertyName = "Name",
                SortOrder = EFCoreSpike5.CommonModels.SortOrder.Descending
            };
            return await studentRepository.GetAsync(paging, sortFields).ToList();
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
                return NotFound();
            }

            return Ok(student);
        }

        // PUT: api/students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent([FromRoute] int id, [FromBody] StudentUpdateRequestDataTransferObject student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != student.Id)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
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
        public async Task<IActionResult> PostStudent([FromBody] StudentCreateRequestDataTransferObject student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //_context.Students.Add(student);
            //await _context.SaveChangesAsync();

            return null;
            //return CreatedAtAction("GetStudent", new { id = student.Id }, student);
        }

        // DELETE: api/students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var student = await _context.Students.SingleOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return Ok(student);
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
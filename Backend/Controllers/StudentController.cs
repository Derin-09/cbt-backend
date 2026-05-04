using Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            try
            {
                var students = await _context.Students.Include(s => s.AppUser)
                    .Select(s => new StudentDto
                    {
                        Id = s.Id,
                        MatricNo = s.MatricNo,
                        FullName = s.FullName,
                        Department = s.Department,
                        UserName = s.AppUser != null ? s.AppUser.UserName : null,
                        Email = s.AppUser != null ? s.AppUser.Email : null
                    })
                    .ToListAsync();
                return Ok(students);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(Student student)
        {
            try
            {
                _context.Students.Add(student);
                await _context.SaveChangesAsync();
                return Ok("Created Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetStudent(int id)
        {
            try
            {
                var student = await _context.Students.Include(s => s.AppUser)
                    .Where(s => s.Id == id)
                    .Select(s => new StudentDto
                    {
                        Id = s.Id,
                        MatricNo = s.MatricNo,
                        FullName = s.FullName,
                        Department = s.Department,
                        UserName = s.AppUser != null ? s.AppUser.UserName : null,
                        Email = s.AppUser != null ? s.AppUser.Email : null
                    })
                    .FirstOrDefaultAsync();
                if (student is null)
                {
                    return NotFound();
                }
                return Ok(student);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        // DTO for Student output
        public class StudentDto
        {
            public int Id { get; set; }
            public string? MatricNo { get; set; }
            public string? FullName { get; set; }
            public string? Department { get; set; }
            public string? UserName { get; set; }
            public string? Email { get; set; }
        }
        
        
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateStudent([FromBody] Student student, int id)
        {
            try
            {
                if (student.Id != id)
                {
                    return BadRequest();
                }
                if (!await _context.Students.AnyAsync(s => student.Id == s.Id))
                {
                    return NotFound();
                }
                _context.Students.Update(student);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {
                var student = await _context.Students.FindAsync(id);
                if (student is null)
                {
                    return NotFound();
                }
                _context.Students.Remove(student);
                return Ok("Deleted Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

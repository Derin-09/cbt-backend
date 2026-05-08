using Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ExamsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("import/exams")]
        public async Task<IActionResult> ImportExams()
        {
            var exams = new List<Exam>
            {
                new Exam { Title = "Mathematics", Duration = TimeSpan.FromHours(1) },
                new Exam { Title = "Science", Duration = TimeSpan.FromMinutes(40) },
                new Exam { Title = "Computer Science", Duration = TimeSpan.FromMinutes(40) },
                new Exam { Title = "History", Duration = TimeSpan.FromMinutes(30) },
                new Exam { Title = "Art", Duration = TimeSpan.FromMinutes(30) }
            };

            _context.Exams.AddRange(exams);
            await _context.SaveChangesAsync();

            return Ok(exams);
        }
        [HttpGet]
        public async Task<IActionResult> GetExams()
        {
            var exams = await _context.Exams.ToListAsync();
            return Ok(exams);
        }
        
        [HttpGet("{title}")]
        public async Task<IActionResult> GetExamByTitle(string title)
        {
            var exam = await _context.Exams.FirstOrDefaultAsync(e => e.Title == title);
            if (exam == null)
            {
                return NotFound();
            }

            var questions = await _context.Questions.Where(q => q.Department == exam.Title).ToListAsync();
            return Ok(new { exam, questions });
        }
    }

}

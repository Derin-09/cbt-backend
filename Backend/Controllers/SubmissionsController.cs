using Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubmissionsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public SubmissionsController(AppDbContext context)
        {
            _context = context;
        }

        public class StudentAnswerDto
        {
            public int QuestionId { get; set; }
            public string SelectedAnswer { get; set; } = string.Empty;
        }

        public class SubmissionDto
        {
            public int ExamId { get; set; }
            public int StudentId { get; set; }
            public List<StudentAnswerDto> Answers { get; set; } = new List<StudentAnswerDto>();
        }

        [HttpPost]
        public async Task<IActionResult> Submit([FromBody] SubmissionDto submissionDto)
        {
            // Check if student is assigned to the exam
            var assignment = await _context.ExamAssignments.FirstOrDefaultAsync(ea => ea.ExamId == submissionDto.ExamId && ea.StudentId == submissionDto.StudentId);
            if (assignment == null)
            {
                return BadRequest("Student is not assigned to this exam.");
            }

            int score = 0;
            foreach (var ans in submissionDto.Answers)
            {
                var question = await _context.Questions.FindAsync(ans.QuestionId);
                if (question != null && ans.SelectedAnswer == question.CorrectAnswer)
                {
                    score++;
                }
            }

            var submission = new Submission
            {
                ExamId = submissionDto.ExamId,
                StudentId = submissionDto.StudentId,
                Score = score,
                SubmittedAt = DateTime.UtcNow.ToString("o")
            };
            _context.Submissions.Add(submission);
            await _context.SaveChangesAsync();

            return Ok(new { submission.Id, submission.Score });
        }
    }
}

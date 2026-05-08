using System.Text.Json;
using Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public QuestionsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("import/mathematics")]
        public async Task<IActionResult> ImportMathematicsQuestions()
        {
            var client = new HttpClient();

            var response = await client.GetStringAsync(
                "https://opentdb.com/api.php?amount=40&category=19&type=multiple"
            );

            var data = JsonSerializer.Deserialize<TriviaResponse>(response);

            foreach (var item in data.results)
            {
                var question = new Question
                {
                    Department = "Mathematics",
                    QuestionText = item.question,
                    OptionA = item.correct_answer,
                    OptionB = item.incorrect_answers[0],
                    OptionC = item.incorrect_answers[1],
                    OptionD = item.incorrect_answers[2],
                    CorrectAnswer = item.correct_answer
                };

                _context.Questions.Add(question);
            }

            await _context.SaveChangesAsync();

            return Ok(data);
        }

        [HttpPost("import/science")]
        public async Task<IActionResult> ImportScienceQuestions()
        {
            var client = new HttpClient();

            var response = await client.GetStringAsync(
                "https://opentdb.com/api.php?amount=40&category=17&type=multiple"
            );

            var data = JsonSerializer.Deserialize<TriviaResponse>(response);

            foreach (var item in data.results)
            {
                var question = new Question
                {
                    Department = "Science",
                    QuestionText = item.question,
                    OptionA = item.correct_answer,
                    OptionB = item.incorrect_answers[0],
                    OptionC = item.incorrect_answers[1],
                    OptionD = item.incorrect_answers[2],
                    CorrectAnswer = item.correct_answer
                };

                _context.Questions.Add(question);
            }

            await _context.SaveChangesAsync();

            return Ok(data);
        }

        [HttpPost("import/computer-science")]
        public async Task<IActionResult> ImportComputerScienceQuestions()
        {
            var client = new HttpClient();

            var response = await client.GetStringAsync(
                "https://opentdb.com/api.php?amount=40&category=18&type=multiple"
            );

            var data = JsonSerializer.Deserialize<TriviaResponse>(response);

            foreach (var item in data.results)
            {
                var question = new Question
                {
                    Department = "Computer Science",
                    QuestionText = item.question,
                    OptionA = item.correct_answer,
                    OptionB = item.incorrect_answers[0],
                    OptionC = item.incorrect_answers[1],
                    OptionD = item.incorrect_answers[2],
                    CorrectAnswer = item.correct_answer
                };

                _context.Questions.Add(question);
            }

            await _context.SaveChangesAsync();

            return Ok(data);
        }

        [HttpPost("import/history")]
        public async Task<IActionResult> ImportHistoryQuestions()
        {
            var client = new HttpClient();

            var response = await client.GetStringAsync(
                "https://opentdb.com/api.php?amount=40&category=23&type=multiple"
            );

            var data = JsonSerializer.Deserialize<TriviaResponse>(response);

            foreach (var item in data.results)
            {
                var question = new Question
                {
                    Department = "History",
                    QuestionText = item.question,
                    OptionA = item.correct_answer,
                    OptionB = item.incorrect_answers[0],
                    OptionC = item.incorrect_answers[1],
                    OptionD = item.incorrect_answers[2],
                    CorrectAnswer = item.correct_answer
                };

                _context.Questions.Add(question);
            }

            await _context.SaveChangesAsync();

            return Ok(data);
        }

        [HttpPost("import/art")]
        public async Task<IActionResult> ImportArtQuestions()
        {
            var client = new HttpClient();

            var response = await client.GetStringAsync(
                "https://opentdb.com/api.php?amount=40&category=25&type=multiple"
            );

            var data = JsonSerializer.Deserialize<TriviaResponse>(response);

            foreach (var item in data.results)
            {
                var question = new Question
                {
                    Department = "Art",
                    QuestionText = item.question,
                    OptionA = item.correct_answer,
                    OptionB = item.incorrect_answers[0],
                    OptionC = item.incorrect_answers[1],
                    OptionD = item.incorrect_answers[2],
                    CorrectAnswer = item.correct_answer
                };

                _context.Questions.Add(question);
            }

            await _context.SaveChangesAsync();

            return Ok(data);
        }
        [HttpGet("mathematics")]
        public async Task<IActionResult> GetMathQuestions()
        {
            var questions = await _context.Questions.Where(q => q.Department == "Mathematics").ToListAsync();
            return Ok(questions);
        }

        [HttpGet("science")]
        public async Task<IActionResult> GetScienceQuestions()
        {
            var questions = await _context.Questions.Where(q => q.Department == "Science").ToListAsync();
            return Ok(questions);
        }

        [HttpGet("computer-science")]
        public async Task<IActionResult> GetComputerScienceQuestions()
        {
            var questions = await _context.Questions.Where(q => q.Department == "Computer Science").ToListAsync();
            return Ok(questions);
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistoryQuestions()
        {
            var questions = await _context.Questions.Where(q => q.Department == "History").ToListAsync();
            return Ok(questions);
        }

        [HttpGet("art")]
        public async Task<IActionResult> GetArtQuestions()
        {
            var questions = await _context.Questions.Where(q => q.Department == "Art").ToListAsync();
            return Ok(questions);
        }
    }
}

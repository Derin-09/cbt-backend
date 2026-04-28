using System;

namespace Backend.Models;

public class Question
{
    
    public int Id { get; set; }
    public int ExamId { get; set; }
    public string QuestionText {get; set;} = string.Empty;
    public string Options { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string CorrectAnswer { get; set; } = string.Empty;
}

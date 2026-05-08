using System;

namespace Backend.Models;

public class Question
{
    public string Department { get; set; } = string.Empty;
    public int Id { get; set; }
    public int ExamId { get; set; }
    public string QuestionText {get; set;} = string.Empty;
    public string OptionA {get; set;} = string.Empty;
    public string OptionB {get; set;} = string.Empty;
    public string OptionC {get; set;} = string.Empty;
    public string OptionD {get; set;} = string.Empty;
    public string Options { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string CorrectAnswer { get; set; } = string.Empty;
}

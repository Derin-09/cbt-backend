using System;

namespace Backend.Models;

public class Submission
{
    public int Id { get; set; }
    public int ExamId { get; set; }
    public int StudentId {get; set;} 
    public int Score { get; set; } 
    public string SubmittedAt { get; set; } = string.Empty;
}

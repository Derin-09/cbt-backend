using System;

namespace Backend.Models;

public class Answer
{
    public int Id { get; set; }
    public int SubmissionId { get; set; }
    public int QuestionId {get; set;} 
    public int SelectedAnswer { get; set; } 
}

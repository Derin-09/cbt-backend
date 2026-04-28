using System;

namespace Backend.Models;

public class Exam
{
    
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Duration {get; set;} = string.Empty;
    public string CreatedBy { get; set; } = string.Empty;
}

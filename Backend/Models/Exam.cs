using System;

namespace Backend.Models;

public class Exam
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}

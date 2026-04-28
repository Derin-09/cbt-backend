using System;

namespace Backend.Models;

public class ExamAssignment
{
     public int Id { get; set; }
    public int ExamId { get; set; }
    public int StudentId { get; set; }
}

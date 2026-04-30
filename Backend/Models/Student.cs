using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Backend.Models;

public class Student
{
    public int Id { get; set; }
    public int UserId { get; set; }
    [Required]
    public string MatricNo {get; set;} = string.Empty; 

    [Required]
    public string FullName {get; set;} = string.Empty; 

    [AllowNull]
    public User? User { get; set; }
    public string Department { get; set; } = string.Empty;

}

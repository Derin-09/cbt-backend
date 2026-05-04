using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Backend.Models;

public class Student
{
    public int Id { get; set; }
    [Required]
    public string AppUserId { get; set; } = string.Empty;
    public AppUser? AppUser { get; set; }

    [Required]
    public string MatricNo { get; set; } = string.Empty;

    [Required]
    public string FullName { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;
}

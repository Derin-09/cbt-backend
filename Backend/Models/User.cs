using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Backend.Models;

public class User
{
    public int Id { get; set; }
    public string Role { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [MinLength(8)]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).+$",
        ErrorMessage = "Password must contain upper, lower, number, and special character")]
    public string Password { get; set; } = string.Empty;
    [AllowNull]
    public Student? Student { get; set; } 
     [AllowNull]
    public Admin? Admin { get; set; }
}

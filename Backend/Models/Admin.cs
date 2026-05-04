using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Backend.Models;

public class Admin
{
    public int Id { get; set; }
    public string AppUserId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string FullName {get; set;} = string.Empty; 
    public string Position { get; set; } = string.Empty;

    [AllowNull]
    public AppUser? User { get; set; }
    
}

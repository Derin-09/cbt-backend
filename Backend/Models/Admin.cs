using System;
using System.Diagnostics.CodeAnalysis;

namespace Backend.Models;

public class Admin
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Email { get; set; } = string.Empty;

    [AllowNull]
    public User User { get; set; }
    
}

using System;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base (options)
    {
        
    }

    public DbSet<User> Users { get; set;}
    public DbSet<Student> Students {get; set;}
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Exam> Exams { get; set; }
    public DbSet<ExamAssignment> ExamAssignments { get; set; }
    public DbSet<Submission> Submissions { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }
}

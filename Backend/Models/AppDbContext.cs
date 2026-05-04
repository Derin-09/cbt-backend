using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models;

    public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser>(options)
    {
        public DbSet<User> AppMembers { get; set;}
        public DbSet<Student> Students {get; set;}
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamAssignment> ExamAssignments { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
    }

    public sealed class AppUser: IdentityUser
    {
        public string? MatricNo { get; set; } 
        public string FullName { get; set; } = string.Empty;
        public string? Department { get; set; }
        public string? Position { get; set; }
    }
  

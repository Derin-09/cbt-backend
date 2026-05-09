
using System;

namespace Backend.Services;

using Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// namespace Backend.Services;

public class AuthService
{
	public record RegisterAdminRequest(string FullName, string Password, string Position, string Email);
	public record RegisterUserRequest(string FullName, string Department, string Password, string MatricNo);

	public record LoginUserRequest(string MatricNo, string Password);
	public record LoginAdminRequest(string Email, string Password);

	public static async Task<IResult> RegisterAdminAsync(RegisterAdminRequest request, UserManager<AppUser> adminManager, AppDbContext dbContext)
	{
		var admin = new AppUser
		{
			UserName = request.Email,
			Email = request.Email,
			FullName = request.FullName,
			Position = request.Position
		};

		IdentityResult identityResult = await adminManager.CreateAsync(admin, request.Password);
		if (!identityResult.Succeeded)
		{
			return Results.BadRequest(identityResult.Errors);
		}
		IdentityResult addToRoleResult = await adminManager.AddToRoleAsync(admin, Roles.Admin);
		if (!addToRoleResult.Succeeded)
		{
			return Results.BadRequest(addToRoleResult.Errors);
		}
		var adminadd = new Admin
		{
			AppUserId = admin.Id,
			FullName = request.FullName,
			Position = request.Position
		};
		dbContext.Admins.Add(adminadd);
		await dbContext.SaveChangesAsync();
		return Results.Ok();
	}

	public static async Task<IResult> RegisterUserAsync(RegisterUserRequest request, UserManager<AppUser> userManager, AppDbContext dbContext)
	{
		var user = new AppUser
		{
			UserName = request.MatricNo,
			Department = request.Department,
			FullName = request.FullName,
			MatricNo = request.MatricNo
		};

		IdentityResult identityResult = await userManager.CreateAsync(user, request.Password);
		if (!identityResult.Succeeded)
		{
			return Results.BadRequest(identityResult.Errors);
		}
		IdentityResult addToRoleResult = await userManager.AddToRoleAsync(user, Roles.Student);
		if (!addToRoleResult.Succeeded)
		{
			return Results.BadRequest(addToRoleResult.Errors);
		}

		var student = new Student
		{
			AppUserId = user.Id,
			FullName = request.FullName,
			MatricNo = request.MatricNo,
			Department = request.Department
		};
		dbContext.Students.Add(student);
		await dbContext.SaveChangesAsync();

		return Results.Ok();
	}

	public static async Task<IResult> LoginUserAsync(LoginUserRequest user, UserManager<AppUser> userManager)
	{
		var founduser = await userManager.FindByNameAsync(user.MatricNo);
		if (founduser == null)
		{
			return Results.NotFound();
		}
		var isPasswordValid = await userManager.CheckPasswordAsync(founduser, user.Password);
		if (!isPasswordValid)
		{
			return Results.NotFound();
		}
		return Results.Ok();
	}
	public static async Task<IResult> LoginAdminAsync(LoginAdminRequest user, UserManager<AppUser> userManager)
	{
		var founduser = await userManager.FindByNameAsync(user.Email);
		if (founduser == null)
		{
			return Results.NotFound();
		}
		var isPasswordValid = await userManager.CheckPasswordAsync(founduser, user.Password);
		if (!isPasswordValid)
		{
			return Results.NotFound();
		}
		return Results.Ok();
	}

	public static async Task<IResult> GetStudentsAsync(AppDbContext dbContext)
	{
		var students = await dbContext.Students.ToListAsync();
		return Results.Ok(students);
	}
	public static async Task<IResult> GetAdminsAsync(AppDbContext dbContext)
	{
		var admins = await dbContext.Admins.ToListAsync();
		return Results.Ok(admins);
	}

	public class AdminDto
	{
		public int Id { get; set; }
		public string? FullName { get; set; }
		public string? Position { get; set; }
		public string? Email { get; set; }
	}

	public class StudentDto
	{
		public int Id { get; set; }
		public string? MatricNo { get; set; }
		public string? FullName { get; set; }
		public string? Department { get; set; }
		public string? UserName { get; set; }
		public string? Email { get; set; }
	}
	public static async Task<AdminDto?> GetAdminByIdAsync(int id, AppDbContext dbContext)
	{
		return await dbContext.Admins.Include(s => s.AppUser)
			.Where(s => s.Id == id)
			.Select(s => new AdminDto
			{
				Id = s.Id,
				FullName = s.FullName,
				Position = s.Position,
				Email = s.AppUser != null ? s.AppUser.Email : null
			})
			.FirstOrDefaultAsync();
	}

	public static async Task<StudentDto?> GetStudentByIdAsync(int id, AppDbContext dbContext)
	{
		return await dbContext.Students.Include(s => s.AppUser)
			.Where(s => s.Id == id)
			.Select(s => new StudentDto
			{
				Id = s.Id,
				MatricNo = s.MatricNo,
				FullName = s.FullName,
				Department = s.Department,
				UserName = s.AppUser != null ? s.AppUser.UserName : null,
				Email = s.AppUser != null ? s.AppUser.Email : null
			})
			.FirstOrDefaultAsync();
	}
}

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


}

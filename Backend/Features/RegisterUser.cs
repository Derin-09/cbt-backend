using System;
using Backend.Models;
using Microsoft.AspNetCore.Identity;

namespace Backend.Features;

public class RegisterUser
{
    public record Request(string FullName, string Department, string Password,  string MatricNo);

    public static void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("register/student", async (Request request, UserManager<AppUser> userManager) =>
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
            return Results.Ok();
        });
    }
}

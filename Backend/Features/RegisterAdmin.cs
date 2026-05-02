using System;
using Backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace Backend.Features;

public class RegisterAdmin
{
    public record RequestAdmin(string FullName, string Password, string Position, string Email );

    public static void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("register/admin", async (RequestAdmin request, UserManager<AppUser> adminManager) =>
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
            return Results.Ok();
        });
    }
}

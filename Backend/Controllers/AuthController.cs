using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AuthController(AppDbContext context)
        {
            _context = context;
        }


        [HttpPost("register/admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] AuthService.RegisterAdminRequest request, [FromServices] UserManager<AppUser> userManager, AppDbContext dbContext)
        {
            var result = await AuthService.RegisterAdminAsync(request, userManager, dbContext);
            if (result is Microsoft.AspNetCore.Http.HttpResults.Ok)
                return Ok();
            if (result is Microsoft.AspNetCore.Http.HttpResults.BadRequest badRequest)
                return BadRequest(badRequest);
            // Add more mappings as needed
            return StatusCode(500, "Unknown result type");
        }

        [HttpPost("register/student")]
        [Microsoft.AspNetCore.Authorization.Authorize(Roles = Backend.Models.Roles.Admin)]
        public async Task<IActionResult> RegisterUser([FromBody] AuthService.RegisterUserRequest request, [FromServices] UserManager<AppUser> userManager, AppDbContext dbContext)
        {
            var result = await AuthService.RegisterUserAsync(request, userManager, dbContext);
            if (result is Microsoft.AspNetCore.Http.HttpResults.Ok)
                return Ok();
            if (result is Microsoft.AspNetCore.Http.HttpResults.BadRequest badRequest)
                return BadRequest(badRequest);
            // Add more mappings as needed
            return StatusCode(500, "Unknown result type");
        }

        [HttpPost("login/student")]

        public async Task<IActionResult> LoginUser([FromBody] AuthService.LoginUserRequest request, [FromServices] UserManager<AppUser> userManager)
        {
            var result = await AuthService.LoginUserAsync(request, userManager);
            if (result is Microsoft.AspNetCore.Http.HttpResults.Ok)
                return Ok();
            if (result is Microsoft.AspNetCore.Http.HttpResults.BadRequest badRequest)
                return BadRequest(badRequest);
            // Add more mappings as needed
            return StatusCode(500, "Unknown result type");
        }

        [HttpPost("login/admin")]

        public async Task<IActionResult> LoginAmin([FromBody] AuthService.LoginAdminRequest request, [FromServices] UserManager<AppUser> userManager)
        {
            var result = await AuthService.LoginAdminAsync(request, userManager);
            if (result is Microsoft.AspNetCore.Http.HttpResults.Ok)
                return Ok();
            if (result is Microsoft.AspNetCore.Http.HttpResults.BadRequest badRequest)
                return BadRequest(badRequest);
            // Add more mappings as needed
            return StatusCode(500, "Unknown result type");
        }

    }
}

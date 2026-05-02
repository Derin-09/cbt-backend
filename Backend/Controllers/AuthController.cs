using Backend.Models;
using Microsoft.AspNetCore.Http;
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

        // [HttpGet]
        // public async Task<IActionResult> GetUsers()
        // {
        //     try
        //     {
        //         var users = await _context.Users.Include(u => u.Student).Include(u => u.Admin).ToListAsync();
        //         return Ok(users);
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //     }
        // }

        //  [HttpPost("login")]
        // public async Task<IActionResult> LoginUser(User user)
        // {
        //     try
        //     { 
        //         // var savedUser = await _context.Users.FirstOrDefaultAsync()
        //         // var unhashedPassword = BCrypt.Net.BCrypt.Verify(user.Password, )
        //         _context.Users.Add(user);
        //         await _context.SaveChangesAsync();
        //         return Ok("Created Successfully");
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //     }
        // }

        // [HttpPost]
        // public async Task<IActionResult> CreateUser(User user)
        // {
        //     try
        //     {
        //         var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
        //         user.Password = hashedPassword;
        //         _context.Users.Add(user);
        //         await _context.SaveChangesAsync();
        //         return Ok("Created Successfully");
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //     }
        // }
        // [HttpGet("{id:int}")]
        // public async Task<IActionResult> GetUser(int id)
        // {
        //     try
        //     {
        //     var user = await _context.Users.Include(u => u.Student)
        //     .Include(u => u.Admin).FirstOrDefaultAsync(u => u.Id == id);
        //         return Ok(user);
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //     }
        // }
        // [HttpPut("{id:int}")]
        // public async Task<IActionResult> UpdateUser([FromBody] User user, int id)
        // {
        //     try
        //     {
        //         if (user.Id != id)
        //         {
        //             return BadRequest();
        //         }
        //         if (!await _context.Users.AnyAsync(u => user.Id == u.Id))
        //         {
        //             return NotFound();
        //         }
        //         _context.Users.Update(user);
        //         await _context.SaveChangesAsync();
        //         return NoContent();
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //     }
        // }
        // [HttpDelete("{id:int}")]
        // public async Task<IActionResult> DeleteUser(int id)
        // {
        //     try
        //     {
        //         var user = await _context.Users.FindAsync(id);
        //         if (user is null)
        //         {
        //             return NotFound();
        //         }
        //         _context.Users.Remove(user);
        //         return Ok("Deleted Successfully");
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //     }
        // }
    }
}

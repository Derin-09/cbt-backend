using Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAdmins()
        {
            var admins = await _context.Admins.Include(a => a.User).ToListAsync();
            return Ok(admins);
        }

        [HttpPost]
         public async Task<IActionResult> AddAdmin(Admin admin)
        {
            try
            {
                if (admin is null)
                {
                    return BadRequest();
                }
            await _context.Admins.AddAsync(admin);
            return Ok("Created Successfully");
            } catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPost("{id:int}")]
         public async Task<IActionResult> GetAdmin(int id)
        {
            try
            {
            var admin = await _context.Admins.Include(a => a.User).FirstOrDefaultAsync(a => a.Id == id);
                if (admin is null)
                {
                    return BadRequest();
                }
            return Ok(admin);
            } catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

         [HttpPut("{id:int}")]
         public async Task<IActionResult> UpdateAdmin([FromBody] Admin admin, int id)
        {
            try
            {
                if (admin is null)
                {
                    return BadRequest();
                }
                
                if (admin.Id != id)
                {
                    return BadRequest();
                }
                
                if (!await _context.Admins.AnyAsync(a => a.Id == admin.Id))
                {
                    return NotFound();
                }
            _context.Admins.Update(admin);
            return NoContent();
            } catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

         [HttpDelete("{id:int}")]
         public async Task<IActionResult> DeleteAdmin(int id)
        {
            try
            {
                var admin = await _context.Admins.FindAsync(id);
                if (admin is null)
                {
                    return NotFound();
                }
            _context.Admins.Remove(admin);
            return NoContent();
            } catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AuthController> _logger;
        public AuthController(AppDbContext context, ILogger<AuthController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [HttpPost("register/admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] AuthService.RegisterAdminRequest? request, [FromServices] UserManager<AppUser> userManager, AppDbContext dbContext)
        {
            string requestId = Guid.NewGuid().ToString("N");
            HttpContext.Items["RequestId"] = requestId;

            string[] bodyKeys = GetSanitizedBodyKeys(request);
            _logger.LogInformation(
                "RegisterAdmin entry. TimestampUtc: {TimestampUtc}, MethodPath: {MethodPath}, RequestId: {RequestId}, BodyKeys: {BodyKeys}",
                DateTime.UtcNow,
                $"{Request.Method} {Request.Path}",
                requestId,
                string.Join(",", bodyKeys));

            var validationErrors = ValidateRegisterAdminRequest(request, ModelState);
            if (validationErrors.Count > 0)
            {
                var invalidResponse = new
                {
                    requestId,
                    message = "Validation failed.",
                    errors = validationErrors
                };

                _logger.LogInformation(
                    "RegisterAdmin response. RequestId: {RequestId}, StatusCode: {StatusCode}, ResponseShape: {ResponseShape}",
                    requestId,
                    StatusCodes.Status400BadRequest,
                    "validation_error_payload");

                return BadRequest(invalidResponse);
            }

            try
            {
                var result = await AuthService.RegisterAdminAsync(request!, userManager, dbContext, _logger, requestId);

                _logger.LogInformation(
                    "RegisterAdmin response. RequestId: {RequestId}, StatusCode: {StatusCode}, ResponseShape: {ResponseShape}",
                    requestId,
                    result.StatusCode,
                    result.Body?.GetType().Name ?? "null");

                return StatusCode(result.StatusCode, result.Body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RegisterAdmin unhandled exception. RequestId: {RequestId}", requestId);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    requestId,
                    message = "An unexpected error occurred."
                });
            }
        }

        private static string[] GetSanitizedBodyKeys(AuthService.RegisterAdminRequest? request)
        {
            if (request is null)
            {
                return Array.Empty<string>();
            }

            var keys = new List<string>();

            if (!string.IsNullOrWhiteSpace(request.FullName)) keys.Add(nameof(request.FullName));
            if (!string.IsNullOrWhiteSpace(request.Position)) keys.Add(nameof(request.Position));
            if (!string.IsNullOrWhiteSpace(request.Email)) keys.Add(nameof(request.Email));
            if (!string.IsNullOrWhiteSpace(request.Password)) keys.Add(nameof(request.Password));

            return keys.ToArray();
        }

        private static Dictionary<string, string[]> ValidateRegisterAdminRequest(AuthService.RegisterAdminRequest? request, ModelStateDictionary modelState)
        {
            var errors = new Dictionary<string, string[]>();
            var emailAttribute = new EmailAddressAttribute();

            if (!modelState.IsValid)
            {
                foreach (var entry in modelState)
                {
                    if (entry.Value is null || entry.Value.Errors.Count == 0)
                    {
                        continue;
                    }

                    string key = string.IsNullOrWhiteSpace(entry.Key) ? "body" : entry.Key;
                    errors[key] = entry.Value.Errors
                        .Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage) ? "Invalid value." : e.ErrorMessage)
                        .ToArray();
                }
            }

            if (request is null)
            {
                if (!errors.ContainsKey("body"))
                {
                    errors["body"] = new[] { "Request body is required." };
                }

                return errors;
            }

            if (string.IsNullOrWhiteSpace(request.FullName))
            {
                errors[nameof(request.FullName)] = new[] { "FullName is required." };
            }

            if (string.IsNullOrWhiteSpace(request.Position))
            {
                errors[nameof(request.Position)] = new[] { "Position is required." };
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                errors[nameof(request.Email)] = new[] { "Email is required." };
            }
            else if (!emailAttribute.IsValid(request.Email))
            {
                errors[nameof(request.Email)] = new[] { "Email format is invalid." };
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                errors[nameof(request.Password)] = new[] { "Password is required." };
            }

            return errors;
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

        public async Task<IActionResult> LoginAdmin([FromBody] AuthService.LoginAdminRequest request, [FromServices] UserManager<AppUser> userManager)
        {
            var result = await AuthService.LoginAdminAsync(request, userManager);
            if (result is Microsoft.AspNetCore.Http.HttpResults.Ok)
                return Ok();
            if (result is Microsoft.AspNetCore.Http.HttpResults.BadRequest badRequest)
                return BadRequest(badRequest);
            // Add more mappings as needed
            return StatusCode(500, "Unknown result type");
        }

       public class AdminDto
        {
            public int Id { get; set; }
            public string? FullName { get; set; }
            public string? Position { get; set; }
            public string? Email { get; set; }
        }
        [HttpGet("admin/{id:int}")]
        public async Task<IActionResult> GetAdmin(int id)
        {
            try
            {
                var admin = await AuthService.GetAdminByIdAsync(id, _context);
                if (admin is null)
                {
                    return NotFound();
                }
                return Ok(admin);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet("student/{id:int}")]
        public async Task<IActionResult> GetStudent(int id)
        {
            try
            {
                var student = await AuthService.GetStudentByIdAsync(id, _context);
                if (student is null)
                {
                    return NotFound();
                }
                return Ok(student);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



    }
}

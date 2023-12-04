using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuotesWebAPI.Services;

namespace QuotesWebAPI.Controllers
{
    [ApiController()]
    public class AccountApiController : Controller
    {
        private RoleManager<IdentityRole> _roleManager;
        private IAuthService _authService;

        public AccountApiController(RoleManager<IdentityRole> roleManager,
                                    IAuthService authService)
        {
            _roleManager = roleManager;
            _authService = authService;
        }

        [HttpPost("/api/register")]
        public async Task<IActionResult> RegisterUser(UserRegistrationRequest request)
        {
            var result = await _authService.RegisterUser(request);

            if (result.Succeeded)
            {
                return StatusCode(201);
            }
            else
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(err.Code, err.Description);
                }

                return BadRequest(ModelState);
            }
        }

        [HttpPost("/api/login")]
        public async Task<IActionResult> LoginUser(LoginRequest request)
        {
            bool result = await _authService.LoginUser(request);

            if (result)
            {
                return Ok(new { Token = await _authService.CreateToken() });
            }

            return Unauthorized();
        }

        [HttpGet("/api/roles")]
        public async Task<IActionResult> GetRoles()
        {
            return Ok(new { Roles = await _roleManager.Roles.Select(r => r.NormalizedName).ToListAsync() });
        }
    }
}

/* AccountApiController.cs
 * Class for AccountApiController
 * 
 * Revision History:
 *      Junseo Yang, 2023-12-10: Created
 */
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuotesWebAPI.Services;

namespace QuotesWebAPI.Controllers
{
    /// <summary>
    /// Class for AccountApiController
    /// </summary>
    [ApiController()]
    public class AccountApiController : Controller
    {
        private RoleManager<IdentityRole> _roleManager;
        private IAuthService _authService;

        /// <summary>
        /// Constructor for AccountApiController
        /// </summary>
        /// <param name="roleManager">RoleManager</param>
        /// <param name="authService">AuthService</param>
        public AccountApiController(RoleManager<IdentityRole> roleManager,
                                    IAuthService authService)
        {
            _roleManager = roleManager;
            _authService = authService;
        }

        /// <summary>
        /// Method for RegisterUser
        /// </summary>
        /// <param name="request">UserRegistrationRequest</param>
        /// <returns>IActionResult</returns>
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

        /// <summary>
        /// Method for LoginUser
        /// </summary>
        /// <param name="request">LoginRequest</param>
        /// <returns>IActionResult</returns>
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

        /// <summary>
        /// Method for GetRoles
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpGet("/api/roles")]
        public async Task<IActionResult> GetRoles()
        {
            return Ok(new { Roles = await _roleManager.Roles.Select(r => r.NormalizedName).ToListAsync() });
        }
    }
}

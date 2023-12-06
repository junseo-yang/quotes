/* AuthService.cs
 * Class for AuthService
 * 
 * Revision History:
 *      Junseo Yang, 2023-12-10: Created
 */
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuotesWebAPI.Controllers;
using QuotesWebAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuotesWebAPI.Services
{
    /// <summary>
    /// Class for AuthService
    /// </summary>
    public class AuthService : IAuthService
    {
        private UserManager<User> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private IConfiguration _configuration;
        private User? _user;

        /// <summary>
        /// Constructor for AuthService Class
        /// </summary>
        /// <param name="userManager">UserManager</param>
        /// <param name="roleManager">RoleManager</param>
        /// <param name="configuration">Configuration</param>
        public AuthService(UserManager<User> userManager,
                            RoleManager<IdentityRole> roleManager,
                            IConfiguration configuration) 
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        /// <summary>
        /// Method for RegisterUser
        /// </summary>
        /// <param name="request">UserRegistrationRequest</param>
        /// <returns></returns>
        public async Task<IdentityResult> RegisterUser(UserRegistrationRequest request)
        {
            var existingRoles = await _roleManager.Roles.Select(r => r.NormalizedName).ToListAsync();
            if (request.Roles != null)
            {
                if (!request.Roles.All(role => existingRoles.Contains(role)))
                {
                    return IdentityResult.Failed(
                        new IdentityError() 
                        {
                            Code = "InvalidRoles",
                            Description = "Roles are not valid"
                        }
                    );
                }
            }

            _user = new User()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            var result = await _userManager.CreateAsync(_user, request.Password);

            if (result.Succeeded)
            {
                if (request.Roles != null)
                {
                    result = await _userManager.AddToRolesAsync(_user, request.Roles);
                }
            }

            return result;
        }

        /// <summary>
        /// Method for LoginUser
        /// </summary>
        /// <param name="request">LoginRequest</param>
        /// <returns></returns>
        public async Task<bool> LoginUser(LoginRequest request)
        {
            _user = await _userManager.FindByNameAsync(request.UserName);
            
            if (_user == null)
            {
                return false;
            }

            return await _userManager.CheckPasswordAsync(_user, request.Password);
        }

        /// <summary>
        /// Method for CreateToken
        /// </summary>
        /// <returns>JwtToken</returns>
        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        /// <summary>
        /// Method for GetSigningCredentials
        /// </summary>
        /// <returns>SigningCredentials</returns>
        private SigningCredentials GetSigningCredentials()
        {
            var secretKeyText = Environment.GetEnvironmentVariable("SECRET");

            var key = Encoding.UTF8.GetBytes(secretKeyText);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        /// <summary>
        /// Method for GetClaims
        /// </summary>
        /// <returns>List of Claims</returns>
        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        /// <summary>
        /// Method for GenerateTokenOptions
        /// </summary>
        /// <param name="signingCredentials">SigningCredentials</param>
        /// <param name="claims">List of Claims</param>
        /// <returns>JwtSecurityToken</returns>
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var tokenOptions = new JwtSecurityToken(
                issuer: jwtSettings["validIssuer"],
                audience: jwtSettings["validAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expires"])),
                signingCredentials: signingCredentials
            );

            return tokenOptions;
        }
    }
}

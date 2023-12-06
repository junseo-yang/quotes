/* IAuthService.cs
 * Interface for AuthService
 * 
 * Revision History:
 *      Junseo Yang, 2023-12-10: Created
 */
using Microsoft.AspNetCore.Identity;
using QuotesWebAPI.Controllers;

namespace QuotesWebAPI.Services
{
    /// <summary>
    /// Interface for AuthService
    /// </summary>
    public interface IAuthService
    {
        public Task<IdentityResult> RegisterUser(UserRegistrationRequest userRegistrationRequest);
        public Task<bool> LoginUser(LoginRequest loginRequest);
        public Task<string> CreateToken();
    }
}

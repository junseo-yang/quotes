using Microsoft.AspNetCore.Identity;
using QuotesWebAPI.Controllers;

namespace QuotesWebAPI.Services
{
    public interface IAuthService
    {
        public Task<IdentityResult> RegisterUser(UserRegistrationRequest userRegistrationRequest);
        public Task<bool> LoginUser(LoginRequest loginRequest);
        public Task<string> CreateToken();
    }
}

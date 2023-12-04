using Microsoft.AspNetCore.Identity;

namespace QuotesWebAPI.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}

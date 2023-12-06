/* UserRegistrationRequest.cs
 * Class for UserRegistrationRequest
 * 
 * Revision History:
 *      Junseo Yang, 2023-12-10: Created
 */
using System.ComponentModel.DataAnnotations;

namespace QuotesWebAPI.Controllers
{
    /// <summary>
    /// Class for UserRegistrationRequest
    /// </summary>
    public class UserRegistrationRequest
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public ICollection<string>? Roles { get; set; }
    }
}

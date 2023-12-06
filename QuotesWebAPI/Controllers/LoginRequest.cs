/* LoginRequest.cs
 * Class for LoginRequest
 * 
 * Revision History:
 *      Junseo Yang, 2023-12-10: Created
 */

using System.ComponentModel.DataAnnotations;

namespace QuotesWebAPI.Controllers
{
    /// <summary>
    /// Class for LoginRequest
    /// </summary>
    public class LoginRequest
    {
        [Required(ErrorMessage = "Username is required")]
        public string? UserName { get; set; }


        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}

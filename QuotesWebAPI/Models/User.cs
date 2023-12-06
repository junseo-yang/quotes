/* User.cs
 * Class for User inherited from IdentityUser
 * 
 * Revision History:
 *      Junseo Yang, 2023-12-10: Created
 */
using Microsoft.AspNetCore.Identity;

namespace QuotesWebAPI.Models
{
    /// <summary>
    /// Class for User inherited from IdentityUser
    /// </summary>
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}

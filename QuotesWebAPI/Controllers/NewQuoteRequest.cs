/* NewQuoteRequest.cs
 * Class for NewQuoteRequest
 * 
 * Revision History:
 *      Junseo Yang, 2023-11-19: Created
 */

using QuotesWebAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace QuotesWebAPI.Controllers
{
    /// <summary>
    /// Class for NewQuoteRequest
    /// </summary>
    public class NewQuoteRequest
    {
        public string? Description { get; set; }

        public string? Author { get; set; }

        public int Like { get; set; } = 0;

        public List<string>? Tags { get; set; }
    }
}

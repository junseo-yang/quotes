/* QuoteViewModel.cs
 * Class for QuoteViewModel
 * 
 * Revision History:
 *      Junseo Yang, 2023-11-19: Created
 */

using QuotesWebAPI.Controllers;

namespace QuotesWebAPI.Models
{
    /// <summary>
    /// Class for QuoteViewModel
    /// </summary>
    public class QuoteViewModel
    {
        public List<QuoteInfo>? Quotes { get; set; }

        public DateTime? QuotesLastModified { get; set; }

        public List<string>? Tags { get; set; }
    }
}

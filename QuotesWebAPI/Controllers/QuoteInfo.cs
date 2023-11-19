/* QuoteInfo.cs
 * Class for QuoteInfo
 * 
 * Revision History:
 *      Junseo Yang, 2023-11-19: Created
 */

namespace QuotesWebAPI.Controllers
{
    /// <summary>
    /// Class for QuoteInfo
    /// </summary>
    public class QuoteInfo : NewQuoteRequest
    {
        public int? QuoteId { get; set; }
    }
}

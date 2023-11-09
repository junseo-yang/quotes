using QuotesWebAPI.Controllers;

namespace QuotesWebAPI.Models
{
    public class QuoteViewModel
    {
        public List<QuoteInfo>? Quotes { get; set; }

        public DateTime? QuotesLastModified { get; set; }

        public List<string>? Tags { get; set; }
    }
}

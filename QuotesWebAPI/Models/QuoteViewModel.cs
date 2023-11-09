namespace QuotesWebAPI.Models
{
    public class QuoteViewModel
    {
        public int? QuoteId { get; set; }

        public string? Description { get; set; }

        public string? Author { get; set; }

        public int? Like { get; set; } = 0;

        public List<string>? Tags { get; set; }
    }
}

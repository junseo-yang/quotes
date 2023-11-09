using QuotesWebAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace QuotesWebAPI.Controllers
{
    public class NewQuoteRequest
    {
        public string? Description { get; set; }

        public string Author { get; set; } = "Anonymous";

        public int Like { get; set; } = 0;

        public List<string>? Tags { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace QuotesWebAPI.Models
{
    public class Quote
    {
        public int QuoteId { get; set; }

        [Required]
        [Display(Name = "Quote")]
        public string? Description { get; set; }

        [Display(Name = "Author")]
        public string Author { get; set; } = "Anonymous";

        [Display(Name = "Like")]
        [Range(0, int.MaxValue, ErrorMessage = "Likes should be greater than or equal to 9.")]
        public int Like { get; set; } = 0;

        public DateTime? LastModified { get; set; } = DateTime.Now;

        public IList<TagAssignment>? TagAssignments { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace QuotesWebAPI.Models
{
    public class TagAssignment
    {
        [Required]
        [Display(Name = "Quote")]
        public int QuoteId { get; set; }
        public Quote? Quote { get; set; }

        [Required]
        [Display(Name = "Tag")]
        public int TagId { get; set; }
        public Tag? Tag { get; set; }
    }
}

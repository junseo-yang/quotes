using System.ComponentModel.DataAnnotations;

namespace QuotesWebAPI.Models
{
    public class Tag
    {
        public int TagId { get; set; }

        [Required]
        public string? Name { get; set; }

        public IList<TagAssignment>? TagAssignments { get; set; }
    }
}

/* Quote.cs
 * Class for Quote
 * 
 * Revision History:
 *      Junseo Yang, 2023-11-19: Created
 */

using System.ComponentModel.DataAnnotations;

namespace QuotesWebAPI.Models
{
    /// <summary>
    /// Class for Quote
    /// </summary>
    public class Quote
    {
        public int QuoteId { get; set; }

        [Required]
        [Display(Name = "Quote")]
        public string? Description { get; set; }

        [Display(Name = "Author")]
        public string? Author { get; set; }

        [Display(Name = "Like")]
        [Range(0, int.MaxValue, ErrorMessage = "Likes should be greater than or equal to 9.")]
        public int Like { get; set; } = 0;

        public DateTime? LastModified { get; set; } = DateTime.Now;

        public IList<TagAssignment>? TagAssignments { get; set; }
    }
}

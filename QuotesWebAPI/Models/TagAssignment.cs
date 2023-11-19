/* Tag.cs
 * Class for TagAssignment
 * 
 * Revision History:
 *      Junseo Yang, 2023-11-19: Created
 */

using System.ComponentModel.DataAnnotations;

namespace QuotesWebAPI.Models
{
    /// <summary>
    /// Class for TagAssignment
    /// </summary>
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

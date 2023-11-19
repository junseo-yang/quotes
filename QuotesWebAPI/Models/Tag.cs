/* Tag.cs
 * Class for Tag
 * 
 * Revision History:
 *      Junseo Yang, 2023-11-19: Created
 */

using System.ComponentModel.DataAnnotations;

namespace QuotesWebAPI.Models
{
    /// <summary>
    /// Class for Tag
    /// </summary>
    public class Tag
    {
        public int TagId { get; set; }

        [Required]
        public string? Name { get; set; }

        public DateTime? LastModified { get; set; } = DateTime.Now;

        public IList<TagAssignment>? TagAssignments { get; set; }
    }
}

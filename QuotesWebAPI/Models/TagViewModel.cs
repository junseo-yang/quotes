/* TagViewModel.cs
 * Class for TagViewModel
 * 
 * Revision History:
 *      Junseo Yang, 2023-11-19: Created
 */

using QuotesWebAPI.Controllers;

namespace QuotesWebAPI.Models
{
    /// <summary>
    /// Class for TagViewModel
    /// </summary>
    public class TagViewModel
    {
        public List<TagInfo>? Tags { get; set; }

        public DateTime? TagsLastModified { get; set; }
    }
}

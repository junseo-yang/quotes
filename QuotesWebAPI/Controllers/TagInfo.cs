/* TagInfo.cs
 * Class for TagInfo
 * 
 * Revision History:
 *      Junseo Yang, 2023-11-19: Created
 */

namespace QuotesWebAPI.Controllers
{
    /// <summary>
    /// Class for TagInfo
    /// </summary>
    public class TagInfo : NewTagRequest
    {
        public int TagId { get; set; }
    }
}

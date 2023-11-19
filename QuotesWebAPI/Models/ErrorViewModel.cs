/* ErrorViewModel.cs
 * Class for ErrorViewModel
 * 
 * Revision History:
 *      Junseo Yang, 2023-11-19: Created
 */

namespace QuotesWebAPI.Models
{
    /// <summary>
    /// Class for ErrorViewModel
    /// </summary>
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
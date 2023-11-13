using QuotesWebAPI.Controllers;

namespace QuotesWebAPI.Models
{
    public class TagViewModel
    {
        public List<TagInfo>? Tags { get; set; }

        public DateTime? TagsLastModified { get; set; }
    }
}

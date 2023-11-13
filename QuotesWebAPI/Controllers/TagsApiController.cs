using Microsoft.AspNetCore.Mvc;
using QuotesWebAPI.Data;
using QuotesWebAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuotesWebAPI.Controllers
{
    [ApiController]
    public class TagsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TagsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/tags
        [HttpGet("api/tags")]
        public IActionResult GetTags()
        {
            List<TagInfo> tags = _context.Tags
                                    .Select(t => new TagInfo()
                                    {
                                        TagId = t.TagId,
                                        Name = t.Name
                                    }).ToList();

            DateTime tagLastModified = new DateTime(1970, 1, 1);
            if (tags.Count > 0)
            {
                tagLastModified = _context.Tags.Max(t => t.LastModified).GetValueOrDefault();
            }

            TagViewModel tagViewModel = new TagViewModel()
            {
                Tags = tags,
                TagsLastModified = tagLastModified
            };

            return Ok(tagViewModel);
        }

        // GET api/tags/5
        [HttpGet("api/tags/{id}")]
        public IActionResult GetTagById(int id)
        {
            TagInfo tag = _context.Tags
                            .Select(t => new TagInfo()
                            {
                                TagId = t.TagId,
                                Name = t.Name
                            })
                            .Where(t => t.TagId == id)
                            .FirstOrDefault();
            return Ok(tag);
        }

        // POST api/tags
        [HttpPost("api/tags")]
        public IActionResult Post([FromBody] NewTagRequest newTagRequest)
        {
            var tags = _context.Tags.ToList();

            var existingTag = tags.Where(t => t.Name == newTagRequest.Name).FirstOrDefault();
            // Tag Name Unique Validation
            if(existingTag != null)
            {
                return Conflict(new { existingTag, error = "The tag name already exists. Try another." });
            }

            Tag newTag = new Tag()
            {
                Name = newTagRequest.Name
            };

            _context.Add(newTag);
            _context.SaveChanges();

            TagInfo tag = new TagInfo()
            {
                TagId = newTag.TagId,
                Name = newTag.Name
            };

            return CreatedAtAction(nameof(GetTagById), new { id = tag.TagId }, tag);
        }
    }
}

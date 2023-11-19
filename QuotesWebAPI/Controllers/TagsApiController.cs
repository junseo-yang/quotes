/* TagsApiController.cs
 * Class for TagsApiController
 * 
 * Revision History:
 *      Junseo Yang, 2023-11-19: Created
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuotesWebAPI.Data;
using QuotesWebAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuotesWebAPI.Controllers
{
    /// <summary>
    /// Class for TagsApiController
    /// </summary>
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
        public async Task<IActionResult> GetTags()
        {
            List<TagInfo> tags = await _context.Tags
                                    .Select(t => new TagInfo()
                                    {
                                        TagId = t.TagId,
                                        Name = t.Name
                                    }).ToListAsync();

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
        public async Task<IActionResult> GetTagById(int id)
        {
            TagInfo? tag = await _context.Tags
                            .Select(t => new TagInfo()
                            {
                                TagId = t.TagId,
                                Name = t.Name
                            })
                            .Where(t => t.TagId == id)
                            .FirstOrDefaultAsync();

            if (tag == null)
            {
                return NotFound(new { tagId = id, error = "The tagId doesn't exist." }); ;
            }

            return Ok(tag);
        }

        // POST api/tags
        [HttpPost("api/tags")]
        public async Task<IActionResult> Post([FromBody] NewTagRequest newTagRequest)
        {
            var tags = await _context.Tags.ToListAsync();

            if (newTagRequest.Name.IsNullOrEmpty())
            {
                return BadRequest(new { error = "Name cannot be empty." });
            }

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

        // PUT api/tags
        [HttpPut("api/tags")]
        public async Task<IActionResult> Put([FromBody] TagInfo newTagInfo)
        {
            var tag = await _context.Tags.Where(t => t.TagId == newTagInfo.TagId).FirstOrDefaultAsync();

            if (tag == null)
            {
                return NotFound(new { tagId = newTagInfo.TagId, error = "The tagId doesn't exist." }); ;
            }

            if (newTagInfo.Name.IsNullOrEmpty())
            {
                return BadRequest(new { error = "Name cannot be empty." });
            }

            var existingTag = await _context.Tags.Where(t => t.Name == newTagInfo.Name).FirstOrDefaultAsync();
            
            // Tag Name Unique Validation
            if (existingTag != null)
            {
                return Conflict(new { existingTag, error = "The tag name already exists. Try another." });
            }

            tag.Name = newTagInfo.Name;
            tag.LastModified = DateTime.Now;

            _context.Update(tag);
            _context.SaveChanges();

            TagInfo tagInfo = new TagInfo()
            {
                TagId = tag.TagId,
                Name = tag.Name
            };

            return Ok(tagInfo);
        }
    }
}

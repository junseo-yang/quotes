using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.IdentityModel.Tokens;
using QuotesWebAPI.Data;
using QuotesWebAPI.Models;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Xml;

namespace QuotesWebAPI.Controllers
{
    [ApiController]
    public class QuotesApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public QuotesApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/quotes
        [HttpGet("api/quotes")]
        public async Task<IActionResult> GetQuotes()
        {
            var tags = await _context.Tags.Select(t => t.Name).ToListAsync();

            List<QuoteInfo> quotes = await _context.Quotes
                                        .Include(q => q.TagAssignments)
                                        .ThenInclude(q => q.Tag)
                                        .Select(q => new QuoteInfo()
                                        {
                                            QuoteId = q.QuoteId,
                                            Description = q.Description,
                                            Author = q.Author,
                                            Like = q.Like,
                                            Tags = _context.TagAssignments.Include(ta => ta.Tag)
                                                        .Where(ta => ta.QuoteId == q.QuoteId)
                                                        .Select(ta => ta.Tag.Name)
                                                        .ToList()
                                        }).ToListAsync();
            
            DateTime quoteLastModified = new DateTime(1970, 1, 1);
            if (quotes.Count > 0)
            {
                quoteLastModified = _context.Quotes.Max(t => t.LastModified).GetValueOrDefault();
            }

            QuoteViewModel quoteViewModel = new QuoteViewModel()
            {
                Quotes = quotes,
                QuotesLastModified = quoteLastModified,
                Tags = tags
            };

            return Ok(quoteViewModel);
        }

        // GET: api/quotes/5
        [HttpGet("api/quotes/{id}")]
        public async Task<IActionResult> GetQuoteById(int id)
        {
            QuoteInfo? quote = await _context.Quotes
                            .Include(q => q.TagAssignments)
                            .ThenInclude(q => q.Tag)
                            .Select(q => new QuoteInfo()
                            {
                                QuoteId = q.QuoteId,
                                Description = q.Description,
                                Author = q.Author,
                                Like = q.Like,
                                Tags = _context.TagAssignments.Include(ta => ta.Tag)
                                            .Where(ta => ta.QuoteId == q.QuoteId)
                                            .Select(ta => ta.Tag.Name)
                                            .ToList()
                            })
                            .Where(q => q.QuoteId == id)
                            .FirstOrDefaultAsync();

            if (quote == null)
            {
                return NotFound(new { quoteId = id, error = "The quoteId doesn't exist." }); ;
            }

            return Ok(quote);
        }

        // POST: api/quotes
        [HttpPost("api/quotes")]
        public async Task<IActionResult> PostQuote([FromBody] NewQuoteRequest newQuoteRequest)
        {
            // Handling Tags
            var tags = await _context.Tags.ToListAsync();

            List<string> unsupportedTags = new List<string>();

            // If newQuoteRequest includes that tags not exist
            foreach (string tag in newQuoteRequest.Tags)
            {
                // If tag doesn't exist in DB
                if (!tags.Exists(t => t.Name == tag))
                {
                    // Then add it to the error list 
                    unsupportedTags.Add(tag);
                }
            }

            if (unsupportedTags.Count > 0)
            {
                return BadRequest(new { error = "Unsupported tags are included. Remove them and try again." });
            }

            // Quote Description Validation
            if (newQuoteRequest.Description.IsNullOrEmpty())
            {
                return BadRequest(new { error = "Description cannot be empty." });
            }

            // Handling Quotes
            Quote newQuote = new Quote()
            {
                Description = newQuoteRequest.Description,
                Author = newQuoteRequest.Author.IsNullOrEmpty() ? "Anonymous" : newQuoteRequest.Author
            };

            _context.Add(newQuote);
            _context.SaveChanges();

            // Re-populate tags to refelect update
            tags = _context.Tags.ToList();

            // Handling TagAssignments
            List<string> tagList = new List<string>();
            foreach (string tag in newQuoteRequest.Tags)
            {
                // Then add it
                _context.TagAssignments.Add(new TagAssignment()
                {
                    QuoteId = newQuote.QuoteId,
                    TagId = tags.Find(t => t.Name == tag).TagId
                });
                tagList.Add(tag);
            }

            _context.SaveChanges();

            QuoteInfo quote = new QuoteInfo()
            {
                QuoteId = newQuote.QuoteId,
                Description = newQuote.Description,
                Author = newQuote.Author,
                Like = newQuote.Like,
                Tags = tagList
            };

            return CreatedAtAction(nameof(GetQuoteById), new { id = quote.QuoteId }, quote);
        }

        // PUT: api/quotes/5
        [HttpPut("api/quotes")]
        public async Task<IActionResult> PutQuote([FromBody] QuoteInfo newQuoteInfo)
        {
            var quote = await _context.Quotes.Include(q => q.TagAssignments).Where(q => q.QuoteId == newQuoteInfo.QuoteId).FirstOrDefaultAsync();

            if (quote == null)
            {
                return NotFound(new { tagId = newQuoteInfo.QuoteId, error = "The quoteId doesn't exist." }); ;
            }

            // If description is empty, return Bad request
            if (newQuoteInfo.Description.IsNullOrEmpty())
            {
                return BadRequest(new { error = "Description cannot be empty." });
            }

            // Handling Tags
            var tags = await _context.Tags.ToListAsync();

            List<string> unsupportedTags = new List<string>();

            // If newQuoteRequest includes that tags not exist
            foreach (string tag in newQuoteInfo.Tags)
            {
                // If tag doesn't exist in DB
                if (!tags.Exists(t => t.Name == tag))
                {
                    // Then add it to the error list 
                    unsupportedTags.Add(tag);
                }
            }

            if (unsupportedTags.Count > 0)
            {
                return BadRequest(new { error = "Unsupported tags are included. Remove them and try again." });
            }

            // Update Quote
            quote.Description = newQuoteInfo.Description;
            quote.Author = newQuoteInfo.Author.IsNullOrEmpty()? "Anonymous" : newQuoteInfo.Author;
            quote.LastModified = DateTime.Now;

            _context.Update(quote);
            _context.SaveChanges();

            // Remove all TagAssignments for update
            List<TagAssignment> tagAssignments = new List<TagAssignment>();
            quote.TagAssignments.ToList().ForEach(ta => tagAssignments.Add(ta));
            _context.RemoveRange(tagAssignments);

            tagAssignments = new List<TagAssignment>();

            // Update TagAssignments
            foreach (var tag in newQuoteInfo.Tags.ToList())
            {
                tagAssignments.Add(
                    new TagAssignment
                    {
                        QuoteId = quote.QuoteId,
                        TagId = tags.Find(t => t.Name == tag).TagId
                    }
                );
            }

            quote.TagAssignments = tagAssignments;
            _context.SaveChanges();

            // Return updated quote
            QuoteInfo quoteInfo = new QuoteInfo()
            {
                QuoteId = quote.QuoteId,
                Description = quote.Description,
                Author = quote.Author,
                Like = quote.Like,
                Tags = _context.TagAssignments.Include(ta => ta.Tag)
                                                .Where(ta => ta.QuoteId == quote.QuoteId)
                                                .Select(ta => ta.Tag.Name)
                                                .ToList()
            };

            return Ok(quoteInfo);
        }

        // GET: api/quotes/5
        [HttpPut("api/quotes/{id}")]
        public async Task<IActionResult> LikeById(int id)
        {
            var quote = await _context.Quotes.Where(q => q.QuoteId == id).FirstOrDefaultAsync();

            if (quote == null)
            {
                return NotFound(new { quoteId = id, error = "The quoteId doesn't exist." }); ;
            }

            quote.Like++;
            quote.LastModified = DateTime.Now;

            _context.Update(quote);
            _context.SaveChanges();

            QuoteInfo quoteInfo = await _context.Quotes
                            .Include(q => q.TagAssignments)
                            .ThenInclude(q => q.Tag)
                            .Select(q => new QuoteInfo()
                            {
                                QuoteId = q.QuoteId,
                                Description = q.Description,
                                Author = q.Author,
                                Like = q.Like,
                                Tags = _context.TagAssignments.Include(ta => ta.Tag)
                                            .Where(ta => ta.QuoteId == q.QuoteId)
                                            .Select(ta => ta.Tag.Name)
                                            .ToList()
                            })
                            .Where(q => q.QuoteId == id)
                            .FirstOrDefaultAsync();

            return Ok(quote);
        }

        [HttpGet("api/topquotes/{number:int?}")]
        public async Task<IActionResult> GetLikedQuotesByNumber(int number = 10)
        {
            if (number < 0)
            {
                return BadRequest(new { error = "Number of Quotes cannot be negative." });
            }

            var tags = await _context.Tags.Select(t => t.Name).ToListAsync();

            List<QuoteInfo> quotes = await _context.Quotes
                                        .Include(q => q.TagAssignments)
                                        .ThenInclude(q => q.Tag)
                                        .Select(q => new QuoteInfo()
                                        {
                                            QuoteId = q.QuoteId,
                                            Description = q.Description,
                                            Author = q.Author,
                                            Like = q.Like,
                                            Tags = _context.TagAssignments.Include(ta => ta.Tag)
                                                        .Where(ta => ta.QuoteId == q.QuoteId)
                                                        .Select(ta => ta.Tag.Name)
                                                        .ToList()
                                        })
                                        .OrderByDescending(q => q.Like)
                                        .Take(number)
                                        .ToListAsync();

            DateTime quoteLastModified = new DateTime(1970, 1, 1);
            if (quotes.Count > 0)
            {
                quoteLastModified = _context.Quotes.Max(t => t.LastModified).GetValueOrDefault();
            }

            QuoteViewModel quoteViewModel = new QuoteViewModel()
            {
                Quotes = quotes,
                QuotesLastModified = quoteLastModified,
                Tags = tags
            };

            return Ok(quoteViewModel);
        }
    }
}

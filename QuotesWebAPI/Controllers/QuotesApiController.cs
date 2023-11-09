using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult GetQuotes()
        {
            var tags = _context.Tags.Select(t => t.Name).ToList();

            List<QuoteInfo> quotes = _context.Quotes
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
                                        }).ToList();
            
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
        public IActionResult GetQuoteById(int id)
        {
            QuoteInfo quote = _context.Quotes
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
                            .FirstOrDefault();

            return Ok(quote);
        }

        //// PUT: api/quotes/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutQuote(int id, Quote quote)
        //{
        //    if (id != quote.QuoteId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(quote).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!QuoteExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/quotes
        [HttpPost("api/quotes")]
        public IActionResult PostQuote([FromBody] NewQuoteRequest newQuoteRequest)
        {
            // Handling Tags
            var tags = _context.Tags.ToList();
            
            // If newQuoteRequest includes that tags not exist
            foreach (string tag in newQuoteRequest.Tags)
            {
                // If tag doesn't exist in DB
                if (!tags.Exists(t => t.Name == tag))
                {
                    // Then add it
                    _context.Tags.Add(new Tag()
                    {
                        Name = tag
                    });
                }
            }

            // Handling Quotes
            Quote newQuote = new Quote()
            {
                Description = newQuoteRequest.Description,
                Author = newQuoteRequest.Author
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
                Tags = tagList
            };

            return CreatedAtAction(nameof(GetQuoteById), new { id = quote.QuoteId }, quote);
        }

        //// DELETE: api/QuotesApi/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteQuote(int id)
        //{
        //    if (_context.Quotes == null)
        //    {
        //        return NotFound();
        //    }
        //    var quote = await _context.Quotes.FindAsync(id);
        //    if (quote == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Quotes.Remove(quote);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool QuoteExists(int id)
        //{
        //    return (_context.Quotes?.Any(e => e.QuoteId == id)).GetValueOrDefault();
        //}
    }
}

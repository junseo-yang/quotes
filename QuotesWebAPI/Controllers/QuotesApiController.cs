using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuotesWebAPI.Data;
using QuotesWebAPI.Models;
using System.Security.Cryptography;
using System.Xml;

namespace QuotesWebAPI.Controllers
{
    [Route("api/quotes")]
    [ApiController]
    public class QuotesApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public QuotesApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/quotes
        [HttpGet]
        public IActionResult GetQuotes()
        {
            var tags = _context.Tags.Select(t => t.Name).ToList();
            //var quotes = _context.Quotes.ToList();

            List<QuoteInfo> quotes = _context.Quotes.
                                        Include(q => q.TagAssignments)
                                        .ThenInclude(q => q.Tag)
                                        .Select(q => new QuoteInfo()
                                        {
                                            QuoteId = q.QuoteId,
                                            Description = q.Description,
                                            Author = q.Author,
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

            //foreach (var quote in quotes)
            //{
            //    quoteViewModels.Add(new QuoteViewModel
            //    {
            //        QuoteId = quote.QuoteId,
            //        Description = quote.Description,
            //        Author = quote.Author,
            //        Like = quote.Like,
            //        Tags = _context.TagAssignments.Include(ta => ta.Tag)
            //                .Where(ta => ta.QuoteId == quote.QuoteId)
            //                .Select(ta => ta.Tag.Name)
            //                .ToList()
            //    });
            //}


            return Ok(quoteViewModel);
        }

        // GET: api/QuotesApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Quote>> GetQuote(int id)
        {
            if (_context.Quotes == null)
            {
                return NotFound();
            }
            var quote = await _context.Quotes.FindAsync(id);

            if (quote == null)
            {
                return NotFound();
            }

            return quote;
        }

        // PUT: api/quotes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuote(int id, Quote quote)
        {
            if (id != quote.QuoteId)
            {
                return BadRequest();
            }

            _context.Entry(quote).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuoteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/quotes
        [HttpPost]
        public async Task<ActionResult<Quote>> PostQuote(Quote quote)
        {
            if (_context.Quotes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Quotes'  is null.");
            }
            _context.Quotes.Add(quote);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuote", new { id = quote.QuoteId }, quote);
        }

        // DELETE: api/QuotesApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuote(int id)
        {
            if (_context.Quotes == null)
            {
                return NotFound();
            }
            var quote = await _context.Quotes.FindAsync(id);
            if (quote == null)
            {
                return NotFound();
            }

            _context.Quotes.Remove(quote);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuoteExists(int id)
        {
            return (_context.Quotes?.Any(e => e.QuoteId == id)).GetValueOrDefault();
        }
    }
}

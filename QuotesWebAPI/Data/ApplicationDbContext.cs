/* ApplicationDbContext.cs
 * Class for ApplicationDbContext
 * 
 * Revision History:
 *      Junseo Yang, 2023-11-19: Created
 */

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuotesWebAPI.Models;
using QuotesWebAPI.Models.Configuration;

namespace QuotesWebAPI.Data
{
    /// <summary>
    /// Class for ApplicationDbContext
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Call base class version to init Identity tables:
            base.OnModelCreating(builder);

            // Apply our custom role configuration:
            builder.ApplyConfiguration(new RoleConfiguration());

            builder.Entity<Quote>().HasData(
                new Quote
                {
                    QuoteId = 1,
                    Description = "Good friends, good books, and a sleepy conscience: this is the ideal life.",
                    Author = "Mark Twain",
                    Like = 3,
                    LastModified = DateTime.Now
                },
                new Quote
                {
                    QuoteId = 2,
                    Description = "Many people lose the small joys in the hope for the big happiness.",
                    Author = "Pearl S. Buck",
                    Like = 4,
                    LastModified = DateTime.Now
                },
                new Quote
                {
                    QuoteId = 3,
                    Description = "A quiet secluded life in the country, with the possibility of being useful " +
                                    "to people to whom it is easy to do good, and who are not accustomed to " +
                                    "have it done to them; then work which one hopes may be of some use; " +
                                    "then rest, nature, books, music, love for one's neighbor — such is my idea " +
                                    "of happiness.",
                    Author = "Leo Tolstoy",
                    Like = 5,
                    LastModified = DateTime.Now
                }
            );

            builder.Entity<Tag>().HasData(
                new Tag
                {
                    TagId = 1,
                    Name = "books",
                    LastModified = DateTime.Now
                },
                new Tag
                {
                    TagId = 2,
                    Name = "contentment",
                    LastModified = DateTime.Now
                },
                new Tag
                {
                    TagId = 3,
                    Name = "friends",
                    LastModified = DateTime.Now
                },
                new Tag
                {
                    TagId = 4,
                    Name = "friendship",
                    LastModified = DateTime.Now
                },
                new Tag
                {
                    TagId = 5,
                    Name = "life",
                    LastModified = DateTime.Now
                },
                new Tag
                {
                    TagId = 6,
                    Name = "conduct-of-life",
                    LastModified = DateTime.Now
                },
                new Tag
                {
                    TagId = 7,
                    Name = "country",
                    LastModified = DateTime.Now
                },
                new Tag
                {
                    TagId = 8,
                    Name = "happiness",
                    LastModified = DateTime.Now
                },
                new Tag
                {
                    TagId = 9,
                    Name = "music",
                    LastModified = DateTime.Now
                },
                new Tag
                {
                    TagId = 10,
                    Name = "nature",
                    LastModified = DateTime.Now
                },
                new Tag
                {
                    TagId = 11,
                    Name = "work",
                    LastModified = DateTime.Now
                }
            );

            // Configure Many-to-Many relationship between Preference-PreferenceGameCategory-GameCategory
            builder.Entity<TagAssignment>()
                .HasKey(ta => new { ta.QuoteId, ta.TagId });
            builder.Entity<TagAssignment>()
                .HasOne(ta => ta.Tag)
                .WithMany(t => t.TagAssignments)
                .HasForeignKey(ta => ta.TagId);
            builder.Entity<TagAssignment>()
                .HasOne(ta => ta.Quote)
                .WithMany(q => q.TagAssignments)
                .HasForeignKey(ta => ta.QuoteId);

            builder.Entity<TagAssignment>().HasData(
                new TagAssignment
                {
                    QuoteId = 1,
                    TagId = 1
                },
                new TagAssignment
                {
                    QuoteId = 1,
                    TagId = 2
                },
                new TagAssignment
                {
                    QuoteId = 1,
                    TagId = 3
                },
                new TagAssignment
                {
                    QuoteId = 1,
                    TagId = 4
                },
                new TagAssignment
                {
                    QuoteId = 1,
                    TagId = 5
                },
                new TagAssignment
                {
                    QuoteId = 2,
                    TagId = 2
                },
                new TagAssignment
                {
                    QuoteId = 3,
                    TagId = 1
                },
                new TagAssignment
                {
                    QuoteId = 3,
                    TagId = 6
                },
                new TagAssignment
                {
                    QuoteId = 3,
                    TagId = 2
                },
                new TagAssignment
                {
                    QuoteId = 3,
                    TagId = 7
                },
                new TagAssignment
                {
                    QuoteId = 3,
                    TagId = 8
                },
                new TagAssignment
                {
                    QuoteId = 3,
                    TagId = 5
                },
                new TagAssignment
                {
                    QuoteId = 3,
                    TagId = 9
                },
                new TagAssignment
                {
                    QuoteId = 3,
                    TagId = 10
                },
                new TagAssignment
                {
                    QuoteId = 3,
                    TagId = 11
                }
            );
        }

        public DbSet<Quote>? Quotes { get; set; }

        public DbSet<Tag>? Tags { get; set; }

        public DbSet<TagAssignment>? TagAssignments { get; set; }
    }
}
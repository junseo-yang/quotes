
using Microsoft.EntityFrameworkCore;
using QuotesWebAPI.Models;

namespace QuotesWebAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Quote>().HasData(
                new Quote
                {
                    QuoteId = 1,
                    Description = "Good friends, good books, and a sleepy conscience: this is the ideal life.",
                    Author = "Mark Twain",
                    Like = 3
                },
                new Quote
                {
                    QuoteId = 2,
                    Description = "Many people lose the small joys in the hope for the big happiness.",
                    Author = "Pearl S. Buck",
                    Like = 4
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
                    Like = 5
                }
            );

            builder.Entity<Tag>().HasData(
                new Tag
                {
                    TagId = 1,
                    Name = "books"
                },
                new Tag
                {
                    TagId = 2,
                    Name = "contentment"
                },
                new Tag
                {
                    TagId = 3,
                    Name = "friends"
                },
                new Tag
                {
                    TagId = 4,
                    Name = "friendship"
                },
                new Tag
                {
                    TagId = 5,
                    Name = "life"
                },
                new Tag
                {
                    TagId = 6,
                    Name = "conduct-of-life"
                },
                new Tag
                {
                    TagId = 7,
                    Name = "country"
                },
                new Tag
                {
                    TagId = 8,
                    Name = "happniess"
                },
                new Tag
                {
                    TagId = 9,
                    Name = "music"
                },
                new Tag
                {
                    TagId = 10,
                    Name = "nature"
                },
                new Tag
                {
                    TagId = 11,
                    Name = "work"
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
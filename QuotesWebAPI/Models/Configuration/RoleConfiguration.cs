using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace QuotesWebAPI.Models.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole()
                {
                    Name = "QuoteManager",
                    NormalizedName = "QUOTE_MANAGER",
                },
                new IdentityRole()
                {
                    Name = "QuoteUser",
                    NormalizedName = "QUOTE_USER",
                }
            );
        }
    }
}

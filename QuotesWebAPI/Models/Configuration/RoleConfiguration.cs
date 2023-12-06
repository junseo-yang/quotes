/* RoleConfiguration.cs
 * Class for RoleConfiguration inherited from IEntityTypeConfiguration<IdentityRole>
 * 
 * Revision History:
 *      Junseo Yang, 2023-12-10: Created
 */
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace QuotesWebAPI.Models.Configuration
{
    /// <summary>
    /// Class for RoleConfiguration inherited from IEntityTypeConfiguration<IdentityRole>
    /// </summary>
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        /// <summary>
        /// Method for Configure
        /// </summary>
        /// <param name="builder">EntityTypeBuilder<IdentityRole></param>
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

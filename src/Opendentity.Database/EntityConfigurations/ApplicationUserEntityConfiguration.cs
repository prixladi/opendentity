using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opendentity.Database.Entities;

namespace Opendentity.Database.EntityConfigurations;

public class ApplicationUserEntityConfiguration: IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasIndex(e => e.NormalizedEmail)
            .IsUnique();

        builder.HasIndex(e => new { e.UserName, e.Email, e.FirstName, e.LastName })
            .IsTsVectorExpressionIndex(DatabaseConstants._FullTextLanguage);
    }
}

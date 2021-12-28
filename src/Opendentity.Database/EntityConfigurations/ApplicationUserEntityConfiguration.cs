using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opendentity.Database.Entities;

namespace Opendentity.Database.EntityConfigurations;

public class ApplicationUserEntityConfiguration: IEntityTypeConfiguration<ApplicationUser>
{
    public const string _TextSearchIndexV1 = "CREATE INDEX ix_asp_net_users_text_search ON public.\"AspNetUsers\" using gin (((((((COALESCE(first_name, '') || ' ') || COALESCE(last_name, '')) || ' ') || COALESCE(normalized_email, '')) || ' ') || COALESCE(normalized_user_name, '')) gin_trgm_ops)";

    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasIndex(e => e.NormalizedEmail)
            .IsUnique();

        // builder.HasIndex(e => new { e.NormalizedEmail, e.NormalizedUserName, e.FirstName, e.LastName }); _TextSearchIndexV1
    }
}

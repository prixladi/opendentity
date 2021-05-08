using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shamyr.Opendentity.Database.Entities;

namespace Shamyr.Opendentity.Database.EntityConfigurations
{
    public class ApplicationUserEntityConfiguration: IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasIndex(e => e.NormalizedEmail)
                .IsUnique();
        }
    }
}

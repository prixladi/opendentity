using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shamyr.Opendentity.Database.Entities;

namespace Shamyr.Opendentity.Database.EntityConfigurations
{
    public class SettingsEntityConfiguration: IEntityTypeConfiguration<Settings>
    {
        public void Configure(EntityTypeBuilder<Settings> builder)
        {
            builder.HasKey(e => e.Key);
        }
    }
}

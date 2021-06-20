using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shamyr.Opendentity.Database.Entities;

namespace Shamyr.Opendentity.Database.EntityConfigurations
{
    public class SettingsEntityConfiguration: IEntityTypeConfiguration<DbSettings>
    {
        public void Configure(EntityTypeBuilder<DbSettings> builder)
        {
            builder.HasKey(e => e.Key);
        }
    }
}

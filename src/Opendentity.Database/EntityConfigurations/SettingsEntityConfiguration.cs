using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opendentity.Database.Entities;

namespace Opendentity.Database.EntityConfigurations;

public class SettingsEntityConfiguration: IEntityTypeConfiguration<DbSettings>
{
    public void Configure(EntityTypeBuilder<DbSettings> builder)
    {
        builder.HasKey(e => e.Key);
    }
}

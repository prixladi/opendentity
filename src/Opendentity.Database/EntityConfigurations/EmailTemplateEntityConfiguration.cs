using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opendentity.Database.Entities;

namespace Opendentity.Database.EntityConfigurations;

public class EmailTemplateEntityConfiguration: IEntityTypeConfiguration<EmailTemplate>
{
    public void Configure(EntityTypeBuilder<EmailTemplate> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasIndex(e => e.Type)
            .IsUnique();
    }
}

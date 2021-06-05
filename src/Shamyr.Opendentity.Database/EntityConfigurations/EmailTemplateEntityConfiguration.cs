using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shamyr.Opendentity.Database.Entities;

namespace Shamyr.Opendentity.Database.EntityConfigurations
{
    public class EmailTemplateEntityConfiguration: IEntityTypeConfiguration<EmailTemplate>
    {
        public void Configure(EntityTypeBuilder<EmailTemplate> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasIndex(e => e.Type)
                .IsUnique();
        }
    }
}

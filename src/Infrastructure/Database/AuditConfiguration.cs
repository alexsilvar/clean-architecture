using Domain.Audits;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database;

internal sealed class AuditConfiguration : IEntityTypeConfiguration<Audit>
{
    public void Configure(EntityTypeBuilder<Audit> builder)
    {
        builder.HasKey(audit => audit.Id);

        builder.Property(audit => audit.EntityName).HasMaxLength(200);
        builder.Property(audit => audit.UserId).HasMaxLength(200);

        builder.OwnsMany(audit => audit.ChangedValues, changedValue =>
        {
            changedValue.WithOwner().HasForeignKey("AuditId");
            changedValue.Property<Guid>("Id");
            changedValue.HasKey("Id");
            changedValue.Property(entry => entry.Key).HasMaxLength(200);
        });

        builder.OwnsMany(audit => audit.ChangedProperties, changedProperty =>
        {
            changedProperty.WithOwner().HasForeignKey("AuditId");
            changedProperty.Property<Guid>("Id");
            changedProperty.HasKey("Id");
            changedProperty.Property(property => property.Column).HasMaxLength(200);
        });

        builder.HasIndex(a => a.CorrelationId);
        builder.HasIndex(a => new { a.EntityName, a.EntityId });
    }
}
namespace TrainsOnline.Persistence.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TrainsOnline.Domain.Entities.Audit;

    public class EntityAuditLogConfiguration : IEntityTypeConfiguration<EntityAuditLog>
    {
        public void Configure(EntityTypeBuilder<EntityAuditLog> builder)
        {
            builder.Property(x => x.Action).HasConversion<int>();
        }
    }
}

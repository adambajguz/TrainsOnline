namespace TrainsOnline.Persistence.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TrainsOnline.Domain.Entities;

    public class RouteConfiguration : IEntityTypeConfiguration<Route>
    {
        public void Configure(EntityTypeBuilder<Route> builder)
        {
            builder.HasOne(route => route.From)
                   .WithMany(station => station.Departures)
                   .HasForeignKey(route => route.FromId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(route => route.To)
                   .WithMany(station => station.Arrivals)
                   .HasForeignKey(route => route.ToId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

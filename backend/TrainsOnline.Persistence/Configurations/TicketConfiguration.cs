namespace TrainsOnline.Persistence.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using TrainsOnline.Domain.Entities;

    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasOne(ticket => ticket.User)
                   .WithMany(user => user.Tickets)
                   .HasForeignKey(ticket => ticket.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

namespace TrainsOnline.Persistence.Configurations
{
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

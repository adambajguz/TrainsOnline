namespace TrainsOnline.Persistence.DbContext
{
    using Microsoft.EntityFrameworkCore;
    using TrainsOnline.Application.Interfaces.DbContext;
    using TrainsOnline.Domain.Entities;

    public class PKPAppDbContext : DbContext, IPKPAppDbContext
    {
        public PKPAppDbContext(DbContextOptions<PKPAppDbContext> options) : base(options)
        {

        }

        public virtual DbSet<EntityAuditLog> EntityAuditLogs { get; set; } = default!;

        public virtual DbSet<Route> Routes { get; set; } = default!;
        public virtual DbSet<Station> Stations { get; set; } = default!;
        public virtual DbSet<Ticket> Tickets { get; set; } = default!;
        public virtual DbSet<User> Users { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PKPAppDbContext).Assembly);
        }
    }
}

namespace TrainsOnline.Persistence.DbContext
{
    using Microsoft.EntityFrameworkCore;
    using Persistence.Infrastructure;

    public class TrainsOnlineRelationalDbContextFactory : DesignTimeDbContextFactoryBase<TrainsOnlineRelationalDbContext>
    {
        protected override TrainsOnlineRelationalDbContext CreateNewInstance(DbContextOptions<TrainsOnlineRelationalDbContext> options)
        {
            return new TrainsOnlineRelationalDbContext(options);
        }
    }
}

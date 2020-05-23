namespace TrainsOnline.Persistence.DbContext
{
    using Microsoft.EntityFrameworkCore;
    using Persistence.Infrastructure;

    public class PKPAppDbContextFactory : DesignTimeDbContextFactoryBase<PKPAppDbContext>
    {
        protected override PKPAppDbContext CreateNewInstance(DbContextOptions<PKPAppDbContext> options)
        {
            return new PKPAppDbContext(options);
        }
    }
}

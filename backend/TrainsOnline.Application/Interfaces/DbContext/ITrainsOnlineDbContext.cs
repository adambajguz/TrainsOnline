namespace TrainsOnline.Application.Interfaces
{
    using Domain.Entities;
    using MongoDB.Driver;

    public interface ITrainsOnlineDbContext : IGenericMongoDatabaseContext
    {
        IMongoCollection<RouteLog> RouteLogs { get; }
    }
}

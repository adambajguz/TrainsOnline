namespace TrainsOnline.Application.Interfaces
{
    using Domain.Entities;
    using MongoDB.Driver;

    public interface ITrainsOnlineMongoDbContext : IGenericMongoDatabaseContext
    {
        IMongoCollection<RouteLog> RouteLogs { get; }
    }
}

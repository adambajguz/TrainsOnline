namespace TrainsOnline.Application.Interfaces
{
    using Domain.Entities;
    using MongoDB.Driver;

    public interface ITrainsOnlineMongoDbContext : IGenericMongoDatabaseContext
    {
        IMongoCollection<AnalyticsRecord> AnalyticsRecords { get; }

        IMongoCollection<RouteLog> RouteLogs { get; }
        IMongoCollection<RouteReport> RouteReports { get; }
    }
}

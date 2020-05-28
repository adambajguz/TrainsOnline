namespace TrainsOnline.Persistence.DbContext
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;
    using MongoDB.Driver;
    using TrainsOnline.Application.Interfaces;
    using TrainsOnline.Domain.Abstractions.Base;
    using TrainsOnline.Domain.Entities;
    using TrainsOnline.Persistence.Extensions;

    public class TrainsOnlineMongoDbContext : ITrainsOnlineMongoDbContext
    {
        public MongoClient DbClient { get; }
        public IMongoDatabase Db { get; }

        public TrainsOnlineMongoDbContext(IConfiguration configuration, IOptions<DatabaseSettings> options)
        {
            DatabaseSettings databaseSettings = options.Value;

            string? connectionString = configuration.GetConnectionString(ConnectionStringsNames.MongoDatabase);
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new System.ArgumentNullException(nameof(options));

            string? databaseName = databaseSettings.MongoDatabaseName;
            if (string.IsNullOrWhiteSpace(databaseName))
                throw new System.ArgumentNullException(nameof(options));

            DbClient = new MongoClient(connectionString);
            Db = DbClient.GetDatabase(databaseName);
        }

        public IMongoCollection<RouteLog> RouteLogs => Db.GetCollection<RouteLog>();
        public IMongoCollection<RouteReport> RouteReports => Db.GetCollection<RouteReport>();

        public IMongoCollection<T> GetCollection<T>()
            where T : class, IBaseMongoEntity
        {
            return Db.GetCollection<T>();
        }
    }
}

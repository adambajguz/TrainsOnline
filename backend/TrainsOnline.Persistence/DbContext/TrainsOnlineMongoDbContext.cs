namespace TrainsOnline.Persistence.DbContext
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
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
                throw new ArgumentNullException(nameof(options));

            string? databaseName = databaseSettings.MongoDatabaseName;
            if (string.IsNullOrWhiteSpace(databaseName))
                throw new ArgumentNullException(nameof(options));

            DbClient = new MongoClient(connectionString);
            Db = DbClient.GetDatabase(databaseName);

            AddOrUpdateAsync<AnalyticsRecord>(x => x.Hash).Wait();
            AddOrUpdateAsync<AnalyticsRecord>(x => x.Visits).Wait();
        }

        public IMongoCollection<AnalyticsRecord> AnalyticsRecords => Db.GetCollection<AnalyticsRecord>();

        public IMongoCollection<RouteLog> RouteLogs => Db.GetCollection<RouteLog>();
        public IMongoCollection<RouteReport> RouteReports => Db.GetCollection<RouteReport>();

        public IMongoCollection<T> GetCollection<T>()
            where T : class, IBaseMongoEntity
        {
            return Db.GetCollection<T>();
        }

        public async Task AddOrUpdateAsync<TDocument>(Expression<Func<TDocument, object>> field)
            where TDocument : class, IBaseMongoEntity
        {
            IMongoCollection<TDocument> mongoCollection = GetCollection<TDocument>();
            //IAsyncCursor<BsonDocument> indexes = await mongoCollection.Indexes.ListAsync();

            CreateIndexModel<TDocument> indexModel = new CreateIndexModel<TDocument>(Builders<TDocument>.IndexKeys.Ascending(field));
            await mongoCollection.Indexes.CreateOneAsync(indexModel);
        }
    }
}

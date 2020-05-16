namespace TrainsOnline.Persistence.DbContext
{
    using Microsoft.Extensions.Options;
    using MongoDB.Driver;
    using TrainsOnline.Application.Interfaces;
    using TrainsOnline.Domain.Entities;
    using TrainsOnline.Persistence.Extensions;

    public class TrainsOnlineDbContext : ITrainsOnlineDbContext
    {
        private MongoClient MongoDbClient { get; }
        private IMongoDatabase Db { get; }

        public TrainsOnlineDbContext(IOptions<DatabaseSettings> options)
        {
            DatabaseSettings databaseSettings = options.Value;

            string? connectionString = databaseSettings.ConnectionString;
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new System.ArgumentNullException(nameof(options));

            string? databaseName = databaseSettings.DatabaseName;
            if (string.IsNullOrWhiteSpace(databaseName))
                throw new System.ArgumentNullException(nameof(options));

            MongoDbClient = new MongoClient(connectionString);
            Db = MongoDbClient.GetDatabase(databaseName);
        }

        public IMongoCollection<Route> Routes => Db.GetCollection<Route>();
        public IMongoCollection<Station> Stations => Db.GetCollection<Station>();
        public IMongoCollection<Ticket> Tickets => Db.GetCollection<Ticket>();
        public IMongoCollection<User> Users => Db.GetCollection<User>();
    }
}

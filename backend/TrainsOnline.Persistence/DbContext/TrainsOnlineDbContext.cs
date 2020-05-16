namespace TrainsOnline.Persistence.DbContext
{
    using Microsoft.Extensions.Options;
    using MongoDB.Driver;
    using TrainsOnline.Application.Interfaces;
    using TrainsOnline.Domain.Entities;

    public class TrainsOnlineDbContext : ITrainsOnlineDbContext
    {
        private MongoClient MongoDbClient { get; }

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

            var collectionName = "Users";
            var database = MongoDbClient.GetDatabase(databaseName);
            Users = database.GetCollection<User>(collectionName);
        }

        public IMongoCollection<Route> Routes { get; set; } = default!;
        public IMongoCollection<Station> Stations { get; set; } = default!;
        public IMongoCollection<Ticket> Tickets { get; set; } = default!;
        public IMongoCollection<User> Users { get; set; } = default!;
    }
}

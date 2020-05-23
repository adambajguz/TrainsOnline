namespace TrainsOnline.Persistence.DbContext
{
    public sealed class DatabaseSettings
    {
        public string? SQLConnectionString { get; set; }
        public string? SQLDatabaseName { get; set; }        
        
        public string? MongoConnectionString { get; set; }
        public string? MongoDatabaseName { get; set; }
    }
}

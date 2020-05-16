namespace TrainsOnline.Application.Interfaces
{
    using MongoDB.Driver;

    public interface IGenericDatabaseContext
    {
        MongoClient DbClient { get; }
        IMongoDatabase Db { get; }

        public IMongoCollection<T> GetCollection<T>()
            where T : class;
    }
}

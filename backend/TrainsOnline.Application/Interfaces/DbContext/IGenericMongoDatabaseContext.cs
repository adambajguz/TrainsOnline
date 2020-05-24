namespace TrainsOnline.Application.Interfaces
{
    using MongoDB.Driver;
    using TrainsOnline.Domain.Abstractions.Base;

    public interface IGenericMongoDatabaseContext
    {
        MongoClient DbClient { get; }
        IMongoDatabase Db { get; }

        public IMongoCollection<T> GetCollection<T>()
            where T : class, IBaseMongoEntity;
    }
}

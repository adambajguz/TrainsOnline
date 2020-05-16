namespace TrainsOnline.Persistence.Extensions
{
    using Humanizer;
    using MongoDB.Driver;

    public static class MongoClientExtensions
    {       
        public static IMongoCollection<T> GetCollection<T>(this IMongoDatabase mongoDatabase, MongoCollectionSettings? mongoCollectionSettings = null)
           where T : class
        {
            string collectionName = nameof(T).Pluralize();

            return mongoDatabase.GetCollection<T>(collectionName, mongoCollectionSettings);
        }
    }
}

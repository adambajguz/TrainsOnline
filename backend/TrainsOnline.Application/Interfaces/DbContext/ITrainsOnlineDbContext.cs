namespace TrainsOnline.Application.Interfaces
{
    using Domain.Entities;
    using MongoDB.Driver;

    public interface ITrainsOnlineDbContext : IGenericMongoDatabaseContext
    {
        IMongoCollection<Route> Routes { get; }
        IMongoCollection<Station> Stations { get; }
        IMongoCollection<Ticket> Tickets { get; }
        IMongoCollection<User> Users { get; }
    }
}

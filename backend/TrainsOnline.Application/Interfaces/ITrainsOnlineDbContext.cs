namespace TrainsOnline.Application.Interfaces
{
    using Domain.Entities;
    using MongoDB.Driver;

    public interface ITrainsOnlineDbContext : IGenericDatabaseContext
    {
        IMongoCollection<Route> Routes { get; set; }
        IMongoCollection<Station> Stations { get; set; }
        IMongoCollection<Ticket> Tickets { get; set; }
        IMongoCollection<User> Users { get; set; }
    }
}

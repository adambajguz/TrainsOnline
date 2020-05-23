namespace TrainsOnline.Application.Interfaces.Repository
{
    using System;
    using System.Threading.Tasks;
    using Application.Interfaces.Repository.Generic;
    using Domain.Entities;

    public interface IStationsRepository : IGenericMongoRepository<Station>
    {
        Task<Station> GetStationFullDetails(Guid id);
    }
}

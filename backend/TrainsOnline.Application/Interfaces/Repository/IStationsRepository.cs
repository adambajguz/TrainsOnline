namespace TrainsOnline.Application.Interfaces.Repository
{
    using System;
    using System.Threading.Tasks;
    using Application.Interfaces.Repository.Generic;
    using Domain.Entities;

    public interface IStationsRepository : IGenericRelationalRepository<Station>
    {
        Task<Station> GetStationFullDetails(Guid id);
    }
}

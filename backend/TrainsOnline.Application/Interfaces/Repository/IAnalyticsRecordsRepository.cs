namespace TrainsOnline.Application.Interfaces.Repository
{
    using Application.Interfaces.Repository.Generic;
    using Domain.Entities;

    public interface IAnalyticsRecordsRepository : IGenericMongoRepository<AnalyticsRecord>
    {

    }
}

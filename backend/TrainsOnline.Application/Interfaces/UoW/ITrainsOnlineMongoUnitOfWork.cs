namespace TrainsOnline.Application.Interfaces.UoW
{
    using TrainsOnline.Application.Interfaces.Repository;
    using TrainsOnline.Application.Interfaces.UoW.Generic;

    public interface ITrainsOnlineMongoUnitOfWork : IGenericMongoUnitOfWork
    {
        IRouteLogsRepository RouteLogs { get; }
    }
}

namespace TrainsOnline.Application.Interfaces.UoW
{
    using TrainsOnline.Application.Interfaces.Repository;
    using TrainsOnline.Application.Interfaces.UoW.Generic;

    public interface ITrainsOnlineSQLUnitOfWork : IGenericAuditableRelationalUnitOfWork
    {
        IRoutesRepository Routes { get; }
        IStationsRepository Stations { get; }
        ITicketsRepository Tickets { get; }
        IUsersRepository Users { get; }
    }
}

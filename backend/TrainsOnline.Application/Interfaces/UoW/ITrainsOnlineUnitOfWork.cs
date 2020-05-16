namespace TrainsOnline.Application.Interfaces.UoW
{
    using TrainsOnline.Application.Interfaces.Repository;
    using TrainsOnline.Application.Interfaces.UoW.Generic;

    public interface ITrainsOnlineUnitOfWork : IGenericUnitOfWork
    {
        IRoutesRepository RoutesRepository { get; }
        IStationsRepository StationsRepository { get; }
        ITicketsRepository TicketsRepository { get; }
        IUsersRepository UsersRepository { get; }
    }
}

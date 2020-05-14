namespace TrainsOnline.Application.Interfaces.UoW.Generic
{
    using Application.Interfaces.UoW;
    using TrainsOnline.Application.Interfaces.Repository;

    public interface IPKPAppDbUnitOfWork : IGenericUnitOfWork
    {
        IRoutesRepository RoutesRepository { get; }
        IStationsRepository StationsRepository { get; }
        ITicketsRepository TicketsRepository { get; }
        IUsersRepository UsersRepository { get; }
    }
}

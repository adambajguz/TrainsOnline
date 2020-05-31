namespace TrainsOnline.Application.Interfaces.Repository
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Application.Interfaces.Repository.Generic;
    using Domain.Entities;

    public interface IRouteLogsRepository : IGenericMongoRepository<RouteLog>
    {
        Task<T> GetMinAsync<T>(Guid routeId, Expression<Func<RouteLog, T>> filter);
        Task<T> GetMaxAsync<T>(Guid routeId, Expression<Func<RouteLog, T>> filter);
        Task<double> GetMeanAsync(Guid routeId, Expression<Func<RouteLog, double>> filter);
        Task<double> GetSdSampleAsync(Guid routeId, Expression<Func<RouteLog, double>> filter);

        Task<TimeSpan> GetDuration(Guid routeId);
    }
}

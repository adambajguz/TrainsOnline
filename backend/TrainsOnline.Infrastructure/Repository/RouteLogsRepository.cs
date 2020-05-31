namespace TrainsOnline.Infrastructure.Repository
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Application.Interfaces;
    using AutoMapper;
    using MongoDB.Driver.Linq;
    using TrainsOnline.Application.Interfaces.Repository;
    using TrainsOnline.Domain.Entities;

    public class RouteLogsRepository : GenericMongoRepository<RouteLog>, IRouteLogsRepository
    {
        public RouteLogsRepository(ICurrentUserService currentUserService,
                                   IGenericMongoDatabaseContext context,
                                   IMapper mapper) : base(currentUserService, context, mapper)
        {

        }

        public async Task<T> GetMinAsync<T>(Guid routeId, Expression<Func<RouteLog, T>> filter)
        {
            return await GetQueryable(x => x.RouteId == routeId).Select(filter)
                                                                .MinAsync();
        }

        public async Task<T> GetMaxAsync<T>(Guid routeId, Expression<Func<RouteLog, T>> filter)
        {
            return await GetQueryable(x => x.RouteId == routeId).Select(filter)
                                                                .MaxAsync();
        }

        public async Task<double> GetMeanAsync(Guid routeId, Expression<Func<RouteLog, double>> filter)
        {
            return await GetQueryable(x => x.RouteId == routeId).AverageAsync(filter);
        }

        public async Task<double> GetSdSampleAsync(Guid routeId, Expression<Func<RouteLog, double>> filter)
        {
            return await GetQueryable(x => x.RouteId == routeId).StandardDeviationSampleAsync(filter);
        }

        public async Task<TimeSpan> GetDuration(Guid routeId)
        {
            DateTime start = await GetQueryable(x => x.RouteId == routeId, x => x.OrderBy(y => y.Timestamp)).Take(1)
                                                                                                            .Select(x => x.Timestamp)
                                                                                                            .FirstOrDefaultAsync();

            DateTime end = await GetQueryable(x => x.RouteId == routeId, x => x.OrderByDescending(y => y.Timestamp)).Take(1)
                                                                                                                    .Select(x => x.Timestamp)
                                                                                                                    .FirstOrDefaultAsync();

            return end - start;
        }
    }
}
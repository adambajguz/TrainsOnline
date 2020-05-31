namespace TrainsOnline.Application.Handlers.RouteReportHandlers.Commands.CreateRouteReport
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Entities;
    using MediatR;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Interfaces.UoW;

    public class CreateRouteReportCommand : IRequest<IdResponse>
    {
        public IdRequest Data { get; }

        public CreateRouteReportCommand(IdRequest data)
        {
            Data = data;
        }

        public class Handler : IRequestHandler<CreateRouteReportCommand, IdResponse>
        {
            private readonly ITrainsOnlineMongoUnitOfWork _uow;
            private readonly ITrainsOnlineSQLUnitOfWork _uowRel;

            public Handler(ITrainsOnlineMongoUnitOfWork uow, ITrainsOnlineSQLUnitOfWork uowRel)
            {
                _uow = uow;
                _uowRel = uowRel;
            }

            public async Task<IdResponse> Handle(CreateRouteReportCommand request, CancellationToken cancellationToken)
            {
                IdRequest data = request.Data;
                Guid routeId = data.Id;

                Route? route = await _uowRel.Routes.GetByIdWithRelatedAsync(routeId, x => x.From, x => x.To) ?? throw new NullReferenceException(nameof(route));
                IEnumerable<RouteLog>? logs = await _uow.RouteLogs.GetAllAsync(x => x.RouteId == routeId);

                RouteReport entity = new RouteReport
                {
                    RouteId = routeId,
                    FromName = route.From.Name,
                    ToName = route.To.Name,

                    VoltageMin = await _uow.RouteLogs.GetMinAsync(routeId, x => x.Voltage),
                    VoltageMean = await _uow.RouteLogs.GetMeanAsync(routeId, x => x.Voltage),
                    VoltageSd = await _uow.RouteLogs.GetSdSampleAsync(routeId, x => x.Voltage),
                    VoltageMax = await _uow.RouteLogs.GetMaxAsync(routeId, x => x.Voltage),

                    CurrentMean = await _uow.RouteLogs.GetMeanAsync(routeId, x => x.Current),
                    CurrentSd = await _uow.RouteLogs.GetSdSampleAsync(routeId, x => x.Current),
                    CurrentMax = await _uow.RouteLogs.GetMaxAsync(routeId, x => x.Current),

                    SpeedMean = await _uow.RouteLogs.GetMeanAsync(routeId, x => x.Speed),
                    SpeedSd = await _uow.RouteLogs.GetSdSampleAsync(routeId, x => x.Speed),
                    SpeedMax = await _uow.RouteLogs.GetMaxAsync(routeId, x => x.Speed),

                    NumberOfStops = await _uow.RouteLogs.GetCountAsync(x => x.Speed == 0),
                    Duration = await _uow.RouteLogs.GetDuration(routeId)
                };

                entity.PowerMean = entity.VoltageMean * entity.CurrentMean;
                entity.PowerSd = entity.VoltageSd * entity.CurrentSd;
                entity.PowerMax = entity.VoltageMax * entity.CurrentMax;

                await _uow.RouteReports.AddAsync(entity);

                return new IdResponse(entity.Id);
            }
        }
    }
}

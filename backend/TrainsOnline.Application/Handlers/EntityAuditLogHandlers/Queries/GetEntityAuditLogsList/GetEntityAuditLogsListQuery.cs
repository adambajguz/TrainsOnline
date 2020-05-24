namespace TrainsOnline.Application.Handlers.EntityAuditLog.Queries.GetRouteLogsList
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using TrainsOnline.Application.Interfaces.UoW;

    public class GetEntityAuditLogsListQuery : IRequest<GetEntityAuditLogsListResponse>
    {
        public GetEntityAuditLogsListQuery()
        {

        }

        public class Handler : IRequestHandler<GetEntityAuditLogsListQuery, GetEntityAuditLogsListResponse>
        {
            private readonly ITrainsOnlineMongoUnitOfWork _uow;

            public Handler(ITrainsOnlineMongoUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<GetEntityAuditLogsListResponse> Handle(GetEntityAuditLogsListQuery request, CancellationToken cancellationToken)
            {
                return new GetEntityAuditLogsListResponse
                {
                    EntityAuditLogs = await _uow.E.ProjectToAsync<GetEntityAuditLogsListResponse.RouteLogLookupModel>(cancellationToken: cancellationToken)
                };
            }
        }
    }
}


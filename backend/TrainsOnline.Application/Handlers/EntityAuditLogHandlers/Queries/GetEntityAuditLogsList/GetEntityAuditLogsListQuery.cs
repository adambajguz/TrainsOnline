namespace TrainsOnline.Application.Handlers.EntityAuditLog.Queries.GetRouteLogsList
{
    using System.Linq;
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
            private readonly ITrainsOnlineSQLUnitOfWork _uow;

            public Handler(ITrainsOnlineSQLUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<GetEntityAuditLogsListResponse> Handle(GetEntityAuditLogsListQuery request, CancellationToken cancellationToken)
            {
                return new GetEntityAuditLogsListResponse
                {
                    EntityAuditLogs = await _uow.EntityAuditLogs.ProjectToAsync<GetEntityAuditLogsListResponse.EntityAuditLogLookupModel>(orderBy: (x) => x.OrderByDescending(x => x.CreatedOn),
                                                                                                                                                    cancellationToken: cancellationToken)
                };
            }
        }
    }
}


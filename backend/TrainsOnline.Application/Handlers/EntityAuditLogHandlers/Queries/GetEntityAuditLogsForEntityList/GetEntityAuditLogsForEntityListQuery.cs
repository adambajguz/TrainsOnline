namespace TrainsOnline.Application.Handlers.EntityAuditLog.Queries.GetRouteLogsList
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Interfaces.UoW;
    using TrainsOnline.Domain.Entities;
    using ResponseListItem = GetEntityAuditLogsForEntityListResponse.EntityAuditLogForEntityLookupModel;

    public class GetEntityAuditLogsForEntityListQuery : IRequest<GetEntityAuditLogsForEntityListResponse>
    {
        public IdRequest Data { get; }

        public GetEntityAuditLogsForEntityListQuery(IdRequest data)
        {
            Data = data;
        }

        public class Handler : IRequestHandler<GetEntityAuditLogsForEntityListQuery, GetEntityAuditLogsForEntityListResponse>
        {
            private readonly ITrainsOnlineSQLUnitOfWork _uow;

            public Handler(ITrainsOnlineSQLUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<GetEntityAuditLogsForEntityListResponse> Handle(GetEntityAuditLogsForEntityListQuery request, CancellationToken cancellationToken)
            {
                List<ResponseListItem> list = await _uow.EntityAuditLogsRepository.ProjectToAsync<ResponseListItem>(filter: x => x.Key == request.Data.Id,
                                                                                                                    orderBy: (x) => x.OrderByDescending(x => x.CreatedOn),
                                                                                                                    cancellationToken: cancellationToken);
                bool listHasValues = list.Count > 0;
                bool exists = listHasValues;
                if (listHasValues)
                {
                    string tableName = list[0].TableName;

                    _uow.GetRepositoryByName<User>(tableName);

                    //_uow.

                    exists = false;
                }

                return new GetEntityAuditLogsForEntityListResponse(list, exists);
            }
        }
    }
}


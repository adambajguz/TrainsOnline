namespace TrainsOnline.Application.Handlers.EntityAuditLog.Queries.GetRouteLogDetails
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using FluentValidation;
    using MediatR;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Handlers;
    using TrainsOnline.Application.Interfaces.UoW;
    using TrainsOnline.Domain.Entities;

    public class GetEntityAuditLogDetailsQuery : IRequest<GetEntityAuditLogDetailsResponse>
    {
        public IdRequest Data { get; }

        public GetEntityAuditLogDetailsQuery(IdRequest data)
        {
            Data = data;
        }

        public class Handler : IRequestHandler<GetEntityAuditLogDetailsQuery, GetEntityAuditLogDetailsResponse>
        {
            private readonly ITrainsOnlineMongoUnitOfWork _uow;
            private readonly IMapper _mapper;

            public Handler(ITrainsOnlineMongoUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<GetEntityAuditLogDetailsResponse> Handle(GetEntityAuditLogDetailsQuery request, CancellationToken cancellationToken)
            {
                IdRequest data = request.Data;

                RouteLog entity = await _uow.RouteLogsRepository.GetByIdAsync(data.Id);

                EntityRequestByIdValidator<RouteLog>.Model validationModel = new EntityRequestByIdValidator<RouteLog>.Model(data, entity);
                await new EntityRequestByIdValidator<RouteLog>().ValidateAndThrowAsync(validationModel, cancellationToken: cancellationToken);

                return _mapper.Map<GetEntityAuditLogDetailsResponse>(entity);
            }
        }
    }
}

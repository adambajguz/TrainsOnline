namespace TrainsOnline.Application.Handlers.EntityAuditLog.Commands.CreateRouteLog
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Entities;
    using FluentValidation;
    using MediatR;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Interfaces.UoW;

    public class CreateEntityAuditLogCommand : IRequest<IdResponse>
    {
        public CreateEntityAuditLogRequest Data { get; }

        public CreateEntityAuditLogCommand(CreateEntityAuditLogRequest data)
        {
            Data = data;
        }

        public class Handler : IRequestHandler<CreateEntityAuditLogCommand, IdResponse>
        {
            private readonly ITrainsOnlineMongoUnitOfWork _uow;
            private readonly IMapper _mapper;

            public Handler(ITrainsOnlineMongoUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<IdResponse> Handle(CreateEntityAuditLogCommand request, CancellationToken cancellationToken)
            {
                CreateEntityAuditLogRequest data = request.Data;

                await new CreateEntityAuditLogCommandValidator(_uow).ValidateAndThrowAsync(data, cancellationToken: cancellationToken);

                RouteLog entity = _mapper.Map<RouteLog>(data);
                await _uow.RouteLogsRepository.AddAsync(entity);

                await _uow.SaveChangesAsync(cancellationToken);

                return new IdResponse(entity.Id);
            }
        }
    }
}

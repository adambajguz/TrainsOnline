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

    public class RevertUsingEntityAuditLogCommand : IRequest<IdResponse>
    {
        public CreateEntityAuditLogRequest Data { get; }

        public RevertUsingEntityAuditLogCommand(CreateEntityAuditLogRequest data)
        {
            Data = data;
        }

        public class Handler : IRequestHandler<RevertUsingEntityAuditLogCommand, IdResponse>
        {
            private readonly ITrainsOnlineSQLUnitOfWork _uow;
            private readonly IMapper _mapper;

            public Handler(ITrainsOnlineSQLUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<IdResponse> Handle(RevertUsingEntityAuditLogCommand request, CancellationToken cancellationToken)
            {
                CreateEntityAuditLogRequest data = request.Data;

                await new CreateEntityAuditLogCommandValidator(_uow).ValidateAndThrowAsync(data, cancellationToken: cancellationToken);

                EntityAuditLog entity = _mapper.Map<EntityAuditLog>(data);
                _uow.EntityAuditLogsRepository.Add(entity);

                await _uow.SaveChangesAsync(cancellationToken);

                return new IdResponse(entity.Id);
            }
        }
    }
}

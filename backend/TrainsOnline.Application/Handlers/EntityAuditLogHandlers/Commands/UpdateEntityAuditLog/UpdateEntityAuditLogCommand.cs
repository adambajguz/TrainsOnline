namespace TrainsOnline.Application.Handlers.EntityAuditLog.Commands.UpdateRouteLog
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Entities;
    using FluentValidation;
    using MediatR;
    using TrainsOnline.Application.Interfaces.UoW;

    public class UpdateEntityAuditLogCommand : IRequest
    {
        public UpdateEntityAuditLogRequest Data { get; }

        public UpdateEntityAuditLogCommand(UpdateEntityAuditLogRequest data)
        {
            Data = data;
        }

        public class Handler : IRequestHandler<UpdateEntityAuditLogCommand, Unit>
        {
            private readonly ITrainsOnlineMongoUnitOfWork _uow;
            private readonly IMapper _mapper;

            public Handler(ITrainsOnlineMongoUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(UpdateEntityAuditLogCommand request, CancellationToken cancellationToken)
            {
                UpdateEntityAuditLogRequest data = request.Data;

                RouteLog routeLog = await _uow.RouteLogsRepository.GetByIdAsync(data.Id);

                UpdateEntityAuditLogCommandValidator.Model validationModel = new UpdateEntityAuditLogCommandValidator.Model(data, routeLog);
                await new UpdateEntityAuditLogCommandValidator(_uow).ValidateAndThrowAsync(validationModel, cancellationToken: cancellationToken);

                _mapper.Map(data, routeLog);
                await _uow.RouteLogsRepository.UpdateAsync(routeLog);

                await _uow.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}

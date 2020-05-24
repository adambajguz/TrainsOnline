namespace TrainsOnline.Application.Handlers.EntityAuditLog.Commands.DeleteRouteLog
{
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Entities;
    using FluentValidation;
    using MediatR;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Handlers;
    using TrainsOnline.Application.Interfaces.UoW;

    public class DeleteEntityAuditLogCommand : IRequest
    {
        public IdRequest Data { get; }

        public DeleteEntityAuditLogCommand(IdRequest data)
        {
            Data = data;
        }

        public class Handler : IRequestHandler<DeleteEntityAuditLogCommand, Unit>
        {
            private readonly ITrainsOnlineMongoUnitOfWork _uow;

            public Handler(ITrainsOnlineMongoUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<Unit> Handle(DeleteEntityAuditLogCommand request, CancellationToken cancellationToken)
            {
                IdRequest data = request.Data;

                RouteLog routeLog = await _uow.RouteLogsRepository.GetByIdAsync(data.Id);

                EntityRequestByIdValidator<RouteLog>.Model validationModel = new EntityRequestByIdValidator<RouteLog>.Model(data, routeLog);
                await new EntityRequestByIdValidator<RouteLog>().ValidateAndThrowAsync(validationModel, cancellationToken: cancellationToken);

                await _uow.RouteLogsRepository.RemoveAsync(routeLog);
                await _uow.SaveChangesAsync();

                return await Unit.Task;
            }
        }
    }
}

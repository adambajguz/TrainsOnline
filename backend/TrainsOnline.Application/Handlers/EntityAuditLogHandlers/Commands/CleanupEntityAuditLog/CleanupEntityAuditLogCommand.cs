namespace TrainsOnline.Application.Handlers.EntityAuditLogHandlers.Commands.CleanupEntityAuditLog
{
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using MediatR;
    using TrainsOnline.Application.Interfaces.UoW;

    public class CleanupEntityAuditLogCommand : IRequest
    {
        public CleanupEntityAuditLogRequest Data { get; }

        public CleanupEntityAuditLogCommand(CleanupEntityAuditLogRequest data)
        {
            Data = data;
        }

        public class Handler : IRequestHandler<CleanupEntityAuditLogCommand>
        {
            private readonly ITrainsOnlineSQLUnitOfWork _uow;

            public Handler(ITrainsOnlineSQLUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<Unit> Handle(CleanupEntityAuditLogCommand request, CancellationToken cancellationToken)
            {
                CleanupEntityAuditLogRequest data = request.Data;

                //EntityAuditLog routeLog = await _uow.EntityAuditLogsRepository.GetByIdAsync(data.Id);

                await new CleanupEntityAuditLogCommandValidator(_uow).ValidateAndThrowAsync(data, cancellationToken: cancellationToken);

                //_uow.EntityAuditLogsRepository.Remove(routeLog);
                //await _uow.SaveChangesAsync();

                return await Unit.Task;
            }
        }
    }
}

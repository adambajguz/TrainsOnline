namespace TrainsOnline.Application.Handlers.EntityAuditLogHandlers.Commands.CleanupEntityAuditLog
{
    using FluentValidation;
    using TrainsOnline.Application.Interfaces.UoW;

    public class CleanupEntityAuditLogCommandValidator : AbstractValidator<CleanupEntityAuditLogRequest>
    {
        public CleanupEntityAuditLogCommandValidator(ITrainsOnlineSQLUnitOfWork uow)
        {

        }
    }
}

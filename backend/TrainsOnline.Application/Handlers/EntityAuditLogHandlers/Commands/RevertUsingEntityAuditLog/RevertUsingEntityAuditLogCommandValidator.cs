namespace TrainsOnline.Application.Handlers.EntityAuditLog.Commands.CreateRouteLog
{
    using FluentValidation;
    using TrainsOnline.Application.Interfaces.UoW;

    public class RevertUsingEntityAuditLogCommandValidator : AbstractValidator<RevertUsingEntityAuditLogRequest>
    {
        public RevertUsingEntityAuditLogCommandValidator(ITrainsOnlineSQLUnitOfWork uow)
        {

        }
    }
}

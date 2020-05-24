namespace TrainsOnline.Application.Handlers.EntityAuditLog.Commands.UpdateRouteLog
{
    using Domain.Entities;
    using FluentValidation;
    using TrainsOnline.Application.Constants;
    using TrainsOnline.Application.Interfaces.UoW;

    public class UpdateEntityAuditLogCommandValidator : AbstractValidator<UpdateEntityAuditLogCommandValidator.Model>
    {
        public UpdateEntityAuditLogCommandValidator(ITrainsOnlineMongoUnitOfWork uow)
        {
            RuleFor(x => x.Data.Distance).GreaterThan(0d)
                                         .WithMessage(ValidationMessages.General.GreaterThenZero);
            RuleFor(x => x.Data.TicketPrice).GreaterThan(0d)
                                            .WithMessage(ValidationMessages.General.GreaterThenZero);
        }

        public class Model
        {
            public UpdateEntityAuditLogRequest Data { get; set; }
            public RouteLog RouteLog { get; set; }

            public Model(UpdateEntityAuditLogRequest data, RouteLog routeLog)
            {
                Data = data;
                RouteLog = routeLog;
            }
        }
    }
}

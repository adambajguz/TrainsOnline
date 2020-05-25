﻿namespace TrainsOnline.Application.Handlers.EntityAuditLog.Commands.CreateRouteLog
{
    using FluentValidation;
    using TrainsOnline.Application.Constants;
    using TrainsOnline.Application.Interfaces.UoW;

    public class RevertUsingEntityAuditLogCommandValidator : AbstractValidator<RevertUsingEntityAuditLogRequest>
    {
        public RevertUsingEntityAuditLogCommandValidator(ITrainsOnlineSQLUnitOfWork uow)
        {
            RuleFor(x => x.Distance).GreaterThan(0d)
                                    .WithMessage(ValidationMessages.General.GreaterThenZero);
            RuleFor(x => x.TicketPrice).GreaterThan(0d)
                                       .WithMessage(ValidationMessages.General.GreaterThenZero);
        }
    }
}
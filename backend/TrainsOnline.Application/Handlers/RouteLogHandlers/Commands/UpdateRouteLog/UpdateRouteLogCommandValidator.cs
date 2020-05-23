namespace TrainsOnline.Application.Handlers.RouteLogHandlers.Commands.UpdateRouteLog
{
    using Domain.Entities;
    using FluentValidation;
    using TrainsOnline.Application.Constants;
    using TrainsOnline.Application.Interfaces.UoW;
    using TrainsOnline.Application.Interfaces.UoW.Generic;

    public class UpdateRouteLogCommandValidator : AbstractValidator<UpdateRouteLogCommandValidator.Model>
    {
        public UpdateRouteLogCommandValidator(ITrainsOnlineMongoUnitOfWork uow)
        {
            RuleFor(x => x.Data.Distance).GreaterThan(0d)
                                         .WithMessage(ValidationMessages.General.GreaterThenZero);
            RuleFor(x => x.Data.TicketPrice).GreaterThan(0d)
                                            .WithMessage(ValidationMessages.General.GreaterThenZero);
        }

        public class Model
        {
            public UpdateRouteLogRequest Data { get; set; }
            public RouteLog RouteLog { get; set; }

            public Model(UpdateRouteLogRequest data, RouteLog routeLog)
            {
                Data = data;
                RouteLog = routeLog;
            }
        }
    }
}

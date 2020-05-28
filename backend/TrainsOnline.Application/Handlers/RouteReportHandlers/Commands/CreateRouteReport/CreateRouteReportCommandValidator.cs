namespace TrainsOnline.Application.Handlers.RouteReportHandlers.Commands.CreateRouteReport
{
    using FluentValidation;
    using TrainsOnline.Application.Constants;
    using TrainsOnline.Application.Interfaces.UoW;

    public class CreateRouteReportCommandValidator : AbstractValidator<CreateRouteReportRequest>
    {
        public CreateRouteReportCommandValidator(ITrainsOnlineMongoUnitOfWork uow)
        {
            RuleFor(x => x.Distance).GreaterThan(0d)
                                    .WithMessage(ValidationMessages.General.GreaterThenZero);
            RuleFor(x => x.TicketPrice).GreaterThan(0d)
                                       .WithMessage(ValidationMessages.General.GreaterThenZero);
        }
    }
}

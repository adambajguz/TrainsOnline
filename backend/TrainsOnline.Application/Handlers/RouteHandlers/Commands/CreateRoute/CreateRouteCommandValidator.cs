namespace TrainsOnline.Application.Handlers.RouteHandlers.Commands.CreateRoute
{
    using FluentValidation;
    using TrainsOnline.Application.Constants;
    using TrainsOnline.Application.Interfaces.UoW;

    public class CreateRouteCommandValidator : AbstractValidator<CreateRouteRequest>
    {
        public CreateRouteCommandValidator(ITrainsOnlineMongoUnitOfWork uow)
        {
            RuleFor(x => x.FromId).NotEmpty().MustAsync(async (request, val, token) =>
            {
                bool exists = await uow.StationsRepository.GetExistsAsync(x => x.Id == val);

                return exists;
            }).WithMessage(ValidationMessages.General.IsIncorrectId);

            RuleFor(x => x.ToId).NotEmpty().MustAsync(async (request, val, token) =>
            {
                bool exists = await uow.StationsRepository.GetExistsAsync(x => x.Id == val);

                return exists;
            }).WithMessage(ValidationMessages.General.IsIncorrectId);

            RuleFor(x => x.Distance).GreaterThan(0d)
                                    .WithMessage(ValidationMessages.General.GreaterThenZero);
            RuleFor(x => x.TicketPrice).GreaterThan(0d)
                                       .WithMessage(ValidationMessages.General.GreaterThenZero);
        }
    }
}

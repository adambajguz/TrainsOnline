namespace TrainsOnline.Application.Handlers.RouteHandlers.Commands.UpdateRoute
{
    using Domain.Entities;
    using FluentValidation;
    using TrainsOnline.Application.Constants;
    using TrainsOnline.Application.Interfaces.UoW;

    public class UpdateRouteCommandValidator : AbstractValidator<UpdateRouteCommandValidator.Model>
    {
        public UpdateRouteCommandValidator(ITrainsOnlineMongoUnitOfWork uow)
        {
            RuleFor(x => x.Data.Id).NotEmpty().Must((request, val, token) =>
            {
                return request.Route != null;
            }).WithMessage(ValidationMessages.General.IsIncorrectId);

            RuleFor(x => x.Data.FromId).NotEmpty().MustAsync(async (request, val, token) =>
            {
                bool exists = await uow.StationsRepository.GetExistsAsync(x => x.Id == val);

                return exists;
            }).WithMessage(ValidationMessages.General.IsIncorrectId);

            RuleFor(x => x.Data.ToId).NotEmpty().MustAsync(async (request, val, token) =>
            {
                bool exists = await uow.StationsRepository.GetExistsAsync(x => x.Id == val);

                return exists;
            }).WithMessage(ValidationMessages.General.IsIncorrectId);

            RuleFor(x => x.Data.Distance).GreaterThan(0d)
                                         .WithMessage(ValidationMessages.General.GreaterThenZero);
            RuleFor(x => x.Data.TicketPrice).GreaterThan(0d)
                                            .WithMessage(ValidationMessages.General.GreaterThenZero);
        }

        public class Model
        {
            public UpdateRouteRequest Data { get; set; }
            public Route Route { get; set; }

            public Model(UpdateRouteRequest data, Route route)
            {
                Data = data;
                Route = route;
            }
        }
    }
}

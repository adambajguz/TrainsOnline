namespace TrainsOnline.Application.Handlers.RouteLogHandlers.Commands.CreateRouteLog
{
    using FluentValidation;
    using TrainsOnline.Application.Constants;
    using TrainsOnline.Application.Interfaces.UoW;

    public class CreateRouteLogCommandValidator : AbstractValidator<CreateRouteLogRequest>
    {
        public CreateRouteLogCommandValidator(ITrainsOnlineMongoUnitOfWork uow)
        {

        }
    }
}

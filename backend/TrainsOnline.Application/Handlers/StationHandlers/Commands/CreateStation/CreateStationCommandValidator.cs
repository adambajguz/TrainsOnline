namespace TrainsOnline.Application.Handlers.StationHandlers.Commands.CreateStation
{
    using FluentValidation;
    using TrainsOnline.Application.Constants;
    using TrainsOnline.Application.Interfaces.UoW;

    public class CreateStationCommandValidator : AbstractValidator<CreateStationRequest>
    {
        public CreateStationCommandValidator(ITrainsOnlineSQLUnitOfWork uow)
        {
            RuleFor(x => x.Name).NotEmpty()
                                .WithMessage(ValidationMessages.General.IsNullOrEmpty);
        }
    }
}

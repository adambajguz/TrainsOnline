namespace TrainsOnline.Application.Handlers.StationHandlers.Commands.CreateStation
{
    using FluentValidation;
    using TrainsOnline.Application.Constants;
    using TrainsOnline.Application.Interfaces.UoW.Generic;

    public class CreateStationCommandValidator : AbstractValidator<CreateStationRequest>
    {
        public CreateStationCommandValidator(IPKPAppDbUnitOfWork uow)
        {
            RuleFor(x => x.Name).NotEmpty()
                                .WithMessage(ValidationMessages.General.IsNullOrEmpty);
        }
    }
}

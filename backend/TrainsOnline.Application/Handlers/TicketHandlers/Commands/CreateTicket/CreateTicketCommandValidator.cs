namespace TrainsOnline.Application.Handlers.TicketHandlers.Commands.CreateTicket
{
    using FluentValidation;
    using TrainsOnline.Application.Constants;
    using TrainsOnline.Application.Interfaces.UoW;

    public class CreateTicketCommandValidator : AbstractValidator<CreateTicketRequest>
    {
        public CreateTicketCommandValidator(ITrainsOnlineUnitOfWork uow)
        {
            RuleFor(x => x.UserId).MustAsync(async (request, val, token) =>
            {
                bool exists = await uow.UsersRepository.GetExistsAsync(x => x.Id == val);

                return exists;
            }).WithMessage(ValidationMessages.General.IsIncorrectId);

            RuleFor(x => x.RouteId).MustAsync(async (request, val, token) =>
            {
                bool exists = await uow.RoutesRepository.GetExistsAsync(x => x.Id == val);

                return exists;
            }).WithMessage(ValidationMessages.General.IsIncorrectId);
        }
    }
}

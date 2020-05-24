namespace TrainsOnline.Application.Handlers.TicketHandlers.Commands.UpdateTicket
{
    using Domain.Entities;
    using FluentValidation;
    using TrainsOnline.Application.Constants;
    using TrainsOnline.Application.Interfaces.UoW;

    public class UpdateTicketCommandValidator : AbstractValidator<UpdateTicketCommandValidator.Model>
    {
        public UpdateTicketCommandValidator(ITrainsOnlineSQLUnitOfWork uow)
        {
            RuleFor(x => x.Data.Id).NotEmpty().Must((request, val, token) =>
            {
                return request.Ticket != null;
            }).WithMessage(ValidationMessages.General.IsIncorrectId);

            RuleFor(x => x.Data.UserId).MustAsync(async (request, val, token) =>
            {
                bool exists = await uow.UsersRepository.GetExistsAsync(x => x.Id == val);

                return exists;
            }).WithMessage(ValidationMessages.General.IsIncorrectId);

            RuleFor(x => x.Data.RouteId).MustAsync(async (request, val, token) =>
            {
                bool exists = await uow.RoutesRepository.GetExistsAsync(x => x.Id == val);

                return exists;
            }).WithMessage(ValidationMessages.General.IsIncorrectId);
        }

        public class Model
        {
            public UpdateTicketRequest Data { get; set; }
            public Ticket Ticket { get; set; }

            public Model(UpdateTicketRequest data, Ticket ticket)
            {
                Data = data;
                Ticket = ticket;
            }
        }
    }
}

namespace TrainsOnline.Application.Handlers.TicketHandlers.Commands.DeleteTicket
{
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Entities;
    using FluentValidation;
    using MediatR;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Interfaces;
    using TrainsOnline.Application.Interfaces.UoW;

    public class DeleteTicketCommand : IRequest
    {
        public IdRequest Data { get; }

        public DeleteTicketCommand(IdRequest data)
        {
            Data = data;
        }

        public class Handler : IRequestHandler<DeleteTicketCommand, Unit>
        {
            private readonly ITrainsOnlineSQLUnitOfWork _uow;
            private readonly IDataRightsService _drs;

            public Handler(ITrainsOnlineSQLUnitOfWork uow, IDataRightsService drs)
            {
                _uow = uow;
                _drs = drs;
            }

            public async Task<Unit> Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
            {
                IdRequest data = request.Data;

                Ticket ticket = await _uow.TicketsRepository.GetByIdAsync(data.Id);

                EntityRequestByIdValidator<Ticket>.Model validationModel = new EntityRequestByIdValidator<Ticket>.Model(data, ticket);
                await new EntityRequestByIdValidator<Ticket>().ValidateAndThrowAsync(validationModel, cancellationToken: cancellationToken);
                await _drs.ValidateUserId(ticket, x => x.UserId);

                _uow.TicketsRepository.Remove(ticket);
                await _uow.SaveChangesAsync();

                return await Unit.Task;
            }
        }
    }
}

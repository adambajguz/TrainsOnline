namespace TrainsOnline.Application.Handlers.TicketHandlers.Commands.UpdateTicket
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Entities;
    using FluentValidation;
    using MediatR;
    using TrainsOnline.Application.Interfaces;
    using TrainsOnline.Application.Interfaces.UoW;

    public class UpdateTicketCommand : IRequest
    {
        public UpdateTicketRequest Data { get; }

        public UpdateTicketCommand(UpdateTicketRequest data)
        {
            Data = data;
        }

        public class Handler : IRequestHandler<UpdateTicketCommand>
        {
            private readonly ITrainsOnlineSQLUnitOfWork _uow;
            private readonly IMapper _mapper;
            private readonly IDataRightsService _drs;

            public Handler(ITrainsOnlineSQLUnitOfWork uow, IMapper mapper, IDataRightsService drs)
            {
                _uow = uow;
                _mapper = mapper;
                _drs = drs;
            }

            public async Task<Unit> Handle(UpdateTicketCommand request, CancellationToken cancellationToken)
            {
                UpdateTicketRequest data = request.Data;

                Ticket? ticket = await _uow.Tickets.SingleByIdOrDefaultAsync(data.Id);

                UpdateTicketCommandValidator.Model validationModel = new UpdateTicketCommandValidator.Model(data, ticket);
                await new UpdateTicketCommandValidator(_uow).ValidateAndThrowAsync(validationModel, cancellationToken: cancellationToken);
                await _drs.ValidateUserId(ticket, x => x.UserId);

                _mapper.Map(data, ticket);
                _uow.Tickets.Update(ticket);

                await _uow.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}

namespace TrainsOnline.Application.Handlers.TicketHandlers.Commands.CreateTicket
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Entities;
    using FluentValidation;
    using MediatR;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Interfaces;
    using TrainsOnline.Application.Interfaces.UoW;

    public class CreateTicketCommand : IRequest<IdResponse>
    {
        public CreateTicketRequest Data { get; }

        public CreateTicketCommand(CreateTicketRequest data)
        {
            Data = data;
        }

        public class Handler : IRequestHandler<CreateTicketCommand, IdResponse>
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

            public async Task<IdResponse> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
            {
                CreateTicketRequest data = request.Data;
                await _drs.ValidateUserId(data, x => x.UserId);

                await new CreateTicketCommandValidator(_uow).ValidateAndThrowAsync(data, cancellationToken: cancellationToken);

                Ticket entity = _mapper.Map<Ticket>(data);
                _uow.TicketsRepository.Add(entity);

                await _uow.SaveChangesAsync(cancellationToken);

                return new IdResponse(entity.Id);
            }
        }
    }
}

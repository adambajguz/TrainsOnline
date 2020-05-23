namespace TrainsOnline.Application.Handlers.TicketHandlers.Queries.ValidateDocument
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using TrainsOnline.Application.Interfaces.UoW;
    using TrainsOnline.Domain.Entities;

    public class ValidateDocumentQuery : IRequest<bool>
    {
        public Guid TId { get; }
        public Guid UId { get; }
        public Guid RId { get; }

        public ValidateDocumentQuery(Guid tid, Guid uid, Guid rid)
        {
            TId = tid;
            UId = uid;
            RId = rid;
        }

        public class Handler : IRequestHandler<ValidateDocumentQuery, bool>
        {
            private readonly ITrainsOnlineMongoUnitOfWork _uow;

            public Handler(ITrainsOnlineMongoUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<bool> Handle(ValidateDocumentQuery request, CancellationToken cancellationToken)
            {
                Guid ticketId = request.TId;
                Ticket ticket = await _uow.TicketsRepository.GetByIdWithRelatedAsync(ticketId, x => x.Route, x => x.Route.From, x => x.Route.To);

                bool isTicketValid = ticket != null &&
                                     ticket.Id == ticketId &&
                                     ticket.UserId == request.UId &&
                                     ticket.RouteId == request.RId;

                return isTicketValid;
            }
        }
    }
}

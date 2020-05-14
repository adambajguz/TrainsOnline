namespace TrainsOnline.Api.SoapEndpoints
{
    using System;
    using System.Threading.Tasks;
    using MediatR;
    using TrainsOnline.Api.SoapEndpoints.Core;
    using TrainsOnline.Api.SoapEndpoints.Interfaces;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Handlers.TicketHandlers.Commands.CreateTicket;
    using TrainsOnline.Application.Handlers.TicketHandlers.Commands.DeleteTicket;
    using TrainsOnline.Application.Handlers.TicketHandlers.Commands.UpdateTicket;
    using TrainsOnline.Application.Handlers.TicketHandlers.Queries.GetTicketDetails;
    using TrainsOnline.Application.Handlers.TicketHandlers.Queries.GetTicketDocument;
    using TrainsOnline.Application.Handlers.TicketHandlers.Queries.GetTicketsList;
    using TrainsOnline.Application.Handlers.TicketHandlers.Queries.GetUserTicketsList;
    using TrainsOnline.Application.Interfaces;

    [SoapRoute("[baseUrl]/ticket", "Ticket", "Create, update, and get ticket")]
    public class TicketSoapEndpointService : ITicketSoapEndpointService
    {
        protected IMediator Mediator { get; }
        protected ICurrentUserService CurrentUser { get; }

        public TicketSoapEndpointService(IMediator mediator, ICurrentUserService currentUser)
        {
            Mediator = mediator;
            CurrentUser = currentUser;
        }

        public async Task<IdResponse> CreateTicket(CreateTicketRequest ticket)
        {
            return await Mediator.Send(new CreateTicketCommand(ticket));
        }

        public async Task<GetTicketDetailsResponse> GetTicketDetails(IdRequest id)
        {
            return await Mediator.Send(new GetTicketDetailsQuery(id));
        }

        public async Task<GetTicketDocumentResponse> GetTicketDocument(IdRequest id)
        {
            return await Mediator.Send(new GetTicketDocumentQuery(id));
        }

        public async Task<Unit> UpdateTicket(UpdateTicketRequest ticket)
        {
            return await Mediator.Send(new UpdateTicketCommand(ticket));
        }

        public async Task<Unit> DeleteTicket(IdRequest id)
        {
            return await Mediator.Send(new DeleteTicketCommand(id));
        }

        public async Task<GetUserTicketsListResponse> GetCurrentUserTicketsList()
        {
            IdRequest data = new IdRequest((Guid)CurrentUser.UserId!);

            return await Mediator.Send(new GetUserTicketsListQuery(data));
        }

        public async Task<GetUserTicketsListResponse> GetUserTicketsList(IdRequest id)
        {
            return await Mediator.Send(new GetUserTicketsListQuery(id));
        }

        public async Task<GetTicketsListResponse> GetTicketsList()
        {
            return await Mediator.Send(new GetTicketsListQuery());
        }
    }
}

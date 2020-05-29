namespace TrainsOnline.Application.Handlers.RouteLogHandlers.Commands.DeleteRouteLogByRouteId
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Interfaces.UoW;

    public class DeleteRouteLogByRouteIdCommand : IRequest
    {
        public IdRequest Data { get; }

        public DeleteRouteLogByRouteIdCommand(IdRequest data)
        {
            Data = data;
        }

        public class Handler : IRequestHandler<DeleteRouteLogByRouteIdCommand, Unit>
        {
            private readonly ITrainsOnlineMongoUnitOfWork _uow;

            public Handler(ITrainsOnlineMongoUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<Unit> Handle(DeleteRouteLogByRouteIdCommand request, CancellationToken cancellationToken)
            {
                IdRequest data = request.Data;

                await _uow.RouteLogs.RemoveManyAsync(x => x.RouteId == data.Id, cancellationToken);

                return await Unit.Task;
            }
        }
    }
}

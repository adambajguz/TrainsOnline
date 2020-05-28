namespace TrainsOnline.Application.Handlers.RouteLogHandlers.Commands.DeleteRouteLogByRouteId
{
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Entities;
    using FluentValidation;
    using MediatR;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Handlers;
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

                RouteLog routeLog = await _uow.RouteLogs.GetByIdAsync(data.Id);

                EntityRequestByIdValidator<RouteLog>.Model validationModel = new EntityRequestByIdValidator<RouteLog>.Model(data, routeLog);
                await new EntityRequestByIdValidator<RouteLog>().ValidateAndThrowAsync(validationModel, cancellationToken: cancellationToken);

                await _uow.RouteLogs.RemoveAsync(routeLog);
                await _uow.SaveChangesAsync();

                return await Unit.Task;
            }
        }
    }
}

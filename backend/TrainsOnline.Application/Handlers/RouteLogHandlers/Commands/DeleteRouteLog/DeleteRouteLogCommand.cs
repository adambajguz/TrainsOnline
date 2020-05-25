namespace TrainsOnline.Application.Handlers.RouteLogHandlers.Commands.DeleteRouteLog
{
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Entities;
    using FluentValidation;
    using MediatR;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Handlers;
    using TrainsOnline.Application.Interfaces.UoW;

    public class DeleteRouteLogCommand : IRequest
    {
        public IdRequest Data { get; }

        public DeleteRouteLogCommand(IdRequest data)
        {
            Data = data;
        }

        public class Handler : IRequestHandler<DeleteRouteLogCommand, Unit>
        {
            private readonly ITrainsOnlineMongoUnitOfWork _uow;

            public Handler(ITrainsOnlineMongoUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<Unit> Handle(DeleteRouteLogCommand request, CancellationToken cancellationToken)
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

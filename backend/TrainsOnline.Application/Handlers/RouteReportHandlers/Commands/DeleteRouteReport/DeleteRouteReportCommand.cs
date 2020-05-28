namespace TrainsOnline.Application.Handlers.RouteReportHandlers.Commands.DeleteRouteReport
{
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Entities;
    using FluentValidation;
    using MediatR;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Handlers;
    using TrainsOnline.Application.Interfaces.UoW;

    public class DeleteRouteReportCommand : IRequest
    {
        public IdRequest Data { get; }

        public DeleteRouteReportCommand(IdRequest data)
        {
            Data = data;
        }

        public class Handler : IRequestHandler<DeleteRouteReportCommand, Unit>
        {
            private readonly ITrainsOnlineMongoUnitOfWork _uow;

            public Handler(ITrainsOnlineMongoUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<Unit> Handle(DeleteRouteReportCommand request, CancellationToken cancellationToken)
            {
                IdRequest data = request.Data;

                RouteReport routeReports = await _uow.RouteLogs.GetByIdAsync(data.Id);

                EntityRequestByIdValidator<RouteReport>.Model validationModel = new EntityRequestByIdValidator<RouteReport>.Model(data, routeReports);
                await new EntityRequestByIdValidator<RouteReport>().ValidateAndThrowAsync(validationModel, cancellationToken: cancellationToken);

                await _uow.RouteLogs.RemoveAsync(routeReports);
                await _uow.SaveChangesAsync();

                return await Unit.Task;
            }
        }
    }
}

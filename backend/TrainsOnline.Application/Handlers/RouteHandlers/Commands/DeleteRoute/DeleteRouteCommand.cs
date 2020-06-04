namespace TrainsOnline.Application.Handlers.RouteHandlers.Commands.DeleteRoute
{
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Entities;
    using FluentValidation;
    using MediatR;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Interfaces.UoW;

    public class DeleteRouteCommand : IRequest
    {
        public IdRequest Data { get; }

        public DeleteRouteCommand(IdRequest data)
        {
            Data = data;
        }

        public class Handler : IRequestHandler<DeleteRouteCommand>
        {
            private readonly ITrainsOnlineSQLUnitOfWork _uow;

            public Handler(ITrainsOnlineSQLUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<Unit> Handle(DeleteRouteCommand request, CancellationToken cancellationToken)
            {
                IdRequest data = request.Data;

                Route? route = await _uow.Routes.GetByIdAsync(data.Id);

                EntityRequestByIdValidator<Route>.Model validationModel = new EntityRequestByIdValidator<Route>.Model(data, route);
                await new EntityRequestByIdValidator<Route>().ValidateAndThrowAsync(validationModel, cancellationToken: cancellationToken);

                _uow.Routes.Remove(route);
                await _uow.SaveChangesAsync();

                return await Unit.Task;
            }
        }
    }
}

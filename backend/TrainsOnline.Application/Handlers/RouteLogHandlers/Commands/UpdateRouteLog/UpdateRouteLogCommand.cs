namespace TrainsOnline.Application.Handlers.RouteLogHandlers.Commands.UpdateRouteLog
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Entities;
    using FluentValidation;
    using MediatR;
    using TrainsOnline.Application.Interfaces.UoW;

    public class UpdateRouteLogCommand : IRequest
    {
        public UpdateRouteLogRequest Data { get; }

        public UpdateRouteLogCommand(UpdateRouteLogRequest data)
        {
            Data = data;
        }

        public class Handler : IRequestHandler<UpdateRouteLogCommand, Unit>
        {
            private readonly ITrainsOnlineMongoUnitOfWork _uow;
            private readonly IMapper _mapper;

            public Handler(ITrainsOnlineMongoUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(UpdateRouteLogCommand request, CancellationToken cancellationToken)
            {
                UpdateRouteLogRequest data = request.Data;

                RouteLog routeLog = await _uow.RouteLogsRepository.GetByIdAsync(data.Id);

                UpdateRouteLogCommandValidator.Model validationModel = new UpdateRouteLogCommandValidator.Model(data, routeLog);
                await new UpdateRouteLogCommandValidator(_uow).ValidateAndThrowAsync(validationModel, cancellationToken: cancellationToken);

                _mapper.Map(data, routeLog);
                await _uow.RouteLogsRepository.UpdateAsync(routeLog);

                await _uow.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}

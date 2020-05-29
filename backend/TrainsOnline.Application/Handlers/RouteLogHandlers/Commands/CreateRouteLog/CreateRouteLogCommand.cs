namespace TrainsOnline.Application.Handlers.RouteLogHandlers.Commands.CreateRouteLog
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Entities;
    using FluentValidation;
    using MediatR;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Interfaces.UoW;

    public class CreateRouteLogCommand : IRequest<IdResponse>
    {
        public CreateRouteLogRequest Data { get; }

        public CreateRouteLogCommand(CreateRouteLogRequest data)
        {
            Data = data;
        }

        public class Handler : IRequestHandler<CreateRouteLogCommand, IdResponse>
        {
            private readonly ITrainsOnlineMongoUnitOfWork _uow;
            private readonly IMapper _mapper;

            public Handler(ITrainsOnlineMongoUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<IdResponse> Handle(CreateRouteLogCommand request, CancellationToken cancellationToken)
            {
                CreateRouteLogRequest data = request.Data;

                await new CreateRouteLogCommandValidator(_uow).ValidateAndThrowAsync(data, cancellationToken: cancellationToken);

                RouteLog entity = _mapper.Map<RouteLog>(data);
                await _uow.RouteLogs.AddAsync(entity);

                return new IdResponse(entity.Id);
            }
        }
    }
}

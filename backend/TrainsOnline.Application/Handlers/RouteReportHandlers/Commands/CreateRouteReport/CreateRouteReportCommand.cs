namespace TrainsOnline.Application.Handlers.RouteReportHandlers.Commands.CreateRouteReport
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Entities;
    using FluentValidation;
    using MediatR;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Interfaces.UoW;

    public class CreateRouteReportCommand : IRequest<IdResponse>
    {
        public CreateRouteReportRequest Data { get; }

        public CreateRouteReportCommand(CreateRouteReportRequest data)
        {
            Data = data;
        }

        public class Handler : IRequestHandler<CreateRouteReportCommand, IdResponse>
        {
            private readonly ITrainsOnlineMongoUnitOfWork _uow;
            private readonly IMapper _mapper;

            public Handler(ITrainsOnlineMongoUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<IdResponse> Handle(CreateRouteReportCommand request, CancellationToken cancellationToken)
            {
                CreateRouteReportRequest data = request.Data;

                await new CreateRouteReportCommandValidator(_uow).ValidateAndThrowAsync(data, cancellationToken: cancellationToken);

                RouteLog entity = _mapper.Map<RouteLog>(data);
                await _uow.RouteLogs.AddAsync(entity);

                await _uow.SaveChangesAsync(cancellationToken);

                return new IdResponse(entity.Id);
            }
        }
    }
}

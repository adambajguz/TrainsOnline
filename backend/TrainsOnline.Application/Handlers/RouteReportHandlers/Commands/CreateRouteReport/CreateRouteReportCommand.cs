namespace TrainsOnline.Application.Handlers.RouteReportHandlers.Commands.CreateRouteReport
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Entities;
    using MediatR;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Interfaces.UoW;

    public class CreateRouteReportCommand : IRequest<IdResponse>
    {
        public IdRequest Data { get; }

        public CreateRouteReportCommand(IdRequest data)
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
                IdRequest data = request.Data;


                RouteReport entity = _mapper.Map<RouteReport>(data);
                await _uow.RouteReports.AddAsync(entity);

                return new IdResponse(entity.Id);
            }
        }
    }
}

namespace TrainsOnline.Application.Handlers.RouteReportHandlers.Queries.GetRouteReportDetails
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using FluentValidation;
    using MediatR;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Handlers;
    using TrainsOnline.Application.Interfaces.UoW;
    using TrainsOnline.Domain.Entities;

    public class GetRouteReportDetailsQuery : IRequest<GetRouteReportDetailsResponse>
    {
        public IdRequest Data { get; }

        public GetRouteReportDetailsQuery(IdRequest data)
        {
            Data = data;
        }

        public class Handler : IRequestHandler<GetRouteReportDetailsQuery, GetRouteReportDetailsResponse>
        {
            private readonly ITrainsOnlineMongoUnitOfWork _uow;
            private readonly IMapper _mapper;

            public Handler(ITrainsOnlineMongoUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<GetRouteReportDetailsResponse> Handle(GetRouteReportDetailsQuery request, CancellationToken cancellationToken)
            {
                IdRequest data = request.Data;

                RouteLog entity = await _uow.RouteLogs.GetByIdAsync(data.Id);

                EntityRequestByIdValidator<RouteLog>.Model validationModel = new EntityRequestByIdValidator<RouteLog>.Model(data, entity);
                await new EntityRequestByIdValidator<RouteLog>().ValidateAndThrowAsync(validationModel, cancellationToken: cancellationToken);

                return _mapper.Map<GetRouteReportDetailsResponse>(entity);
            }
        }
    }
}

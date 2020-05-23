namespace TrainsOnline.Application.Handlers.RouteLogHandlers.Queries.GetRouteLogDetails
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using FluentValidation;
    using MediatR;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Handlers;
    using TrainsOnline.Application.Interfaces.UoW;
    using TrainsOnline.Application.Interfaces.UoW.Generic;
    using TrainsOnline.Domain.Entities;

    public class GetRouteLogDetailsQuery : IRequest<GetRouteLogDetailsResponse>
    {
        public IdRequest Data { get; }

        public GetRouteLogDetailsQuery(IdRequest data)
        {
            Data = data;
        }

        public class Handler : IRequestHandler<GetRouteLogDetailsQuery, GetRouteLogDetailsResponse>
        {
            private readonly ITrainsOnlineMongoUnitOfWork _uow;
            private readonly IMapper _mapper;

            public Handler(ITrainsOnlineMongoUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<GetRouteLogDetailsResponse> Handle(GetRouteLogDetailsQuery request, CancellationToken cancellationToken)
            {
                IdRequest data = request.Data;

                RouteLog entity = await _uow.RouteLogsRepository.GetByIdAsync(data.Id);

                EntityRequestByIdValidator<RouteLog>.Model validationModel = new EntityRequestByIdValidator<RouteLog>.Model(data, entity);
                await new EntityRequestByIdValidator<RouteLog>().ValidateAndThrowAsync(validationModel, cancellationToken: cancellationToken);

                return _mapper.Map<GetRouteLogDetailsResponse>(entity);
            }
        }
    }
}

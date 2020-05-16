namespace TrainsOnline.Application.Handlers.StationHandlers.Queries.GetStationDetails
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using FluentValidation;
    using MediatR;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Interfaces.UoW;
    using TrainsOnline.Domain.Entities;

    public class GetStationDetailsQuery : IRequest<GetStationDetailsResponse>
    {
        public IdRequest Data { get; }

        public GetStationDetailsQuery(IdRequest data)
        {
            Data = data;
        }

        public class Handler : IRequestHandler<GetStationDetailsQuery, GetStationDetailsResponse>
        {
            private readonly ITrainsOnlineUnitOfWork _uow;
            private readonly IMapper _mapper;

            public Handler(ITrainsOnlineUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<GetStationDetailsResponse> Handle(GetStationDetailsQuery request, CancellationToken cancellationToken)
            {
                IdRequest data = request.Data;

                Station entity = await _uow.StationsRepository.GetStationFullDetails(data.Id);

                EntityRequestByIdValidator<Station>.Model validationModel = new EntityRequestByIdValidator<Station>.Model(data, entity);
                await new EntityRequestByIdValidator<Station>().ValidateAndThrowAsync(validationModel, cancellationToken: cancellationToken);

                return _mapper.Map<GetStationDetailsResponse>(entity);
            }
        }
    }
}

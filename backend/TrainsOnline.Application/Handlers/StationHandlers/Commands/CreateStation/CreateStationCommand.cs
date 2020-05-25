namespace TrainsOnline.Application.Handlers.StationHandlers.Commands.CreateStation
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Entities;
    using FluentValidation;
    using MediatR;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Interfaces.UoW;

    public class CreateStationCommand : IRequest<IdResponse>
    {
        public CreateStationRequest Data { get; }

        public CreateStationCommand(CreateStationRequest data)
        {
            Data = data;
        }

        public class Handler : IRequestHandler<CreateStationCommand, IdResponse>
        {
            private readonly ITrainsOnlineSQLUnitOfWork _uow;
            private readonly IMapper _mapper;

            public Handler(ITrainsOnlineSQLUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<IdResponse> Handle(CreateStationCommand request, CancellationToken cancellationToken)
            {
                CreateStationRequest data = request.Data;

                await new CreateStationCommandValidator(_uow).ValidateAndThrowAsync(data, cancellationToken: cancellationToken);

                Station entity = _mapper.Map<Station>(data);
                _uow.Stations.Add(entity);

                await _uow.SaveChangesAsync(cancellationToken);

                return new IdResponse(entity.Id);
            }
        }
    }
}

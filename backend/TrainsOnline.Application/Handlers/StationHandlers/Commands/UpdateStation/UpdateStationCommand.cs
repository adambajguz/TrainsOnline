namespace TrainsOnline.Application.Handlers.StationHandlers.Commands.UpdateStation
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Entities;
    using FluentValidation;
    using MediatR;
    using TrainsOnline.Application.Interfaces.UoW;

    public class UpdateStationCommand : IRequest
    {
        public UpdateStationRequest Data { get; }

        public UpdateStationCommand(UpdateStationRequest data)
        {
            Data = data;
        }

        public class Handler : IRequestHandler<UpdateStationCommand, Unit>
        {
            private readonly ITrainsOnlineMongoUnitOfWork _uow;
            private readonly IMapper _mapper;

            public Handler(ITrainsOnlineMongoUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(UpdateStationCommand request, CancellationToken cancellationToken)
            {
                UpdateStationRequest data = request.Data;

                Station station = await _uow.StationsRepository.GetByIdAsync(data.Id);

                UpdateStationCommandValidator.Model validationModel = new UpdateStationCommandValidator.Model(data, station);
                await new UpdateStationCommandValidator(_uow).ValidateAndThrowAsync(validationModel, cancellationToken: cancellationToken);

                _mapper.Map(data, station);
                await _uow.StationsRepository.UpdateAsync(station);

                await _uow.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}

namespace TrainsOnline.Application.Handlers.AnalyticsRecordHandlers.Commands.CreateOrUpdateAnalyticsRecord
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Entities;
    using MediatR;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Handlers.AnalyticsRecordHandlers.Commands.CreateRouteReport;
    using TrainsOnline.Application.Interfaces.UoW;

    public class CreateOrUpdateAnalyticsRecordCommand : IRequest<IdResponse>
    {
        public AnalyticsRecordRequest Data { get; }

        public CreateOrUpdateAnalyticsRecordCommand(AnalyticsRecordRequest data)
        {
            Data = data;
        }

        public class Handler : IRequestHandler<CreateOrUpdateAnalyticsRecordCommand, IdResponse>
        {
            private readonly ITrainsOnlineMongoUnitOfWork _uow;
            private readonly IMapper _mapper;

            public Handler(ITrainsOnlineMongoUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<IdResponse> Handle(CreateOrUpdateAnalyticsRecordCommand request, CancellationToken cancellationToken)
            {
                AnalyticsRecordRequest data = request.Data;

                DateTime now = DateTime.UtcNow.Date;

                AnalyticsRecord? prevEntity = await _uow.AnalyticsRecords.GetOneAsync(x => x.Timestamp == now && x.Uri == data.Uri);

                if (prevEntity is null)
                {
                    AnalyticsRecord entity = _mapper.Map<AnalyticsRecord>(data);
                    entity.Visits = 0;

                    await _uow.AnalyticsRecords.AddAsync(entity);

                    return new IdResponse(entity.Id);
                }

                ++prevEntity.Visits;
                await _uow.AnalyticsRecords.UpdateAsync(prevEntity);

                return new IdResponse(prevEntity.Id);
            }
        }
    }
}

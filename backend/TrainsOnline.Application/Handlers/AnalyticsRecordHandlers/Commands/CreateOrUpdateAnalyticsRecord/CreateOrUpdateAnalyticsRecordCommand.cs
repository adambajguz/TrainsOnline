namespace TrainsOnline.Application.Handlers.AnalyticsRecordHandlers.Commands.CreateOrUpdateAnalyticsRecord
{
    using System;
    using System.Data.HashFunction;
    using System.Data.HashFunction.MurmurHash;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Entities;
    using MediatR;
    using TrainsOnline.Application.DTO;
    using TrainsOnline.Application.Interfaces.UoW;

    public class CreateOrUpdateAnalyticsRecordCommand : IRequest<IdResponse>
    {
        public CreateOrUpdateAnalyticsRecordRequest Data { get; }

        public CreateOrUpdateAnalyticsRecordCommand(CreateOrUpdateAnalyticsRecordRequest data)
        {
            Data = data;
        }

        public class Handler : IRequestHandler<CreateOrUpdateAnalyticsRecordCommand, IdResponse>
        {
            private static readonly IMurmurHash2 _hasher = MurmurHash2Factory.Instance.Create(new MurmurHash2Config
            {
                HashSizeInBits = 64,
                Seed = 46789130U,
            });

            private readonly ITrainsOnlineMongoUnitOfWork _uow;
            private readonly IMapper _mapper;

            public Handler(ITrainsOnlineMongoUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<IdResponse> Handle(CreateOrUpdateAnalyticsRecordCommand request, CancellationToken cancellationToken)
            {
                CreateOrUpdateAnalyticsRecordRequest data = request.Data;

                DateTime now = DateTime.UtcNow.Date;

                IHashValue? hashValue = _hasher.ComputeHash(now.ToString() + data.Uri ?? string.Empty + data.UserAgent ?? string.Empty);
                long hash = BitConverter.ToInt64(hashValue.Hash);

                AnalyticsRecord? prevEntity = await _uow.AnalyticsRecords.GetOneAsync(x => x.Hash == hash && x.Timestamp == now && x.Uri == data.Uri);

                if (prevEntity is null)
                {
                    AnalyticsRecord entity = _mapper.Map<AnalyticsRecord>(data);
                    entity.Hash = hash;
                    entity.Timestamp = now;
                    entity.Visits = 1;

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

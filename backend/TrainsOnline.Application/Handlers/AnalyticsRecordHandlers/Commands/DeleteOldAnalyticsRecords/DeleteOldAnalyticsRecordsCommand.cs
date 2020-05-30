namespace TrainsOnline.Application.Handlers.AnalyticsRecordHandlers.Commands.DeleteOldAnalyticsRecords
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using TrainsOnline.Application.Interfaces.UoW;

    public class DeleteOldAnalyticsRecordsCommand : IRequest
    {
        public DeleteOldAnalyticsRecordsRequest Data { get; }

        public DeleteOldAnalyticsRecordsCommand(DeleteOldAnalyticsRecordsRequest data)
        {
            Data = data;
        }

        public class Handler : IRequestHandler<DeleteOldAnalyticsRecordsCommand>
        {
            private readonly ITrainsOnlineMongoUnitOfWork _uow;

            public Handler(ITrainsOnlineMongoUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<Unit> Handle(DeleteOldAnalyticsRecordsCommand request, CancellationToken cancellationToken)
            {
                DeleteOldAnalyticsRecordsRequest data = request.Data;

                DateTime date = data.OlderThanOrEqualToDate?.Date ?? DateTime.UtcNow.Date;
                await _uow.AnalyticsRecords.RemoveManyAsync(x => x.Timestamp <= date);

                return await Unit.Task;
            }
        }
    }
}

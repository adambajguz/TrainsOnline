namespace TrainsOnline.Desktop.Application.Interfaces.RemoteDataProvider
{
    using System;
    using System.Threading.Tasks;
    using TrainsOnline.Desktop.Domain.DTO.Analytics;
    using TrainsOnline.Desktop.Domain.DTO.EntityAuditLog;

    public interface IRemoteDataProviderService : IUserData, IStationData, IRouteData, ITicketData
    {
        bool UseLocalUrl { get; set; }

        #region User
        Guid GetUserId();
        bool HasRole(string role);
        bool HasAnyOfRoles(params string[] roles);

        Task<Guid> CreateTicketForCurrentUser(Guid routeId);
        #endregion

        Task<GetAnalyticsRecordsListResponse> GetAnalytics();
        Task<GetEntityAuditLogsListResponse> GetEntityAudtiLogs();
    }
}

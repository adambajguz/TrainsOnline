namespace TrainsOnline.Tracker.Application.DTO.RouteLog
{
    using System.Collections.Generic;
    using TrainsOnline.Tracker.Application.DTO;

    public class GetRouteLogsListResponse : IDataTransferObject
    {
        public IList<RouteLogLookupModel> RouteLogs { get; set; }
    }
}

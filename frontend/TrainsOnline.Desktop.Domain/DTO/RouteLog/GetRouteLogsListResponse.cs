namespace TrainsOnline.Desktop.Domain.DTO.RouteLog
{
    using System.Collections.Generic;

    public class GetRouteLogsListResponse : IDataTransferObject
    {
        public IList<RouteLogLookupModel> RouteLogs { get; set; }
    }
}

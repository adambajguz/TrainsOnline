namespace TrainsOnline.Tracker.Application.DTO.RouteReport
{
    using System.Collections.Generic;
    using TrainsOnline.Tracker.Application.DTO;

    public class GetRouteReportsListResponse : IDataTransferObject
    {
        public IList<RouteReportLookupModel> RouteReports { get; set; }
    }
}

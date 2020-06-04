namespace TrainsOnline.Desktop.Domain.DTO.RouteReport
{
    using System.Collections.Generic;

    public class GetRouteReportsListResponse : IDataTransferObject
    {
        public IList<RouteReportLookupModel> RouteReports { get; set; }
    }
}

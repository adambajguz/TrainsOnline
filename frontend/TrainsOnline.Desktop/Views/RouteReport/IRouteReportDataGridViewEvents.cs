namespace TrainsOnline.Desktop.Views.RouteReport
{
    using Microsoft.Toolkit.Uwp.UI.Controls;

    internal interface IRouteReportDataGridViewEvents
    {
        void LoadingRowGroup(DataGridRowGroupHeaderEventArgs e);
        void ResetView();
    }
}

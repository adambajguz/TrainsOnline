namespace TrainsOnline.Desktop.Views.RouteLog
{
    using Microsoft.Toolkit.Uwp.UI.Controls;

    internal interface IRouteLogDataGridViewEvents
    {
        void LoadingRowGroup(DataGridRowGroupHeaderEventArgs e);
        void ResetView();
    }
}

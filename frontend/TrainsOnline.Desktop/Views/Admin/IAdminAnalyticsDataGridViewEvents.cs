namespace TrainsOnline.Desktop.Views.Route
{
    using Microsoft.Toolkit.Uwp.UI.Controls;

    internal interface IAdminAnalyticsDataGridViewEvents
    {
        void LoadingRowGroup(DataGridRowGroupHeaderEventArgs e);
        void ResetView();
    }
}

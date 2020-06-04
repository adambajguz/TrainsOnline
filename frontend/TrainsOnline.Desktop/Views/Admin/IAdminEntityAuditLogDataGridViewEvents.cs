namespace TrainsOnline.Desktop.Views.Route
{
    using Microsoft.Toolkit.Uwp.UI.Controls;

    internal interface IAdminEntityAuditLogDataGridViewEvents
    {
        void LoadingRowGroup(DataGridRowGroupHeaderEventArgs e);
        void ResetView();
    }
}

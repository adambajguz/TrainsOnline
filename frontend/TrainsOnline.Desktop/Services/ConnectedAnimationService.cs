namespace TrainsOnline.Desktop.Services
{
    using Microsoft.Toolkit.Uwp.UI.Animations;
    using TrainsOnline.Desktop.Interfaces;
    using Windows.UI.Xaml.Controls;

    public class ConnectedAnimationService : IConnectedAnimationService
    {
        private readonly Frame _frame;

        public ConnectedAnimationService(Frame frame)
        {
            _frame = frame;
        }

        public void SetListDataItemForNextConnectedAnimation(object item)
        {
            _frame.SetListDataItemForNextConnectedAnimation(item);
        }
    }
}

namespace TrainsOnline.Desktop.Views.Example
{
    using TrainsOnline.Desktop.ViewModels.Example;
    using Windows.UI.Xaml.Controls;

    public sealed partial class ExampleBlankPage : Page
    {
        public ExampleBlankPage()
        {
            InitializeComponent();
        }

        private ExampleBlankViewModel ViewModel => DataContext as ExampleBlankViewModel;
    }
}

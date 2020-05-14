namespace TrainsOnline.Desktop.ViewModels.Ticket
{
    using System.Threading.Tasks;
    using Caliburn.Micro;
    using TrainsOnline.Desktop.Application.Exceptions;
    using TrainsOnline.Desktop.Application.Interfaces.RemoteDataProvider;
    using TrainsOnline.Desktop.Common.Extensions;
    using TrainsOnline.Desktop.Domain.DTO.Ticket;
    using TrainsOnline.Desktop.Domain.Models.Ticket;
    using TrainsOnline.Desktop.Interfaces;
    using TrainsOnline.Desktop.Views.Ticket;
    using Windows.UI.Xaml.Media.Imaging;

    public class TicketContentGridDetailViewModel : Screen, ITicketContentGridDetailViewEvents
    {
        private readonly IConnectedAnimationService _connectedAnimationService;
        private IRemoteDataProviderService RemoteDataProvider { get; }
        private IFileService FileService { get; }

        private GetTicketDetailsResponse _item;
        public GetTicketDetailsResponse Item
        {
            get => _item;
            set => Set(ref _item, value);
        }

        private BitmapImage _pdfRenderingImage;
        public BitmapImage PdfRenderingImage
        {
            get => _pdfRenderingImage;
            set => Set(ref _pdfRenderingImage, value);
        }

        private bool _downloadInProgress;
        public bool DownloadInProgress
        {
            get => _downloadInProgress;
            set => Set(ref _downloadInProgress, value);
        }

        public TicketContentGridDetailsParameters Parameter { get; set; }

        public TicketContentGridDetailViewModel(IConnectedAnimationService connectedAnimationService,
                                                IRemoteDataProviderService remoteDataProvider,
                                                IFileService fileService)
        {
            _connectedAnimationService = connectedAnimationService;
            RemoteDataProvider = remoteDataProvider;
            FileService = fileService;
        }

        public async Task InitializeAsync()
        {
            GetTicketDetailsResponse data = await RemoteDataProvider.GetTicket(Parameter.TicketId);

            Item = data;
        }

        public void SetListDataItemForNextConnectedAnimation()
        {
            _connectedAnimationService.SetListDataItemForNextConnectedAnimation(Item);
        }

        public async void PreviewTicketPDF()
        {
            DownloadInProgress = true;
            try
            {
                GetTicketDocumentResponse documentData = await RemoteDataProvider.GetTicketDocument(Item.Id);

                PdfRenderingImage = await PdfRenderingHelper.RenderPdfToImage(documentData.Document);
                //Refresh();
                //if (GetView() is ITicketContentGridDetailView view)
                //{
                //    view.SetImage(PdfRenderingImage);
                //}
            }
            catch (RemoteDataException)
            {

            }
            DownloadInProgress = false;
        }

        public async void DownloadTicketPDF()
        {
            DownloadInProgress = true;
            try
            {
                GetTicketDocumentResponse documentData = await RemoteDataProvider.GetTicketDocument(Item.Id);
                byte[] data = await documentData.Document.DecodeBase64Async();

                await FileService.SaveToPdfFile(data, documentData.Id.ToShortGuid() + ".pdf");
            }
            catch (RemoteDataException)
            {

            }
            DownloadInProgress = false;
        }
    }
}

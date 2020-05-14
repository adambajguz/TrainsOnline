namespace TrainsOnline.Desktop.Common.Extensions
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices.WindowsRuntime;
    using System.Threading.Tasks;
    using Windows.Data.Pdf;
    using Windows.Graphics.Imaging;
    using Windows.Storage.Streams;
    using Windows.UI.Xaml.Media.Imaging;

    public static class PdfRenderingHelper
    {
        //public static async Task<PdfDocument> ByteArrayToPdf(byte[] data)
        //{
        //    using (Stream stream = data.AsBuffer().AsStream())
        //    using (IRandomAccessStream randomAccessStream = stream.AsRandomAccessStream())
        //    {
        //        PdfDocument pdf = await PdfDocument.LoadFromStreamAsync(randomAccessStream);
        //        return pdf;
        //    }
        //}

        //public static async Task<BitmapImage> RenderBase64PdfToImage(string base64Data, uint pageIndex = 0)
        //{
        //    byte[] data = await base64Data.DecodeBase64Async();

        //    return await RenderPdfToImage(data, pageIndex);
        //}

        //public static async Task<BitmapImage> RenderBase64BinaryPdfToImage(byte[] base64Data, uint pageIndex = 0)
        //{
        //    byte[] data = await base64Data.DecodeBase64BinaryAsync();

        //    return await RenderPdfToImage(data, pageIndex);
        //}

        //public static async Task<BitmapImage> RenderPdfToImage(byte[] data, uint pageIndex = 0)
        //{
        //    PdfDocument pdf = await ByteArrayToPdf(data);
        //    return await RenderPdfToImage(pdf, pageIndex);
        //}

        public static async Task<BitmapImage> RenderPdfToImage(string base64Data, uint pageIndex = 0)
        {
            byte[] data = await base64Data.DecodeBase64Async();

            using (Stream stream = data.AsBuffer().AsStream())
            using (IRandomAccessStream randomAccessStream = stream.AsRandomAccessStream())
            {
                PdfDocument pdf = await PdfDocument.LoadFromStreamAsync(randomAccessStream);

                if (pdf == null)
                {
                    throw new Exception("No document open.");
                }

                if (pageIndex >= pdf.PageCount)
                {
                    throw new ArgumentOutOfRangeException($"Document has only {pdf.PageCount} pages.");
                }

                // Get the page you want to render.
                PdfPage page = pdf.GetPage(pageIndex);

                // Create an image to render into.
                BitmapImage image = new BitmapImage();

                using (InMemoryRandomAccessStream streamPdf = new InMemoryRandomAccessStream())
                {
                    await page.RenderToStreamAsync(streamPdf, new PdfPageRenderOptions
                    {
                        BitmapEncoderId = BitmapEncoder.PngEncoderId,
                    });
                    streamPdf.Seek(0);

                    await image.SetSourceAsync(streamPdf);

                    return image;
                }
            }
        }

        public static async Task<BitmapImage> RenderPdfToImage(PdfDocument pdf, uint pageIndex = 0)
        {
            if (pdf == null)
            {
                throw new Exception("No document open.");
            }

            if (pageIndex >= pdf.PageCount)
            {
                throw new ArgumentOutOfRangeException($"Document has only {pdf.PageCount} pages.");
            }

            // Get the page you want to render.
            PdfPage page = pdf.GetPage(pageIndex);

            // Create an image to render into.
            BitmapImage image = new BitmapImage();

            using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
            {
                await page.RenderToStreamAsync(stream, new PdfPageRenderOptions
                {
                    BitmapEncoderId = BitmapEncoder.PngEncoderId,
                });
                stream.Seek(0);

                await image.SetSourceAsync(stream);

                return image;
            }
        }
    }
}

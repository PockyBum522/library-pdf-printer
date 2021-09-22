using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.Versioning;
using System.Windows.Media.Imaging;
using Jds2.Interfaces;
using Serilog;

namespace Jds2
{
    [SupportedOSPlatform("windows")]
    
    public class SimpleFreePdfPrinter : IPdfPrinter
    {
        private readonly ILogger _logger;

        public SimpleFreePdfPrinter(ILogger logger = null)
        {
            _logger = logger;
        }
        
        public void PrintPdfToDefaultPrinter(string pdfPathToPrint, int adjustmentMargin = -50)
        {
            PrinterSettings settings = new PrinterSettings();

            var defaultPrinterName = settings.PrinterName;
            
            PrintPdfTo(pdfPathToPrint, defaultPrinterName, adjustmentMargin);
        }

        public void PrintPdfTo(string pdfPathToPrint, string printerName, int adjustmentMargin = -50)
        {
            var pdfConverter = new PdfToBitmapListConverter();

            var pagesAsBitmapList = pdfConverter.GetPdfPagesAsBitmapList(pdfPathToPrint);

            PrintAllPages(printerName, adjustmentMargin, pagesAsBitmapList);
        }

        private void PrintAllPages(string printerName, int adjustmentMargin, List<Bitmap> pagesAsBitmapList)
        {
            foreach (var page in pagesAsBitmapList)
            {
                var memoryStream = StreamBitmap(page, out var bitmapImage);

                var pd = new PrintDocument();
                pd.DefaultPageSettings.PrinterSettings.PrinterName = printerName;

                SetLandscapePerPage(pd, bitmapImage);

                pd.PrintPage += (sender, args) =>
                {
                    var imageBitmap = new Bitmap(memoryStream);

                    var boundsRectangle = CalculateFullPageSizeWithAspectRatio(adjustmentMargin, args, imageBitmap);

                    args.Graphics.DrawImage(imageBitmap, boundsRectangle);
                };

                pd.Print();
            }
        }

        private static Rectangle CalculateFullPageSizeWithAspectRatio(int adjustmentMargin, PrintPageEventArgs args,
            Bitmap imageBitmap)
        {
            var boundsRectangle = args.PageBounds;

            // PageBounds was slightly hanging off the page on my printer. Included an adjustment.
            boundsRectangle.Width = boundsRectangle.Width + adjustmentMargin;

            // Set up image size, respecting aspect ratio
            if (imageBitmap.Width / (double)imageBitmap.Height > boundsRectangle.Width / (double)boundsRectangle.Height)
            {
                boundsRectangle.Height = (int)(imageBitmap.Height / (double)imageBitmap.Width * boundsRectangle.Width);
            }
            else
            {
                boundsRectangle.Width = (int)(imageBitmap.Width / (double)imageBitmap.Height * boundsRectangle.Height);
            }

            return boundsRectangle;
        }

        private static void SetLandscapePerPage(PrintDocument pd, BitmapImage bitmapImage)
        {
            pd.DefaultPageSettings.Landscape = false;

            if (bitmapImage.Width > bitmapImage.Height)
            {
                pd.DefaultPageSettings.Landscape = true;
            }
        }

        private static MemoryStream StreamBitmap(Bitmap page, out BitmapImage bitmapImage)
        {
            var memoryStream = new MemoryStream();
            page.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);
            memoryStream.Position = 0;

            bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memoryStream;
            bitmapImage.EndInit();
            return memoryStream;
        }
    }
}
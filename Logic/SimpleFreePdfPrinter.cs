using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.Versioning;
using JetBrains.Annotations;

namespace SimpleFreePdfPrinter
{
    [PublicAPI, SupportedOSPlatform("windows")]
    
    public class SimpleFreePdfPrinter
    {
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

            foreach (var page in pagesAsBitmapList)
            {
                var memoryStream = new MemoryStream();
                page.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);
                memoryStream.Position = 0;

                var bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();

                var pd = new PrintDocument();
                pd.DefaultPageSettings.PrinterSettings.PrinterName = printerName;
                pd.DefaultPageSettings.Landscape = false;

                if (bitmapImage.Width > bitmapImage.Height)
                {
                    pd.DefaultPageSettings.Landscape = true;
                }

                pd.PrintPage += (sender, args) =>
                {
                    var imageBitmap = new Bitmap(memoryStream);
                    
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
                    
                    args.Graphics.DrawImage(imageBitmap, boundsRectangle);
                };
                
                pd.Print();
            }
        }
    }
}
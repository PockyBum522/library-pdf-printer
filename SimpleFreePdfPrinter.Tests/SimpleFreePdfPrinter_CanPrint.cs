using Xunit;

namespace Jds2
{
    public class SimpleFreePdfPrinterTests
    {
        [Fact]
        public void SimpleFreePdfPrinter_CanPrint()
        {
            var pdfPrinter = new SimpleFreePdfPrinter();
            
            pdfPrinter.PrintPdfToDefaultPrinter(@"..\net5.0-windows\Resources\SamplePdfFile.pdf");
            
            // Document should print, two pages, with pages filled, from system default printer in Windows when this test is run.
        }
    }
}
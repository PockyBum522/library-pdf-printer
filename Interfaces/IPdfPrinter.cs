namespace Jds2.Interfaces
{
    public interface IPdfPrinter
    {
        public void PrintPdfToDefaultPrinter(string pdfPathToPrint, int adjustmentMargin = -50);
        public void PrintPdfTo(string pdfPathToPrint, string printerName, int adjustmentMargin = -50);
    }
}
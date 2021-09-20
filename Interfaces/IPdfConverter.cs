using System.Collections.Generic;
using System.Drawing;

namespace Jds2.Interfaces
{
    public interface IPdfConverter
    {
        public List<Bitmap> GetPdfPagesAsBitmapList(string pdfFileToWork);
        public List<string> GetPdfPagesAsPngs(string pdfFileToWork);
    }
}
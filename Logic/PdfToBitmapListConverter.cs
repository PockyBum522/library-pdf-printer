using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Jds2.Interfaces;

namespace Jds2
{
    public class PdfToBitmapListConverter : IPdfConverter
    {
        public List<Bitmap> GetPdfPagesAsBitmapList(string pdfFileToWork)
        {           
            var outputTempFile = Path.GetTempFileName().Replace(".tmp", ".png");

            var errorsList = Pdf2Image.Convert(pdfFileToWork, outputTempFile);

            var pagePaths = GetAllMatchingPagePngs(
                Path.GetFileNameWithoutExtension(outputTempFile));
         
            var outputList = new List<Bitmap>();
            
            ConvertFilePathList(pagePaths, outputList);

            return outputList;
        }
        
        public List<string> GetPdfPagesAsPngs(string pdfFileToWork)
        {           
            var outputTempFile = Path.GetTempFileName().Replace(".tmp", ".png");

            var errorsList = Pdf2Image.Convert(pdfFileToWork, outputTempFile);

            var pagePaths = GetAllMatchingPagePngs(
                Path.GetFileNameWithoutExtension(outputTempFile));
         
            return pagePaths;
        }

        private static void ConvertFilePathList(List<string> pagePaths, List<Bitmap> outputList)
        {
            foreach (var pagePath in pagePaths)
            {
                using MemoryStream pngStream = new MemoryStream(File.ReadAllBytes(pagePath));

                var outputImage = Image.FromStream(pngStream);

                var bitmapOfPage = new Bitmap(outputImage);

                outputList.Add(bitmapOfPage);
            }
        }

        private List<string> GetAllMatchingPagePngs(string fileNameOnlyToMatch)
        {
            var tempDirectoryInfo = new DirectoryInfo(Path.GetTempPath());

            var outputFileNames = tempDirectoryInfo.GetFiles($"*{fileNameOnlyToMatch}*.png");    // Get all files matching pattern of file without extension

            var outputList = new List<string>();

            foreach (var fileName in outputFileNames)
            {
                outputList.Add(fileName.FullName);
            }
            
            return outputList;
        }
    }
}
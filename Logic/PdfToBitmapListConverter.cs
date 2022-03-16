using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using Jds2.Interfaces;
using Serilog;

namespace Jds2
{
    public class PdfToBitmapListConverter : IPdfConverter
    {
        private readonly ILogger _logger;

        public PdfToBitmapListConverter(ILogger logger = null)
        {
            _logger = logger;
        }
        
        public List<Bitmap> GetPdfPagesAsBitmapList(string pdfFileToWork)
        {
            EnsureGsDllExists();

            var outputList = new List<Bitmap>();
                
            var outputTempFile = Path.GetTempFileName().Replace(".tmp", ".png");

            _ = Pdf2Image.Convert(pdfFileToWork, outputTempFile);

            var pagePaths = GetAllMatchingPagePngs(Path.GetFileNameWithoutExtension(outputTempFile));
     
            ConvertFilePathList(pagePaths, outputList);
            
            return outputList;
        }

        private void EnsureGsDllExists()
        {
            var applicationPath = Path.GetDirectoryName(Environment.ProcessPath);
            
            var gsDllPath = Path.Join(
                Path.GetDirectoryName(Environment.ProcessPath), 
                "lib",
                "gsdll64.dll");

            var folderPath = Path.GetDirectoryName(gsDllPath);
            
            Directory.CreateDirectory(folderPath ?? "");

            _logger?.Information("Checking if file exists at: {Path}", gsDllPath);

            if (File.Exists(gsDllPath))
            {
                _logger?.Information("File exists, halting gsDll restore"); 
                return;
            }
            
            // Otherwise:
            
            _logger?.Information("Gsdll does not exist");
            
            var assembly = typeof(SimpleFreePdfPrinter).Assembly;

            var gsDllStream = assembly.GetManifestResourceStream(@"Jds2.Resources.gsdll64.dll");

            if (gsDllStream == null) throw new FileNotFoundException();
            
            using var fileStream = File.Create(gsDllPath);

            gsDllStream.Seek(0, SeekOrigin.Begin);
                
            _logger?.Information("Now copying stream to file");
            
            gsDllStream.CopyTo(fileStream);
        }

        public List<string> GetPdfPagesAsPngs(string pdfFileToWork)
        {
            var ghostScriptMutex = new Mutex(false, ResourceStrings.GhostScriptMutexString);

            var outputTempFile = Path.GetTempFileName().Replace(".tmp", ".png");
                
            var pagePaths = GetAllMatchingPagePngs(Path.GetFileNameWithoutExtension(outputTempFile));  
                
            try
            {
                ghostScriptMutex.WaitOne(10000);
                
                _ = Pdf2Image.Convert(pdfFileToWork, outputTempFile);

            }
            finally
            {
                ghostScriptMutex.ReleaseMutex();
            }
            
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
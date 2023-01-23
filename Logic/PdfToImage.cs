using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Jds2
{
    public static class Pdf2Image
    {
        private static string mGSPath;
        public static string GSPath
        {
            get { return mGSPath; }
            set { mGSPath = value; }
        }

        private static int mPrintQuality = 80;
        public static int PrintQuality
        {
            get { return mPrintQuality; }
            set { mPrintQuality = value; }
        }

        
        public static string GetAppRoot(out string error)
        {
            error = null;
            try
            {
                return Path.GetDirectoryName(Environment.ProcessPath);
            }
            catch (IOException e)
            {
                error = "Can't get app root directory\n" + e.StackTrace;
            }
            return null;
        }

        public static string GetProgramFilePath(string relative_path, out string error)
        {
            var app_root = GetAppRoot(out error);
            return app_root + "\\" + relative_path;
        }

        public static string GetProgramFilePath(string relative_path)
        {
            var app_root = GetAppRoot(out _);
            return app_root + "\\" + relative_path;
        }

        public static List<string> Convert(string filename, string img_filename)
        {
            var ghostScriptMutex = new Mutex(false, ResourceStrings.GhostScriptMutexString);
            
            var errors = new List<string>();
            
            try
            {
                ghostScriptMutex.WaitOne(10000);
                
                string error = null;    
            
                var gsPath = GetProgramFilePath(@"lib\gsdll64.dll", out error);
            
                if (!File.Exists(gsPath))
                {
                    throw new FileNotFoundException("Couldn't find {0}", gsPath);
                }
                if (error != null) errors.Add(error);

                if (File.Exists(img_filename))
                {
                    throw new FileNotFoundException("Path: {0} exists but never should", img_filename);
                }

                //This is the object that perform the real conversion!
                var converter = new PdfRawConvert();

                //Ok now check what version is!
                _ = converter.GetRevision();

                //lblVersion.Text = version.intRevision.ToString() + " " + version.intRevisionDate;
                
                //Setup the converter 0 uses LogicalProcessorCount -1 
                converter.RenderingThreads = 0;
            
                converter.TextAlphaBit = -1;
                converter.TextAlphaBit = -1;
            
                converter.FitPage = true;
                converter.ResolutionX = 200;
                converter.OutputFormat = "png256";

                converter.OutputToMultipleFile = true;
                converter.FirstPageToConvert = 1;
                converter.LastPageToConvert = -1;

                var input = new FileInfo(filename);
                if (!string.IsNullOrEmpty(mGSPath))
                {
                    converter.GSPath = mGSPath;
                }
            
                _ = converter.Convert(input.FullName, img_filename);
            }
            catch (Exception ex)
            {
                throw new Exception("Ghostscript caused exception", ex);
            }
            finally
            {
                ghostScriptMutex.ReleaseMutex();
            }
            
            return errors;
        }
    }
}
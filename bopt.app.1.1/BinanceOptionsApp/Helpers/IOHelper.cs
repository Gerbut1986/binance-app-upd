using System;
using System.Reflection;

namespace BinanceOptionsApp.Helpers
{
    public static class IOHelper
    {
        public static string EscapePath(string path)
        {
            char[] invalid = System.IO.Path.GetInvalidPathChars();
            foreach (var c in invalid)
            {
                path = path.Replace(c, ' ');
            }
            return path;
        }
        public static string GetLogPath(string folderTitle)
        {
            string stime = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            string logfolder = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\.logs";
            logfolder = System.IO.Path.Combine(logfolder, EscapePath(folderTitle));
            try
            {
                System.IO.Directory.CreateDirectory(logfolder);
            }
            catch
            {

            }
            return System.IO.Path.Combine(logfolder, "log_" + stime + ".log");
        }

    }
}

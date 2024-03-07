using MultiTerminal.Connections.API.Future;
using System.Collections.Generic;
using System;

namespace Helpers
{
    public class _
    {
        object lockObj = new object();

        #region IO:
        public void WriteToTxtFile(object msg, string path, bool isAdd_on = true, bool isDate = false)
        {
            lock (lockObj)
            {
                var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\{path}.txt";
                try
                {
                    using (var sw = new System.IO.StreamWriter(desktopPath, isAdd_on))
                    {
                        var text = isDate ? $"{DateTime.UtcNow} => {msg}" : msg;
                        sw.WriteLine(text);
                    }
                }
                catch { }
            }
        }

        public void WriteArrayToFile(List<TimeAndSale_BidAskFuture> list, string path, bool isAdd_on = true)
        {
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\{path}.txt";

            var sw = new System.IO.StreamWriter(desktopPath, isAdd_on);
            foreach (var rec in list)
            {
                sw.WriteLine($"\t{rec.Id} | Ask: {rec.Ask} | Bid: {rec.Bid} | Pr: {rec.Price} | V: {rec.Volume} | BID: {rec.BuyerID} | SID: {rec.SellerID} | " +
                    $"BLmt{rec.IsBuyLimit} | SLmt: {rec.IsSellLimit} | Tm: {rec.EventDate}");
            }
            sw.Close();
        }

        public static string ReadFromTxtFile(string path)
        {
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var allTxt = string.Empty;
            using (var sr = new System.IO.StreamReader(desktopPath + path))
            {
                allTxt = sr.ReadToEnd();
            }
            return allTxt;
        }
        #endregion
    }
}

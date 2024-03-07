namespace MultiTerminal.Models
{
    using System.Linq;
    using MultiTerminal.Connections;
    using System.Collections.Generic;
    using System;

    internal class OptChainExecution
    {
        internal IConnectorLogger _logger;
        private IEnumerable<OptChain> OptChainsCpy { get; }

        public OptChainExecution(IConnectorLogger logger, IEnumerable<OptChain> OptChains)
        {
            _logger = logger;
            this.OptChainsCpy = OptChains;
        }

        public void WriteToTxtFile(string fName = "OptChains")
        {
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\{fName}.txt";
            using (var sw=new System.IO.StreamWriter(desktopPath, true))
            {
                sw.WriteLine("Strike\tVolC_C\tVolUSDT_C\tOIC_C\tOIUSDT_C\tBidS_C\tAskS_C\tBid_C\tAsk_C |Bid_P\tAsk_P\tAskS_P\tBidS_P\tOIUSDT_P\tOIC_P\tVolUSDT_P\tVolC_P");
                foreach (var oc in OptChainsCpy.OrderBy(m => m.Strike))
                {
                    sw.WriteLine(Get1Row(oc));
                }
            }
        }

        public void Output()
        {
            if (OptChainsCpy.Count() != 0)
            {
                _logger.LogInfo("Strike\tVolC_C\tVolUSDT_C\tOIC_C\tOIUSDT_C\t\t\t\tBidS_C\tAskS_C\tBid_C\tAsk_C\tBid_P\tAsk_P\tAskS_P\tBidS_P\tOIUSDT_P\t\t\t\tOIC_P\tVolUSDT_P\tVolC_P");
                foreach (var oc in OptChainsCpy.OrderBy(m => m.Strike)) 
                {
                    _logger.LogInfo(Get1Row(oc));                        
                }
            }
        }

        private string Get1Row(OptChain oc)
        {
            return $"{oc.Strike}\t{oc.VolumeCont_Calls}\t{oc.VolumeUSDT_Calls}\t{oc.OpenICont_Calls}\t{oc.OpenIUSDT_Calls}\t\t\t\t{oc.BidSize_Calls}\t{oc.AskSize_Calls}\t{oc.Bid_Calls}\t{oc.Ask_Calls}\t" +
                        $"{oc.Bid_Puts}\t{oc.Ask_Puts}\t{oc.AskSize_Puts}\t{oc.BidSize_Puts}\t{oc.OpenIUSDT_Puts}\t\t\t\t{oc.OpenICont_Puts}\t{oc.VolumeUSDT_Puts}\t{oc.VolumeCont_Puts}";
        }
    }
}

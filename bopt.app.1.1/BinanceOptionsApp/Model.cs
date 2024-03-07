using Arbitrage.Api.Dto;
using MultiTerminal.Connections.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using BinanceOptionsApp.Helpers;
using BinanceOptionsApp.Models;

namespace BinanceOptionsApp
{
    [DebuggerDisplay("{Name} - {Id}")]
    internal class AllowedInstrument
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public AllowedInstrument()
        {
        }
        public AllowedInstrument(string name, string id)
        {
            Name = name;
            Id = id;
        }
    }
    
    internal static class Model
    {
        public static Action<string> CommonLogSave;
        public static EMailSenderHelper EMailSender;
        public static Models.ProjectModel Project { get; set; }
        public static ConfigModel Current { get; set; }

        public static OptionsModel Options { get; set; }

        private static Dictionary<string, List<AllowedInstrument>> AllowedInstruments { get; set; } = new Dictionary<string, List<AllowedInstrument>>();

        public static ConnectionsModel ConnectionsConfig { get; set; }

        public static ObservableCollection<ConnectionModel> AllConnections { get; set; } = new ObservableCollection<ConnectionModel>();

        public static bool Loading { get; set; }

        public static bool Closing { get; set; }

        public static bool UseOneLeg { get; set; }

        public static bool UseOneLegHedge { get; set; }

        public static bool UseOneLegMulti { get; set; }

        public static bool UseMultiLeg { get; set; }

        public static bool UseTwoLegSimple { get; set; }

        public static bool UseTwoLegLock { get; set; }

        public static bool UseFreezeTime { get; set; }

        public static bool UseOneLegHidden { get; set; }

        public static bool UseTwoLegStandard { get; set; }

        public static bool UseThreeLeg { get; set; }

        public static bool TradeProcessingEngineError { get; set; } = false;

        public static bool IsAlgoSupported(TradeAlgorithm algo) =>
             algo == TradeAlgorithm.TwoLegFutures && Model.UseTwoLegSimple ||
            algo == TradeAlgorithm.LatencyArbitrage && Model.UseOneLeg ||
            (algo == TradeAlgorithm.ZigZag && Model.UseTwoLegLock ||
             algo == TradeAlgorithm.TwoLegArbitrage && Model.UseTwoLegSimple) ||
            algo == TradeAlgorithm.MultiLegSpread && Model.UseMultiLeg; // test algo

        public static bool IsSubscriptionFeaturePresent(string featureCode) => App.Subscription != null && App.Subscription.SubscriptionFeatures != null && App.Subscription.SubscriptionFeatures.FirstOrDefault<SubscriptionFeatureExDto>((Func<SubscriptionFeatureExDto, bool>)(x => x.Feature.Code == featureCode)) != null;

        public static string GetBrokerDisplayName(string brokerCode)
        {
            if (App.Subscription != null && App.Subscription.Brokers != null)
            {
                SubscriptionLoginResponseDto.BrokerInfoDto brokerInfoDto = App.Subscription.Brokers.FirstOrDefault<SubscriptionLoginResponseDto.BrokerInfoDto>((Func<SubscriptionLoginResponseDto.BrokerInfoDto, bool>)(x => x.Broker.Code == brokerCode));
                if (brokerInfoDto != null)
                    return brokerInfoDto.Broker.DisplayName;
            }
            return "";
        }

        public static bool IsBrokerPresent(string brokerCode) => !string.IsNullOrEmpty(Model.GetBrokerDisplayName(brokerCode));

        public static bool IsBrokerHasCustomSymbolName(string brokerCode)
        {
            if (App.Subscription != null && App.Subscription.Brokers != null)
            {
                SubscriptionLoginResponseDto.BrokerInfoDto brokerInfoDto = App.Subscription.Brokers.FirstOrDefault<SubscriptionLoginResponseDto.BrokerInfoDto>((Func<SubscriptionLoginResponseDto.BrokerInfoDto, bool>)(x => x.Broker.Code == brokerCode));
                if (brokerInfoDto != null)
                    return brokerInfoDto.BrokerFeatures == null || brokerInfoDto.BrokerFeatures.FirstOrDefault<BrokerFeatureExDto>((Func<BrokerFeatureExDto, bool>)(x => x.Feature.Code == "Broker.Private7.CustomSymbolName")) != null;
            }
            return false;
        }

        public static List<AllowedInstrument> GetAllowedInstruments(
          string brokerCode)
        {
            List<AllowedInstrument> allowedInstrumentList = (List<AllowedInstrument>)null;
            if (Model.AllowedInstruments.ContainsKey(brokerCode))
                allowedInstrumentList = Model.AllowedInstruments[brokerCode].ToList<AllowedInstrument>();
            if (allowedInstrumentList == null)
                allowedInstrumentList = new List<AllowedInstrument>();
            if (allowedInstrumentList.Count == 0)
                allowedInstrumentList.Add(new AllowedInstrument("XXXXXX", "XXXXXX"));
            return allowedInstrumentList;
        }

        public static void InitializeLicense()
        {
            Model.UseOneLegHedge = Model.IsSubscriptionFeaturePresent("Private7.1LegHedge");
            Model.UseOneLegMulti = Model.IsSubscriptionFeaturePresent("Private7.1LegMulti");
            Model.UseOneLeg = Model.IsSubscriptionFeaturePresent("Private7.1Leg");
            Model.UseTwoLegLock = Model.IsSubscriptionFeaturePresent("Private7.2LegLock");
            Model.UseTwoLegSimple = Model.IsSubscriptionFeaturePresent("Private7.2LegSimpleHedge");
            Model.UseMultiLeg = Model.IsSubscriptionFeaturePresent("Private7.MultiLeg");
            if (App.Subscription == null)
                return;
            if (!string.IsNullOrEmpty(App.Subscription.ProductSettings))
            {
                try
                {
                    using (StringReader stringReader = new StringReader(App.Subscription.ProductSettings))
                    {
                        using (XmlReader xmlReader = XmlReader.Create((TextReader)stringReader))
                        {
                            while (xmlReader.Read())
                            {
                                if (xmlReader.NodeType == XmlNodeType.Element)
                                {
                                    if (xmlReader.Name == "Provider")
                                    {
                                        try
                                        {
                                           
                                        }
                                        catch
                                        {
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
            }
            if (App.Subscription.Brokers == null)
                return;
            foreach (SubscriptionLoginResponseDto.BrokerInfoDto broker in App.Subscription.Brokers)
            {
                if (broker.Instruments != null)
                {
                    List<AllowedInstrument> allowedInstrumentList = new List<AllowedInstrument>();
                    foreach (InstrumentDto instrument in broker.Instruments)
                    {                      
                        allowedInstrumentList.Add(new AllowedInstrument()
                        {
                            Id = instrument.Code,
                            Name = instrument.DisplayName
                        });
                    }
                    allowedInstrumentList.Sort((IComparer<AllowedInstrument>)new Model.AllowedInstrumentsComparer());
                    Model.AllowedInstruments[broker.Broker.Code] = allowedInstrumentList;
                }
            }
        }

        public static event EventHandler UpdateDashboardStatus;

        public static void OnUpdateDashboardStatus()
        {
            if (Model.Closing)
                return;
            EventHandler updateDashboardStatus = Model.UpdateDashboardStatus;
            if (updateDashboardStatus == null)
                return;
            updateDashboardStatus((object)null, EventArgs.Empty);
        }

        private class AllowedInstrumentsComparer : IComparer<AllowedInstrument>
        {
            public int Compare(AllowedInstrument x, AllowedInstrument y) => string.Compare(x.Name, y.Name);
        }
    }
}

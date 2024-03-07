using BinanceOptionsApp;
using BinanceOptionsApp.Models;
using MultiTerminal.Connections;
using MultiTerminal.Connections.API.Future;
using MultiTerminal.Connections.API.Spot;
using MultiTerminal.Connections.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace BinanceOptionApp
{
    public partial class TradeScalper : UserControl, IConnectorLogger, ITradeTabInterface
    {
        private TradeModel model;
        private ManualResetEvent threadStop;
        private ManualResetEvent threadStopped;
        private readonly object loglock = new object();
        private bool PosSpotMarg = false, PosSpotMargBuy = false, PosSpotMargSell = false;
        private decimal avgGapBuy = 0;
        private decimal avgGapSell = 0;
        private decimal deviationBuy = 0;
        private decimal deviationSell = 0;
        private List<decimal> GapBuyArr = new List<decimal>();
        private List<decimal> GapSellArr = new List<decimal>();
        private decimal PreGapBuy { get; set; }
        private decimal PreGapSell { get; set; }

        private decimal PreAskS = 0, PreBidS = 0, PreAskF = 0, PreBidF = 0;
        private decimal AssetBalS { get; set; }
        private decimal CurrBalS { get; set; }

        private DateTime TimeAskF, TimeBidF, TimeAskS, TimeBidS, PreTimeAskF, PreTimeBidF, PreTimeAskS, PreTimeBidS;
        private DateTime StrtInterval, BookInterval_1, BookInterval_2, BookInterval_3;
        decimal V_1, V_2, VV_1, VV_2;
        double averageVolumeA = 0, averageVolumeB = 0, priceResistence = 0, priceSupport = 0;
        double volResistence = 0, volSupport = 0;
        private List<List<string>> Bids { get; set; }
        private List<List<string>> Asks { get; set; }

        public BinanceCryptoClient bsc = null;
        public BinanceCryptoClient bmc = null;
        public BinanceFutureClient bfc = null;

      //  private decimal GapBuy = 0.0m;
       // private decimal GapSell = 0.0m;

        private decimal MaxGapBuyA = 0.0m;
        private decimal MinGapSellA = 0.0m;
        private decimal MaxGapBuyB = 0.0m;
        private decimal MinGapSellB = 0.0m;
        private decimal ThresholdVolume = 1.0m;
        private decimal ThresholdVolume2 = 50.0m;

        private List<List<string>> BidsLeg1 { get; set; }
        private List<List<string>> AsksLeg1 { get; set; }
        private List<List<string>> BidsLeg2 { get; set; }
        private List<List<string>> AsksLeg2 { get; set; }

        private TimeAndSale_BidAsk PreTASM { get; set; }
        private AggTradeFuture     PreTASF { get; set; }

        private IConnector _1LegConnector;
        private IConnector _2LegConnector;

        private string leg1Type, leg2Type;

        //public static BinanceFutureClient LatencyArb = null;

        string swLogPath;
        string swDebugPath;
        string swQuotesPath;
        System.IO.FileStream fsData;

        //DispatcherTimer timerEvent;

        // Test flag for open:
        bool isOpen = false;

        public TradeScalper()
        {
            InitializeComponent();
        }

        public void InitializeTab()
        {
            model = DataContext as TradeModel;
            fast.InitializeProviderControl(model.Leg1, true);
            slow.InitializeProviderControl(model.Leg2, true);

            var spt1 = model.Leg1.Name.Split(new char[] { '[', ']' });
            var spt2 = model.Leg2.Name.Split(new char[] { '[', ']' });
            leg1Type = spt1[1];
            leg2Type = spt2[1];

            model.LogError = LogError;
            model.LogInfo = LogInfo;
            model.LogWarning = LogWarning;
            model.LogClear = LogClear;
            model.LogOrderSuccess = LogOrderSuccess;
            HiddenLogs.LogHeader(model);
        }

        #region Log's methods:
        public void LogOrderSuccess(string message)
        {
            Log(message, Colors.Orange, Color.FromRgb(255, 165, 0));
        }
        public void LogInfo(string message)
        {
            Log(message, Colors.White, Color.FromRgb(0x00, 0x23, 0x44));
        }
        public void LogError(string message)
        {
            Log(message, Color.FromRgb(0xf3, 0x56, 0x51), Color.FromRgb(0xf3, 0x56, 0x51));
        }
        public void LogWarning(string message)
        {
            Log(message, Colors.LightBlue, Colors.Blue);
        }
        public void LogClear()
        {
            logBlock.Text = "";
        }
        public void Log(string _message, Color color, Color dashboardColor)
        {
            string message = DateTime.Now.ToString("HH:mm:ss.ffffff") + "> " + _message + "\r\n";
            lock (loglock)
            {
                if (swLogPath != null)
                {
                    System.IO.File.AppendAllText(swLogPath, message);
                    Model.CommonLogSave(message);
                }
            }
            SafeInvoke(() =>
            {
                model.LastLog = _message;
                model.LastLogBrush = new SolidColorBrush(dashboardColor);
                Run r = new Run(message)
                {
                    Tag = DateTime.Now,
                    Foreground = new SolidColorBrush(color)
                };
                try
                {
                    while (logBlock.Inlines.Count > 250)
                    {
                        logBlock.Inlines.Remove(logBlock.Inlines.LastInline);
                    }
                }
                catch
                {

                }
                int count = logBlock.Inlines.Count;
                if (count == 0) logBlock.Inlines.Add(r);
                else
                {
                    logBlock.Inlines.InsertBefore(logBlock.Inlines.FirstInline, r);
                }
            });
        }
        public void SafeInvoke(Action action)
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                if (!Model.Closing)
                {
                    action();
                }
            }));
        }
        #endregion

        #region RestoreNullCombo & EscapePath methods:
        public void RestoreNullCombo(ConnectionModel cm)
        {
            fast.RestoreNullCombo(cm);
            slow.RestoreNullCombo(cm);
        }

        private string EscapePath(string path)
        {
            char[] invalid = System.IO.Path.GetInvalidPathChars();
            foreach (var c in invalid)
            {
                path = path.Replace(c, ' ');
            }
            return path;
        }
        #endregion

        #region Window's Event Handlers
        private void BuStart_Click(object sender, RoutedEventArgs e)
        {
            Start();
        }

        private void BuStop_Click(object sender, RoutedEventArgs e)
        {
            Stop(true);
        }

        private void LogClear_Click(object sender, RoutedEventArgs e)
        {
            LogClear();
        }

        private void TbOpenOrderType_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
        #endregion

        #region Start & Stop methods
        public void Start()
        {
            if (model.Started) return;
            model.Started = true;
            model.FeederOk = false;
            LogClear();
            HiddenLogs.LogHeader(model);

            model.Leg1.Symbol = fast.AssetTb.Text + fast.CurrencyTb.Text;
            model.Leg2.Symbol = slow.AssetTb.Text + slow.CurrencyTb.Text;

            TradeModel.currencySpot = model.Leg1.SymbolCurrency;
            TradeModel.currencyFuture = model.Leg2.SymbolCurrency;
            TradeModel.fullSymbolFuture = model.Leg2.Symbol;

            threadStop = new ManualResetEvent(false);
            threadStopped = new ManualResetEvent(false);
            new Thread(ThreadProc).Start();
            ///timerEvent.Start();
            Model.OnUpdateDashboardStatus();
        }

        public void Stop(bool wait)
        {
            if (!model.Started) return;
            threadStop.Set();
            if (wait)
            {
                threadStopped.WaitOne();
                threadStop.Dispose();
                threadStopped.Dispose();
            }
            model.Started = false;
            model.FeederOk = false;
            Model.OnUpdateDashboardStatus();
        }
        #endregion

        #region THread Process Method

        void ThreadProc()
        {
            model.Leg1.InitView();
            model.Leg2.InitView();

            _1LegConnector = model.Leg1.CreateConnector(this, threadStop, model.SleepMs, Dispatcher, true);
            _2LegConnector = model.Leg2.CreateConnector(this, threadStop, model.SleepMs, Dispatcher);
            _1LegConnector.Tick += _1LegConnector_Tick;
            _2LegConnector.Tick += _2LegConnector_Tick;
            _1LegConnector.LoggedIn += OneLegConnector_LoggedIn;
            _2LegConnector.LoggedIn += TwoLegConnector_LoggedIn;

            model.LogInfo(model.Title + " logging in...");
            while (!threadStop.WaitOne(100))
            {
                if (_1LegConnector.IsLoggedIn && _2LegConnector.IsLoggedIn)
                {
                    model.LogInfo(model.Title + " logged in OK.");
                    break;
                }
            }
            if (!threadStop.WaitOne(0))
            {
                if (_1LegConnector.IsLoggedIn)
                {
                    On_1LegLogin();
                }
                if (_2LegConnector.IsLoggedIn)
                {
                    On_2LegLogin();
                }
            }

            #region Log process:
            if (model.Log)
            {
                string stime = DateTime.Now.ToString("yyyyMMddHHmmss");
                string logfolder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\.logs";
                logfolder = System.IO.Path.Combine(logfolder, EscapePath(model.Title));
                try { System.IO.Directory.CreateDirectory(logfolder); }
                catch { }
                swLogPath = System.IO.Path.Combine(logfolder, "lg_" + stime + ".log");
                swDebugPath = System.IO.Path.Combine(logfolder, "db_" + stime + ".log");
                swQuotesPath = System.IO.Path.Combine(logfolder, "qu_" + stime + ".log");
            }
            else
            {
                swLogPath = null;
                swDebugPath = null;
                swQuotesPath = null;
            }
            if (model.SaveTicks)
            {
                string stime = DateTime.Now.ToString("yyyyMMddHHmmss");
                string datafolder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\.data";
                datafolder = System.IO.Path.Combine(datafolder, EscapePath(model.Title));
                try
                {
                    System.IO.Directory.CreateDirectory(datafolder);
                }
                catch
                {

                }
                fsData = new System.IO.FileStream(datafolder + "\\" + stime + ".LatencyArbitrage", System.IO.FileMode.Create);
            }
            #endregion

            TimeSpan startTime = model.Open.StartTimeSpan();
            TimeSpan endTime = model.Open.EndTimeSpan();

            while (!threadStop.WaitOne(model.SleepMs))
            {
                //var koef1 = model.Open.Koef1;
                //var koef2 = model.Open.Koef2;
                //  Here you can write arbitrage of strategy and algo:
                decimal Ask1=0, VolAsk1 = 0, Bid1 = 0, VolBid1 = 0, Ask2 = 0, VolAsk2 = 0, Bid2 = 0, VolBid2 = 0;
                decimal GapBuy = 0; bool MRshort = false, MRlong = false;
                decimal GapSell = 0;
                if (model.Leg2.Ask > 0 && model.Leg1.Bid > 0)//book ev
                {
                     var askByIndexl1 = GetAskByIndex(0,true).Split(new char[] { ',' });
                     Ask1 = decimal.Parse(askByIndexl1[0]);
                     VolAsk1 = decimal.Parse(askByIndexl1[1]);
                     var bidByIndexl1 = GetBidByIndex(0,true).Split(new char[] { ',' });
                     Bid1 = decimal.Parse(bidByIndexl1[0]);
                     VolBid1 = decimal.Parse(bidByIndexl1[1]);

                     var askByIndexl2 = GetAskByIndex(0,false).Split(new char[] { ',' });
                     Ask2 = decimal.Parse(askByIndexl2[0]);
                     VolAsk2 = decimal.Parse(askByIndexl2[1]);
                     var bidByIndexl2 = GetBidByIndex(0,false).Split(new char[] { ',' });
                     Bid2 = decimal.Parse(bidByIndexl2[0]);
                     VolBid2 = decimal.Parse(bidByIndexl2[1]);
                    if ((DateTime.Now - BookInterval_2).TotalMilliseconds > 1023)
                    {
                        V_1 = VolAsk1 / VolBid1; 
                        V_2 = VolAsk2 / VolBid2;
                        //if(V_1 > 3) { }
                        BookInterval_2 = DateTime.Now;
                    }
                    if ((DateTime.Now - BookInterval_3).TotalMilliseconds > 3057)
                    {
                        VV_1 = VolAsk1 / VolBid1;
                        VV_2 = VolAsk2 / VolBid2;

                        BookInterval_3 = DateTime.Now;
                    }

                }

                ThresholdVolume = model.Open.Threshold;
                // ThresholdVolume = model.Open.Threshold2;

                if (bmc != null && bmc.timeAndSale != null)
                {
                    List<TimeAndSale_BidAsk> lentaList = bmc.timeAndSale;  // margin
                    if (lentaList != null && lentaList.Count != 0)
                    {
                        TimeAndSale_BidAsk lastM = lentaList[lentaList.Count -1];
                        if (lastM != null)
                        {
                            if (PreTASM != lastM)
                            {
                                var AskM = lastM.Ask;
                                var BidM = lastM.Bid;
                                var BuyerIDM = lastM.BuyerID;
                                var DealTimeM = lastM.DealTime;
                                var EventDateM = lastM.EventDate;
                                var EventTimeM = lastM.EventTime;
                                var EventTypeM = lastM.EventType;
                                var IsBuyLimitM = lastM.IsBuyLimit;
                                var IsSellLimitM = lastM.IsSellLimit;
                                var PriceM = lastM.Price;
                                var SellerIDM = lastM.SellerID;
                                var SymbolM = lastM.Symbol;
                                var TicketM = lastM.Ticket;
                                var VolumeM = lastM.Volume;


                               if (VolumeM >= ThresholdVolume)
                               {
                                   if (PriceM >= AskM)
                                   {
                                        MRlong = true;
                                   }
                                   else if (PriceM <= BidM)
                                   {
                                        MRshort = true;
                                   }
                                   else { }
                               }

                               PreTASM = lastM;
                            }
                              //  model.LogError($"[MARGIN] => Ask: {AskM} | Bid: {BidM} | Volume: {VolumeM} | EventType: {EventTypeM}");

                            //model.LogInfo($"Ask: {Ask} | Bid: {Bid} | BuyerID: {BuyerID} | SellerID: {SellerID} | Price: {Price}");
                        }
                    }
                }

                if (bfc != null && bfc.TasF != null)//future
                {
                    decimal LongVol = 0, ShortVol = 0;

                    var lentaFList = bfc.TasF;        // futures
                    if (lentaFList != null && lentaFList.Count != 0)
                    {
                        AggTradeFuture lastF = lentaFList.Last();
                        if (lastF != null)
                        {
                            if (PreTASF != lastF)
                            {

                                var Ask = lastF.data.Ask;
                                var Bid = lastF.data.Bid;
                                var AggTradeId = lastF.data.AggTradeId;
                                var IsMarketMaker = lastF.data.IsMarketMaker;
                                var EventDate = lastF.data.EventDate;
                                var EventTime = lastF.data.EventTime;
                                var EventType = lastF.data.EventType;
                                var LastTradeId = lastF.data.LastTradeId;
                                var TradeTime = lastF.data.TradeTime;
                                var Price = lastF.data.Price;
                                var FirstTradeId = lastF.data.FirstTradeId;
                                var Symbol = lastF.data.Symbol;
                                var Volume = lastF.data.Volume;

                                if (Price >= Ask)
                                {
                                    
                                }
                                else if (Price <= Bid)
                                {

                                }
                                else { }
                                PreTASF = lastF;
                            }
                             //   model.LogOrderSuccess($"[FUTURE] => Ask: {Ask} | Bid: {Bid} | Volume: {Volume} | EventType: {EventType}");
                        }
                        
                    }
                }

                if (model.Leg2.Bid != 0 && model.Leg1.Ask != 0)
                {
                    GapBuy = model.Leg1.Ask - model.Leg2.Ask;
                    GapSell = model.Leg1.Bid - model.Leg2.Bid;

                    if (PreBidF != model.Leg1.Bid) { TimeBidF = DateTime.Now; PreBidF = model.Leg1.Bid; }

                    if (PreAskF != model.Leg1.Ask) { TimeAskF = DateTime.Now; PreAskF = model.Leg1.Ask; }

                    if (PreAskS != model.Leg2.Ask) { TimeAskS = DateTime.Now; PreAskS = model.Leg2.Ask; }

                    if (PreBidS != model.Leg2.Bid) { TimeBidS = DateTime.Now; PreBidS = model.Leg2.Bid; }

                    if (model.Leg2.Bid != 0 && model.Leg1.Ask != 0 && model.Leg2.Ask != 0 && model.Leg1.Bid != 0)
                    {
                        if (model.Leg2.Ask > model.Leg2.Bid && model.Leg1.Ask > model.Leg1.Bid)
                        {
                            GapBuy = model.Leg1.Ask - model.Leg2.Ask;
                            GapSell = model.Leg1.Bid - model.Leg2.Bid;
                            //GapBuy = model.Leg2.Bid - model.Leg1.Ask;
                            //GapSell = model.Leg2.Ask - model.Leg1.Bid;
                        }
                        //if (model.Leg2.Ask == model.Leg1.Ask && model.Leg2.Bid == model.Leg1.Bid)
                        
                        //    //model.LogError($"Gap Error | model.Leg2.Ask == model.Leg1.Ask && model.Leg2.Bid == model.Leg1.Bid = {model.Leg2.Ask == model.Leg1.Ask && model.Leg2.Bid == model.Leg1.Bid}");
                        //var askByIndexl1 = GetAskByIndex(0,true) .Split(new char[] { ',' });
                        //var bidByIndexl1 = GetBidByIndex(0,true) .Split(new char[] { ',' });
                        //var askByIndexl2 = GetAskByIndex(0,false).Split(new char[] { ',' });
                        //var bidByIndexl2 = GetBidByIndex(0,false).Split(new char[] { ',' });
                        
                        if (GapBuy != PreGapBuy)
                        {
                            GapBuyArr.Add(GapBuy);
                            PreGapBuy = GapBuy;
                        }

                        if (GapSell != PreGapSell)
                        {
                            GapSellArr.Add(GapSell);
                            PreGapSell = GapSell;
                        }

                        var useAlignment = model.UseAlignment; //  if(useAlignment) 
                        var buyCnt = GapBuyArr.Count;
                        var sellCnt = GapSellArr.Count;
                        var Period = model.Open.PeriodAlignment;

                        if ((DateTime.Now - StrtInterval).TotalSeconds > 600)
                        {
                            if (sellCnt > Period) avgGapSell = GetAvgGapSell(Period);
                            else avgGapSell = GetAvgGapSell(sellCnt);

                            if (buyCnt > Period) avgGapBuy = GetAvgGapBuy(Period);
                            else avgGapBuy = GetAvgGapBuy(buyCnt);

                            StrtInterval = DateTime.Now;
                        }

                        if (GapBuy != 0 && avgGapBuy != 0)
                        {
                            deviationBuy = GapBuy - avgGapBuy;
                            if (deviationBuy > MaxGapBuyA || MaxGapBuyA == 0)
                            {
                                MaxGapBuyA = deviationBuy;
                               // model.LogInfo($"New Max DevBuy: {MaxGapBuyA} GapBuy:{GapBuy} avgGapBuy:{avgGapBuy} deviationBuy:{deviationBuy}");
                              //  model.LogInfo($"Ask1: {model.Leg1.Ask} Bid1:{model.Leg1.Bid} Ask2:{model.Leg2.Ask} Bid2:{model.Leg2.Bid}");
                                model.MaxGapBuyA = Math.Round(MaxGapBuyA, model.Open.Point);
                            }
                        }
                        if (GapSell != 0 && avgGapSell != 0)
                        {
                            deviationSell = GapSell - avgGapSell;
                            if (deviationSell < MinGapSellA || MinGapSellA == 0)
                            {
                                MinGapSellA = deviationSell;
                               // model.LogInfo($"New Min DevSell: {MinGapSellA} GapSell:{GapSell} avgGapSell:{avgGapSell} deviationSell:{deviationSell}");
                               // model.LogInfo($"Ask1: {model.Leg1.Ask} Bid1:{model.Leg1.Bid} Ask2:{model.Leg2.Ask} Bid2:{model.Leg2.Bid}");
                                model.MinGapSellA = Math.Round(MinGapSellA, model.Open.Point);

                            }
                        }
                    }                    
                    
                    model.MaxGapBuyA = MaxGapBuyA;
                    model.MinGapSellA = MinGapSellA;

                }

                var spotLot = model.Leg1.Lot;
                var futureLot = model.Leg2.Lot;

                var spotLotStep = model.Leg1.LotStep;
                var futureLotStep = model.Leg2.LotStep;

                #region OnView [for Future] (NOT USAGE):
                //if (LatencyArb.AccountInfo != null)
                //{
                //model.Leg2.TotalInitMargin = decimal.Parse(LatencyArb.AccountInfo.TotalInitialMargin, CultureInfo.InvariantCulture);
                //model.Leg2.AvailableBalance = decimal.Parse(LatencyArb.AccountInfo.AvailableBalance, CultureInfo.InvariantCulture);
                //model.Leg2.TotalCrossUnPnl = decimal.Parse(LatencyArb.AccountInfo.TotalCrossUnPnl, CultureInfo.InvariantCulture);
                //model.Leg2.TotalMarginBalance = decimal.Parse(LatencyArb.AccountInfo.TotalMarginBalance, CultureInfo.InvariantCulture);
                //model.Leg2.TotalCrossWalletBalance = decimal.Parse(LatencyArb.AccountInfo.TotalCrossWalletBalance, CultureInfo.InvariantCulture);
                //// Position model
                //var res = LatencyArb.AccountInfo.Positions.FirstOrDefault(s => s.Symbol == model.Leg2.Symbol);
                //if (res != null)
                //{
                //    model.Leg2.EntryPrice = decimal.Parse(res.EntryPrice, CultureInfo.InvariantCulture);
                //    model.Leg2.PositionAmt = decimal.Parse(res.PositionAmt, CultureInfo.InvariantCulture);
                //}
                #endregion

                #region OnView [for Spot/Margin]:
                if (bmc.MarginAccount != null)
                {
                    var assetFound = bmc.MarginAccount.userAssets.FirstOrDefault(a => a.asset == model.Leg1.SymbolAsset);
                    model.Leg1.CollateralMarginLevel = decimal.Parse(bmc.MarginAccount.collateralMarginLevel, CultureInfo.InvariantCulture);
                    model.Leg1.MarginLevel = decimal.Parse(bmc.MarginAccount.marginLevel, CultureInfo.InvariantCulture);
                    model.Leg1.TotalCollateralValueInUSDT = decimal.Parse(bmc.MarginAccount.totalCollateralValueInUSDT, CultureInfo.InvariantCulture);
                    if (assetFound != null)
                    {
                        model.Leg1.Borrowed = assetFound.borrowed;
                        model.Leg1.Free = assetFound.free;
                        model.Leg1.Interest = assetFound.interest;
                        model.Leg1.Locked = assetFound.locked;
                        model.Leg1.NetAsset = assetFound.netAsset;
                    }
                }
                #endregion

                AssetBalS = model.Leg1.NetAsset;
                CurrBalS = model.Leg1.TotalCollateralValueInUSDT;
                var Profit = model.Leg1.TotalCrossUnPnl;
                var gapForOpen = model.Open.GapForOpen;
                var gapForClose = model.Open.GapForClose;
                
               // ThresholdVolume2 = model.Open.Threshold2;
                //model.Open.Threshold
                model.GapSell = GapSell;
                model.GapBuy = GapBuy;
                //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
                /*
                if (model.Leg1.Ask != 0 && model.Leg1.Bid != 0 && bmc.MarginAccount != null) //bcc.SpotAccount != null)
                {
                    if (!isOpen)
                    {
                        isOpen = true;
                        var price = model.Leg1.Bid - 20.2m;
                        //bmc.GetOpenOrder(); 
                       // OpenPos(model.Leg1.Symbol, price, "1Leg",
                       // MultiTerminal.Connections.FillPolicy.GTC, OrderSide.Buy, 0, OrderType.Limit);
                        if (bfc != null)
                        {
                            if (bfc.PlacedOrdersUsd_M.Count != 0)
                            {
                                var orders = bfc.PlacedOrdersUsd_M;
                                Thread.Sleep(300);
                                //OrderDelete(model.Leg1.Symbol, OrderSide.Buy, OrderType.Limit, price.ToString(), orders[0].OrderId.ToString(),
                                //orders[0].ClientOrderId);
                               // OrderModify(model.Leg1.Symbol, "1Leg", orders[0].ClientOrderId, orders[0].OrderId.ToString(),
                                 //  OrderSide.Buy, price - 10, 1);
                            }
                        }
                    }
                }
                */
                //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

                //***********************************************************
                if (PosSpotMarg)
                {
                    if (PosSpotMargSell)
                    {
                        if (TimeAskF > TimeAskS && deviationBuy > gapForClose && MRlong)
                        {
                            model.LogInfo($"Try Close Sell Pos, deviationBuy: {deviationBuy} Bid: {model.Leg1.Bid}");
                            model.LogInfo($"Support: {priceSupport} VolS:{volSupport} Resistence: {priceResistence} VolR:{volResistence}");
                            model.LogInfo($"Ask1: {Ask1} Vol:{VolAsk1} Bid1: {Bid1} Vol:{VolBid1}");
                            model.LogInfo($"Ask2: {Ask2} Vol:{VolAsk2} Bid2: {Bid2} Vol:{VolBid2}");

                            if (model.AllowOpen)
                            {
                                bmc.GetOpenOrder(); 
                                
                                if (OpenPos(model.Leg1.Symbol, model.Leg1.Bid, "1Leg",
                                MultiTerminal.Connections.FillPolicy.GTC, OrderSide.Buy, GapBuy, OrderType.Limit))
                                {
                                    model.LogInfo($"Close Sell Pos GapBuy: {GapBuy}");
                                    PosSpotMarg = false; PosSpotMargBuy = false;
                                    //await TradeOneLeg.bfc.GetBalance(model.Leg2.Symbol);
                                }
                                else
                                {
                                    PosSpotMarg = true; PosSpotMargBuy = true;
                                }
                            }
                        }
                    }
                    else if (PosSpotMargBuy)
                    {
                        if (TimeBidF > TimeBidS && deviationSell * -1 > gapForClose && MRshort)
                        {
                            model.LogInfo($"Try Close Buy Pos, deviationSell: {deviationSell} Ask: {model.Leg1.Ask}");
                            model.LogInfo($"Support: {priceSupport} VolS:{volSupport} Resistence: {priceResistence} VolR:{volResistence}");
                            model.LogInfo($"Ask1: {Ask1} Vol:{VolAsk1} Bid1: {Bid1} Vol:{VolBid1}");
                            model.LogInfo($"Ask2: {Ask2} Vol:{VolAsk2} Bid2: {Bid2} Vol:{VolBid2}");

                            if (model.AllowOpen)
                            {
                                if (OpenPos(model.Leg1.Symbol, model.Leg1.Ask, "1Leg",
                                    MultiTerminal.Connections.FillPolicy.GTC, OrderSide.Sell, GapSell, OrderType.Limit))
                                {
                                    model.LogInfo($"Close Buy Pos, deviationSell: {deviationSell}");
                                    PosSpotMarg = false; PosSpotMargBuy = false;
                                    //await TradeOneLeg.bfc.GetBalance(model.Leg2.Symbol);
                                }
                                else
                                {
                                    PosSpotMarg = true; PosSpotMargBuy = true;
                                }
                            }
                        }
                    }
                    else
                    {
                    }
                }
                else if (!PosSpotMarg)
                { 
                    if (TimeAskF > TimeAskS && deviationBuy > gapForOpen && MRlong)
                    {
                        model.LogInfo($"Try Open Buy Pos, deviationBuy: {deviationBuy} Bid: {model.Leg1.Bid} ");
                        model.LogInfo($"Support: {priceSupport} VolS:{volSupport} Resistence: {priceResistence} VolR:{volResistence}");
                        model.LogInfo($"Ask1: {Ask1} Vol:{VolAsk1} Bid1: {Bid1} Vol:{VolBid1}");
                        model.LogInfo($"Ask2: {Ask2} Vol:{VolAsk2} Bid2: {Bid2} Vol:{VolBid2}");

                        if (model.AllowOpen)
                        {
                            if (OpenPos(model.Leg1.Symbol, model.Leg1.Bid, "1Leg",
                                MultiTerminal.Connections.FillPolicy.GTC, OrderSide.Buy, deviationBuy, OrderType.Limit))
                            {
                                model.LogInfo($"Elapsted Open Buy (1Leg)");
                                PosSpotMarg = true; PosSpotMargBuy = true;
                                //await TradeOneLeg.bfc.GetBalance(model.Leg2.SymbolAsset);
                            }
                            else
                            {
                                PosSpotMarg = false; PosSpotMargBuy = false;
                            }
                        }
                    }
                    if (TimeBidF > TimeBidS && deviationSell * -1 > gapForOpen && MRshort)
                    {
                        model.LogInfo($"Try Open Sell Pos, deviationSell: {deviationSell} Ask: {model.Leg1.Ask}");
                        model.LogInfo($"Support: {priceSupport} VolS:{volSupport} Resistence: {priceResistence} VolR:{volResistence}");
                        model.LogInfo($"Ask1: {Ask1} Vol:{VolAsk1} Bid1: {Bid1} Vol:{VolBid1}");
                        model.LogInfo($"Ask2: {Ask2} Vol:{VolAsk2} Bid2: {Bid2} Vol:{VolBid2}");

                        if (model.AllowOpen)
                        {
                            if (OpenPos(model.Leg1.Symbol, model.Leg1.Ask, "1Leg",
                                    MultiTerminal.Connections.FillPolicy.GTC, OrderSide.Sell, deviationSell, OrderType.Limit))
                            {
                                model.LogInfo($"Elapsted Open Sell (1Leg)");
                                PosSpotMarg = true; PosSpotMargSell = true;
                                //await TradeOneLeg.bfc.GetBalance(model.Leg2.SymbolAsset);
                            }
                            else
                            {
                                PosSpotMarg = false; PosSpotMargSell = false;
                            }
                        }
                    }
                   
                }
                //***********************************************************
               
            }

            _2LegConnector.Tick -= _2LegConnector_Tick;
            _1LegConnector.Tick -= _1LegConnector_Tick;
            _2LegConnector.LoggedIn -= TwoLegConnector_LoggedIn;
            _1LegConnector.LoggedIn -= OneLegConnector_LoggedIn;
            ConnectorsFactory.Current.CloseConnector(model.Leg2.Name, true);
            ConnectorsFactory.Current.CloseConnector(model.Leg1.Name, true);

            swQuotesPath = null;
            swDebugPath = null;
            swLogPath = null;
            if (fsData != null)
            {
                fsData.Flush();
                fsData.Dispose();
                fsData = null;
            }
            threadStopped.Set();
        }

        private void _2LegConnector_Tick(object sender, TickEventArgs e)
        {
            if (e.Symbol.ToLower() == model.Leg2.Symbol.ToLower())
            {
                //Asks = e.Asks;
                //Bids = e.Bids;
                AsksLeg2 = e.Asks;
                BidsLeg2 = e.Bids;


                model.Leg2.Bid = e.Bid;
                model.Leg2.Ask = e.Ask;
                model.Leg2.Time = DateTime.Now;
            }
        }

        private void _1LegConnector_Tick(object sender, TickEventArgs e)
        {
            if (e.Symbol.ToLower() == model.Leg1.Symbol.ToLower())
            {
                //Asks = e.Asks;
                //Bids = e.Bids;
                AsksLeg1 = e.Asks;
                BidsLeg1 = e.Bids;

                 var maxVolumeRow = AsksLeg1
               .Where(row => row.Count > 1 && double.TryParse(row[1], out _))
               .OrderByDescending(row => double.Parse(row[1]))
               .FirstOrDefault();

                if (maxVolumeRow != null && double.TryParse(maxVolumeRow[1], out double maxVolume))
                {
                    priceResistence = double.Parse(maxVolumeRow[0]);
                    volResistence = double.Parse(maxVolumeRow[1]);
                }

                var maxVolumeRow2 = BidsLeg1
               .Where(row => row.Count > 1 && double.TryParse(row[1], out _))
               .OrderByDescending(row => double.Parse(row[1]))
               .FirstOrDefault();

                if (maxVolumeRow2 != null && double.TryParse(maxVolumeRow2[1], out double maxVolume2))
                {
                    priceSupport = double.Parse(maxVolumeRow2[0]);
                    volSupport = double.Parse(maxVolumeRow2[1]);

                }


                if ((DateTime.Now - BookInterval_1).TotalMilliseconds > 2000)
                {
                    double totalVolumeAsks = 0;
                    foreach (List<string> row in AsksLeg1)
                    {
                        if (row.Count > 1 && double.TryParse(row[1], out double volume))
                        {
                            totalVolumeAsks += volume;
                        }
                    }
                    double averageVolumeA = 0; 
                    if (AsksLeg1.Count>0) averageVolumeA = totalVolumeAsks / AsksLeg1.Count;
                    
                   double totalVolumeBids = 0; 
                    foreach (List<string> row in BidsLeg1)
                    {
                        if (row.Count > 1 && double.TryParse(row[1], out double volume))
                        {
                            totalVolumeBids += volume;
                        }
                    }
                    averageVolumeB = 0;
                    if (BidsLeg1.Count > 0) averageVolumeB = totalVolumeBids / BidsLeg1.Count;

                    BookInterval_1 = DateTime.Now;
                }

                model.Leg1.Bid = e.Bid;
                model.Leg1.Ask = e.Ask;
                model.Leg1.Time = DateTime.Now;
            }
        }

        private decimal GetAvgGapBuy(int count)
        {
            int startIdx = Math.Max(0, GapBuyArr.Count - count);
            if (GapBuyArr.Count >= count)
            {
                decimal sum = 0;
                for (int i = startIdx; i < GapBuyArr.Count; i++)
                    sum += GapBuyArr[i];
                if (count != 0) 
                    return sum / count;
            }
            else
            {
                if (GapBuyArr.Count > 0)
                {
                    decimal sum = 0;
                    for (int i = 0; i < GapBuyArr.Count; i++)
                        sum += GapBuyArr[i];
                    return sum / GapBuyArr.Count;
                }
            }
            return 0.0m;
        }

        private decimal GetAvgGapSell(int count)
        {
            int startIdx = Math.Max(0, GapSellArr.Count - count);
            if (count > 0 && GapSellArr.Count >= count)
            {
                decimal sum = 0;
                for (int i = startIdx; i < GapSellArr.Count; i++)
                    sum += GapSellArr[i];
                return sum / count;
            }
            else
            {
                if (GapSellArr.Count > 0)
                {
                    decimal sum = 0;
                    for (int i = 0; i < GapSellArr.Count; i++)
                        sum += GapSellArr[i];
                    return sum / GapSellArr.Count;
                }
            }
            return 0.0m;
        }

        private string GetAskByIndex(int index, bool is1stLeg)
        {
            var Asks_ = is1stLeg ? AsksLeg1 : AsksLeg2;
            if (Asks_ != null)
                if (Asks_.Count != 0 && index < 20)
                {
                    var foundAsk = Asks_[index];
                    return $"{foundAsk[0]},{foundAsk[1]}";
                }
            return "0";
        }

        private string GetBidByIndex(int index, bool is1stLeg)
        {
            var Bids_ = is1stLeg ? BidsLeg1 : BidsLeg2;
            if (Bids_ != null)
                if (Bids_.Count != 0 && index < 20)
                {
                    var foundBid = Bids_[index];
                    return $"{foundBid[0]},{foundBid[1]}";
                }
            return "0";
        }

        public bool OpenPos(string symb, decimal price, string type, MultiTerminal.Connections.FillPolicy policy, OrderSide bs, decimal gap, OrderType orderType)
        {
            bool isSuccess = false;

            OrderOpenResult result = _1LegConnector.Open(symb, price, model.Leg1.Lot, policy, bs, model.Magic, model.Slippage, 1,
                            orderType, model.Open.PendingLifeTimeMs);

            // Check if Order was Successfully send:
            if (string.IsNullOrEmpty(result.Error))
            {
                decimal slippage = -(result.OpenPrice - model.Leg1.Ask);
                model.LogOrderSuccess($"[{type}]: {bs.ToString()} OK " + model.Leg1.Symbol + " at " + model.FormatPrice(result.OpenPrice) + ";Gap="
                    + gap +
                    ";Price=" + model.FormatPrice(model.Leg1.Ask) + ";Slippage=" + ToStr1(slippage) +
                    ";Execution=" + ToStrMs(result.ExecutionTime) + " ms;");
                isSuccess = true;
            }
            else
            {
                model.LogError(_1LegConnector.ViewId + " " + result.Error);
          //      var balanceActual = bcc.MarginAccount.totalCollateralValueInUSDT;
                model.LogInfo($"[{type}]: {bs.ToString()} FAILED " + model.Leg1.Symbol + ";Gap=" + gap + ";Price=" + model.FormatPrice(model.Leg1.Ask));
                isSuccess = false;
            }

            return isSuccess;
        }

        public bool OrderModify(string symbol, string legType,  string origClientOrderId, string orderId, OrderSide side, decimal newPrice, decimal lot)
        {
            bool isSuccess = false;

            var result = _1LegConnector.Modify(symbol, origClientOrderId, orderId, side, newPrice, lot);

            // check if order modify was successfully send:
            if (string.IsNullOrEmpty(result.Error))
            {
                model.LogOrderSuccess($"[{legType}]: {result.Side.ToString()} OK MODIFY " + model.Leg1.Symbol + $" at {result.Lot} lot" +
                    ";price=" + model.FormatPrice(result.OpenPrice) +
                    ";execution=" + ToStrMs(result.ExecutionTime) + " ms;");
                isSuccess = true;
            }
            else
            {
                model.LogError(_1LegConnector.ViewId + " " + result.Error);
                model.LogInfo($"[{legType}]: {side.ToString()} failed " + model.Leg1.Symbol + ";price=" + 
                    model.FormatPrice(model.Leg1.Ask));
                isSuccess = false;
            }

            return isSuccess;
        }

        public bool OrderDelete(string symbol, OrderSide side, OrderType type, string price, string orderId, string origClientOrderId)
        {
            bool isSuccess = false;
            var start = DateTime.Now;
            var result = _1LegConnector.OrderDelete(symbol, orderId, origClientOrderId);
            var   end = DateTime.Now;
            // check if order modify was successfully send:
            if (result)
            {
                model.LogOrderSuccess($"OrderId: [{orderId}]: {side.ToString()} {type.ToString()} OK DELETE  " + model.Leg1.Symbol +
                    ";price=" + price +
                    ";Execution=" + ToStrMs(end - start) + " ms;");
                isSuccess = true;
            }
            else
            {
                model.LogError(_1LegConnector.ViewId + " DELETE FAILURE.....");
                isSuccess = false;
            }

            return isSuccess;
        }

        void On_2LegLogin()
        {
            _2LegConnector.Fill = (MultiTerminal.Connections.FillPolicy)model.Open.Fill;
            _2LegConnector.Subscribe(model.Leg2.Symbol, model.Leg2.Symbol, "Scalper");
        }
        void On_1LegLogin()
        {
            _1LegConnector.Fill = (MultiTerminal.Connections.FillPolicy)model.Open.Fill;
            _1LegConnector.Subscribe(model.Leg1.Symbol, model.Leg2.Symbol, "Scalper");
        }

        string ToStr1(decimal value)
        {
            return value.ToString("F1", CultureInfo.InvariantCulture);
        }




        string ToStrMs(TimeSpan span)
        {
            return span.TotalMilliseconds.ToString("F3", CultureInfo.InvariantCulture);
        }

        private void TwoLegConnector_LoggedIn(object sender, EventArgs e)
        {
            On_2LegLogin();
        }

        private void OneLegConnector_LoggedIn(object sender, EventArgs e)
        {
            On_1LegLogin();
        }
        #endregion
    }
}


using MultiTerminal.Connections;
using MultiTerminal.Connections.API.Future;
using MultiTerminal.Connections.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;

namespace BinanceOptionsApp
{
    public partial class TradeOneLeg : UserControl, IConnectorLogger, ITradeTabInterface
    {
        private bool PosSpotMarg = false, PosFuture = false, PosSpotMargBuy = false, PosFutureBuy = false,
        PosFutureSell = false, PosSpotMargSell = false;
        private DateTime PosOpenTimeS { get; set; }
        private DateTime PosOpenTimeF { get; set; }
        #region Asset's Currency's balancy:
        private decimal AssetBalSpot { get; set; }
        private decimal CurrBalSpot { get; set; }
        private decimal AssetBalFuture { get; set; }
        private decimal CurrBalFuture { get; set; }
        #endregion

        private BinanceCryptoClient bcc = null;

        private List<List<string>> BidsLeg1 { get; set; }
        private List<List<string>> AsksLeg1 { get; set; }
        private List<List<string>> BidsLeg2 { get; set; }
        private List<List<string>> AsksLeg2 { get; set; }

        //public static BinanceCryptoClient bcc = null;
        //public static BinanceFutureClient bfc = null;

        private decimal PreGapBuy { get; set; }
        private decimal PreGapSell { get; set; }

        //private readonly DispatcherTimer timerEvent = null, timer1, timer2;

        Models.TradeModel model;
        ManualResetEvent threadStop;
        ManualResetEvent threadStopped;
        readonly object loglock = new object();

        public TradeOneLeg()
        {
            InitializeComponent();
            //isOpen=true;

            //timerEvent = new DispatcherTimer();
            //timer1 = new DispatcherTimer();
            //timer2 = new DispatcherTimer();
            //timerEvent.Tick += TimerEvent_Tick;
            //timerEvent.Interval = new TimeSpan(0, 0, 0, 0, 1000);

            //timer1 = new DispatcherTimer();
            //timer1.Tick += Timer1_Tick;
            //timer1.Interval = new TimeSpan(0, 0, 0, 0, 1000);

            //timer2 = new DispatcherTimer();
            //timer2.Tick += Timer2_Tick;
            //timer2.Interval = new TimeSpan(0, 0, 0, 0, 1000);

            //new BinanceFutureClient(this, threadStop, new BinanceFutureConnectionModel { Key = null, Secret = null, Name = "Futures", AccountTradeType = AccountTradeType.SPOT }).CallMarketDepth();
            //new BinanceCryptoClient(this, threadStop, new BinanceConnectionModel { Key = null, Secret = null, Name = "Spot", AccountTradeType = AccountTradeType.SPOT }).CallMarketDepth();
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {

        }

        private void Timer1_Tick(object sender, EventArgs e)
        {

        }

        private async void TimerEvent_Tick(object sender, EventArgs e)
        {
            //if (bcc != null)
            //{
            //if (BinanceCryptoClient.Balances != null)
            //{
            //    AssetBalSpot = CheckBalanceSpot(model.Spot.SymbolCurrency);
            //    CurrBalSpot = CheckBalanceSpot(model.Spot.SymbolAsset);
            //}

            //CurrBalSpot = await bcc.GetBalance(model.Leg1.SymbolCurrency);
            //AssetBalSpot = await bcc.GetBalance(model.Leg1.SymbolAsset);
            //if ((int)AssetBalSpot < 0) PosSpotMargSell = true;
            //else { PosSpotMargSell = false; }

            //if ((int)AssetBalSpot > 0) PosSpotMargBuy = true;
            //else PosSpotMargBuy = false;

            //CurrBalFuture = await bfc.GetBalance(model.Leg2.Symbol);
            //AssetBalFuture = await bfc.GetBalance(model.Leg2.Symbol);
            //if ((int)AssetBalFuture < 0) PosFutureSell = true;
            //else PosFutureSell = false;
            //if ((int)AssetBalFuture > 0) PosFutureBuy = true;
            //else PosFutureBuy = false;
            //}
        }

        public void InitializeTab()
        {
            model = DataContext as Models.TradeModel;
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

        public void RestoreNullCombo(ConnectionModel cm)
        {
            fast.RestoreNullCombo(cm);
            slow.RestoreNullCombo(cm);
        }

        #region Log's methods:
        private void LogOrderSuccess(string message)
        {
            Log(message, Colors.Green, Color.FromRgb(0, 255, 0));
        }
        private void LogInfo(string message)
        {
            Log(message, Colors.White, Color.FromRgb(0x00, 0x23, 0x44));
        }
        private void LogError(string message)
        {
            Log(message, Color.FromRgb(0xf3, 0x56, 0x51), Color.FromRgb(0xf3, 0x56, 0x51));
        }
        private void LogWarning(string message)
        {
            Log(message, Colors.LightBlue, Colors.Blue);
        }
        private void LogClear()
        {
            logBlock.Text = "";
        }
        private void Log(string _message, Color color, Color dashboardColor)
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

        #region Start & Stop:
        private void BuStart_Click(object sender, RoutedEventArgs e)
        {
            Start();
        }

        private void BuStop_Click(object sender, RoutedEventArgs e)
        {
            Stop(true);
        }

        public void Start()
        {
            if (model.Started) return;
            model.Started = true;
            model.FeederOk = false;
            LogClear();
            HiddenLogs.LogHeader(model);

            Models.TradeModel.currencySpot = model.Leg1.SymbolCurrency;
            Models.TradeModel.currencyFuture = model.Leg2.SymbolCurrency;
            Models.TradeModel.fullSymbolFuture = model.Leg2.Symbol;

            model.Leg1.Symbol = fast.AssetTb.Text + fast.CurrencyTb.Text;
            model.Leg2.Symbol = slow.AssetTb.Text + slow.CurrencyTb.Text;

            threadStop = new ManualResetEvent(false);
            threadStopped = new ManualResetEvent(false);
            new Thread(ThreadProc).Start();
            //timerEvent.Start();
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

        decimal gapBuy;
        decimal gapSell;
        IConnector spotConnector;
        IConnector futureConnector;
        string swDebugPath;
        string swQuotesPath;
        string swLogPath;
        System.IO.FileStream fsData;
        DateTime lastOpenCloseTime;
        string closeSignal;
        private string leg1Type, leg2Type;

        private string EscapePath(string path)
        {
            char[] invalid = System.IO.Path.GetInvalidPathChars();
            foreach (var c in invalid)
            {
                path = path.Replace(c, ' ');
            }
            return path;
        }

        DateTime lastClickOpenTime;
        DateTime lastClickCloseTime;
        decimal MaxGapBuyA = 0, MaxGapBuyB = 0;
        decimal MinGapSellA = 0, MinGapSellB = 0;
        private decimal avgGapBuy = 0;
        private decimal avgGapSell = 0;
        private decimal deviationBuy = 0;
        private decimal deviationSell = 0;
        private List<decimal> GapBuyArr = new List<decimal>();
        private List<decimal> GapSellArr = new List<decimal>();

        private void ThreadProc()
        {
            model.Leg1.InitView();
            model.Leg2.InitView();

            spotConnector = model.Leg1.CreateConnector(this, threadStop, model.SleepMs, Dispatcher, true);
            futureConnector = model.Leg2.CreateConnector(this, threadStop, model.SleepMs, Dispatcher);
            spotConnector.Tick += SpotConnector_Tick;
            futureConnector.Tick += FutureConnector_Tick;
            spotConnector.LoggedIn += SpotConnector_LoggedIn;
            futureConnector.LoggedIn += FutureConnector_LoggedIn;

            model.LogInfo(model.Title + " logging in...");
            while (!threadStop.WaitOne(100))
            {
                if (spotConnector.IsLoggedIn && futureConnector.IsLoggedIn)
                {
                    model.LogInfo(model.Title + " logged in OK.");
                    break;
                }
            }
            if (!threadStop.WaitOne(0))
            {
                if (spotConnector.IsLoggedIn)
                {
                    OnSpotLogin();
                }
                if (futureConnector.IsLoggedIn)
                {
                    OnFutureLogin();
                }
            }

            #region Log process:
            if (model.Log)
            {
                string stime = DateTime.Now.ToString("yyyyMMddHHmmss");
                string logfolder = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\.logs";
                logfolder = System.IO.Path.Combine(logfolder, EscapePath(model.Title));
                try
                {
                    System.IO.Directory.CreateDirectory(logfolder);
                }
                catch
                {

                }
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
                string datafolder = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\.data";
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


            lastOpenCloseTime = DateTime.UtcNow;

            while (!threadStop.WaitOne(model.SleepMs))
            {
                //  Here you can write arbitrage of strategy and algo^
                decimal GapBuy = 0;
                decimal GapSell = 0;

                if (model.Leg2.Bid != 0 && model.Leg1.Ask != 0)
                {
                    GapBuy = model.Leg2.Bid - model.Leg1.Ask;
                    GapSell = model.Leg2.Ask - model.Leg1.Bid;

                    if (GapBuy != PreGapBuy)
                    {
                        GapBuyArr.Add(GapBuy);
                        PreGapBuy = GapBuy;
                    }
                    if (GapSell != PreGapSell)
                    {
                        GapSellArr.Add(GapSell);
                    }

                    avgGapBuy = GetAvgGapBuy(GapBuyArr.Count);
                    avgGapSell = GetAvgGapSell(GapSellArr.Count);
                    deviationBuy = GapBuy - avgGapBuy;
                    deviationSell = GapSell - avgGapSell;

                    if (deviationBuy > MaxGapBuyA || MaxGapBuyA == 0) MaxGapBuyA = deviationBuy;
                    if (deviationSell < MinGapSellA || MinGapSellA == 0) MinGapSellA = deviationSell;

                    model.MaxGapBuyA = Math.Round(MaxGapBuyA, model.Open.Point);
                    model.MinGapSellA = Math.Round(MinGapSellA, model.Open.Point);
                }

                var spotLot = model.Leg1.Lot;
                var futureLot = model.Leg2.Lot;

                var spotLotStep = model.Leg1.LotStep;
                var futureLotStep = model.Leg2.LotStep;

                var gapForOpen = model.Open.GapForOpen;
                var gapForClose = model.Open.GapForClose;

                model.GapSell = GapSell;
                model.GapBuy = GapBuy;

                model.GapSell = GapSell;
                model.GapBuy = GapBuy;

                model.AvgGapSell = Math.Round(avgGapSell, model.Open.Point);
                model.AvgGapBuy = Math.Round(avgGapBuy, model.Open.Point);

                model.DeviationSell = Math.Round(deviationSell, model.Open.Point);
                model.DeviationBuy = Math.Round(deviationBuy, model.Open.Point);

                //***********************************************************

                if (GapBuy != 0m && GapSell != 0m)
                {
                    if (Math.Abs((int)AssetBalSpot) > 0) PosSpotMarg = true;
                    else PosSpotMarg = false;

                    if (Math.Abs((int)AssetBalFuture) > 0) PosFuture = true;
                    else PosFuture = false;

                    if (PosSpotMarg && PosFuture)
                    {
                        if (GapBuy >= gapForClose)
                        {
                            if (PosSpotMargSell)
                            {
                                if (model.AllowOpen)
                                {
                                    var start1 = DateTime.Now;
                                    if (OpenPos(model.Leg1.Symbol, "Spot", FillPolicy.FOK, OrderSide.Buy, GapBuy))
                                    {
                                        var end1 = DateTime.Now;
                                        PosOpenTimeS = end1;
                                        model.LogInfo($"Elapsted Close(Spot)[GapBuy >= gapForClose]: {(end1 - start1).TotalMilliseconds}ms");
                                        PosSpotMarg = false; PosSpotMargSell = false;
                                    }
                                    else
                                    {
                                        PosSpotMarg = true; PosSpotMargSell = true;
                                    }
                                    var start2 = DateTime.Now;

                                    if (OpenPos(model.Leg2.Symbol, "Future", FillPolicy.FOK, OrderSide.Sell, GapBuy))
                                    {
                                        var end2 = DateTime.Now;
                                        PosOpenTimeF = end2;
                                        model.LogInfo($"Elapsted Close(Future)[GapBuy >= gapForClose]: {(end2 - start2).TotalMilliseconds}ms");
                                        PosFutureBuy = false; PosFuture = false;
                                    }
                                    else
                                    {
                                        PosFutureBuy = true; PosFuture = true;
                                    }
                                }
                            }
                        }
                        else if (GapSell * -1 >= gapForClose)
                        {
                            if (PosSpotMargBuy)
                            {
                                if (model.AllowOpen)
                                {
                                    var start1 = DateTime.Now;
                                    if (OpenPos(model.Leg1.Symbol, "Spot", FillPolicy.FOK, OrderSide.Sell, GapSell))
                                    {
                                        var end1 = DateTime.Now;
                                        PosOpenTimeS = end1;
                                        model.LogInfo($"Elapsted Open(Spot)[GapSell * -1 >= gapForClose]: {(end1 - start1).TotalMilliseconds}ms");
                                        PosSpotMarg = false; PosSpotMargBuy = false;
                                    }
                                    else
                                    {
                                        PosSpotMarg = true; PosSpotMargBuy = true;
                                    }

                                    var start2 = DateTime.Now;

                                    if (OpenPos(model.Leg2.Symbol, "Future", FillPolicy.FOK, OrderSide.Buy, GapSell))
                                    {
                                        var end2 = DateTime.Now;
                                        PosOpenTimeF = end2;
                                        model.LogInfo($"Elapsted Open(Future)[GapSell * -1 >= gapForClose]: {(end2 - start2).TotalMilliseconds}ms");
                                        PosFutureSell = false; PosFuture = false;
                                    }
                                    else
                                    {
                                        PosFutureSell = true; PosFuture = true;
                                    }
                                }
                            }
                        }
                    }
                    else if (!PosSpotMarg && !PosFuture)
                    {
                        if (GapBuy >= gapForOpen)
                        {
                            if (model.AllowOpen)
                            {
                                var start1 = DateTime.Now;
                                if (OpenPos(model.Leg1.Symbol, "Spot", FillPolicy.FOK, OrderSide.Buy, GapBuy))
                                {
                                    var end1 = DateTime.Now;
                                    PosOpenTimeS = end1;
                                    model.LogInfo($"Elapsted Open(Spot)[!PosSpotMarg && !PosFuture]: {(end1 - start1).TotalMilliseconds}ms");
                                    PosSpotMarg = true; PosSpotMargBuy = true;
                                }
                                else
                                {
                                    PosSpotMarg = false; PosSpotMargBuy = false;
                                }

                                var start2 = DateTime.Now;

                                if (OpenPos(model.Leg2.Symbol, "Future", FillPolicy.FOK, OrderSide.Sell, GapBuy))
                                {
                                    var end2 = DateTime.Now;
                                    PosOpenTimeF = end2;
                                    model.LogInfo($"Elapsted Open(Future)[!PosSpotMarg && !PosFuture]: {(end2 - start2).TotalMilliseconds}ms");
                                    PosFutureSell = true; PosFuture = true;
                                }
                                else
                                {
                                    PosFutureSell = false; PosFuture = false;
                                }
                            }
                        }
                        else if (GapSell * -1 >= gapForOpen)
                        {
                            if (model.AllowOpen)
                            {
                                var start1 = DateTime.Now;
                                if (OpenPos(model.Leg1.Symbol, "Spot", FillPolicy.FOK, OrderSide.Sell, GapSell))
                                {
                                    var end1 = DateTime.Now;
                                    PosOpenTimeS = end1;
                                    model.LogInfo($"Elapsted Open(Spot)[GapSell * -1 >= gapForOpen]: {(end1 - start1).TotalMilliseconds}ms");
                                }
                                else
                                {
                                    PosSpotMargSell = false; PosSpotMarg = false;
                                }
                                var start2 = DateTime.Now;
                                if (OpenPos(model.Leg2.Symbol, "Future", FillPolicy.FOK, OrderSide.Buy, GapSell))
                                {
                                    var end2 = DateTime.Now;
                                    PosOpenTimeF = end2;
                                    model.LogInfo($"Elapsted Open(Future)[GapSell * -1 >= gapForOpen]: {(end2 - start2).TotalMilliseconds}ms");
                                    PosFutureBuy = true; PosFuture = true;

                                }
                                else
                                {
                                    PosFutureBuy = false; PosFuture = false;
                                }
                            }

                        }
                    }
                    else if (PosSpotMarg && !PosFuture)
                    {
                        var curT = DateTime.Now;
                        if ((curT - PosOpenTimeS).TotalSeconds > 10)
                        {
                            if (PosSpotMargBuy)
                            {
                                var start1 = DateTime.Now;
                                if (OpenPos(model.Leg1.Symbol, "Spot", FillPolicy.FOK, OrderSide.Sell, 0m))
                                {
                                    var end1 = DateTime.Now;
                                    PosOpenTimeS = end1;
                                    model.LogInfo($"Elapsted Close(Spot)[cleaning trade]: {(end1 - start1).TotalMilliseconds}ms");
                                    model.LogInfo(model.Leg1.Name + $"Spot Balance: {model.Leg1.Balance} USDT");
                                    PosSpotMarg = false; PosSpotMargBuy = false;
                                }
                                else
                                {
                                    PosSpotMarg = true; PosSpotMargBuy = true;
                                }
                            }
                            if (PosSpotMargSell)
                            {
                                var start1 = DateTime.Now;
                                if (OpenPos(model.Leg1.Symbol, "Spot", FillPolicy.FOK, OrderSide.Buy, 0m))
                                {
                                    var end1 = DateTime.Now;
                                    PosOpenTimeS = end1;
                                    model.LogInfo($"Elapsted Close(Spot)[Cleaning Trade]: {(end1 - start1).TotalMilliseconds}ms");
                                    model.LogInfo(model.Leg1.Name + $"Spot Balance: {model.Leg1.Balance} USDT");
                                    PosSpotMarg = false; PosSpotMargSell = false;
                                }
                                else
                                {
                                    PosSpotMarg = true; PosSpotMargSell = true;
                                }
                            }
                        }

                    }
                    else if (!PosSpotMarg && PosFuture)
                    {
                        var curT = DateTime.Now;
                        if ((curT - PosOpenTimeF).TotalSeconds > 10)
                        {
                            if (PosFutureBuy)
                            {
                                var start2 = DateTime.Now;

                                if (OpenPos(model.Leg2.Symbol, "Future", FillPolicy.FOK, OrderSide.Sell, 0m))
                                {
                                    var end2 = DateTime.Now;
                                    PosOpenTimeF = end2;
                                    model.LogInfo($" Elapsted Close(Future)[Cleaning Trade]: {(end2 - start2).TotalMilliseconds}ms");
                                    model.LogInfo(model.Leg1.Name + $"Future Balance: {model.Leg2.Balance} USDT");
                                    PosFutureBuy = false; PosFuture = false;
                                }
                                else
                                {
                                    PosFutureBuy = true; PosFuture = true;
                                }
                            }
                            else if (PosFutureSell)
                            {
                                var start2 = DateTime.Now;

                                if (OpenPos(model.Leg2.Symbol, "Future", FillPolicy.FOK, OrderSide.Buy, 0m))
                                {
                                    var end2 = DateTime.Now;
                                    PosOpenTimeF = end2;
                                    model.LogInfo($"Elapsted Close(Future)[Cleaning Trade]: {(end2 - start2).TotalMilliseconds}ms");
                                    model.LogInfo(model.Leg1.Name + $"Future Balance: {model.Leg2.Balance} USDT");
                                    PosFutureSell = false; PosFuture = false;
                                }
                                else
                                {
                                    PosFutureSell = true; PosFuture = true;
                                }
                            }

                        }
                    }
                }
                else
                {
                }
            }

            futureConnector.Tick -= FutureConnector_Tick;
            spotConnector.Tick -= SpotConnector_Tick;
            futureConnector.LoggedIn -= FutureConnector_LoggedIn;
            spotConnector.LoggedIn -= SpotConnector_LoggedIn;
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

        private decimal GetAvgGapBuy(int count)
        {
            if (count != 0)
            {
                decimal sum = 0;
                for (int i = 0; i < count; i++)
                    sum += GapBuyArr[i];
                return sum / count;
            }
            return 0.0m;
        }

        private decimal GetAvgGapSell(int count)
        {
            if (count != 0)
            {
                decimal sum = 0;
                for (int i = 0; i < count; i++)
                    sum += GapSellArr[i];
                return sum / count;
            }
            return 0.0m;
        }

        private string GetAskByIndex(int index, bool is1stLeg)
        {
            var Asks = is1stLeg ? AsksLeg1 : AsksLeg2;
            if (Asks != null)
                if (Asks.Count != 0 && index < 10)
                {
                    var foundAsk = Asks[index];
                    return $"{foundAsk[0]},{foundAsk[1]}";
                }
            return "0";
        }

        private string GetBidByIndex(int index, bool is1stLeg)
        {
            var Bids = is1stLeg ? BidsLeg1 : BidsLeg2;
            if (Bids != null)
                if (Bids.Count != 0 && index < 10)
                {
                    var foundBid = Bids[index];
                    return $"{foundBid[0]},{foundBid[1]}";
                }
            return "0";
        }


        internal decimal CheckBalanceMargin(string assetOrCurrency)
        {
            return bcc.MargBalances.Single(b => b.asset == assetOrCurrency).free;
        }

        internal decimal CheckBalanceSpot(string assetOrCurrency)
        {
            string bal = bcc.Balances.Single(b => b.Asset == assetOrCurrency).Free;
            if (bal != null)
                return decimal.Parse(bal, CultureInfo.InvariantCulture);
            else return 0.0m;
        }
                private bool OpenPos(string symb, string type, FillPolicy policy, OrderSide bs, decimal gap)
        {
            bool isSuccess = false, isSpot = false;
            OrderOpenResult result = null;
            if (type == "Spot")
            {
                isSpot = true;
                result = spotConnector.Open(symb, model.Leg1.Ask, model.Leg1.Lot, policy, bs, model.Magic, model.Slippage, 1,
                            model.Open.OrderType, model.Open.PendingLifeTimeMs);
            }
            else if (type == "Future")
            {
                isSpot = false;
                result = futureConnector.Open(symb, model.Leg2.Ask, model.Leg2.Lot, policy, bs, model.Magic, model.Slippage, 1,
                           model.Open.OrderType, model.Open.PendingLifeTimeMs);
            }
            // Check if Order was Successfully send:
            if (isSpot) // If result for the Spot/Margin:
            {
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
                    model.LogError(spotConnector.ViewId + " " + result.Error);
                    model.LogInfo($"[{type}]: {bs.ToString()} FAILED " + model.Leg1.Symbol + ";Gap="
                        +// ToStr2(GapBuy) + 
                        ";Price=" + model.FormatPrice(model.Leg1.Ask));
                    isSuccess = false;
                }
            }
            else // If result for the Future:
            {
                if (string.IsNullOrEmpty(result.Error))
                {
                    decimal slippage = -(result.OpenPrice - model.Leg2.Ask);
                    model.LogOrderSuccess($"{bs.ToString()} OK " + model.Leg2.Symbol + " at " + model.FormatPrice(result.OpenPrice) +
                        ";Gap=" + gap +
                        ";Price=" + model.FormatPrice(model.Leg2.Ask) + ";Slippage=" + ToStr1(slippage) +
                        ";Execution=" + ToStrMs(result.ExecutionTime) + " ms;");
                    isSuccess = true;
                }
                else
                {
                    model.LogError(futureConnector.ViewId + " " + result.Error);
                    model.LogInfo($"{bs.ToString()} FAILED " + model.Leg2.Symbol + ";Gap="
                        + gap +
                        ";Price=" + model.FormatPrice(model.Leg2.Ask));
                    isSuccess = false;
                }
            }

            return isSuccess;
        }

        void OnFutureLogin()
        {
            futureConnector.Fill = (FillPolicy)model.Open.Fill;
            futureConnector.Subscribe(model.Leg2.Symbol, model.Leg2.GetSymbolId(), leg2Type);
        }
        void OnSpotLogin()
        {
            spotConnector.Fill = (FillPolicy)model.Open.Fill;
            var symbId = model.Leg1.GetSymbolId();
            spotConnector.Subscribe(model.Leg1.Symbol, symbId, leg1Type);
        }

        private void FutureConnector_LoggedIn(object sender, EventArgs e)
        {
            OnFutureLogin();
        }

        private void SpotConnector_LoggedIn(object sender, EventArgs e)
        {
            OnSpotLogin();
        }

        void SaveData(byte[] data)
        {
            fsData.Write(data, 0, data.Length);
        }

        private ReaderWriterLockSlim lockSlim = new ReaderWriterLockSlim();
        // Method to modify marketDepthList (ensure proper synchronization)
        public TickEventArgs ModifyMarketDepthList(Dictionary<decimal, MarketDepthUpdateFuture> copy)
        {
            lockSlim.EnterWriteLock();
            try
            {
                var asks = copy
                .Where(kv => kv.Value.Type == 1)
                .OrderBy(s => s.Key).Select(a => a.Key).ToList();

                var bids = copy
                     .Where(kv => kv.Value.Type == 2)
                     .OrderByDescending(s => s.Key).Select(a => a.Key).ToList();

                return new TickEventArgs
                {
                    Ask = asks.First(),
                    Bid = bids.First(),
                    Symbol = "btcusd"
                };
            }
            finally
            {
                lockSlim.ExitWriteLock();
            }
        }

        private void FutureConnector_Tick(object sender, TickEventArgs e)
        {
            if (e.Symbol == model.Leg2.Symbol)
            {
                AsksLeg2 = e.Asks;
                BidsLeg2 = e.Bids;
                model.Leg2.Bid = e.Bid;
                model.Leg2.Ask = e.Ask;
                model.Leg2.Time = DateTime.Now;
            }
        }

        private void SpotConnector_Tick(object sender, TickEventArgs e)
        {
            if (e.Symbol == model.Leg1.Symbol)
            {
                model.Leg1.Bid = e.Bid;
                model.Leg1.Ask = e.Ask;
                model.Leg1.Time = DateTime.Now;
            }
        }

        int GetMs(DateTime begin0)
        {
            return (int)((DateTime.UtcNow - begin0).TotalMilliseconds);
        }
        string ToStr1(decimal value)
        {
            return value.ToString("F1", CultureInfo.InvariantCulture);
        }
        string ToStr2(decimal value)
        {
            return value.ToString("F2", CultureInfo.InvariantCulture);
        }
        string ToStrMs(TimeSpan span)
        {
            return span.TotalMilliseconds.ToString("F3", CultureInfo.InvariantCulture);
        }
        void IConnectorLogger.LogInfo(string msg)
        {
            LogInfo(msg);
        }

        void IConnectorLogger.LogError(string msg)
        {
            LogError(msg);
        }

        void IConnectorLogger.LogWarning(string msg)
        {
            LogWarning(msg);
        }
        void IConnectorLogger.LogOrderSuccess(string msg)
        {
            LogOrderSuccess(msg);
        }

        private void BuLoad_Click(object sender, RoutedEventArgs e)
        {
            Models.PresetModel.LoadDialog(model);
        }

        private void BuSave_Click(object sender, RoutedEventArgs e)
        {
            Models.PresetModel.SaveDialog(model);
        }
        private void LogClear_Click(object sender, RoutedEventArgs e)
        {
            LogClear();
        }

        private void TbOpenOrderType_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OrderTypeEditor dlg = new OrderTypeEditor(model.Open.OrderType, model.Open.PendingDistance, model.Open.PendingLifeTimeMs, model.Open.Fill)
            {
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            if (dlg.ShowDialog() == true)
            {
                model.Open.OrderType = dlg.OrderType;
                model.Open.Fill = dlg.Fill.Value;
                model.Open.PendingDistance = dlg.PendingDistance;
                model.Open.PendingLifeTimeMs = dlg.PendingLifeTime;
            }
        }

        private void TbCloseOrderType_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OrderTypeEditor dlg = new OrderTypeEditor(model.Close.OrderType, model.Close.PendingDistance, model.Close.PendingLifeTimeMs, null)
            {
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            if (dlg.ShowDialog() == true)
            {
                model.Close.OrderType = dlg.OrderType;
                model.Close.PendingDistance = dlg.PendingDistance;
                model.Close.PendingLifeTimeMs = dlg.PendingLifeTime;
            }
        }
    }
}

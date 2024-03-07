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

    public partial class TwoLegFutures : UserControl, IConnectorLogger, ITradeTabInterface
    {
        private bool PosSpotMarg = false, PosFuture = false, PosSpotMargBuy = false, PosFutureBuy = false,
        PosFutureSell = false, PosSpotMargSell = false;
        private DateTime PosOpenTimeS { get; set; }
        private DateTime PosOpenTimeF { get; set; }
        private DateTime StrtInterval;
        #region Asset's & Currency's balancy:
        private decimal AssetBalCoin_M { get; set; }
        private decimal CurrBalCoin_M { get; set; }
        private decimal AssetBalFuture { get; set; }
        private decimal CurrBalFuture { get; set; }
        #endregion

        private string leg1Type, leg2Type;

        private readonly DispatcherTimer timerEvent = null;

        private decimal PreGapBuy { get; set; }
        private decimal PreGapSell { get; set; }

        private Models.TradeModel model;
        private ManualResetEvent threadStop;
        private ManualResetEvent threadStopped;
        private readonly object loglock = new object();

        private BinanceFutureClient bfc = null;
        private BinanceCryptoClient bcc = null;

        public TwoLegFutures()
        {
            InitializeComponent();
            //isOpen=true;

            //timerEvent = new DispatcherTimer();
            //timerEvent.Tick += TimerEvent_Tick;
            //timerEvent.Interval = new TimeSpan(0, 0, 0, 0, 1000);

           // new BinanceFutureClient(this, threadStop, new BinanceFutureConnectionModel { Key = null, Secret = null, Name = "Futures", AccountTradeType = AccountTradeType.SPOT }).CallMarketDepth();
        }

        private async void TimerEvent_Tick(object sender, EventArgs e)
        {
            //if (USD_M != null && COIN_M != null)
            //{
            //    model.Leg1.Balance = CurrBalCoin_M = await COIN_M.GetBalance(model.Leg1.SymbolCurrency);
            //    model.Leg1.Balance = AssetBalCoin_M = await COIN_M.GetBalance(model.Leg1.SymbolAsset);
            //    //if ((int)AssetBalCoin_M < 0) PosSpotMargSell = true;
            //    //else { PosSpotMargSell = false; }

            //    //if ((int)AssetBalCoin_M > 0) PosSpotMargBuy = true;
            //    //else PosSpotMargBuy = false;

            //    CurrBalFuture = await USD_M.GetBalance(model.Leg2.Symbol);
            //    AssetBalFuture = await USD_M.GetBalance(model.Leg2.Symbol);
            //    //if ((int)AssetBalFuture < 0) PosFutureSell = true;
            //    //else PosFutureSell = false;
            //    //if ((int)AssetBalFuture > 0) PosFutureBuy = true;
            //    //else PosFutureBuy = false;
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

            Models.TradeModel.currencyFuture = model.Leg2.SymbolCurrency;
            Models.TradeModel.fullSymbolFuture = model.Leg2.Symbol;

            model.Leg1.Symbol = fast.AssetTb.Text + fast.CurrencyTb.Text;
            model.Leg2.Symbol = slow.AssetTb.Text + slow.CurrencyTb.Text;

            threadStop = new ManualResetEvent(false);
            threadStopped = new ManualResetEvent(false);
            new Thread(ThreadProc).Start();
            timerEvent.Start();
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
        IConnector coin_MConnector;
        IConnector usd_MConnector;
        string swDebugPath;
        string swQuotesPath;
        string swLogPath;
        System.IO.FileStream fsData;
        DateTime lastOpenCloseTime;
        string closeSignal;

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

        private decimal MaxGapBuyA = 0.0m;
        private decimal MinGapSellA = 0.00m;
        private decimal MaxGapBuyB = 0.0m;
        private decimal MinGapSellB = 0.0m;

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

            coin_MConnector = model.Leg1.CreateConnector(this, threadStop, model.SleepMs, Dispatcher);
            usd_MConnector = model.Leg2.CreateConnector(this, threadStop, model.SleepMs, Dispatcher);
            coin_MConnector.Tick += coin_MConnector_Tick;
            usd_MConnector.Tick += usd_MConnector_Tick;
            coin_MConnector.LoggedIn += SpotConnector_LoggedIn;
            usd_MConnector.LoggedIn += usd_MConnector_LoggedIn;

            model.LogInfo(model.Title + " logging in...");
            while (!threadStop.WaitOne(100))
            {
                if (coin_MConnector.IsLoggedIn && usd_MConnector.IsLoggedIn)
                {
                    model.LogInfo(model.Title + " logged in OK.");
                    break;
                }
            }
            if (!threadStop.WaitOne(0))
            {
                if (coin_MConnector.IsLoggedIn)
                {
                    OnSpotLogin();
                }
                if (usd_MConnector.IsLoggedIn)
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
                            model.LogInfo($"New Max DevBuy: {MaxGapBuyA} GapBuy:{GapBuy} avgGapBuy:{avgGapBuy} deviationBuy:{deviationBuy}");
                            model.LogInfo($"Ask1: {model.Leg1.Ask} Bid1:{model.Leg1.Bid} Ask2:{model.Leg2.Ask} Bid2:{model.Leg2.Bid}");
                            model.MaxGapBuyA = Math.Round(MaxGapBuyA, model.Open.Point);
                        }
                    }
                    if (GapSell != 0 && avgGapSell != 0)
                    {
                        deviationSell = GapSell - avgGapSell;
                        if (deviationSell < MinGapSellA || MinGapSellA == 0)
                        {
                            MinGapSellA = deviationSell;
                            model.LogInfo($"New Min DevSell: {MinGapSellA} GapSell:{GapSell} avgGapSell:{avgGapSell} deviationSell:{deviationSell}");
                            model.LogInfo($"Ask1: {model.Leg1.Ask} Bid1:{model.Leg1.Bid} Ask2:{model.Leg2.Ask} Bid2:{model.Leg2.Bid}");
                            model.MinGapSellA = Math.Round(MinGapSellA, model.Open.Point);
                        }
                    }

                  

                    if (deviationBuy > MaxGapBuyA || MaxGapBuyA == 0) MaxGapBuyA = deviationBuy;
                    if (deviationSell < MinGapSellA || MinGapSellA == 0) MinGapSellA = deviationSell;

                    model.MaxGapBuyA = Math.Round(MaxGapBuyA, model.Open.Point);
                    model.MinGapSellA = Math.Round(MinGapSellA, model.Open.Point);
                }


                // ...trying to batchOrders....
                //if (model.Leg2.Ask != 0 && model.Leg2.Bid != 0) 
                //BatchOrders(model.Leg2.Symbol, OrderSide.Buy, OrderType.Market, model.Leg2.Lot, model.Leg2.Bid);

                var spotLot = model.Leg1.Lot;
                var futureLot = model.Leg2.Lot;

                var spotLotStep = model.Leg1.LotStep;
                var futureLotStep = model.Leg2.LotStep;

                // OnView [for Future]:
                //if (USD_M.AccountInfo != null)
                //{
                    //model.Leg2.TotalInitMargin = decimal.Parse(USD_M.AccountInfo.TotalInitialMargin, CultureInfo.InvariantCulture);
                    //model.Leg2.AvailableBalance = CurrBalFuture = decimal.Parse(USD_M.AccountInfo.AvailableBalance, CultureInfo.InvariantCulture);
                    //model.Leg2.TotalCrossUnPnl = decimal.Parse(USD_M.AccountInfo.TotalCrossUnPnl, CultureInfo.InvariantCulture);
                    //model.Leg2.TotalMarginBalance = decimal.Parse(USD_M.AccountInfo.TotalMarginBalance, CultureInfo.InvariantCulture);
                    //model.Leg2.TotalCrossWalletBalance = decimal.Parse(USD_M.AccountInfo.TotalCrossWalletBalance, CultureInfo.InvariantCulture);
                    //// Position model
                    //var res = USD_M.AccountInfo.Positions.FirstOrDefault(s => s.Symbol == model.Leg2.Symbol);
                    //if (res != null)
                    //{
                    //    model.Leg2.EntryPrice = decimal.Parse(res.EntryPrice, CultureInfo.InvariantCulture);
                    //    model.Leg2.PositionAmt = AssetBalFuture = decimal.Parse(res.PositionAmt, CultureInfo.InvariantCulture);
                    //}
                
               
                    //// OnView [for Future COIN-M]:
                    //if (COIN_M.Coin_MAccInfo != null)
                    //{
                    //    var assetFound = COIN_M.Coin_MAccInfo.Assets.FirstOrDefault(a => a.AssetName == model.Leg1.SymbolAsset);
                    //    var postionFound = COIN_M.Coin_MAccInfo.Positions.FirstOrDefault(a => a.Symbol == model.Leg1.Symbol);
                    //    model.Leg1.PositionAmt = postionFound.PositionAmt;

                    //    model.Leg1.FeeTier = COIN_M.Coin_MAccInfo.FeeTier;

                    //    model.Leg1.WalletBalance = assetFound.WalletBalance;
                    //    model.Leg1.UnrealizedProfit = assetFound.UnrealizedProfit;
                    //    model.Leg1.PositionInitialMargin = assetFound.PositionInitialMargin;
                    //}
 
                    var gapForOpen = model.Open.GapForOpen;
                    var gapForClose = model.Open.GapForClose;

                    model.GapSell = GapSell;
                    model.GapBuy = GapBuy;

                    model.AvgGapSell = Math.Round(avgGapSell,model.Open.Point);
                    model.AvgGapBuy = Math.Round(avgGapBuy, model.Open.Point);

                    model.DeviationSell = Math.Round(deviationSell, model.Open.Point);
                    model.DeviationBuy = Math.Round(deviationBuy, model.Open.Point);

                //***********************************************************

                    if (GapBuy != 0m && GapSell != 0m)
                    {
                        //if (Math.Abs((int)AssetBalCoin_M) > 0) PosSpotMarg = true;
                        //else PosSpotMarg = false;

                       // if (Math.Abs((int)AssetBalFuture) > 0) PosFuture = true;
                       // else PosFuture = false;

                        if (PosSpotMarg && PosFuture)
                        {
                            if (GapBuy >= gapForClose)
                            {
                                if (PosSpotMargSell)
                                {
                                    if (model.AllowOpen)
                                    {
                                        var start1 = DateTime.Now;
                                        if (OpenPos(model.Leg1.Symbol, "Coin_M", FillPolicy.FOK, OrderSide.Buy, GapBuy))
                                        {
                                            var end1 = DateTime.Now;
                                            PosOpenTimeS = end1;
                                            model.LogInfo($"Elapsted Close(Future Coin-M)[GapBuy >= gapForClose]: {(end1 - start1).TotalMilliseconds}ms");
                                            model.LogInfo(model.Leg1.Name + $"Coin-M Balance: {model.Leg1.Balance} USDT");
                                            PosSpotMarg = false; PosSpotMargSell = false;
                                        }
                                        else
                                        {
                                            PosSpotMarg = true; PosSpotMargSell = true;
                                        }
                                        var start2 = DateTime.Now;

                                        if (OpenPos(model.Leg2.Symbol, "Usd_M", FillPolicy.FOK, OrderSide.Sell, GapBuy))
                                        {
                                            var end2 = DateTime.Now;
                                            PosOpenTimeF = end2;
                                            model.LogInfo($"Elapsted Close(Future Usd-M)[GapBuy >= gapForClose]: {(end2 - start2).TotalMilliseconds}ms");
                                            model.LogInfo(model.Leg1.Name + $"Usd-M Balance: {model.Leg1.Balance} USDT");
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
                                        if (OpenPos(model.Leg1.Symbol, "Coin_M", FillPolicy.FOK, OrderSide.Sell, GapSell))
                                        {
                                            var end1 = DateTime.Now;
                                            PosOpenTimeS = end1;
                                            model.LogInfo($"Elapsted Open(Future Coin-M)[GapSell * -1 >= gapForClose]: {(end1 - start1).TotalMilliseconds}ms");
                                            model.LogInfo(model.Leg1.Name + $"Coin-M Balance: {model.Leg1.Balance} USDT");
                                            PosSpotMarg = false; PosSpotMargBuy = false;
                                        }
                                        else
                                        {
                                            PosSpotMarg = true; PosSpotMargBuy = true;
                                        }

                                        var start2 = DateTime.Now;

                                        if (OpenPos(model.Leg2.Symbol, "Usd_M", FillPolicy.FOK, OrderSide.Buy, GapSell))
                                        {
                                            var end2 = DateTime.Now;
                                            PosOpenTimeF = end2;
                                            model.LogInfo($"Elapsted Open(Future Usd-M)[GapSell * -1 >= gapForClose]: {(end2 - start2).TotalMilliseconds}ms");
                                            model.LogInfo(model.Leg1.Name + $"Usd-M Balance: {model.Leg1.Balance} USDT");
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
                                    if (OpenPos(model.Leg1.Symbol, "Coin_M", FillPolicy.FOK, OrderSide.Buy, GapBuy))
                                    {
                                        var end1 = DateTime.Now;
                                        PosOpenTimeS = end1;
                                        model.LogInfo($"Elapsted Open(Future Coin-M)[!PosSpotMarg && !PosFuture]: {(end1 - start1).TotalMilliseconds}ms");
                                        model.LogInfo(model.Leg1.Name + $"Coin-M Balance: {model.Leg1.Balance} USDT");
                                        PosSpotMarg = true; PosSpotMargBuy = true;
                                    }
                                    else
                                    {
                                        PosSpotMarg = false; PosSpotMargBuy = false;
                                    }

                                    var start2 = DateTime.Now;

                                    if (OpenPos(model.Leg2.Symbol, "Usd-M", FillPolicy.FOK, OrderSide.Sell, GapBuy))
                                    {
                                        var end2 = DateTime.Now;
                                        PosOpenTimeF = end2;
                                        model.LogInfo($"Elapsted Open(Future Usd-M)[!PosSpotMarg && !PosFuture]: {(end2 - start2).TotalMilliseconds}ms");
                                        model.LogInfo(model.Leg1.Name + $"Usd-M Balance: {model.Leg1.Balance} USDT");
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
                                    if (OpenPos(model.Leg1.Symbol, "Coin_M", FillPolicy.FOK, OrderSide.Sell, GapSell))
                                    {
                                        var end1 = DateTime.Now;
                                        PosOpenTimeS = end1;
                                        model.LogInfo($"Elapsted Open(Future Coin-M)[GapSell * -1 >= gapForOpen]: {(end1 - start1).TotalMilliseconds}ms");
                                        model.LogInfo(model.Leg1.Name + $"Coin-M Balance: {model.Leg1.Balance} USDT");
                                    }
                                    else
                                    {
                                        PosSpotMargSell = false; PosSpotMarg = false;
                                    }
                                    var start2 = DateTime.Now;
                                    if (OpenPos(model.Leg2.Symbol, "Usd_M", FillPolicy.FOK, OrderSide.Buy, GapSell))
                                    {
                                        var end2 = DateTime.Now;
                                        PosOpenTimeF = end2;
                                        model.LogInfo($"Elapsted Open(Future Usd-M)[GapSell * -1 >= gapForOpen]: {(end2 - start2).TotalMilliseconds}ms");
                                        model.LogInfo(model.Leg1.Name + $"Usd-M Balance: {model.Leg1.Balance} USDT");
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
                                    if (OpenPos(model.Leg1.Symbol, "Coin_M", FillPolicy.FOK, OrderSide.Sell, 0m))
                                    {
                                        var end1 = DateTime.Now;
                                        PosOpenTimeS = end1;
                                        model.LogInfo($"Elapsted Close(Future Coin-M)[cleaning trade]: {(end1 - start1).TotalMilliseconds}ms");
                                        model.LogInfo(model.Leg1.Name + $"Coin-M Balance: {model.Leg1.Balance} USDT");
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
                                    if (OpenPos(model.Leg1.Symbol, "Coin_M", FillPolicy.FOK, OrderSide.Buy, 0m))
                                    {
                                        var end1 = DateTime.Now;
                                        PosOpenTimeS = end1;
                                        model.LogInfo($"Elapsted Close(Future Coin-M)[Cleaning Trade]: {(end1 - start1).TotalMilliseconds}ms");
                                        model.LogInfo(model.Leg1.Name + $"Coin-M Balance: {model.Leg1.Balance} USDT");
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

                                    if (OpenPos(model.Leg2.Symbol, "Usd_M", FillPolicy.FOK, OrderSide.Sell, 0m))
                                    {
                                        var end2 = DateTime.Now;
                                        PosOpenTimeF = end2;
                                        model.LogInfo($" Elapsted Close(Future Usd-M)[Cleaning Trade]: {(end2 - start2).TotalMilliseconds}ms");
                                        model.LogInfo(model.Leg1.Name + $"Usd-M Balance: {model.Leg1.Balance} USDT");
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

                                    if (OpenPos(model.Leg2.Symbol, "Usd_M", FillPolicy.FOK, OrderSide.Buy, 0m))
                                    {
                                        var end2 = DateTime.Now;
                                        PosOpenTimeF = end2;
                                        model.LogInfo($"Elapsted Close(Future Usd-M)[Cleaning Trade]: {(end2 - start2).TotalMilliseconds}ms");
                                        model.LogInfo(model.Leg1.Name + $"Usd-M Balance: {model.Leg1.Balance} USDT");
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
                

                //***********************************************************
                //}
            }     
            
            usd_MConnector.Tick -= usd_MConnector_Tick;
            coin_MConnector.Tick -= coin_MConnector_Tick;
            usd_MConnector.LoggedIn -= usd_MConnector_LoggedIn;
            coin_MConnector.LoggedIn -= SpotConnector_LoggedIn;
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

        //internal decimal CheckBalanceFuture(string assetOrCurrency)
        //{
        //    return decimal.Parse(USD_M.FutureBalances.FirstOrDefault(b => b.asset == assetOrCurrency).AvailableBalance
        //        , CultureInfo.InvariantCulture);
        //}

        //internal decimal GetPosAmt_Future(string symbol)
        //{
        //    return decimal.Parse(USD_M.AccountInfo.Positions.FirstOrDefault(b => b.Symbol == symbol).PositionAmt
        //        , CultureInfo.InvariantCulture);
        //}

        private void BatchOrders(string symb, OrderSide bs, OrderType ot, decimal lot, decimal price)
        {
           var result = (usd_MConnector as BinanceFutureClient).BatchOrder(symb, bs, ot, lot, price);
        }

        private bool OpenPos(string symb, string type, FillPolicy policy, OrderSide bs, decimal gap)
        {
            bool isSuccess = false, isCoin_M = false;
            OrderOpenResult result = null;
            if (type == "Coin_M")
            {
                isCoin_M = true;
                result = coin_MConnector.Open(symb, model.Leg1.Ask, model.Leg1.Lot, policy, bs, model.Magic, model.Slippage, 1,
                            model.Open.OrderType, model.Open.PendingLifeTimeMs);
            }
            else if (type == "Usd_M")
            {
                isCoin_M = false;
                result = usd_MConnector.Open(symb, model.Leg2.Ask, model.Leg2.Lot, policy, bs, model.Magic, model.Slippage, 1,
                           model.Open.OrderType, model.Open.PendingLifeTimeMs);
            }
            // Check if Order was Successfully send:
            if (isCoin_M) // If result for the Future Coin_M:
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
                    model.LogError(coin_MConnector.ViewId + " " + result.Error);
                    model.LogInfo($"[{type}]: {bs.ToString()} FAILED " + model.Leg1.Symbol + ";Gap="
                        +// ToStr2(GapBuy) + 
                        ";Price=" + model.FormatPrice(model.Leg1.Ask));
                    isSuccess = false;
                }
            }
            else // If result for the Future:
            {
                if (result == null || result != null)
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
                        model.LogError(usd_MConnector.ViewId + " " + result.Error);
                        model.LogInfo($"{bs.ToString()} FAILED " + model.Leg2.Symbol + ";Gap="
                            + gap +
                            ";Price=" + model.FormatPrice(model.Leg2.Ask));
                        isSuccess = false;
                    }
                }
            }

            return isSuccess;
        }
    
        void OnFutureLogin()
        {
            usd_MConnector.Fill = (FillPolicy)model.Open.Fill;
            usd_MConnector.Subscribe(model.Leg2.Symbol, model.Leg2.GetSymbolId(), leg2Type);
        }
        void OnSpotLogin()
        {
            coin_MConnector.Fill = (FillPolicy)model.Open.Fill;
            coin_MConnector.Subscribe(model.Leg1.Symbol, model.Leg1.GetSymbolId(), leg1Type);
        }

        private void usd_MConnector_LoggedIn(object sender, EventArgs e)
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

        private void usd_MConnector_Tick(object sender, TickEventArgs e)
        {
            if (e.Symbol == model.Leg2.Symbol)
            {
                if (model.Leg2.Time.Year > 2000)
                {
                    TimeSpan delta = DateTime.Now - model.Leg2.Time;
                    //model.Slow.AverageTimeBetweenTicks = averageSlowTimeBetweenTicks.Process((decimal)delta.TotalMilliseconds);
                    decimal tps = model.Leg2.GetTicksPerSecond();
                    if (tps > model.Leg2.MaxTPS) model.Leg2.MaxTPS = tps;
                }

                //var maketDepthViewResult = e.MarketDepthList;
                model.Leg2.Bid = e.Bid;
                model.Leg2.Ask = e.Ask;
                model.Leg2.Time = DateTime.Now;
            }
        }

        private void coin_MConnector_Tick(object sender, TickEventArgs e)
        {
            model.Leg1.Bid = e.Bid;
            model.Leg1.Ask = e.Ask;
            model.Leg1.Time = DateTime.Now;
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

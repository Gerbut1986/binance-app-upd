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

    public partial class TwoLegArbFutureUSD_M : UserControl, IConnectorLogger, ITradeTabInterface
    {
        private decimal gapBuy;
        private decimal gapSell;
        IConnector Leg1Connector;
        IConnector Leg2Connector;
        string swDebugPath;
        string swQuotesPath;
        string swLogPath;
        System.IO.FileStream fsData;

        private decimal MaxGapBuyA = 0, MinGapSellA = 0;
        private decimal MaxGapBuyB = 0.0m, MinGapSellB = 0.0m;
        private decimal avgGapBuy = 0;
        private decimal avgGapSell = 0;
        private decimal deviationBuy = 0;
        private decimal deviationSell = 0;
        private List<decimal> GapBuyArr = new List<decimal>();
        private List<decimal> GapSellArr = new List<decimal>();
        private decimal PreGapBuy { get; set; }
        private decimal PreGapSell { get; set; }

        private bool PosSpotMarg = false, PosFuture = false, PosSpotMargBuy = false, PosFutureBuy = false,
        PosFutureSell = false, PosSpotMargSell = false;
        private DateTime PosOpenTimeS { get; set; }
        private DateTime PosOpenTimeF { get; set; }
        #region Asset's & Currency's balancy:
        private decimal AssetBalCoin_M { get; set; }
        private decimal CurrBalCoin_M { get; set; }
        private decimal AssetBalFuture { get; set; }
        private decimal CurrBalFuture { get; set; }
        #endregion

        private string leg1Type, leg2Type;

        Models.TradeModel model;
        ManualResetEvent threadStop;
        ManualResetEvent threadStopped;
        readonly object loglock = new object();
        readonly object lockObj = new object();
        private DateTime StrtInterval;

        public BinanceCryptoClient smc = null; // Spot/Margin client
        public BinanceFutureClient fc = null;  // Future client

        private List<List<string>> BidsLeg1 { get; set; }
        private List<List<string>> AsksLeg1 { get; set; }
        private List<List<string>> BidsLeg2 { get; set; }
        private List<List<string>> AsksLeg2 { get; set; }

        public TwoLegArbFutureUSD_M()
        {
            InitializeComponent();
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

            FillOutputsLabelLeg1();
            FillOutputsLabelLeg2();
            //Models.TradeModel.currencyFuture = model.Leg2.SymbolCurrency;
            //Models.TradeModel.fullSymbolFuture = model.Leg2.Symbol;

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

        private string EscapePath(string path)
        {
            char[] invalid = System.IO.Path.GetInvalidPathChars();
            foreach (var c in invalid)
            {
                path = path.Replace(c, ' ');
            }
            return path;
        }

        private void ThreadProc()
        {
            model.Leg1.InitView();
            model.Leg2.InitView();

            Leg1Connector = model.Leg1.CreateConnector(this, threadStop, model.SleepMs, Dispatcher);
            Leg2Connector = model.Leg2.CreateConnector(this, threadStop, model.SleepMs, Dispatcher);
            Leg1Connector.Tick += Leg1Connector_Tick;
            Leg2Connector.Tick += Leg2Connector_Tick;
            Leg1Connector.LoggedIn += Leg1Connector_LoggedIn;
            Leg2Connector.LoggedIn += Leg2Connector_LoggedIn;

            model.LogInfo(model.Title + " logging in...");
            while (!threadStop.WaitOne(100))
            {
                if (Leg1Connector.IsLoggedIn && Leg2Connector.IsLoggedIn)
                {
                    model.LogInfo(model.Title + " logged in OK.");
                    break;
                }
            }
            if (!threadStop.WaitOne(0))
            {
                if (Leg1Connector.IsLoggedIn)
                {
                    On1LegLogin();
                }
                if (Leg2Connector.IsLoggedIn)
                {
                    On2LegLogin();
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

            while (!threadStop.WaitOne(model.SleepMs))
            {
                //  Here you can write arbitrage of strategy and algo^
                decimal GapBuy = 0;
                decimal GapSell = 0;

                bool asd = false;

                if (asd) model.Open.OrderType = OrderType.Limit;
                else model.Open.OrderType = OrderType.Market;

                if (model.Leg2.Bid != 0 && model.Leg1.Ask != 0 && model.Leg2.Ask != 0 && model.Leg1.Bid != 0)
                {
                    if (model.Leg2.Ask > model.Leg2.Bid && model.Leg1.Ask > model.Leg1.Bid)
                    {
                        GapBuy = model.Leg2.Bid - model.Leg1.Ask;
                        GapSell = model.Leg2.Ask - model.Leg1.Bid;
                    }
                    //if (model.Leg2.Ask == model.Leg1.Ask && model.Leg2.Bid == model.Leg1.Bid)
                    //{
                    //    //model.LogError($"Gap Error | model.Leg2.Ask == model.Leg1.Ask && model.Leg2.Bid == model.Leg1.Bid = {model.Leg2.Ask == model.Leg1.Ask && model.Leg2.Bid == model.Leg1.Bid}");
                    //    var askByIndexl1 = GetAskByIndex(0,true).Split(new char[] { ',' });
                    //    var bidByIndexl1 = GetBidByIndex(0,true).Split(new char[] { ',' });
                    //    var askByIndexl2 = GetAskByIndex(0,false).Split(new char[] { ',' });
                    //    var bidByIndexl2 = GetBidByIndex(0,false).Split(new char[] { ',' });
                    //}
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
                    var IntervalAlignment = model.IntervalA;
                    if ((DateTime.Now - StrtInterval).TotalSeconds > IntervalAlignment)
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
                }

                var spotLot = model.Leg1.Lot;
                var futureLot = model.Leg2.Lot;

                var spotLotStep = model.Leg1.LotStep;
                var futureLotStep = model.Leg2.LotStep;

                if (fc != null)
                {
                    FillAccInfoView1Leg();
                    FillAccInfoView2Leg();
                }

                var gapForOpen = model.Open.GapForOpen;
                var gapForClose = model.Open.GapForClose;

                model.GapSell = GapSell;
                model.GapBuy = GapBuy;

                model.AvgGapSell = Math.Round(avgGapSell, model.Open.Point);
                model.AvgGapBuy = Math.Round(avgGapBuy, model.Open.Point);

                model.DeviationSell = Math.Round(deviationSell, model.Open.Point);
                model.DeviationBuy = Math.Round(deviationBuy, model.Open.Point);

                //var askByIndex = GetAskByIndex(0).Split(new char[] { ',' });
                ////int sz=askByIndex.Length;
                //if (askByIndex[0] != "0" && askByIndex[1] != "0")
                //{
                //    var ask = decimal.Parse(askByIndex[0], CultureInfo.InvariantCulture);
                //    var askVol = decimal.Parse(askByIndex[1], CultureInfo.InvariantCulture);//?
                //} 


                //***********************************************************

                if (GapBuy != 0m && GapSell != 0m)
                {
                    //if (Math.Abs((int)AssetBalCoin_M) > 0) PosSpotMarg = true;
                    //else PosSpotMarg = false;

                    // if (Math.Abs((int)AssetBalFuture) > 0) PosFuture = true;
                    // else PosFuture = false;

                    if (PosSpotMarg && PosFuture)
                    {
                        if (deviationBuy >= gapForClose)
                        {
                            if (PosSpotMargSell)
                            {
                                if (model.AllowOpen)
                                {
                                    var start1 = DateTime.Now;
                                    model.LogInfo(model.Leg1.Name + $"Leg1 Try to Close pos Sell. ");

                                    if (OpenPos(model.Leg1.Symbol, "USD_M_1", FillPolicy.FOK, OrderSide.Buy, GapBuy))
                                    {
                                        var end1 = DateTime.Now;
                                        PosOpenTimeS = end1;
                                        model.LogInfo($"OK! Close Sell pos Leg 1, GapBuy: {deviationBuy}  Execution: {(end1 - start1).TotalMilliseconds}ms");
                                        model.LogInfo(model.Leg1.Name + $"USD-M_1 Balance: {model.Leg1.Balance} USDT");
                                        PosSpotMarg = false; PosSpotMargSell = false;
                                    }
                                    else
                                    {
                                        PosSpotMarg = true; PosSpotMargSell = true;
                                        model.LogInfo(model.Leg1.Name + $"Fail Close Leg1");
                                    }
                                    var start2 = DateTime.Now;
                                    model.LogInfo(model.Leg2.Name + $"Leg2 Try to Close pos Buy. ");

                                    if (OpenPos(model.Leg2.Symbol, "Usd_M_2", FillPolicy.FOK, OrderSide.Sell, GapBuy))
                                    {
                                        var end2 = DateTime.Now;
                                        PosOpenTimeF = end2;
                                        model.LogInfo($"OK! Close Buy pos Leg 2, GapBuy: {deviationBuy}  Execution: {(end2 - start2).TotalMilliseconds}ms");
                                        model.LogInfo(model.Leg2.Name + $"Usd_M_2 Balance: {model.Leg2.Balance} USDT");
                                        PosFutureBuy = false; PosFuture = false;
                                    }
                                    else
                                    {
                                        PosFutureBuy = true; PosFuture = true;
                                        model.LogInfo(model.Leg2.Name + $"Fail Close Leg2");
                                    }
                                }
                            }
                        }
                        else if (deviationSell * -1 >= gapForClose)
                        {
                            if (PosSpotMargBuy)
                            {
                                if (model.AllowOpen)
                                {
                                    var start1 = DateTime.Now;
                                    model.LogInfo(model.Leg1.Name + $"Leg1 Try to Close pos Buy. GapSell: {deviationSell} ");

                                    if (OpenPos(model.Leg1.Symbol, "Usd_M_1", FillPolicy.FOK, OrderSide.Sell, GapSell))
                                    {
                                        var end1 = DateTime.Now;
                                        PosOpenTimeS = end1;
                                        model.LogInfo($"OK! Close Buy pos Leg 1, Execution: {(end1 - start1).TotalMilliseconds}ms");
                                        model.LogInfo(model.Leg1.Name + $"Usd_M_1 Balance: {model.Leg1.Balance} USDT");
                                        PosSpotMarg = false; PosSpotMargBuy = false;
                                    }
                                    else
                                    {
                                        PosSpotMarg = true; PosSpotMargBuy = true;
                                        model.LogInfo(model.Leg1.Name + $"Fail Close Leg1");
                                    }

                                    var start2 = DateTime.Now;
                                    model.LogInfo(model.Leg2.Name + $"Leg2 Try to Close pos Sell. GapSell: {deviationSell} ");

                                    if (OpenPos(model.Leg2.Symbol, "Usd_M_2", FillPolicy.FOK, OrderSide.Buy, GapSell))
                                    {
                                        var end2 = DateTime.Now;
                                        PosOpenTimeF = end2;
                                        model.LogInfo($"OK! Close Sell pos Leg 2, Execution: {(end2 - start2).TotalMilliseconds}ms");
                                        model.LogInfo(model.Leg2.Name + $"Usd_M_2 Balance: {model.Leg2.Balance} USDT");
                                        PosFutureSell = false; PosFuture = false;
                                    }
                                    else
                                    {
                                        PosFutureSell = true; PosFuture = true;
                                        model.LogInfo(model.Leg2.Name + $"Fail Close Leg2");
                                    }
                                }
                            }
                        }
                    }
                    else if (!PosSpotMarg && !PosFuture)
                    {
                        if (deviationBuy >= gapForOpen)
                        {
                            if (model.AllowOpen)
                            {
                                var start1 = DateTime.Now;
                                model.LogInfo(model.Leg1.Name + $"Leg1 Try to Open pos Buy. GapBuy: {deviationBuy} ");

                                if (OpenPos(model.Leg1.Symbol, "Usd_M_1", FillPolicy.FOK, OrderSide.Buy, GapBuy))
                                {
                                    var end1 = DateTime.Now;
                                    PosOpenTimeS = end1;
                                    model.LogInfo($"OK! Open Buy pos Leg 1,  Execution: {(end1 - start1).TotalMilliseconds}ms");
                                    model.LogInfo(model.Leg1.Name + $"Usd_M_1 Balance: {model.Leg1.Balance} USDT");
                                    PosSpotMarg = true; PosSpotMargBuy = true;
                                }
                                else
                                {
                                    PosSpotMarg = false; PosSpotMargBuy = false;
                                    model.LogInfo(model.Leg1.Name + $"Fail Open Leg1");
                                }

                                var start2 = DateTime.Now;
                                model.LogInfo(model.Leg2.Name + $"Leg2 Try to Open pos Sell. ");

                                if (OpenPos(model.Leg2.Symbol, "Usd_M_2", FillPolicy.FOK, OrderSide.Sell, GapBuy))
                                {
                                    var end2 = DateTime.Now;
                                    PosOpenTimeF = end2;
                                    model.LogInfo($"OK! Open Buy pos Leg 2, Execution: {(end2 - start2).TotalMilliseconds}ms");
                                    model.LogInfo(model.Leg2.Name + $"Usd_M_2 Balance: {model.Leg2.Balance} USDT");
                                    PosFutureSell = true; PosFuture = true;
                                }
                                else
                                {
                                    PosFutureSell = false; PosFuture = false;
                                    model.LogInfo(model.Leg2.Name + $"Fail Open Leg2");
                                }
                            }
                        }
                        else if (deviationSell * -1 >= gapForOpen)
                        {
                            if (model.AllowOpen)
                            {
                                var start1 = DateTime.Now;
                                model.LogInfo(model.Leg1.Name + $"Leg1 Try to Open pos Sell, GapSell: {deviationSell}");

                                if (OpenPos(model.Leg1.Symbol, "Usd_M_1", FillPolicy.FOK, OrderSide.Sell, GapSell))
                                {
                                    var end1 = DateTime.Now;
                                    PosOpenTimeS = end1;
                                    model.LogInfo($"OK! Open Sell pos Leg 1,  Execution: {(end1 - start1).TotalMilliseconds}ms");
                                    model.LogInfo(model.Leg1.Name + $"Usd_M_1 Balance: {model.Leg1.Balance} USDT");
                                }
                                else
                                {
                                    PosSpotMargSell = false; PosSpotMarg = false;
                                    model.LogInfo(model.Leg1.Name + $"Fail Open Leg1");
                                }
                                var start2 = DateTime.Now;
                                model.LogInfo(model.Leg2.Name + $"Leg2 Try to Open pos Buy. ");

                                if (OpenPos(model.Leg2.Symbol, "Usd_M_2", FillPolicy.FOK, OrderSide.Buy, GapSell))
                                {
                                    var end2 = DateTime.Now;
                                    PosOpenTimeF = end2;
                                    model.LogInfo($"OK! Open Buy pos Leg 2, Execution: {(end2 - start2).TotalMilliseconds}ms");
                                    model.LogInfo(model.Leg2.Name + $"Usd_M_2 Balance: {model.Leg2.Balance} USDT");
                                    PosFutureBuy = true; PosFuture = true;

                                }
                                else
                                {
                                    PosFutureBuy = false; PosFuture = false;
                                    model.LogInfo(model.Leg2.Name + $"Fail Open Leg2");
                                }
                            }

                        }
                    }
                    else if (PosSpotMarg && !PosFuture)
                    {
                        var curT = DateTime.Now;
                        if ((curT - PosOpenTimeS).TotalSeconds > 20)
                        {
                            if (PosSpotMargBuy)
                            {
                                var start1 = DateTime.Now;
                                model.LogInfo(model.Leg1.Name + $"Leg1 Try to Close alone pos Buy. ");

                                if (OpenPos(model.Leg1.Symbol, "Usd_M_1", FillPolicy.FOK, OrderSide.Sell, 0m))
                                {
                                    var end1 = DateTime.Now;
                                    PosOpenTimeS = end1;
                                    model.LogInfo($"OK! [Cleaning Trade] Close Buy pos Leg 1, Execution: {(end1 - start1).TotalMilliseconds}ms");
                                    model.LogInfo(model.Leg1.Name + $"Usd_M_1 Balance: {model.Leg1.Balance} USDT");
                                    PosSpotMarg = false; PosSpotMargBuy = false;
                                }
                                else
                                {
                                    PosSpotMarg = true; PosSpotMargBuy = true;
                                    model.LogInfo(model.Leg1.Name + $"Fail Close Leg1");
                                }
                            }
                            if (PosSpotMargSell)
                            {
                                var start1 = DateTime.Now;
                                model.LogInfo(model.Leg1.Name + $"Leg1 Try to Close alone pos Sell. ");

                                if (OpenPos(model.Leg1.Symbol, "Usd_M_1", FillPolicy.FOK, OrderSide.Buy, 0m))
                                {
                                    var end1 = DateTime.Now;
                                    PosOpenTimeS = end1;
                                    model.LogInfo($"OK! [Cleaning Trade] Close Sell pos Leg 1, Execution: {(end1 - start1).TotalMilliseconds}ms");
                                    model.LogInfo(model.Leg1.Name + $"Usd_M_1 Balance: {model.Leg1.Balance} USDT");
                                    PosSpotMarg = false; PosSpotMargSell = false;
                                }
                                else
                                {
                                    PosSpotMarg = true; PosSpotMargSell = true;
                                    model.LogInfo(model.Leg2.Name + $"Fail Close Leg1");
                                }
                            }
                        }

                    }
                    else if (!PosSpotMarg && PosFuture)
                    {
                        var curT = DateTime.Now;
                        if ((curT - PosOpenTimeF).TotalSeconds > 20)
                        {
                            if (PosFutureBuy)
                            {
                                var start2 = DateTime.Now;
                                model.LogInfo(model.Leg2.Name + $"Leg2 Try to Close alone position Buy. ");

                                if (OpenPos(model.Leg2.Symbol, "Usd_M_2", FillPolicy.FOK, OrderSide.Sell, 0m))
                                {
                                    var end2 = DateTime.Now;
                                    PosOpenTimeF = end2;
                                    model.LogInfo($"OK! [Cleaning Trade] Close Buy pos Leg 2, Execution: {(end2 - start2).TotalMilliseconds}ms");
                                    model.LogInfo(model.Leg2.Name + $"Usd-M Balance: {model.Leg2.Balance} USDT");
                                    PosFutureBuy = false; PosFuture = false;
                                }
                                else
                                {
                                    PosFutureBuy = true; PosFuture = true;
                                    model.LogInfo(model.Leg2.Name + $"Fail Close Leg2");
                                }
                            }
                            else if (PosFutureSell)
                            {
                                var start2 = DateTime.Now;
                                model.LogInfo(model.Leg2.Name + $"Leg2 Try to Close alone position Sell. ");

                                if (OpenPos(model.Leg2.Symbol, "Usd_M_2", FillPolicy.FOK, OrderSide.Buy, 0m))
                                {
                                    var end2 = DateTime.Now;
                                    PosOpenTimeF = end2;
                                    model.LogInfo($"OK! [Cleaning Trade] Close Sell pos Leg 2, Execution: {(end2 - start2).TotalMilliseconds}ms");
                                    model.LogInfo(model.Leg2.Name + $"Usd-M Balance: {model.Leg2.Balance} USDT");
                                    PosFutureSell = false; PosFuture = false;
                                }
                                else
                                {
                                    PosFutureSell = true; PosFuture = true;
                                    model.LogInfo(model.Leg2.Name + $"Fail Close Leg2");
                                }
                            }

                        }
                    }
                }
                else
                {
                }
                //***********************************************************
                // }
            }

            Leg2Connector.Tick -= Leg2Connector_Tick;
            Leg1Connector.Tick -= Leg1Connector_Tick;
            Leg2Connector.LoggedIn -= Leg2Connector_LoggedIn;
            Leg1Connector.LoggedIn -= Leg1Connector_LoggedIn;
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
            int startIdx = Math.Max(0, GapBuyArr.Count - count);
            if (GapBuyArr.Count > count)
            {
                decimal sum = 0;
                for (int i = startIdx; i < GapBuyArr.Count; i++)
                    sum += GapBuyArr[i];
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
            if (count > 0 && GapSellArr.Count > count)
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

        private bool OpenPos(string symb, string type, FillPolicy policy, OrderSide bs, decimal gap)
        {
            bool isSuccess = false, isCoin_M = false;
            OrderOpenResult result = null;
            if (type == "Usd_M_1")
            {
                isCoin_M = true;
                result = Leg1Connector.Open(symb, model.Leg1.Ask, model.Leg1.Lot, policy, bs, model.Magic, model.Slippage, 1,
                            model.Open.OrderType, model.Open.PendingLifeTimeMs);
            }
            else if (type == "Usd_M_2")
            {
                isCoin_M = false;
                result = Leg2Connector.Open(symb, model.Leg2.Ask, model.Leg2.Lot, policy, bs, model.Magic, model.Slippage, 1,
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
                    model.LogError(Leg1Connector.ViewId + " " + result.Error);
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
                        model.LogError(Leg2Connector.ViewId + " " + result.Error);
                        model.LogInfo($"{bs.ToString()} FAILED " + model.Leg2.Symbol + ";Gap="
                            + gap +
                            ";Price=" + model.FormatPrice(model.Leg2.Ask));
                        isSuccess = false;
                    }
                }
            }

            return isSuccess;
        }

        void On2LegLogin()
        {
            Leg2Connector.Fill = (FillPolicy)model.Open.Fill;
            Leg2Connector.Subscribe(model.Leg2.Symbol, model.Leg2.Symbol, leg2Type);
        }
        void On1LegLogin()
        {
            Leg1Connector.Fill = (FillPolicy)model.Open.Fill;
            Leg1Connector.Subscribe(model.Leg1.Symbol, model.Leg1.Symbol, leg1Type);
        }

        private void Leg2Connector_LoggedIn(object sender, EventArgs e)
        {
            On2LegLogin();
        }

        private void Leg1Connector_LoggedIn(object sender, EventArgs e)
        {
            On1LegLogin();
        }

        private void Leg2Connector_Tick(object sender, TickEventArgs e)
        {
            if (model.Leg2.Symbol == e.Symbol)
            {
                AsksLeg2 = e.Asks;
                BidsLeg2 = e.Bids;
                //model.Leg2.Bid = decimal.Parse(GetBidByIndex(0).Split(new char[] { ',' })[0], CultureInfo.InvariantCulture);
                //model.Leg2.Ask = decimal.Parse(GetAskByIndex(0).Split(new char[] { ',' })[0], CultureInfo.InvariantCulture);
                model.Leg2.Bid = e.Bid;
                model.Leg2.Ask = e.Ask;
                model.Leg2.Time = DateTime.Now;
                var symb = e.Symbol;
            }
        }

        private void Leg1Connector_Tick(object sender, TickEventArgs e)
        {
            if (model.Leg1.Symbol == e.Symbol)
            {
                AsksLeg1 = e.Asks;
                BidsLeg1 = e.Bids;
                //model.Leg1.Bid = decimal.Parse(GetBidByIndex(0).Split(new char[] { ',' })[0], CultureInfo.InvariantCulture);
                //model.Leg1.Ask = decimal.Parse(GetAskByIndex(0).Split(new char[] { ',' })[0], CultureInfo.InvariantCulture);
                model.Leg1.Bid = e.Bid;
                model.Leg1.Ask = e.Ask;
                model.Leg1.Time = DateTime.Now;
                var symb = e.Symbol;
            }
        }

        string ToStr1(decimal value)
        {
            return value.ToString("F1", CultureInfo.InvariantCulture);
        }

        string ToStrMs(TimeSpan span)
        {
            return span.TotalMilliseconds.ToString("F3", CultureInfo.InvariantCulture);
        }

        void IConnectorLogger.LogInfo(string msg)
        {
            LogInfo(msg);
        }

        private void IntegerTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

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

        private void FillAccInfoView1Leg()
        {
            switch (leg1Type)
            {
                case "USD_M":
                    // OnView [for Future USD-M[1]]:
                    if (fc.AccountInfo != null)
                    {
                        model.Leg1.TotalInitMargin = decimal.Parse(fc.AccountInfo.TotalInitialMargin, CultureInfo.InvariantCulture);
                        model.Leg1.AvailableBalance = CurrBalFuture = decimal.Parse(fc.AccountInfo.AvailableBalance, CultureInfo.InvariantCulture);
                        model.Leg1.TotalCrossUnPnl = decimal.Parse(fc.AccountInfo.TotalCrossUnPnl, CultureInfo.InvariantCulture);
                        model.Leg1.TotalMarginBalance = decimal.Parse(fc.AccountInfo.TotalMarginBalance, CultureInfo.InvariantCulture);
                        model.Leg1.TotalCrossWalletBalance = decimal.Parse(fc.AccountInfo.TotalCrossWalletBalance, CultureInfo.InvariantCulture);                    // Position model
                        var res = fc.AccountInfo.Positions.FirstOrDefault(s => s.Symbol == model.Leg1.Symbol);
                        if (res != null)
                        {
                            model.Leg1.EntryPrice = decimal.Parse(res.EntryPrice, CultureInfo.InvariantCulture);
                            model.Leg1.PositionAmt = AssetBalFuture = decimal.Parse(res.PositionAmt, CultureInfo.InvariantCulture);
                        }
                    }
                    break;
                case "MARGIN":                   
                    // OnView [for Spot/Margin]:
                    if (smc.MarginAccount != null)
                    {
                        var assetFound = smc.MarginAccount.userAssets.FirstOrDefault(a => a.asset == model.Leg1.SymbolAsset);
                
                        model.Leg1.TotalInitMargin = decimal.Parse(smc.MarginAccount.collateralMarginLevel, CultureInfo.InvariantCulture);
                        model.Leg1.AvailableBalance = decimal.Parse(smc.MarginAccount.marginLevel, CultureInfo.InvariantCulture);
                        model.Leg1.TotalCrossUnPnl = decimal.Parse(smc.MarginAccount.totalCollateralValueInUSDT, CultureInfo.InvariantCulture);
                        model.Leg1.TotalMarginBalance = assetFound.borrowed;
                        model.Leg1.TotalCrossWalletBalance = assetFound.free;
                        model.Leg1.EntryPrice = assetFound.interest;
                        //model.Leg1.Locked = assetFound.locked;
                        model.Leg1.PositionAmt = assetFound.netAsset;
                    }
                    break;
                case "SPOT":

                    break;
                default: break;
            }
        }

        private void FillAccInfoView2Leg()
        {
            switch (leg1Type)
            {
                case "USD_M":
                    if (fc.AccountInfo != null)
                    {
                        // OnView [for Future USD-M[2]]:
                        model.Leg2.TotalInitMargin = decimal.Parse(fc.AccountInfo.TotalInitialMargin, CultureInfo.InvariantCulture);
                        model.Leg2.AvailableBalance = CurrBalFuture = decimal.Parse(fc.AccountInfo.AvailableBalance, CultureInfo.InvariantCulture);
                        model.Leg2.TotalCrossUnPnl = decimal.Parse(fc.AccountInfo.TotalCrossUnPnl, CultureInfo.InvariantCulture);
                        model.Leg2.TotalMarginBalance = decimal.Parse(fc.AccountInfo.TotalMarginBalance, CultureInfo.InvariantCulture);
                        model.Leg2.TotalCrossWalletBalance = decimal.Parse(fc.AccountInfo.TotalCrossWalletBalance, CultureInfo.InvariantCulture);
                        // Position model
                        var res = fc.AccountInfo.Positions.FirstOrDefault(s => s.Symbol == model.Leg1.Symbol);
                        if (res != null)
                        {
                            model.Leg2.EntryPrice = decimal.Parse(res.EntryPrice, CultureInfo.InvariantCulture);
                            model.Leg2.PositionAmt = AssetBalFuture = decimal.Parse(res.PositionAmt, CultureInfo.InvariantCulture);
                        }
                    }
                    break;
                case "MARGIN":
                    // OnView [for Spot/Margin]:
                    if (smc.MarginAccount != null)
                    {
                        var assetFound = smc.MarginAccount.userAssets.FirstOrDefault(a => a.asset == model.Leg1.SymbolAsset);
                        model.Leg2.TotalInitMargin = decimal.Parse(smc.MarginAccount.collateralMarginLevel, CultureInfo.InvariantCulture);
                        model.Leg2.AvailableBalance = decimal.Parse(smc.MarginAccount.marginLevel, CultureInfo.InvariantCulture);
                        model.Leg2.TotalCrossUnPnl = decimal.Parse(smc.MarginAccount.totalCollateralValueInUSDT, CultureInfo.InvariantCulture);
                        model.Leg2.TotalMarginBalance = assetFound.borrowed;
                        model.Leg2.TotalCrossWalletBalance = assetFound.free;
                        model.Leg2.EntryPrice = assetFound.interest;
                        //model.Leg1.Locked = assetFound.locked;
                        model.Leg2.PositionAmt = assetFound.netAsset;
                    }
                    break;
                case "SPOT":

                    break;
                default: break;
            }
        }

        private void FillOutputsLabelLeg1()
        {
            switch (leg1Type)
            {
                case "USD_M":
                    lock (lockObj)
                    {
                        outp1leg1.Text = "Total Init Margin";
                        outp1leg2.Text = "Available Balance";
                        outp1leg3.Text = "Total Cross UnPnl";
                        outp1leg4.Text = "Total Margin Balance";
                        outp1leg5.Text = "TotalCrossWalletBalance";
                        outp1leg6.Text = "Entry Price";
                        outp1leg7.Text = "Position Amt";
                    }
                    break;
                case "MARGIN":
                    lock (lockObj)
                    {
                        outp1leg1.Text = "Collateral Margin Level";
                        outp1leg2.Text = "Margin Level";
                        outp1leg3.Text = "TotalCollateralValueInUSDT";
                        outp1leg4.Text = "Borrowed";
                        outp1leg5.Text = "Free";
                        outp1leg6.Text = "Interest";
                        outp1leg7.Text = "Net Asset";
                    }
                    break;
            }
        }

        private void FillOutputsLabelLeg2()
        {
            switch (leg2Type)
            {
                case "USD_M":
                    lock (lockObj)
                    {
                        outp2leg1.Text = "Total Init Margin";
                        outp2leg2.Text = "Available Balance";
                        outp2leg3.Text = "Total Cross UnPnl";
                        outp2leg4.Text = "Total Margin Balance";
                        outp2leg5.Text = "TotalCrossWalletBalance";
                        outp2leg6.Text = "Entry Price";
                        outp2leg7.Text = "Position Amt";
                    }
                    break;
                case "MARGIN":
                    lock (lockObj)
                    {
                        outp2leg1.Text = "Collateral Margin Level";
                        outp2leg2.Text = "Margin Level";
                        outp2leg3.Text = "TotalCollateralValueInUSDT";
                        outp2leg4.Text = "Borrowed";
                        outp2leg5.Text = "Free";
                        outp2leg6.Text = "Interest";
                        outp2leg7.Text = "Net Asset";
                    }
                    break;
            }
        }
    }
}


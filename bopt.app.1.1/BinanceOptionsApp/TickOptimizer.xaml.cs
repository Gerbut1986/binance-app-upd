using MultiTerminal.Connections.Models;
using MultiTerminal.Connections;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using VisualMarketsEngine;
using System.Collections.Generic;

namespace BinanceOptionsApp
{
    public partial class TickOptimizer : UserControl, IConnectorLogger, ITradeTabInterface
    {
        private string leg1Type, leg2Type;
        Models.TradeModel model;
        ManualResetEvent threadStop;
        ManualResetEvent threadStopped;
        readonly object loglock = new object();
        private List<decimal> GapBuyArr = new List<decimal>();
        private List<decimal> GapSellArr = new List<decimal>();

        private List<List<string>> BidsLeg1 { get; set; }
        private List<List<string>> AsksLeg1 { get; set; }
        private List<List<string>> BidsLeg2 { get; set; }
        private List<List<string>> AsksLeg2 { get; set; }

        private decimal PreGapBuy { get; set; }
        private decimal PreGapSell { get; set; }

        private decimal MaxGapBuy = 0;
        private decimal MinGapSell = 0;
        private decimal avgGapBuy = 0;
        private decimal avgGapSell = 0;

        private decimal deviationBuy = 0;
        private decimal deviationSell = 0;


        public TickOptimizer()
        {
            InitializeComponent();
        }

        public void InitializeTab()
        {
            model = DataContext as Models.TradeModel;
            fast.InitializeProviderControl( model.Leg1, true);
            slow.InitializeProviderControl(model.Leg2, false);

            var spt1 = model.Leg1.Name.Split(new char[] { '[', ']' });
            var spt2 = model.Leg2.Name.Split(new char[] { '[', ']' });
            leg1Type = spt1[1];
            leg2Type = spt2[1];

            model.LogError = LogError;
            model.LogInfo = LogInfo;
            model.LogWarning = LogWarning;
            model.LogClear = LogClear;
            HiddenLogs.LogHeader(model);
            //tbFreezeTime.IsEnabled = Model.UseFreezeTime;
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
        void IConnectorLogger.LogOrderSuccess(string msg)
        {
            LogOrderSuccess(msg);
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

            model.Leg1.Symbol = fast.AssetTb.Text + fast.CurrencyTb.Text;
            model.Leg2.Symbol = slow.AssetTb.Text + slow.CurrencyTb.Text;

            ClearChart();
            threadStop = new ManualResetEvent(false);
            threadStopped = new ManualResetEvent(false);
            new Thread(ThreadProc).Start();
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

        decimal prevSlowBid;
        decimal prevSlowAsk;
        decimal prevFastBid;
        decimal prevFastAsk;

        Helpers.MovingAverage averageBid;
        Helpers.MovingAverage averageAsk;
        Helpers.MovingAverage averageSpread;
        Helpers.MovingAverage averageFastChange;
        Helpers.MovingAverage averageSlowChange;
        Helpers.MovingAverage averageSlowTimeBetweenTicks;
        Helpers.MovingAverage averageFastTimeBetweenTicks;
        Helpers.Ring fastBidRing;
        Helpers.Ring fastAskRing;

        decimal gapBuy;
        decimal gapSell;
        IConnector leg1Connector;
        IConnector leg2Connector;
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

        DoubleTimeValue[] chartValues;
        ChartGroup.InternalChart chartPrice;
        ChartGroup.InternalChart chartGap;
        int chartFastBidFlow;
        int chartSlowBidFlow;
        int chartFastAskFlow;
        int chartSlowAskFlow;
        int chartGapBuyFlow;
        int chartGapSellFlow;

        DateTime lastClickOpenTime;
        DateTime lastClickCloseTime;

        public void ClearChart()
        {
            chartGroup.Clear();
            chartGroup.Settings.FirstShowMaximumBars = 150;
            chartGroup.Settings.ShowLegend = false;
            chartGroup.Settings.ShiftOnUpdate = true;
            //chartGroup.Settings.ScaleDateFormat2 = "HH:mm:ss.ffffff";
            chartGroup.Settings.Foreground = Color.FromRgb(0xff, 0xff, 0xff);
            chartGroup.Settings.Background = Color.FromRgb(0x00, 0x23, 0x44);
            chartGroup.Settings.NavigatorBackground = Color.FromRgb(0x00, 0x6d, 0xc5);
            chartGroup.Settings.NavigatorThumb = Color.FromRgb(0x20, 0x8d, 0xe5);
            chartGroup.Settings.TransparentPanel = Color.FromRgb(0x00, 0x23, 0x44);
            chartGroup.Settings.ChartBackground = Color.FromRgb(0x52, 0xbb, 0xea);
            chartGroup.Settings.GridPen = Color.FromArgb(50, 0x00, 0x23, 0x44);

            chartValues = new DoubleTimeValue[1];
            if (model.AllowView)
            {
                chartPrice = chartGroup.CreateChart("Price", true, 1.0, model.Digits);
                //chartFastBidFlow = chartPrice.AddDoubleTimeFlow("FastBid", TimeFrame.Tick, Color.FromRgb(0xbf, 0xd7, 0x30), 2, false);
                chartFastBidFlow = chartPrice.AddDoubleTimeFlow("FastBid", TimeFrame.Tick, Color.FromRgb(0, 0, 250), 2, false);
                //chartSlowBidFlow = chartPrice.AddDoubleTimeFlow("SlowBid", TimeFrame.Tick, Color.FromArgb(100, 0xbf, 0xd7, 0x30), 3, false);
                chartSlowBidFlow = chartPrice.AddDoubleTimeFlow("SlowBid", TimeFrame.Tick, Color.FromRgb(0, 0, 250), 3, false);
                //chartFastAskFlow = chartPrice.AddDoubleTimeFlow("FastAsk", TimeFrame.Tick, Color.FromRgb(0xd4, 0x66, 0x00), 2, false);
                chartFastAskFlow = chartPrice.AddDoubleTimeFlow("FastAsk", TimeFrame.Tick, Color.FromRgb(250, 0, 0), 2, false);
                //chartSlowAskFlow = chartPrice.AddDoubleTimeFlow("SlowAsk", TimeFrame.Tick, Color.FromArgb(100, 0xd4, 0xd7, 0x30), 3, false);
                chartSlowAskFlow = chartPrice.AddDoubleTimeFlow("SlowAsk", TimeFrame.Tick, Color.FromRgb(250, 0, 0), 3, false);

                chartGap = chartGroup.CreateChart("Gap", true, 1.0, model.Digits);
                chartGapBuyFlow = chartGap.AddDoubleTimeFlow("GapBuy", TimeFrame.Tick, Color.FromRgb(0, 255, 0), 2, false);
                chartGapSellFlow = chartGap.AddDoubleTimeFlow("GapSell", TimeFrame.Tick, Color.FromRgb(250, 0, 0), 2, false);
            }
        }

        private void ThreadProc()
        {
            decimal? stopBalance = null;
            decimal? trailSl = null;
            bool balanceOk = true;

            model.Leg1.InitView();
            model.Leg2.InitView();
            model.GapBuy = 0;
            model.GapSell = 0;

            prevSlowAsk = 0;
            prevSlowBid = 0;
            prevFastAsk = 0;
            prevFastBid = 0;

            fastBidRing = new Helpers.Ring(model.Open.GapFastTicks + 1);
            fastAskRing = new Helpers.Ring(model.Open.GapFastTicks + 1);
            averageBid = new Helpers.MovingAverage(model.Open.AvtoShiftPeriod);
            averageAsk = new Helpers.MovingAverage(model.Open.AvtoShiftPeriod);
            averageSpread = new Helpers.MovingAverage(model.Open.AvtoShiftPeriod);
            averageFastChange = new Helpers.MovingAverage(model.Open.FastCoefPeriod);
            averageSlowChange = new Helpers.MovingAverage(model.Open.FastCoefPeriod);
            averageSlowTimeBetweenTicks = new Helpers.MovingAverage(100);
            averageFastTimeBetweenTicks = new Helpers.MovingAverage(100);
            gapSell = 0;
            gapBuy = 0;
            lastClickCloseTime = DateTime.UtcNow.AddDays(-1);
            lastClickOpenTime = DateTime.UtcNow.AddDays(-1);

            leg1Connector =  model.Leg1.CreateConnector(this, threadStop, model.SleepMs, Dispatcher);
            leg2Connector =  model.Leg2.CreateConnector(this, threadStop, model.SleepMs, Dispatcher);
            leg1Connector.Tick += Leg1Connector_Tick;
            leg2Connector.Tick += Leg2Connector_Tick;
            leg1Connector.LoggedIn += Leg1Connector_LoggedIn;
            leg2Connector.LoggedIn += Leg2Connector_LoggedIn;

            model.Leg1.Symbol = model.Leg1.SymbolAsset + model.Leg1.SymbolCurrency;
            model.Leg2.Symbol = model.Leg2.SymbolAsset + model.Leg2.SymbolCurrency;

            model.LogInfo(model.Title + " logging in...");
            while (!threadStop.WaitOne(100))
            {
                if (leg1Connector.IsLoggedIn && leg2Connector.IsLoggedIn)
                {
                    model.LogInfo(model.Title + " logged in OK.");
                    break;
                }
            }
            if (!threadStop.WaitOne(0))
            {
                if (leg1Connector.IsLoggedIn)
                {
                    OnLeg2Login();
                }
                if (leg2Connector.IsLoggedIn)
                {
                    OnLeg1Login();
                }
            }
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
                System.IO.File.AppendAllText(swDebugPath, "GapBuy;GapSell;FastCoef;Position;AvBid;AvAsk;Spread;SpreadK;MinLev;MinLevClose;FixTp;FixSl;TrailSl;FastSpread;dTFast;tpsFast;dTSlow;tpsSlow;\r\n");
                System.IO.File.AppendAllText(swQuotesPath, "LocalTime;FastTime;FastBid;FastAsk;SlowBid;SlowAsk;GapLong;GapShort;FastCoef;dTFast;tpsFast;dTSlow;tpsSlow;\r\n");
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
                fsData = new System.IO.FileStream(datafolder + "\\" + stime + ".1leg", System.IO.FileMode.Create);
            }

            TimeSpan startTime = model.Open.StartTimeSpan();
            TimeSpan endTime = model.Open.EndTimeSpan();

            lastOpenCloseTime = DateTime.UtcNow;
            while (!threadStop.WaitOne(model.SleepMs))
            {
                decimal GapSell = 0;
                decimal GapBuy = 0;
                var threshhold = model.Open.Threshold;
                var alignmentPer = model.Open.PeriodAlignment;
                var use = model.UseAlignment;
                if (use)
                {
                    
                }

                decimal fastAsk =  model.Leg1.Ask;
                decimal fastBid =  model.Leg1.Bid;
                decimal slowBid =  model.Leg2.Bid;
                decimal slowAsk =  model.Leg2.Ask;

                if (fastBid == 0 || fastAsk == 0 || slowBid == 0 || slowAsk == 0) continue;

                bool skip = true;
                if (fastBid != prevFastBid) skip = false;
                if (fastAsk != prevFastAsk) skip = false;
                if (slowBid != prevSlowBid) skip = false;
                if (slowAsk != prevSlowAsk) skip = false;

                decimal fastQuoteUpdateTimeMs = (decimal)(DateTime.Now -  model.Leg1.Time).TotalMilliseconds;
                decimal slowQuoteUpdateTimeMs = (decimal)(DateTime.Now -  model.Leg2.Time).TotalMilliseconds;
                model.FeederOk = leg1Connector.IsLoggedIn && fastQuoteUpdateTimeMs < 200000 && leg2Connector.IsLoggedIn && slowQuoteUpdateTimeMs < 200000;
                if (!model.FeederOk)
                {
                    skip = true;
                }
                if (fastBid > 0)
                {
                    decimal k = slowBid / fastBid;
                    if (k > 3 || k < 0.3M) skip = true;
                }
                if (fastBid > fastAsk || fastAsk <= 0) skip = true;

                if (!skip)
                {
                    if (model.Leg2.Ask > model.Leg2.Bid && model.Leg1.Ask > model.Leg1.Bid)
                    {
                        GapBuy = model.Leg2.Bid - model.Leg1.Ask;
                        GapSell = model.Leg2.Ask - model.Leg1.Bid;
                    }
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
                    var useAlignment = model.UseAlignment;
                    var buyCnt = GapBuyArr.Count;
                    var sellCnt = GapSellArr.Count;
                    var Period = model.Open.PeriodAlignment;
                    //  if(useAlignment)  
                    if (sellCnt > Period) avgGapSell = GetAvgGapSell(Period);
                    else avgGapSell = GetAvgGapSell(sellCnt);

                    if (buyCnt > Period) avgGapBuy = GetAvgGapBuy(Period);
                    else avgGapBuy = GetAvgGapBuy(buyCnt);

                    deviationBuy = GapBuy - avgGapBuy;
                    deviationSell = GapSell - avgGapSell;


                    fastBidRing.Push(fastBid);
                    fastAskRing.Push(fastAsk);


                    bool tickSignalBuy = fastBid > prevFastBid;
                    bool tickSignalSell = fastAsk < prevFastAsk;
                    decimal slowGapBid = slowBid - prevSlowBid;
                    decimal slowGapAsk = slowAsk - prevSlowAsk;
                    decimal fastGapBid = fastBidRing.Head(0) - fastBidRing.Head(model.Open.GapFastTicks);
                    decimal fastGapAsk = fastAskRing.Head(0) - fastAskRing.Head(model.Open.GapFastTicks);

                    decimal fastChange = Math.Abs(fastBid - prevFastBid) + Math.Abs(fastAsk - prevFastAsk);
                    decimal slowChange = Math.Abs(slowBid - prevSlowBid) + Math.Abs(slowAsk - prevSlowAsk);
                    fastChange = averageFastChange.Process(fastChange);
                    slowChange = averageSlowChange.Process(slowChange);
                    decimal fastCoef = slowChange > 0 ? fastChange / slowChange : 10000000.0M;

                    prevFastBid = fastBid;
                    prevFastAsk = fastAsk;
                    prevSlowBid = slowBid;
                    prevSlowAsk = slowAsk;

                    if (model.Leg2.Ask == model.Leg1.Ask && model.Leg2.Bid == model.Leg1.Bid)
                    {
                        model.LogError($"Gap Error | model.Leg2.Ask == model.Leg1.Ask && model.Leg2.Bid == model.Leg1.Bid = {model.Leg2.Ask == model.Leg1.Ask && model.Leg2.Bid == model.Leg1.Bid}");
                        var askByIndexl1 = GetAskByIndex(0, true).Split(new char[] { ',' });
                        var bidByIndexl1 = GetBidByIndex(0, true).Split(new char[] { ',' });
                        var askByIndexl2 = GetAskByIndex(0, false).Split(new char[] { ',' });
                        var bidByIndexl2 = GetBidByIndex(0, false).Split(new char[] { ',' });
                    }

                    model.GapSell = GapSell;
                    model.GapBuy = GapBuy;

                    if (true)
                    {
                        SafeInvoke(() =>
                        {
                            DateTime chartTime = DateTime.UtcNow;
                            chartValues[0] = new DoubleTimeValue((double)fastBid, chartTime);
                            chartPrice.AddDoubleTimeValuesTo(chartFastBidFlow, chartValues);
                            chartValues[0] = new DoubleTimeValue((double)slowBid, chartTime);
                            chartPrice.AddDoubleTimeValuesTo(chartSlowBidFlow, chartValues);
                            chartValues[0] = new DoubleTimeValue((double)fastAsk, chartTime);
                            chartPrice.AddDoubleTimeValuesTo(chartFastAskFlow, chartValues);
                            chartValues[0] = new DoubleTimeValue((double)slowAsk, chartTime);
                            chartPrice.AddDoubleTimeValuesTo(chartSlowAskFlow, chartValues);
                            chartValues[0] = new DoubleTimeValue((double)deviationBuy, chartTime);
                            chartGap.AddDoubleTimeValuesTo(chartGapBuyFlow, chartValues);
                            chartValues[0] = new DoubleTimeValue((double)deviationSell, chartTime);
                            chartGap.AddDoubleTimeValuesTo(chartGapSellFlow, chartValues);
                        });
                    }
                    if (fsData != null)
                    {
                        DateTime saveTime = DateTime.UtcNow;
                        SaveData(BitConverter.GetBytes((double)fastBid));
                        SaveData(BitConverter.GetBytes((double)fastAsk));
                        SaveData(BitConverter.GetBytes((double)slowBid));
                        SaveData(BitConverter.GetBytes((double)slowAsk));
                        SaveData(BitConverter.GetBytes((double)gapBuy));
                        SaveData(BitConverter.GetBytes((double)gapSell));
                        SaveData(BitConverter.GetBytes(saveTime.Ticks));
                    }

                    if (swQuotesPath != null)
                    {
                        System.IO.File.AppendAllText(swQuotesPath, DateTime.UtcNow.ToString("HH:mm:ss.ffffff") + ";" +
                                                            model.Leg1.Time.ToString("HH:mm:ss.ffffff") + ";" +
                                                           model.FormatPrice(fastBid) + ";" +
                                                           model.FormatPrice(fastAsk) + ";" +
                                                           model.FormatPrice(slowBid) + ";" +
                                                           model.FormatPrice(slowAsk) + ";" +
                                                           ToStr2(gapBuy) + ";" +
                                                           ToStr2(gapSell) + ";" +
                                                           ToStr2(fastCoef) +
                                                           ToStr2( model.Leg1.AverageTimeBetweenTicks) + ";" +
                                                           ToStr2( model.Leg1.GetTicksPerSecond()) + ";" +
                                                           ToStr2(model.Leg2.AverageTimeBetweenTicks) + ";" +
                                                           ToStr2(model.Leg2.GetTicksPerSecond()) + ";\r\n");
                    }
                    decimal spread = (slowAsk - slowBid) / model.Point;
                    if (model.Open.UseAverageSpread)
                    {
                        spread = averageSpread.Process(spread);
                    }
                    decimal spreadK = 1.0M;
                    if (true) //(model.Open.AvtoSettings)
                    {
                        spreadK = spread;
                        if (spreadK < 1.0M) spreadK = 1.0M;
                    }

                    var orders = leg2Connector.GetOrders(model.Leg2.FullSymbol, model.Magic, 1);
                    var order = orders.FirstOrDefault();
                    decimal profit = 0;
                    if (order == null)
                    {
                        trailSl = null;
                         model.Leg2.Volume = 0;
                    }
                    else
                    {
                        if (order.Side == OrderSide.Buy)
                        {
                            profit = slowBid - order.OpenPrice;
                        }
                        else
                        {
                            profit = order.OpenPrice - slowAsk;
                        }
                         model.Leg2.Volume = order.Volume * orders.Count;
                    }
                    decimal minlev = model.Open.MinimumLevel * spreadK + spread + model.Open.Comission;
                    decimal minlevclose = model.Close.MinimumLevelClose * spreadK;
                    decimal fixtp = model.Close.FixTP * spreadK + model.Open.Comission;
                    decimal fixsl = model.Close.FixSL * spreadK + spread;
                    decimal fastSpread = (fastAsk - fastBid) / model.Point;
                    decimal slowSpread = (slowAsk - slowBid) / model.Point;

                    decimal fixtrailstart = model.Close.FixTrailStart * spreadK + model.Open.Comission;
                    decimal fixtrailstop = model.Close.FixTrailStop * spreadK + spread;

                    if (swDebugPath != null)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append(ToStr2(gapBuy) + ";");
                        sb.Append(ToStr2(gapSell) + ";");
                        sb.Append(ToStr2(fastCoef) + ";");
                        if (order != null)
                        {
                            sb.Append(order.Side.ToString() + " " + model.FormatPrice(order.OpenPrice) + " [" + model.FormatPrice(profit) + " pts];");
                        }
                        else
                        {
                            sb.Append("none;");
                        }
                        decimal avbid_ = averageBid.Output;
                        decimal avask_ = averageAsk.Output;
                        sb.Append(model.FormatPrice(avbid_) + ";");
                        sb.Append(model.FormatPrice(avask_) + ";");
                        sb.Append(ToStr2(spread) + ";");
                        sb.Append(ToStr2(spreadK) + ";");
                        sb.Append(ToStr2(minlev) + ";");
                        sb.Append(ToStr2(minlevclose) + ";");
                        sb.Append(ToStr2(fixtp) + ";");
                        sb.Append(ToStr2(fixsl) + ";");
                        sb.Append(ToStr2(trailSl ?? -1000) + ";");
                        sb.Append(ToStr2(fastSpread) + ";");
                        sb.Append(ToStr2( model.Leg1.AverageTimeBetweenTicks) + ";");
                        sb.Append(ToStr2( model.Leg1.GetTicksPerSecond()) + ";");
                        sb.Append(ToStr2(model.Leg2.AverageTimeBetweenTicks) + ";");
                        sb.Append(ToStr2(model.Leg2.GetTicksPerSecond()) + ";\r\n");
                        System.IO.File.AppendAllText(swDebugPath, sb.ToString());
                    }

                    bool slowSignal = false;
                    if (Math.Abs(slowGapBid) <= -minlev) slowSignal = true;
                    if (Math.Abs(slowGapAsk) >= minlev) slowSignal = true;

                     model.Leg1.Balance = leg1Connector.Balance;
                     model.Leg2.Balance = leg2Connector.Balance;

                    if (leg2Connector.Balance != null)
                    {
                        if (stopBalance == null)
                        {
                            stopBalance = leg2Connector.Balance - (leg2Connector.Balance * model.Open.RiskDeposit * 0.01M);
                            model.LogInfo("StopBalance = " + ToStr2(stopBalance.Value));
                        }
                        if (stopBalance != null)
                        {
                            if (leg2Connector.Balance < stopBalance)
                            {
                                if (balanceOk)
                                {
                                    model.LogError("Drfvdown reached. Trading stopped.");
                                }
                                balanceOk = false;
                            }
                        }
                    }

                    bool fastGapOkForBuy = true;
                    bool fastGapOkForSell = true;
                    if (model.Open.MinGapFast > 0)
                    {
                        fastGapOkForBuy = fastGapAsk >= model.Open.MinGapFast;
                        fastGapOkForSell = fastGapBid <= -model.Open.MinGapFast;
                    }
                    bool fastSpreadOk = fastSpread >= model.Open.MinSpread && (fastSpread <= model.Open.MaxSpread || model.Open.MaxSpread == 0);
                    bool slowSpreadOk = slowSpread >= model.Open.MinSpreadSlow && (slowSpread <= model.Open.MaxSpreadSlow || model.Open.MaxSpreadSlow == 0);

                }
            }
            leg2Connector.Tick -= Leg2Connector_Tick;
            leg1Connector.Tick -= Leg1Connector_Tick;
            leg2Connector.LoggedIn -= Leg1Connector_LoggedIn;
            leg1Connector.LoggedIn -= Leg2Connector_LoggedIn;
            //ConnectorsFactory.Current.CloseNonStateConnectors(fastConnector);
            //ConnectorsFactory.Current.CloseNonStateConnectors(slowConnector);
            ConnectorsFactory.Current.CloseConnector(model.Leg2.Name, true);
            ConnectorsFactory.Current.CloseConnector( model.Leg1.Name, true);

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

        private void Leg2Connector_Tick(object sender, TickEventArgs e)
        {
            if (model.Leg2.Symbol == e.Symbol)
            {
                AsksLeg2 = e.Asks;
                BidsLeg2 = e.Bids;
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
                AsksLeg1= e.Asks;
                BidsLeg1 = e.Bids;
                model.Leg1.Bid = e.Bid;
                model.Leg1.Ask = e.Ask;
                model.Leg1.Time = DateTime.Now;
                var symb = e.Symbol;
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

        void OnLeg1Login()
        {
            leg2Connector.Fill = (FillPolicy)model.Open.Fill;
            leg2Connector.Subscribe(model.Leg2.FullSymbol,  model.Leg2.GetSymbolId(), leg2Type);
        }
        void OnLeg2Login()
        {
            leg1Connector.Fill = (FillPolicy)model.Open.Fill;
            var symbId =  model.Leg1.GetSymbolId();
            leg1Connector.Subscribe( model.Leg1.FullSymbol, symbId, leg1Type);
        }

        private void Leg1Connector_LoggedIn(object sender, EventArgs e)
        {
            OnLeg1Login();
        }

        private void Leg2Connector_LoggedIn(object sender, EventArgs e)
        {
            OnLeg2Login();
        }

        void SaveData(byte[] data)
        {
            fsData.Write(data, 0, data.Length);
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

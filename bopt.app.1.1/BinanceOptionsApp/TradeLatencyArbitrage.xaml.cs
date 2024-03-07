using MultiTerminal.Connections;
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
    public partial class TradeLatencyArbitrage : UserControl, IConnectorLogger, ITradeTabInterface
    {
        private Models.TradeModel model;
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
        private DateTime StrtInterval;
        private List<List<string>> Bids { get; set; }
        private List<List<string>> Asks { get; set; }

        private decimal gapBuy = 0.0m;
        private decimal gapSell = 0.0m;

        private decimal MaxGapBuyA = 0.0m;
        private decimal MinGapSellA = 0.0m;
        private decimal MaxGapBuyB = 0.0m;
        private decimal MinGapSellB = 0.0m;

        private IConnector _1LegConnector;
        private IConnector _2LegConnector;
        private string leg1Type, leg2Type;

        public BinanceCryptoClient bcm = null;
        public BinanceFutureClient bfc = null;

        string swLogPath;
        string swDebugPath;
        string swQuotesPath;
        System.IO.FileStream fsData;

        //DispatcherTimer timerEvent;

        // Test flag for open:
        bool isOpen = false;

        public TradeLatencyArbitrage()
        {
            InitializeComponent();
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
            //tbFreezeTime.IsEnabled = Model.UseFreezeTime;
        }

        #region Log's methods:
        public void LogOrderSuccess(string message)
        {
            Log(message, Colors.Green, Color.FromRgb(0, 255, 0));
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

            Models.TradeModel.currencySpot = model.Leg1.SymbolCurrency;
            Models.TradeModel.currencyFuture = model.Leg2.SymbolCurrency;
            Models.TradeModel.fullSymbolFuture = model.Leg2.Symbol;

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

        #region Thread Process Method:
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
                string logfolder = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\.logs";
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
                //  Here you can write arbitrage of strategy and algo:
                decimal GapBuy = 0;
                decimal GapSell = 0;

                if (model.Leg2.Bid != 0 && model.Leg1.Ask != 0)
                {
                    GapBuy = model.Leg1.Ask - model.Leg2.Ask;
                    GapSell = model.Leg1.Bid - model.Leg2.Bid;


                    if (PreBidF != model.Leg2.Bid) { TimeBidF = DateTime.Now; PreBidF = model.Leg2.Bid; }

                    if (PreAskF != model.Leg2.Ask) { TimeAskF = DateTime.Now; PreAskF = model.Leg2.Ask; }

                    if (PreAskS != model.Leg1.Ask) { TimeAskS = DateTime.Now; PreAskS = model.Leg1.Ask; }

                    if (PreBidS != model.Leg1.Bid) { TimeBidS = DateTime.Now; PreBidS = model.Leg1.Bid; }

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


                        if ((DateTime.Now - StrtInterval).TotalSeconds > 600)
                        {
                            if (sellCnt > Period) avgGapSell = GetAvgGapSell(Period);
                            else avgGapSell = GetAvgGapSell(sellCnt);

                            if (buyCnt > Period) avgGapBuy = GetAvgGapBuy(Period);
                            else avgGapBuy = GetAvgGapBuy(buyCnt);

                            StrtInterval = DateTime.Now;
                        }


                        if (buyCnt > Period) avgGapBuy = GetAvgGapBuy(Period);
                        else avgGapBuy = GetAvgGapBuy(buyCnt);

                        if (GapBuy != 0 && avgGapBuy != 0)
                        {
                            deviationBuy = GapBuy - avgGapBuy;
                            if (deviationBuy > MaxGapBuyA || MaxGapBuyA == 0)
                            {
                                MaxGapBuyA = deviationBuy;
                             //   model.LogInfo($"New Max DevBuy: {MaxGapBuyA} GapBuy:{GapBuy} avgGapBuy:{avgGapBuy} deviationBuy:{deviationBuy}");
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
                                //model.LogInfo($"Ask1: {model.Leg1.Ask} Bid1:{model.Leg1.Bid} Ask2:{model.Leg2.Ask} Bid2:{model.Leg2.Bid}");
                                model.MinGapSellA = Math.Round(MinGapSellA, model.Open.Point);
                            }
                        }
                    }                    //if (GapBuy > MaxGapBuy || MaxGapBuy == 0) MaxGapBuy = GapBuy;
                    //if (GapSell < MinGapSell || MinGapSell == 0) MinGapSell = GapSell;

                    model.MaxGapBuyA = MaxGapBuyA;
                    model.MinGapSellA = MinGapSellA;

                }

                var spotLot = model.Leg1.Lot;
                var futureLot = model.Leg2.Lot;

                var spotLotStep = model.Leg1.LotStep;
                var futureLotStep = model.Leg2.LotStep;

                if (bfc != null)
                {
                    // OnView [for Future]:
                    if (bfc.AccountInfo != null)
                    {
                        model.Leg2.TotalInitMargin = decimal.Parse(bfc.AccountInfo.TotalInitialMargin, CultureInfo.InvariantCulture);
                        model.Leg2.AvailableBalance = decimal.Parse(bfc.AccountInfo.AvailableBalance, CultureInfo.InvariantCulture);
                        model.Leg2.TotalCrossUnPnl = decimal.Parse(bfc.AccountInfo.TotalCrossUnPnl, CultureInfo.InvariantCulture);
                        model.Leg2.TotalMarginBalance = decimal.Parse(bfc.AccountInfo.TotalMarginBalance, CultureInfo.InvariantCulture);
                        model.Leg2.TotalCrossWalletBalance = decimal.Parse(bfc.AccountInfo.TotalCrossWalletBalance, CultureInfo.InvariantCulture);
                        // Position model
                        var res = bfc.AccountInfo.Positions.FirstOrDefault(s => s.Symbol == model.Leg2.Symbol);
                        if (res != null)
                        {
                            model.Leg2.EntryPrice = decimal.Parse(res.EntryPrice, CultureInfo.InvariantCulture);
                            model.Leg2.PositionAmt = decimal.Parse(res.PositionAmt, CultureInfo.InvariantCulture);
                        }
                    }
                }
                if (bcm != null)
                {
                    // OnView [for Spot/Margin]:
                    if (bcm.MarginAccount != null)
                    {
                        var assetFound = bcm.MarginAccount.userAssets.FirstOrDefault(a => a.asset.ToLower() == model.Leg1.SymbolAsset);
                        model.Leg1.CollateralMarginLevel = decimal.Parse(bcm.MarginAccount.collateralMarginLevel, CultureInfo.InvariantCulture);
                        model.Leg1.MarginLevel = decimal.Parse(bcm.MarginAccount.marginLevel, CultureInfo.InvariantCulture);
                        model.Leg1.TotalCollateralValueInUSDT = decimal.Parse(bcm.MarginAccount.totalCollateralValueInUSDT, CultureInfo.InvariantCulture);
                        if (assetFound != null)
                        {
                            model.Leg1.Borrowed = assetFound.free;
                            model.Leg1.Interest = assetFound.interest;
                            model.Leg1.Locked = assetFound.locked;
                            model.Leg1.NetAsset = assetFound.netAsset;
                        }
                    }
                }
                AssetBalS = model.Leg1.NetAsset;
                CurrBalS = model.Leg1.TotalCollateralValueInUSDT;
                var Profit = model.Leg1.TotalCrossUnPnl;
                var gapForOpen = model.Open.GapForOpen;
                var gapForClose = model.Open.GapForClose;
                //var ask = GetAskByIndex(0);
                //var bid = GetBidByIndex(0);

                model.GapSell = GapSell;
                model.GapBuy = GapBuy;

                /*
                if (Math.Abs((int)AssetBalS) == 0) PosSpotMarg = false;
                else if ((int)AssetBalS > 0) { PosSpotMargBuy = true; PosSpotMargSell = false; }
                else if ((int)AssetBalS < 0) { PosSpotMargSell = true; PosSpotMargBuy = false; }
                */


                //***********************************************************
                if (PosSpotMarg)
                {
                    if (PosSpotMargSell)
                    {
                        if (TimeAskF > TimeAskS && GapBuy > gapForClose)
                        {
                            if (model.AllowOpen)
                            {
                                if (OpenPos(model.Leg1.Symbol, "1Leg", FillPolicy.FOK, OrderSide.Buy, GapBuy, OrderType.Market))
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
                        if (TimeBidF > TimeBidS && GapSell * -1 > gapForClose)
                        {
                            if (OpenPos(model.Leg1.Symbol, "1Leg", FillPolicy.FOK, OrderSide.Sell, GapSell, OrderType.Market))
                            {
                                model.LogInfo($"Elapsted Close Buy Pos (Spot)[GapSell * -1 >= gapForClose]");
                                PosSpotMarg = false; PosSpotMargBuy = false;
                                //await TradeOneLeg.bfc.GetBalance(model.Leg2.Symbol);
                            }
                            else
                            {
                                PosSpotMarg = true; PosSpotMargBuy = true;
                            }
                        }
                    }
                    else
                    {
                    }
                }
                else if (!PosSpotMarg)
                {
                    if (model.AllowOpen)
                    {
                        if (TimeAskF > TimeAskS && GapBuy > gapForOpen)
                        {
                            if (OpenPos(model.Leg1.Symbol, "1Leg", FillPolicy.FOK, OrderSide.Buy, GapBuy, OrderType.Market))
                            {
                                model.LogInfo($"Elapsted Open Buy (1Leg)[]");
                                PosSpotMarg = true; PosSpotMargBuy = true;
                                //await TradeOneLeg.bfc.GetBalance(model.Leg2.SymbolAsset);
                            }
                            else
                            {
                                PosSpotMarg = false; PosSpotMargBuy = false;
                            }
                        }
                        else if (TimeBidF > TimeBidS && GapSell * -1 > gapForOpen)
                        {
                            if (OpenPos(model.Leg1.Symbol, "1Leg", FillPolicy.FOK, OrderSide.Sell, GapSell, OrderType.Market))
                            {
                                model.LogInfo($"Elapsted Open Sell (1Leg)[]");
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
                //}
                //}
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
                Asks = e.Asks;
                Bids = e.Bids;
                model.Leg2.Bid = e.Bid;
                model.Leg2.Ask = e.Ask;
                model.Leg2.Time = DateTime.Now;
            }
        }

        private void _1LegConnector_Tick(object sender, TickEventArgs e)
        {
            if (e.Symbol.ToLower() == model.Leg1.Symbol.ToLower())
            {
                Asks = e.Asks;
                Bids = e.Bids;
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

        private string GetAskByIndex(int index)
        {
            if (Asks != null)
                if (Asks.Count != 0 && index < 10)
                {
                    var foundAsk = Asks[index];
                    return $"{foundAsk[0]},{foundAsk[1]}";
                }
            return "0";
        }

        private string GetBidByIndex(int index)
        {
            if (Bids != null)
                if (Bids.Count != 0 && index < 10)
                {
                    var foundBid = Bids[index];
                    return $"{foundBid[0]},{foundBid[1]}";
                }
            return "0";
        }

        private bool OpenPos(string symb, string type, FillPolicy policy, OrderSide bs, decimal gap, OrderType orderType)
        {
            bool isSuccess = false;
            OrderOpenResult result = null;
            if (type == "1Leg")
            {
                result = _1LegConnector.Open(symb.ToUpper(), model.Leg1.Ask, model.Leg1.Lot, policy, bs, model.Magic, model.Slippage, 1,
                            orderType, model.Open.PendingLifeTimeMs);
            }

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
                    model.LogInfo($"[{type}]: {bs.ToString()} FAILED " + model.Leg1.Symbol + ";Gap="
                        + gap +
                        ";Price=" + model.FormatPrice(model.Leg1.Ask));
                    isSuccess = false;
                }


            return isSuccess;
        }

        void On_2LegLogin()
        {
            _2LegConnector.Fill = (FillPolicy)model.Open.Fill;
            _2LegConnector.Subscribe(model.Leg2.Symbol, model.Leg2.GetSymbolId(), "LatencyArbitrage");
        }
        void On_1LegLogin()
        {
            _1LegConnector.Fill = (FillPolicy)model.Open.Fill;
            var symbId = model.Leg1.GetSymbolId();
            _1LegConnector.Subscribe(model.Leg1.Symbol, symbId, "LatencyArbitrage");
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

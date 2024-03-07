using BinanceOptionsApp;
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
//using VisualMarketsEngine.PropertyGrid;

namespace BinanceOptionsApp
{
    public partial class TradeMultiLegSpread : UserControl, IConnectorLogger, ITradeTabInterface
    {
        public enum TradingState
        {
            Initial,
            WaitOpenGap,
            OpeningGap,
            WaitCloseGap,
            ClosingGap
        }
        public class ConnectorInfo
        {
            internal IConnector Connector { get; set; }
            public Models.ProviderModel Info { get; set; }
            internal OrdersStatistic Stat { get; set; }
        }
        readonly List<ConnectorInfo> connectors = new List<ConnectorInfo>();
        readonly List<ConnectorInfo> positions = new List<ConnectorInfo>();
        readonly List<TradingState> states = new List<TradingState>();
        readonly List<DateTime> times = new List<DateTime>();

        Models.TradeModel model;
        ManualResetEvent threadStop;
        ManualResetEvent threadStopped;

        TradingState mode;

        readonly object quoteLock = new object();
        readonly object loglock = new object();

        int viewCounter = 0;

        public TradeMultiLegSpread()
        {
            InitializeComponent();
        }
        public void InitializeTab()
        {
            model = DataContext as Models.TradeModel;
            fast.InitializeProviderControl(model.Leg1,false);

            model.LogError = LogError;
            model.LogInfo = LogInfo;
            model.LogWarning = LogWarning;
            model.LogClear = LogClear;
            HiddenLogs.LogHeader(model);
            EnableAddRemoveButtons();
        }
        public void RestoreNullCombo(ConnectionModel cm)
        {
            fast.RestoreNullCombo(cm);
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
        private void BuStart_Click(object sender, RoutedEventArgs e)
        {
            Start();
        }
        private void BuStop_Click(object sender, RoutedEventArgs e)
        {
            Stop(false);
        }
        public void Start()
        {
            if (model.Started) return;
            model.Started = true;
            model.FeederOk = false;
            LogClear();
            HiddenLogs.LogHeader(model);

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
        string swLogPath;

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
            mode = TradingState.Initial;
            model.Leg1.Bid = 0;
            model.Leg1.Ask = 0;
            model.Leg1.Time = DateTime.MinValue;
            model.Leg1.Volume = 0;
            model.Leg1.Balance = null;
            model.Leg2.Bid = 0;
            model.Leg2.Ask = 0;
            model.Leg2.Time = DateTime.MinValue;
            model.Leg2.Volume = 0;
            model.Leg2.Balance = null;
            model.GapBuy = 0;
            model.GapSell = 0;

            connectors.Clear();
            positions.Clear();
            foreach (var provider in model.Providers)
            {
                provider.Parent = model;
                ConnectorInfo connector = new ConnectorInfo
                {
                    Info = provider,
                    Connector = provider.CreateConnector(this, threadStop, model.SleepMs,Dispatcher)
                };
                connector.Connector.Tick += Connector_Tick;
                connector.Connector.LoggedIn += Connector_LoggedIn;
                connectors.Add(connector);
            }

            model.LogInfo(model.Title + " logging in...");
            while (!threadStop.WaitOne(100))
            {
                bool loggedIn = true;
                foreach (var connector in connectors) if (!connector.Connector.IsLoggedIn) loggedIn = false;
                if (loggedIn)
                {
                    model.LogInfo(model.Title + " logged in OK.");
                    break;
                }
            }

            if (!threadStop.WaitOne(0))
            {
                foreach (var connector in connectors)
                {
                    if (connector.Connector.IsLoggedIn)
                    {
                        OnConnectorLogin(connector.Connector);
                    }
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
            }
            else
            {
                swLogPath = null;
            }

            if (connectors.Count<2)
            {
                model.LogError("Please, Add more brokers (minimum 2) for trade Multi Leg Spread Arbitrage Algorithm");
            }
            TimeSpan startTime = model.Open.StartTimeSpan();
            TimeSpan endTime = model.Open.EndTimeSpan();

            while (!threadStop.WaitOne(model.SleepMs))
            {
                bool feederOk = true;
                foreach (var connector in connectors)
                {
                    connector.Info.MaxMinState = Models.ProviderMaxMinState.None;
                    connector.Info.MultiLegProfit = 0;
                    connector.Info.MultiLegGap = 0;
                    decimal timeMs = (decimal)(DateTime.Now - connector.Info.Time).TotalMilliseconds;
                    if (!connector.Connector.IsLoggedIn || timeMs > 200000) feederOk = false;
                    if (connector.Info.Bid == 0 || connector.Info.Ask == 0) feederOk = false;
                }
                model.FeederOk = feederOk;
                if (model.FeederOk)
                {
                    int maxBidIndex = -1;
                    int minAskIndex = -1;
                    lock (quoteLock)
                    {
                        decimal maxBid = -1;
                        for (int i = 0; i < connectors.Count; i++)
                        {
                            if (connectors[i].Info.Bid > maxBid && !positions.Contains(connectors[i]))
                            {
                                maxBid = connectors[i].Info.Bid;
                                maxBidIndex = i;
                            }
                        }
                        if (maxBidIndex >= 0)
                        {
                            connectors[maxBidIndex].Info.MaxMinState = Models.ProviderMaxMinState.Max;

                            decimal minAsk = decimal.MaxValue;
                            for (int i = 0; i < connectors.Count; i++)
                            {
                                if (i != maxBidIndex)
                                {
                                    if (connectors[i].Info.Ask < minAsk && !positions.Contains(connectors[i]))
                                    {
                                        minAsk = connectors[i].Info.Ask;
                                        minAskIndex = i;
                                    }
                                }
                            }
                            if (minAskIndex >= 0)
                            {
                                connectors[minAskIndex].Info.MaxMinState = Models.ProviderMaxMinState.Min;
                            }
                        }
                    }
                    bool ordersPresent = false;
                    for (int i=0;i<connectors.Count;i++)
                    {
                        connectors[i].Stat = new OrdersStatistic(connectors[i].Connector.GetOrders(connectors[i].Info.FullSymbol, model.Magic,i+1),connectors[i].Info.Bid,connectors[i].Info.Ask);
                        connectors[i].Info.Volume = connectors[i].Stat.Volume;
                        if (connectors[i].Stat.OrdersCount > 0) ordersPresent = true;
                        if (i == 0)
                        {
                            model.Leg1.Ask = connectors[i].Info.Ask;
                            model.Leg1.Bid = connectors[i].Info.Bid;
                            model.Leg1.Volume = connectors[i].Info.Volume;
                            model.Leg1.Time = connectors[i].Info.Time;
                            model.Leg1.Balance = connectors[i].Info.Balance;
                        }
                        if (i == 1)
                        {
                            model.Leg2.Ask = connectors[i].Info.Ask;
                            model.Leg2.Bid = connectors[i].Info.Bid;
                            model.Leg2.Volume = connectors[i].Info.Volume;
                            model.Leg2.Time = connectors[i].Info.Time;
                            model.Leg2.Balance = connectors[i].Info.Balance;
                        }
                    }
                    decimal minlev = model.Open.MinimumLevel;
                    decimal minlevclose = model.Close.MinimumLevelClose;
                    decimal fixtp = model.Close.FixTP;
                    decimal fixsl = model.Close.FixSL;

                    if (mode == TradingState.Initial)
                    {
                        if (!ordersPresent)
                        {
                            mode = TradingState.WaitOpenGap;
                            positions.Clear();
                        }
                        else
                        {
                            MyParallel.For(0, connectors.Count, (int i) =>
                            {
                                if (connectors[i].Stat.OrdersCount > 0)
                                {
                                    Close(connectors[i].Connector, connectors[i].Stat.Orders[0], connectors[i].Stat.Orders[0].Side == OrderSide.Buy ? connectors[i].Info.Bid : connectors[i].Info.Ask, 0, "initial close");
                                }
                            });
                        }
                    }
                    else
                    {
                        decimal spread = 0;
                        if (minAskIndex >= 0 && maxBidIndex >= 0)
                        {
                            spread=(connectors[maxBidIndex].Info.Bid-connectors[minAskIndex].Info.Ask) / model.Point;
                            connectors[minAskIndex].Info.MultiLegGap = spread;
                            connectors[maxBidIndex].Info.MultiLegGap = spread;
                            if (spread >= minlev && model.AllowOpen && model.Open.IsInStartEndSpan(DateTime.Now, startTime, endTime))
                            {
                                positions.Add(connectors[minAskIndex]);
                                positions.Add(connectors[maxBidIndex]);
                                states.Add(TradingState.OpeningGap);
                                states.Add(TradingState.OpeningGap);
                                times.Add(DateTime.UtcNow);
                                times.Add(DateTime.UtcNow);
                                connectors[minAskIndex].Info.MultiLegOperationGap = spread;
                                connectors[maxBidIndex].Info.MultiLegOperationGap = spread;
                                model.LogInfo("Open signal received gap=" + ToStr2(spread) + "; buy on " + connectors[minAskIndex].Info.Name + " vs sell on " + connectors[maxBidIndex].Info.Name);
                            }
                        }
                        model.GapBuy = spread;
                        model.GapSell = spread;

                        for (int i=0;i<positions.Count;i+=2)
                        {
                            int totalOrders = positions[i].Stat.OrdersCount + positions[i+1].Stat.OrdersCount;
                            decimal totalProfit = positions[i].Stat.Profit + positions[i + 1].Stat.Profit;
                            decimal costs = (positions[i].Info.Ask - positions[i].Info.Bid + positions[i + 1].Info.Ask - positions[i + 1].Info.Bid) / model.Point;
                            spread = (positions[i+1].Info.Bid - positions[i].Info.Ask) / model.Point;
                            positions[i].Info.MultiLegGap = spread;
                            positions[i+1].Info.MultiLegGap = spread;
                            positions[i].Info.MultiLegProfit = positions[i + 1].Info.MultiLegProfit = (totalProfit/model.Point);
                            if (states[i] == TradingState.OpeningGap)
                            {
                                if (totalOrders >= 2)
                                {
                                    states[i] = states[i+1]=TradingState.WaitCloseGap;
                                    times[i] = times[i + 1] = DateTime.UtcNow;
                                }
                                else
                                {
                                    MyParallel.Invoke(
                                        () => { if (positions[i+1].Stat.OrdersCount == 0) Sell(positions[i+1].Connector, positions[i+1].Info.FullSymbol, positions[i+1].Info.MultiLegOpenVolume, positions[i+1].Info.Bid, positions[i+1].Info.MultiLegOperationGap,i+2); },
                                        () => { if (positions[i].Stat.OrdersCount == 0) Buy(positions[i].Connector, positions[i].Info.FullSymbol, positions[i].Info.MultiLegOpenVolume, positions[i].Info.Ask, positions[i].Info.MultiLegOperationGap,i+1); }
                                    );
                                }
                            }
                            else if (states[i] == TradingState.WaitCloseGap)
                            {
                                bool timerok = (DateTime.UtcNow - times[i]).TotalSeconds >= model.Open.MinOrderTimeSec;
                                if (model.Close.CloseTimerSec > 0)
                                {
                                    if ((DateTime.UtcNow - times[i]).TotalSeconds >= model.Close.CloseTimerSec)
                                    {
                                        states[i] = TradingState.ClosingGap;
                                        model.LogInfo("Close signal received by TIMER  on " + positions[i].Info.Name + " vs " + positions[i + 1].Info.Name);
                                    }
                                }
                                if (states[i]!=TradingState.ClosingGap && model.Close.MinimumLevelClose != 0)
                                {
                                    if (spread <= -minlevclose && timerok)
                                    {
                                        states[i] = TradingState.ClosingGap;
                                        model.LogInfo("Close signal received gap=" + ToStr2(spread) + " on " + positions[i].Info.Name + " vs " + positions[i + 1].Info.Name);
                                    }
                                }
                                if (states[i] != TradingState.ClosingGap)
                                {
                                    if (totalProfit >= (fixtp * model.Point) && timerok)
                                    {
                                        states[i] = TradingState.ClosingGap;
                                        model.LogInfo("Close signal received by TP on " + positions[i].Info.Name + " vs " + positions[i + 1].Info.Name);
                                    }
                                    else if (totalProfit <= ((-fixsl - costs) * model.Point))
                                    {
                                        states[i] = TradingState.ClosingGap;
                                        model.LogInfo("Close signal received by SL on " + positions[i].Info.Name + " vs " + positions[i + 1].Info.Name);
                                    }
                                }
                                if (states[i] == TradingState.ClosingGap)
                                {
                                    positions[i].Info.MultiLegOperationGap = spread;
                                    positions[i+1].Info.MultiLegOperationGap = spread;
                                }
                            }
                            else if (states[i] == TradingState.ClosingGap)
                            {
                                if (totalOrders == 0)
                                {
                                    positions.RemoveAt(i + 1);
                                    positions.RemoveAt(i);
                                    states.RemoveAt(i + 1);
                                    states.RemoveAt(i);
                                    times.RemoveAt(i + 1);
                                    times.RemoveAt(i);
                                    break;
                                }
                                else
                                {
                                    MyParallel.For(i, 2, (int pos) =>
                                    {
                                        if (positions[pos].Stat.OrdersCount > 0)
                                        {
                                            Close(positions[pos].Connector, positions[pos].Stat.Orders[0], positions[pos].Stat.Orders[0].Side == OrderSide.Buy ? positions[pos].Info.Bid : positions[pos].Info.Ask, positions[pos].Info.MultiLegOperationGap, "");
                                        }
                                    });
                                }
                            }
                        }
                    }
                }
            }
            foreach (var connector in connectors)
            {
                connector.Connector.Tick -= Connector_Tick;
                connector.Connector.LoggedIn -= Connector_LoggedIn;
                ConnectorsFactory.Current.CloseConnector(connector.Info.Name,true);
            }
            swLogPath = null;
            threadStopped.Set();
        }

        private void Connector_LoggedIn(object sender, EventArgs e)
        {
            OnConnectorLogin(sender as IConnector);
        }
        private void Connector_Tick(object sender, TickEventArgs e)
        {
            try
            {
                lock (quoteLock)
                {
                    if (sender is IConnector connector)
                    {
                        ConnectorInfo info = connectors.FirstOrDefault(x => x.Connector == connector);
                        if (info != null)
                        {
                            if (e.Symbol == info.Info.FullSymbol)
                            {
                                info.Info.Bid = e.Bid;
                                info.Info.Ask = e.Ask;
                                info.Info.Time = DateTime.Now;
                                info.Info.Balance = connector.Balance;
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }
        void OnConnectorLogin(IConnector connector)
        {
            ConnectorInfo info = connectors.FirstOrDefault(x => x.Connector == connector);
            if (info != null)
            {
                connector.Fill = (FillPolicy)model.Open.Fill;
                connector.Subscribe(info.Info.FullSymbol, info.Info.GetSymbolId());
            }
        }
        internal void Buy(IConnector connector, string symbol, decimal lot, decimal price, decimal gap, int track)
        {
            if (model.Open.OrderType == OrderType.Limit)
            {
                price -= model.Open.PendingDistance * model.Point;
            }
            else if (model.Open.OrderType == OrderType.Stop)
            {
                price += model.Open.PendingDistance * model.Point;
            }

            model.LogInfo("BUY " + symbol + " " + ToStr2(lot) + " signal received at " + model.FormatPrice(price) + " on " + connector.ViewId);
            if (Model.TradeProcessingEngineError)
            {
                model.LogError("Trade processing engine error");
                return;
            }

            //var result = connector.Open(symbol, price, lot, OrderSide.Buy, model.Magic, model.Slippage,track, model.Open.OrderType, model.Open.PendingLifeTimeMs);
            //if (string.IsNullOrEmpty(result.Error))
            //{
            //    decimal slippage = -(result.OpenPrice - price) / model.Point;
            //    model.LogInfo("BUY OK " + symbol + " at " + model.FormatPrice(result.OpenPrice) + ";Gap=" + ToStr2(gap) + ";Price=" + model.FormatPrice(price) + ";Slippage=" + ToStr1(slippage) +
            //        ";Execution=" + ToStrMs(result.ExecutionTime) + " ms");
            //}
            //else
            //{
            //    model.LogError(connector.ViewId+" "+result.Error);
            //    model.LogInfo("BUY FAILED " + symbol + ";Gap=" + ToStr2(gap) + ";Price=" + model.FormatPrice(price));
            //}
        }
        internal void Sell(IConnector connector, string symbol, decimal lot, decimal price, decimal gap, int track)
        {
            if (model.Open.OrderType == OrderType.Limit)
            {
                price += model.Open.PendingDistance * model.Point;
            }
            else if (model.Open.OrderType == OrderType.Stop)
            {
                price -= model.Open.PendingDistance * model.Point;
            }

            model.LogInfo("SELL " + symbol + " " + ToStr2(lot) + " signal received at " + model.FormatPrice(price) + " on " + connector.ViewId);
            if (Model.TradeProcessingEngineError)
            {
                model.LogError("Trade processing engine error");
                return;
            }

            //var result = connector.Open(symbol, price, lot, OrderSide.Sell, model.Magic, model.Slippage,track, model.Open.OrderType,model.Open.PendingLifeTimeMs);
            //if (string.IsNullOrEmpty(result.Error))
            //{
            //    decimal slippage = -(price - result.OpenPrice) / model.Point;
            //    model.LogInfo("SELL OK " + symbol + " at " + model.FormatPrice(result.OpenPrice) + ";Gap=" + ToStr2(gap) + ";Price=" + model.FormatPrice(price) + ";Slippage=" + ToStr1(slippage) +
            //        ";Execution=" + ToStrMs(result.ExecutionTime) + " ms");
            //}
            //else
            //{
            //    model.LogError(connector.ViewId+" "+result.Error);
            //    model.LogInfo("SELL FAILED " + symbol + ";Gap=" + ToStr2(gap) + ";Price=" + model.FormatPrice(price));
            //}
        }
        internal void Close(IConnector connector, OrderInformation order, decimal price, decimal gap, string extraMessage)
        {
            if (model.Open.OrderType == OrderType.Limit)
            {
                if (order.Side == OrderSide.Buy) price += model.Open.PendingDistance * model.Point;
                else price -= model.Open.PendingDistance * model.Point;
            }
            else if (model.Open.OrderType == OrderType.Stop)
            {
                if (order.Side == OrderSide.Buy) price -= model.Open.PendingDistance * model.Point;
                else price += model.Open.PendingDistance * model.Point;
            }

            model.LogInfo("CLOSE " + order.Symbol + " " + ToStr2(order.Volume) + " signal received at " + model.FormatPrice(price) + " on " + connector.ViewId);
            if (Model.TradeProcessingEngineError)
            {
                model.LogError("Trade processing engine error");
                return;
            }

            var result = connector.Close(order.Symbol, order.Id, price, order.Volume, order.Side, model.Slippage, model.Close.OrderType, model.Close.PendingLifeTimeMs);
            if (string.IsNullOrEmpty(result.Error))
            {
                if (result != null)
                {
                    decimal slippage = -(order.Side == OrderSide.Buy ? (price - result.ClosePrice) / model.Point : (result.ClosePrice - price) / model.Point);
                    model.LogInfo("CLOSE OK " + order.Symbol + " at " + model.FormatPrice(result.ClosePrice) + ";Gap=" + ToStr2(gap) + ";Price=" + model.FormatPrice(price) + ";Slippage=" + ToStr1(slippage) + " " + extraMessage + " " +
                        ";Execution=" + ToStrMs(result.ExecutionTime) + " ms");
                }
            }
            else
            {
                model.LogError(connector.ViewId+" "+result.Error);
                model.LogError("CLOSE FAILED " + order.Symbol + ";Gap=" + ToStr2(gap) + ";Price=" + model.FormatPrice(price) + " " + extraMessage);
            }
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Models.ProviderModel p = new Models.ProviderModel
            {
                Name = model.Leg1.Name,
                Symbol = model.Leg1.Symbol,
                Prefix = model.Leg1.Prefix,
                Postfix = model.Leg1.Postfix,
                Parent = model,
                ViewNumber = viewCounter++
            };
            model.Providers.Add(p);
            if (viewCounter > 100000) viewCounter = 0;
        }
        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableAddRemoveButtons();
        }
        void EnableAddRemoveButtons()
        {
            removeBroker.IsEnabled = list.SelectedValue is Models.ProviderModel;
            openLotText.Visibility = removeBroker.IsEnabled ? Visibility.Visible : Visibility.Hidden;
            openLotValue.Visibility = removeBroker.IsEnabled ? Visibility.Visible : Visibility.Hidden;
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var provider = list.SelectedValue as Models.ProviderModel;
            model.Providers.Remove(provider);
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

        public void LogOrderSuccess(string msg)
        {
            throw new NotImplementedException();
        }
    }
}

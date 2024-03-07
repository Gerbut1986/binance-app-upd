using MultiTerminal.Connections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using VisualMarketsEngine;
using BinanceOptionsApp.Controls;
using BinanceOptionsApp.Models;

namespace BinanceOptionsApp
{
    public interface ITradeVisualizerDebugData
    {
        DateTime GetCurrentTime();
    }
    public interface ITradeVisualizerDiagnosticsData
    {
        DateTime GetCurrentTime();
    }
    public interface ITradeVisualizerListener
    {
        void SaveTraceInfo(string pathname);
        void SaveDiagnosticsToTrace(ITradeVisualizerDiagnosticsData data, FileStream fs);
         void SaveOrderToTrace(TradeOrderInformation order, FileStream fs);
         void SaveSignalToTrace(TradeSignal signal, FileStream fs);
        void ShowDiagnostics(ITradeVisualizerDiagnosticsData data);
         void ShowChart(ITradeVisualizerDiagnosticsData[] data, TradeOrderInformation[] liveOrders, TradeSignal[] tradeSignals);
        ITradeVisualizerDebugData[] GetDebug(List<ITradeVisualizerDiagnosticsData> data);
    }
    public class TradeVisualizerNormalizedDouble
    {
        public int Digits { get; set; }
        public double Value { get; set; }
        public string VisibleValue { get; set; }
        public TradeVisualizerNormalizedDouble(double value, int digits)
        {
            string format = "F" + digits;
            Digits = digits;
            Value = value;
            VisibleValue = value.ToString(format, CultureInfo.InvariantCulture);
        }
        public override string ToString()
        {
            return VisibleValue;
        }
    }
    public class TradeVisualizerOrder
    {
        public string Side { get; set; }
        public TradeVisualizerNormalizedDouble Volume { get; set; }
        public TradeVisualizerNormalizedDouble OpenPrice { get; set; }
        public DateTime OpenTime { get; set; }
        public int OpenExecutionTimeMs { get; set; }
        public int OpenSlippagePt { get; set; }
        public TradeVisualizerNormalizedDouble ClosePrice { get; set; }
        public DateTime CloseTime { get; set; }
        public int CloseExecutionTimeMs { get; set; }
        public int CloseSlippagePt { get; set; }
        public int ProfitPt { get; set; }

        internal TradeVisualizerOrder(TradeOrderInformation source, int digits)
        {
            Side = source.Side.ToString();
            Volume = new TradeVisualizerNormalizedDouble(source.Volume, 5);
            OpenPrice = new TradeVisualizerNormalizedDouble(source.OpenPrice, digits);
            OpenTime = source.OpenTime;
            OpenExecutionTimeMs = source.OpenExecutionTimeMs;
            OpenSlippagePt = source.OpenSlippagePt;
            ClosePrice = new TradeVisualizerNormalizedDouble(source.ClosePrice, digits);
            CloseTime = source.CloseTime;
            CloseExecutionTimeMs = source.CloseExecutionTimeMs;
            CloseSlippagePt = source.CloseSlippagePt;
            ProfitPt = source.ProfitPt;
        }
    }
    public class TradeSignal
    {
        public double Price;
        public DateTime Time;
        internal OrderSide Side;
    }
    public partial class TradeVisualizer : UserControl, IConnectorLogger
    {
        bool backtest;
        AlgoControlModel control;
        readonly ManualResetEvent threadStop = new ManualResetEvent(false);
        readonly ManualResetEvent threadStopped = new ManualResetEvent(false);
        ITradeVisualizerListener listener;
        int digits;
        readonly ObservableCollection<TradeVisualizerOrder> liveOrders = new ObservableCollection<TradeVisualizerOrder>();
        readonly ObservableCollection<TradeVisualizerOrder> backtestOrders = new ObservableCollection<TradeVisualizerOrder>();
        readonly ObservableCollection<ITradeVisualizerDebugData> debug = new ObservableCollection<ITradeVisualizerDebugData>();
        class Event
        {
            public DateTime Time;
            public Event()
            {
                Time = DateTime.UtcNow;
            }
        }
        class LogEvent : Event
        {
            public string Message;
            public Color Color;
            public Color DashboardColor;
            public LogEvent(string message, Color color, Color dashboardColor) : base()
            {
                Message = message;
                Color = color;
                DashboardColor = dashboardColor;
            }
        }
        class DiagnosticsEvent : Event 
        {
            public ITradeVisualizerDiagnosticsData Diagnostics;
            public DiagnosticsEvent(ITradeVisualizerDiagnosticsData diagnostics) : base()
            {
                Diagnostics = diagnostics;
            }
        }
        class HistoryOrderEvent : Event
        {
            public TradeOrderInformation Order;
            public bool Backtest;
            public HistoryOrderEvent(TradeOrderInformation order, bool backtest) : base()
            {
                Order = order;
                Backtest = backtest;
            }
        }
        class TradeSignalEvent : Event
        {
            public TradeSignal Signal;
            public TradeSignalEvent(TradeSignal signal) : base()
            {
                Signal = signal;
            }
        }
        readonly List<Event> queue = new List<Event>();
        public TradeVisualizer()
        {
            InitializeComponent();
        }
        public DiagnosticsStringValue AddDiagnosticsString(int row, int col)
        {
            var cell = new DiagnosticsStringValue(row, col);
            diagnosticsControl.Values.Add(cell);
            return cell;
        }

        public DiagnosticsDoubleValue AddDiagnosticsDouble(int row, int col, string format = "F0")
        {
            var cell = new DiagnosticsDoubleValue(row, col, format);
            diagnosticsControl.Values.Add(cell);
            return cell;
        }

        public DiagnosticsDateTimeValue AddDiagnosticsDateTime(int row, int col, string format = "yyyy.MM.dd HH:mm:ss")
        {
            var cell = new DiagnosticsDateTimeValue(row, col, format);
            diagnosticsControl.Values.Add(cell);
            return cell;
        }
        public DiagnosticsIntValue AddDiagnosticsInt(int row, int col, string format = null)
        {
            var cell = new DiagnosticsIntValue(row, col, format);
            diagnosticsControl.Values.Add(cell);
            return cell;
        }
        public void InitializeDiagnostics(int diagnosticsRowsCount, int diagnosticsColumnsCount)
        {
            diagnosticsControl.RowsCount = diagnosticsRowsCount;
            diagnosticsControl.ColumnsCount = diagnosticsColumnsCount;
        }
        static void ClearChart(ChartGroup cg)
        {
            cg.Clear();
            cg.Settings.FirstShowMaximumBars = 150;
            cg.Settings.ShowLegend = false;
            cg.Settings.ShiftOnUpdate = true;
            //cg.Settings.ScaleDateFormat2 = "HH:mm:ss.ffffff";
            cg.Settings.Foreground = Color.FromRgb(0xff, 0xff, 0xff);
            cg.Settings.Background = Color.FromRgb(0x00, 0x23, 0x44);
            cg.Settings.NavigatorBackground = Color.FromRgb(0x00, 0x6d, 0xc5);
            cg.Settings.NavigatorThumb = Color.FromRgb(0x20, 0x8d, 0xe5);
            cg.Settings.TransparentPanel = Color.FromRgb(0x00, 0x23, 0x44);
            cg.Settings.ChartBackground = Color.FromRgb(0x52, 0xbb, 0xea);
            cg.Settings.GridPen = Color.FromArgb(50, 0x00, 0x23, 0x44);
        }
        public ChartGroup.InternalChart CreateChartPane(bool backtest, string name, int digits, double scale)
        {
            ChartGroup cg = backtest ? vmBacktest : vmLive;
            return cg.CreateChart(name, true, scale, digits);
        }
        public void AddDebugColumn<TItem>(string header, TItem item, string name, int index)
        {
            Type t = item.GetType();
            var property = t.GetProperty(name);
            DiagnosticsValue diagnosticsValue;
            if (property.PropertyType.IsArray)
            {
                IList array = property.GetValue(item) as IList;
                diagnosticsValue = array[index] as DiagnosticsValue;
            }
            else
            {
                diagnosticsValue = property.GetValue(item) as DiagnosticsValue;
            }
            var binding = new Binding(property.PropertyType.IsArray ? $"{name}[{index}].Value" : $"{name}.Value")
            {
                ConverterCulture = CultureInfo.InvariantCulture
            };
            if (!string.IsNullOrEmpty(diagnosticsValue.Format))
            {
                binding.StringFormat = diagnosticsValue.Format;
            }
            var column = new DataGridTextColumn
            {
                Header = header,
                Binding = binding,
                IsReadOnly = true,
            };
            dgDebug.Columns.Add(column);

        }
        FileStream fsTrace;
        public void Start(ITradeVisualizerListener listener,AlgoControlModel control, int digits, bool backtest)
        {
            this.listener = listener;
            this.control = control;
            this.digits = digits;
            this.backtest = backtest;
            logControl.Text = "";
            queue.Clear();
            liveOrders.Clear();
            backtestOrders.Clear();
            debug.Clear();
            dgLiveOrders.ItemsSource = liveOrders;
            dgBacktestOrders.ItemsSource = backtestOrders;
            diagnosticsControl.Values.Clear();
            dgDebug.Columns.Clear();
            dgDebug.ItemsSource = debug;
            ClearChart(vmLive);
            ClearChart(vmBacktest);
            tiLiveChart.Visibility = control.ViewChart ? Visibility.Visible : Visibility.Collapsed;
            tiBacktestChart.Visibility = control.ViewChart && backtest ? Visibility.Visible : Visibility.Collapsed;
            tiLiveOrders.Visibility = control.ViewOrders ? Visibility.Visible : Visibility.Collapsed;
            tiBacktestOrders.Visibility = control.ViewOrders && backtest ? Visibility.Visible : Visibility.Collapsed;
            tiDebug.Visibility = control.ViewDebug ? Visibility.Visible : Visibility.Collapsed;
            if (control.SaveTrace && !backtest)
            {
                string traceFolder = Path.Combine(Path.GetDirectoryName(typeof(TradeVisualizer).Assembly.Location),".traces");
                Directory.CreateDirectory(traceFolder);
                string traceTitle = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
                listener.SaveTraceInfo(Path.Combine(traceFolder, $"{traceTitle}.xml"));
                fsTrace = new FileStream(Path.Combine(traceFolder, $"{traceTitle}.trace"), FileMode.Create);
            }
            else
            {
                fsTrace = null;
            }
            threadStop.Reset();
            threadStopped.Reset();
            new Thread(ThreadProc).Start();
        }
        public void Stop(bool wait)
        {
            threadStop.Set();
            if (wait) threadStopped.WaitOne();
        }
        public void LogInfo(string message)
        {
            lock (queue)
            {
                queue.Add(new LogEvent(message, Colors.White, Color.FromRgb(0x00, 0x23, 0x44)));
            }
        }

        public void LogOrderSuccess(string message)
        {
            lock (queue)
            {
                queue.Add(new LogEvent(message, Colors.Green, Color.FromRgb(0, 255, 0)));
            }
        }
        public void LogError(string message)
        {
            lock (queue)
            {
                queue.Add(new LogEvent(message, Color.FromRgb(0xf3, 0x56, 0x51), Color.FromRgb(0xf3, 0x56, 0x51)));
            }
        }
        public void LogWarning(string message)
        {
            lock (queue)
            {
                queue.Add(new LogEvent(message, Colors.LightBlue, Colors.Blue));
            }
        }
        public void Diagnostics(ITradeVisualizerDiagnosticsData diagnostics) 
        {
            lock (queue)
            {
                queue.Add(new DiagnosticsEvent(diagnostics));
            }
        }
        internal void HistoryOrder(TradeOrderInformation order, bool backtest)
        {
            lock (queue)
            {
                queue.Add(new HistoryOrderEvent(order,backtest));
            }
        }
        internal void TradeSignal(TradeSignal signal)
        {
            lock (queue)
            {
                queue.Add(new TradeSignalEvent(signal));
            }
        }
        void SafeInvoke(Action action)
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                if (!Model.Closing)
                {
                    action();
                }
            }));
        }
        void ThreadProc()
        {
            List<ITradeVisualizerDiagnosticsData> diagnosticsData = new List<ITradeVisualizerDiagnosticsData>();
            Queue<TradeOrderInformation> chartPositions = new Queue<TradeOrderInformation>();
            Queue<TradeSignal> tradeSignals = new Queue<TradeSignal>();
            while (!threadStop.WaitOne(100))
            {
                Event[] events = null;
                lock (queue)
                {
                    if (queue.Count>0)
                    {
                        events = queue.ToArray();
                        queue.Clear();
                    }
                }
                if (events!=null)
                {
                    DiagnosticsEvent lastDiagnosticsEvent = null;
                    for (int i = 0; i < events.Length; i++)
                    {
                        if (events[i] is LogEvent le)
                        {
                            Log(le);
                        }
                        else if (events[i] is DiagnosticsEvent de)
                        {
                            lastDiagnosticsEvent = de;
                            if (control.ViewChart || control.ViewDebug)
                            {
                                diagnosticsData.Add(de.Diagnostics);
                            }
                            if (fsTrace!=null)
                            {
                                listener.SaveDiagnosticsToTrace(de.Diagnostics, fsTrace);
                            }
                        }
                        else if (events[i] is HistoryOrderEvent he)
                        {
                            if (control.ViewOrders)
                            {
                                SafeInvoke(() =>
                                {
                                    if (he.Backtest)
                                    {
                                        backtestOrders.Add(new TradeVisualizerOrder(he.Order, digits));
                                    }
                                    else
                                    {
                                        liveOrders.Add(new TradeVisualizerOrder(he.Order, digits));
                                    }
                                });
                                if (fsTrace != null)
                                {
                                    listener.SaveOrderToTrace(he.Order, fsTrace);
                                }
                            }
                            if (control.ViewChart)
                            {
                                chartPositions.Enqueue(he.Order);
                            }
                        }
                        else if (events[i] is TradeSignalEvent ts)
                        {
                            if (control.ViewChart)
                            {
                                tradeSignals.Enqueue(ts.Signal);
                            }
                        }
                    }
                    if (lastDiagnosticsEvent != null)
                    {
                        SafeInvoke(() =>
                        {
                            listener.ShowDiagnostics(lastDiagnosticsEvent.Diagnostics);
                        });
                    }
                    if (diagnosticsData.Count>0)
                    {
                        if (control.ViewChart)
                        {
                            DateTime lastTime = diagnosticsData[diagnosticsData.Count - 1].GetCurrentTime();
                            ITradeVisualizerDiagnosticsData[] chartDataToShow = diagnosticsData.ToArray();
                            List<TradeOrderInformation> chartPositionsToShow = new List<TradeOrderInformation>();
                            List<TradeSignal> tradeSignalsToShow = new List<TradeSignal>();
                            while (chartPositions.Count > 0)
                            {
                                var pos = chartPositions.Peek();
                                if (pos.CloseTime > lastTime || pos.OpenTime>lastTime) break;
                                chartPositionsToShow.Add(chartPositions.Dequeue());
                            }
                            while (tradeSignals.Count > 0)
                            {
                                var sig = tradeSignals.Peek();
                                if (sig.Time > lastTime) break;
                                tradeSignalsToShow.Add(tradeSignals.Dequeue());
                            }
                            SafeInvoke(() =>
                            {
                                listener.ShowChart(chartDataToShow, chartPositionsToShow.ToArray(), tradeSignalsToShow.ToArray());
                            });
                        }
                        if (control.ViewDebug)
                        {
                            var debugData = listener.GetDebug(diagnosticsData);
                            SafeInvoke(() =>
                            {
                                foreach (var d in debugData)
                                {
                                    debug.Add(d);
                                }
                            });
                        }
                        diagnosticsData.Clear();
                    }
                }
            }
            if (fsTrace != null)
            {
                fsTrace.Flush();
                fsTrace.Close();
                fsTrace.Dispose();
                fsTrace = null;
            }

            threadStopped.Set();
        }
        private void Log(LogEvent le)
        {
            string message = le.Time.ToString("HH:mm:ss.ffffff") + "> " + le.Message + "\r\n";
            SafeInvoke(() =>
            {
                Run r = new Run(message)
                {
                    Tag = DateTime.Now,
                    Foreground = new SolidColorBrush(le.Color)
                };
                int count = logControl.Inlines.Count;
                if (count == 0) logControl.Inlines.Add(r);
                else
                {
                    logControl.Inlines.InsertBefore(logControl.Inlines.FirstInline, r);
                }
            });
        }
        private void SelectTabItem(TabItem ti)
        {
            int index = tabs.Items.IndexOf(ti);
            if (index >= 0)
            {
                ti.IsSelected = true;
                tabs.SelectedIndex = index;
                tabs.SelectedItem = ti;
            }
        }
        private void DgLiveOrders_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (dgLiveOrders.SelectedValue is TradeVisualizerOrder order)
            {
                SelectTabItem(tiLiveChart);
                vmLive.ShowDate(order.OpenTime);
                e.Handled = true;
            }
        }

        private void DgBacktestOrders_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (dgBacktestOrders.SelectedValue is TradeVisualizerOrder order)
            {
                SelectTabItem(tiBacktestChart);
                vmBacktest.ShowDate(order.OpenTime);
                e.Handled = true;
            }
        }

        private void DgDebug_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (dgDebug.SelectedValue is ITradeVisualizerDebugData dd)
            {
                if (backtest)
                {
                    SelectTabItem(tiBacktestChart);
                    vmBacktest.ShowDate(dd.GetCurrentTime());
                }
                else
                {
                    SelectTabItem(tiLiveChart);
                    vmLive.ShowDate(dd.GetCurrentTime());
                }
                e.Handled = true;
            }
        }
    }
}

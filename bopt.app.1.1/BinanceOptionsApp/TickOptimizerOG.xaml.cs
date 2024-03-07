using MultiTerminal.Connections;
using MultiTerminal.Connections.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using VisualMarketsEngine;

namespace BinanceOptionsApp
{
    public partial class TickOptimizerOG : UserControl, IConnectorLogger, ITradeTabInterface
    {
        Models.TradeModel model;
        Models.InputItemModel modelTO = new Models.InputItemModel();
        ManualResetEvent threadStop;
        ManualResetEvent threadStopped;
        readonly object loglock = new object();

        public TickOptimizerOG()
        {
            InitializeComponent();
        }

        public void InitializeTab()
        {
            model = DataContext as Models.TradeModel;
            fast.InitializeProviderControl(model.Leg1, true);
            slow.InitializeProviderControl(model.Leg2, false);

            model.LogError = LogError;
            model.LogInfo = LogInfo;
            model.LogWarning = LogWarning;
            model.LogClear = LogClear;
            HiddenLogs.LogHeader(model);
        }
        public void RestoreNullCombo(ConnectionModel cm)
        {
            fast.RestoreNullCombo(cm);
            slow.RestoreNullCombo(cm);
        }

        #region Log's methods:
        public void LogOrderSuccess(string msg)
        {
            Log(msg, Colors.Green, Color.FromRgb(0, 255, 0));
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
            Model.Project = new Models.ProjectModel();
            Model.Project.Inputs.Add(new Models.InputItemModel());
            Model.Project.Inputs.Add(new Models.InputItemModel());
            foreach (var input in Model.Project.Inputs)
            {
                input.Ask = 0;
                input.Bid = 0;
                input.PrevAsk = 0;
                input.PrevBid = 0;
                input.Time = DateTime.MinValue;
                register_chart(input.ChartGroup, input.Chart, input.Digits);
            }

            foreach (var input in Model.Project.Inputs)
            {
                input.VM = create_chart(input.ChartGroup, input.Chart);
                input.FlowBid = input.VM.AddDoubleTimeFlow(input.Chart, TimeFrame.Tick, get_color(input.Style, 0), get_width(input.Style), false);
                input.FlowAsk = input.VM.AddDoubleTimeFlow(input.Chart, TimeFrame.Tick, get_color(input.Style, 1), get_width(input.Style), false);
            }
            //ClearChart();
            threadStop = new ManualResetEvent(false);
            threadStopped = new ManualResetEvent(false);
            update_charts();
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

        #region Chart methods:
        class chart_info
        {
            public string name { get; set; }
            public int digits { get; set; }
            public ChartGroup.InternalChart chart { get; set; }
        }

        class chart_group_info
        {
            public string name { get; set; }
            public ChartGroup cg { get; set; }
            public TabItem item { get; set; }
            public Dictionary<string, chart_info> charts { get; set; } = new Dictionary<string, chart_info>();
        }

        Dictionary<string, chart_group_info> chartGroups = new Dictionary<string, chart_group_info>();
        void register_chart(string group_name, string chart_name, int digits)
        {
            chart_group_info cg = null;
            if (chartGroups.ContainsKey(group_name))
            {
                cg = chartGroups[group_name];
            }
            else
            {
                cg = new chart_group_info();
                cg.name = group_name;
                chartGroups[group_name] = cg;
            }
            chart_info chart = null;
            if (cg.charts.ContainsKey(chart_name))
            {
                chart = cg.charts[chart_name];
            }
            else
            {
                chart = new chart_info();
                chart.name = chart_name;
                cg.charts[chart_name] = chart;
            }
            if (chart.digits < digits) chart.digits = digits;
        }

        ChartGroup.InternalChart create_chart(string group_name, string chart_name)
        {
            chart_group_info cg = chartGroups[group_name];
            if (cg.cg == null)
            {
                cg.cg = new ChartGroup();
                cg.cg.Settings.FirstShowMaximumBars = 150;
                cg.cg.Settings.ShowLegend = false;
                cg.cg.Settings.ShiftOnUpdate = true;
                cg.cg.Settings.ShowMilliseconds = true;
                cg.cg.Settings.ShowSeconds = true;
                cg.item = new TabItem();
                cg.item.Header = group_name;
                cg.item.Content = cg.cg;
                tabs.Items.Add(cg.item);
            }
            chart_info chart = cg.charts[chart_name];
            if (chart.chart == null)
            {
                chart.chart = cg.cg.CreateChart(chart_name, true, 1.0, chart.digits);
            }
            return chart.chart;
        }
        void update_charts()
        {
            if (tabs.Items.Count > 0)
            {
                for (int i = tabs.Items.Count - 1; i >= 0; i--)
                {
                    tabs.SelectedIndex = i;
                    tabs.UpdateLayout();
                }
                tabs.SelectedIndex = 0;
            }
        }
        byte from_style(char v)
        {
            string res = (v + "0").ToUpper();
            return Convert.ToByte(res, 16);
        }
        Color get_color(string style, int no)
        {
            try
            {
                string[] split = style.Split(',');
                byte alpha = byte.Parse(split[split.Length - 1]);
                if (split[no].Length == 4)
                {
                    return Color.FromArgb(alpha, from_style(split[no][1]), from_style(split[no][2]), from_style(split[no][3]));
                }
            }
            catch
            {
            }
            return no == 0 ? Colors.Blue : Colors.Red;
        }
        int get_width(string style)
        {
            try
            {
                string[] split = style.Split(',');
                return byte.Parse(split[split.Length - 2]);
            }
            catch
            {
            }
            return 1;
        }
        #endregion

        decimal prevSlowBid;
        decimal prevSlowAsk;
        decimal prevFastBid;
        decimal prevFastAsk;

        decimal gapBuy;
        decimal gapSell;
        IConnector leg2Connector;
        IConnector leg1Connector;
        string swDebugPath;
        string swQuotesPath;
        string swLogPath;
        FileStream fsData;
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
        class local_input : Proccessors.IProcessorInput
        {
            public Models.InputItemModel model;
            public double Bid { get; set; }
            public double Ask { get; set; }
            public double Point { get; set; }
            public DateTime Time { get; set; }
            public int digits;
        }


        object sync = new object();
        private void ThreadProc()
        {
            model.Leg1.InitView();
            model.Leg2.InitView();
            model.GapBuy = 0;
            model.GapSell = 0;

            prevSlowAsk = 0;
            prevSlowBid = 0;
            prevFastAsk = 0;
            prevFastBid = 0;

            gapSell = 0;
            gapBuy = 0;
            lastClickCloseTime = DateTime.UtcNow.AddDays(-1);
            lastClickOpenTime = DateTime.UtcNow.AddDays(-1);

            leg1Connector = model.Leg1.CreateConnector(this, threadStop, model.SleepMs, Dispatcher);
            leg2Connector = model.Leg2.CreateConnector(this, threadStop, model.SleepMs, Dispatcher);
            leg2Connector.Tick += Leg2Connector_Tick;
            leg1Connector.Tick += Leg1Connector_Tick;
            leg2Connector.LoggedIn += Leg2Connector_LoggedIn;
            leg1Connector.LoggedIn += Leg1Connector_LoggedIn;

            model.LogInfo(model.Title + " logging in...");
            while (!threadStop.WaitOne(100))
            {
                if (leg2Connector.IsLoggedIn && leg1Connector.IsLoggedIn)
                {
                    model.LogInfo(model.Title + " logged in OK.");
                    break;
                }
            }
            if (!threadStop.WaitOne(0))
            {
                if (leg2Connector.IsLoggedIn) OnLeg2Login();
                if (leg1Connector.IsLoggedIn) OnLeg1Login();
            }
            string inputfile = null;
            bool view = true;
            bool save = Model.Project.SaveSession;

            FileStream fi = null;
            if (inputfile != null)
            {
                if (File.Exists(inputfile))
                {
                    view = true;
                    save = false;
                    fi = new FileStream(inputfile, FileMode.Open);
                }
            }

            FileStream fs = null;
            if (save)
            {
                string folder = Models.ConfigModel.ProjectSavesFolder("BinanceOptionsApp");
                string fname = DateTime.Now.ToString("yyyyMMdd HHmmss");
                Model.Project.Save(Path.Combine(folder, fname + ".xml"));
                fs = new FileStream(Path.Combine(folder, fname + ".dat"), FileMode.Create);
            }

            List<local_input> inputs = new List<local_input>();
            for (int i = 0; i < Model.Project.Inputs.Count; i++)
            {
                inputs.Add(new local_input()
                {
                    model = Model.Project.Inputs[i],
                    digits = Model.Project.Inputs[i].Digits
                });
            }

            List<Proccessors.IProcessorInput> iinputs = inputs.Cast<Proccessors.IProcessorInput>().ToList();

            DateTime fiTime = DateTime.MinValue;
            while (!threadStop.WaitOne(model.SleepMs))
            {
                bool changed = false;
                bool valid = true;
                if (fi == null)
                {
                    lock (sync)
                    {
                        for (int i = 0; i < inputs.Count; i++)
                        {
                            changed = changed || inputs[i].model.CopyToPrev();
                            valid = valid && inputs[i].model.IsValid();
                            inputs[i].Bid = inputs[i].model.Bid;
                            inputs[i].Ask = inputs[i].model.Ask;
                            inputs[i].Point = inputs[i].model.Point;
                            inputs[i].Time = inputs[i].model.Time;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < inputs.Count; i++)
                    {
                        inputs[i].Bid = readDouble(fi);
                        inputs[i].Ask = readDouble(fi);
                        inputs[i].Time = new DateTime(readLong(fi));
                        inputs[i].Point = inputs[i].model.Point;
                    }
                    long ticks = readLong(fi);
                    if (ticks == 0) break;
                    fiTime = new DateTime(ticks);
                    changed = true;
                }
                if (changed && valid)
                {
                    Model.Project.InputTime = fi != null ? fiTime : DateTime.UtcNow;

                    if (view && !Model.Closing)
                    {
                        DoubleTimeValue[] v = new DoubleTimeValue[1];
                        for (int i = 0; i < inputs.Count; i++)
                        {
                            v[0] = new DoubleTimeValue(inputs[i].Bid, Model.Project.InputTime);
                            inputs[i].model.VM.AddDoubleTimeValuesTo(inputs[i].model.FlowBid, v);
                            v[0] = new DoubleTimeValue(inputs[i].Ask, Model.Project.InputTime);
                            inputs[i].model.VM.AddDoubleTimeValuesTo(inputs[i].model.FlowAsk, v);
                        }
                    }
                    if (save)
                    {
                        for (int i = 0; i < inputs.Count; i++)
                        {
                            byte[] bid = BitConverter.GetBytes(inputs[i].Bid);
                            fs.Write(bid, 0, 8);
                            byte[] ask = BitConverter.GetBytes(inputs[i].Ask);
                            fs.Write(ask, 0, 8);
                            byte[] btime = BitConverter.GetBytes(inputs[i].Time.Ticks);
                            fs.Write(btime, 0, 8);
                        }
                        byte[] time = BitConverter.GetBytes(Model.Project.InputTime.Ticks);
                        fs.Write(time, 0, 8);
                    }
                }

            }
            leg1Connector.Tick -= Leg1Connector_Tick;
            leg2Connector.Tick -= Leg2Connector_Tick;
            leg1Connector.LoggedIn -= Leg1Connector_LoggedIn;
            leg2Connector.LoggedIn -= Leg2Connector_LoggedIn;
            //ConnectorsFactory.Current.CloseNonStateConnectors(fastConnector);
            //ConnectorsFactory.Current.CloseNonStateConnectors(slowConnector);
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

        byte[] tmp = new byte[8];
        double readDouble(FileStream fs)
        {
            int readed = fs.Read(tmp, 0, 8);
            if (readed == 8)
            {
                return BitConverter.ToDouble(tmp, 0);
            }
            return 0.0;
        }

        long readLong(FileStream fs)
        {
            int readed = fs.Read(tmp, 0, 8);
            if (readed == 8)
            {
                return BitConverter.ToInt64(tmp, 0);
            }
            return 0;
        }

        void OnLeg1Login()
        {
            leg1Connector.Fill = (FillPolicy)model.Open.Fill;
            leg1Connector.Subscribe(model.Leg1.FullSymbol, model.Leg1.Symbol, Models.TradeAlgorithm.TickOptimizerOG.ToString());
        }

        void OnLeg2Login()
        {
            leg2Connector.Fill = (FillPolicy)model.Open.Fill;
            leg2Connector.Subscribe(model.Leg2.FullSymbol, model.Leg2.Symbol, Models.TradeAlgorithm.TickOptimizerOG.ToString());
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
        private void Leg1Connector_Tick(object sender, TickEventArgs e)
        {
            if (e.Symbol == model.Leg1.FullSymbol)
            {
                //inputs[0] = new local_input { Ask = (double)e.Ask, Bid = (double)e.Bid, Time = DateTime.Now };
                Model.Project.Inputs[0].Ask = (double)e.Bid;
                Model.Project.Inputs[0].Bid = (double)e.Bid;
                Model.Project.Inputs[0].Time = DateTime.Now;
                model.Leg1.Bid = e.Bid;
                model.Leg1.Ask = e.Ask;
                model.Leg1.Time = DateTime.Now;
            }
        }

        private void Leg2Connector_Tick(object sender, TickEventArgs e)
        {
            if (e.Symbol == model.Leg2.FullSymbol)
            {
                //inputs[1] = new local_input { Ask = (double)e.Ask, Bid = (double)e.Bid, Time = DateTime.Now };
                Model.Project.Inputs[1].Ask = (double)e.Ask;
                Model.Project.Inputs[1].Bid = (double)e.Bid;
                Model.Project.Inputs[1].Time = DateTime.Now;
                model.Leg2.Bid = e.Bid;
                model.Leg2.Ask = e.Ask;
                model.Leg2.Time = DateTime.Now;
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

        #region Logs private interface implements:
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
        #endregion

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

        private void tabsNavigateTo(DateTime time)
        {
            foreach (var item in tabs.Items)
            {
                TabItem ti = item as TabItem;
                if (ti != null)
                {
                    ChartGroup cg = ti.Content as ChartGroup;
                    if (cg != null)
                    {
                        cg.ShowDate(time);
                    }
                }
            }
        }

        private void NavigateTo_Click(object sender, RoutedEventArgs e)
        {
            StringValueDialog svd = new StringValueDialog("Time", DateTime.UtcNow.ToString("yyyy.MM.dd HH:mm:ss"), this);
            if (svd.ShowDialog() == true)
            {
                try
                {
                    DateTime time = DateTime.Parse(svd.StringResult);
                    tabsNavigateTo(time);
                }
                catch
                {
                }
            }
        }

        private void DgLog_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
    }
}

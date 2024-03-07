using Arbitrage.Api.Enums;
using Arbitrage.Api.Security;
using BinanceOptionApp;
using MultiTerminal.Connections;
using MultiTerminal.Connections.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BinanceOptionsApp
{
    public partial class MainWindow : Window
    {
        public List<TabItem> TabItems;
        TabItem tabAdd;
        TabItem tabDash;

        public MainWindow()
        {
            InitializeComponent();
            Title = "Binance Options App";
            //this.WindowStyle = WindowStyle.None;
            //this.AllowsTransparency = true;
        }

        void CreateTmpFolder()
        {
            string tmpFolder = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), ".tmp");
            try
            {
                System.IO.Directory.CreateDirectory(tmpFolder);
            }
            catch
            {

            }
        }

        string commonLogPath;
        readonly object commonLogLock = new object();

        static bool isNormalState = true;
        private void BuFullScrn_Click(object sender, RoutedEventArgs e)
        {
            if (isNormalState)
            {
                isNormalState = false;
                this.WindowState = WindowState.Maximized; // Збільшення вікна на весь екран
            }
            else
            {
                isNormalState = true;
                this.WindowState = WindowState.Normal; // Повернення до звичайного розміру вікна
            }
        }

        public void CommonLogSave(string message)
        {
            lock (commonLogLock)
            {
                System.IO.File.AppendAllText(commonLogPath, message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //tbSerialNumber.Text = "#" + Arbitrage.App.SerialNumber.Value;
            CustomStyles.CustomWindowSettings.SetUseImageBackground(this, true);
            CustomStyles.CustomWindowSettings.SetImageBackground(this, new SolidColorBrush(Colors.Transparent));
            CreateTmpFolder();

            string stime = DateTime.Now.ToString("yyyyMMddHHmmss");
            string logfolder = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\.logs";
            try
            {
                System.IO.Directory.CreateDirectory(logfolder);
            }
            catch
            {
            }
            commonLogPath = System.IO.Path.Combine(logfolder, "applog_" + stime + ".log");

            Model.CommonLogSave = CommonLogSave;
            Model.Loading = true;
            Model.ConnectionsConfig = ConnectionsModel.Load();
            Model.Current = Models.ConfigModel.Load();
            Model.Options = Models.OptionsModel.Load();
            Model.EMailSender = new Helpers.EMailSenderHelper();

            List<ConnectionModel> deletedConnections = new List<ConnectionModel>();
            foreach (var connection in Model.ConnectionsConfig.Connections)
            {
                if (!Model.IsBrokerPresent(connection.GetBrokerCode())) deletedConnections.Add(connection);
            }
            foreach (var connection in deletedConnections) Model.ConnectionsConfig.Connections.Remove(connection);

            List<Models.TradeModel> deletedTabs = new List<Models.TradeModel>();
            foreach (var tab in Model.Current.Tabs)
            {
                if (tab.Algo == Models.TradeAlgorithm.LatencyArbitrage && !Model.UseOneLeg) deletedTabs.Add(tab);
                if (tab.Algo == Models.TradeAlgorithm.TwoLegFutures && !Model.UseTwoLegSimple) deletedTabs.Add(tab);
                if (tab.Algo == Models.TradeAlgorithm.ZigZag && !Model.UseOneLeg) deletedTabs.Add(tab);
                if (tab.Algo == Models.TradeAlgorithm.TwoLegArbitrage && !Model.UseTwoLegSimple) deletedTabs.Add(tab);
                if (tab.Algo == Models.TradeAlgorithm.MultiLegSpread && !Model.UseMultiLeg) deletedTabs.Add(tab);
                if (tab.Algo == Models.TradeAlgorithm.TickOptimizer && !Model.UseOneLeg) deletedTabs.Add(tab);
                if (tab.Algo == Models.TradeAlgorithm.TickOptimizerOG && !Model.UseOneLeg) deletedTabs.Add(tab);
                if (tab.Algo == Models.TradeAlgorithm.Scalper && !Model.UseOneLeg) deletedTabs.Add(tab);
            }
            foreach (var tab in deletedTabs) Model.Current.Tabs.Remove(tab);
            Model.Current.Save();
            Model.ConnectionsConfig.Save();
            foreach (var cm in Model.ConnectionsConfig.Connections)
            {
                Model.AllConnections.Add(cm);
            }

            Title = App.LanguageKey("locTitle");

            TabItems = new List<TabItem>();
            tabAdd = new TabItem
            {
                Header = new AddHeader()
            };
            tabAdd.MouseLeftButtonUp += TabAdd_MouseLeftButtonUp;
            tabDash = new TabItem
            {
                Header = new DashHeader(),
                Content = new Dashboard(),
                DataContext = Model.Current
            };
            TabItems.Add(tabDash);
            TabItems.Add(tabAdd);

            foreach (var trade in Model.Current.Tabs)
            {
                AddTabItem(trade);
            }

            tabDynamic.DataContext = TabItems;
            tabDynamic.SelectedIndex = 0;
            Model.Loading = false;
            HiddenLogs.Start();
            new Thread(BackgroundThread).Start();
        }

        public void StopAll(bool wait)
        {
            foreach (var tab in TabItems)
            {
                if (tab.Content is ITradeTabInterface)
                {
                    (tab.Content as ITradeTabInterface).Stop(wait);
                }
            }
        }

        public void StartAll()
        {
            foreach (var tab in TabItems)
            {
                if (tab.Content is ITradeTabInterface)
                {
                    (tab.Content as ITradeTabInterface).Start();
                }
            }
        }

        public void StopOne(Models.TradeModel model, bool wait)
        {
            foreach (var tab in TabItems)
            {
                if (tab.Content is ITradeTabInterface && tab.DataContext is Models.TradeModel)
                {
                    Models.TradeModel tabModel = tab.DataContext as Models.TradeModel;
                    if (tabModel == model)
                    {
                        (tab.Content as ITradeTabInterface).Stop(wait);
                    }
                }
            }
        }

        public void StartOne(Models.TradeModel model)
        {
            foreach (var tab in TabItems)
            {
                if (tab.Content is ITradeTabInterface && tab.DataContext is Models.TradeModel)
                {
                    Models.TradeModel tabModel = tab.DataContext as Models.TradeModel;
                    if (tabModel == model)
                    {
                        (tab.Content as ITradeTabInterface).Start();
                    }
                }
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (e is null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            Model.Closing = true;
            Model.Current.Save();
            StopAll(false);
            ConnectorsFactory.Current.CloseAll(false);
            Model.EMailSender.Dispose();
            HiddenLogs.Stop();
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr FindWindowW(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, ref uint lpdwProcessId);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern bool SendMessage(IntPtr hWnd, uint Msg, int wParam, StringBuilder lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SendMessage(int hWnd, int Msg, int wparam, int lparam);

        const int WM_GETTEXT = 0x000D;
        const int WM_GETTEXTLENGTH = 0x000E;

        public string GetControlText(IntPtr hWnd)
        {
            // Get the size of the string required to hold the window title (including trailing null.) 
            Int32 titleSize = SendMessage((int)hWnd, WM_GETTEXTLENGTH, 0, 0).ToInt32();

            // If titleSize is 0, there is no title so return an empty string (or null)
            if (titleSize == 0)
                return String.Empty;

            StringBuilder title = new StringBuilder(titleSize + 1);

            SendMessage(hWnd, (int)WM_GETTEXT, title.Capacity, title);

            return title.ToString();
        }

        void BackgroundThread()
        {
            while (!Model.Closing)
            {
                bool clicker = Model.Options.Clicker.UseClickerForClose || Model.Options.Clicker.UseClickerForOpen;
                if (clicker)
                {
                    IntPtr handle = FindWindowW("#32770", null);
                    if (handle != IntPtr.Zero)
                    {
                        uint pid = 0;
                        GetWindowThreadProcessId(handle, ref pid);
                        if (pid != 0)
                        {
                            var process = Process.GetProcessById((int)pid);
                            if (process != null)
                            {
                                if (process.ProcessName.ToLower() == "terminal" || process.ProcessName.ToLower() == "terminal64")
                                {
                                    string txt = GetControlText(handle);
                                    if (!string.IsNullOrEmpty(txt))
                                    {
                                        if (txt.ToLower().Contains("requote"))
                                        {
                                            SendMessage(handle, 0x0010, IntPtr.Zero, IntPtr.Zero);
                                        }
                                    }
                                }
                            }

                        }
                    }
                    Thread.Sleep(50);
                }
                else
                {
                    Thread.Sleep(250);
                }
            }
        }

        // Create New Tab Item
        UserControl CreateTradeTab(Models.TradeAlgorithm algo)
        {
            if (algo == Models.TradeAlgorithm.ZigZag)
                return new TradeZigZag();
            if (algo == Models.TradeAlgorithm.MultiLegSpread) 
                return new TradeMultiLegSpread();
            if (algo == Models.TradeAlgorithm.LatencyArbitrage) 
                return new TradeLatencyArbitrage();
            if (algo == Models.TradeAlgorithm.TwoLegFutures)
                return new TwoLegFutures();
            if (algo == Models.TradeAlgorithm.TwoLegArbFutureUSD_M)
                return new TwoLegArbFutureUSD_M();
            if (algo == Models.TradeAlgorithm.TickOptimizer)
                return new TickOptimizer();
            if (algo == Models.TradeAlgorithm.TickOptimizerOG)
                return new TickOptimizerOG();
            if (algo == Models.TradeAlgorithm.Scalper)
                return new TradeScalper();
            return new TradeOneLeg();
        }

        private TabItem AddTabItem(Models.TradeModel model)
        {
            int count = TabItems.Count;
            TabItem tab = new TabItem
            {
                HeaderTemplate = tabDynamic.FindResource("TabHeader") as DataTemplate,
                DataContext = model,
                Content = CreateTradeTab(model.Algo)
            };
            (tab.Content as ITradeTabInterface).InitializeTab();
            TabItems.Insert(count - 1, tab);
            return tab;
        }

        DateTime lastLoginTime = DateTime.UtcNow.AddDays(-1);
        bool loginError = false;
        private void TabAdd_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if ((DateTime.UtcNow - lastLoginTime).TotalMinutes >= 5)
            {
                var client = new Arbitrage.Api.Clients.Client(App.ServerAddress, new Arbitrage.Api.Json.Net.ClientJsonConverter(), "BinanceOptionsApp", App.Login.Encrypt(App.ClientCryptoKey), App.HostId.Encrypt(App.ClientCryptoKey), Arbitrage.App.SerialNumber.Value, App.ClientVersion);
                // var response = await client.SubscriptionLogin(false);
                var response = Data.SubscribeRequest.Subscribe;
                bool loginOk = false;
                if (response != null && response.Status == ResponseStatus.Ok)
                {
                    loginOk = true;
                }
                if (!loginOk)
                {
                    loginError = true;
                }
                lastLoginTime = DateTime.UtcNow;
            }
            if (loginError) return;

            if (Model.UseOneLeg || Model.UseTwoLegSimple || Model.UseMultiLeg )
            {
                Models.TradeAlgorithm[] algos = Enum.GetValues(typeof(Models.TradeAlgorithm)) as Models.TradeAlgorithm[];
                ContextMenu menu = new ContextMenu();
                foreach (var algo in algos)
                {
                    //if (algo == Models.TradeAlgorithm.OneLegHedge && !Model.UseOneLegHedge) continue;
                    //if (algo == Models.TradeAlgorithm.OneLeg && !Model.UseOneLeg) continue;
                    //if (algo == Models.TradeAlgorithm.TwoLegLock && !Model.UseTwoLegLock) continue;
                    //if (algo == Models.TradeAlgorithm.TwoLegSimpleHedge && !Model.UseTwoLegSimple) continue;
                    //if (algo == Models.TradeAlgorithm.MultiLegSpread && !Model.UseMultiLeg) continue;
                    //if (algo == Models.TradeAlgorithm.OneLegMulti && !Model.UseOneLegMulti) continue;
                    MenuItem item = new MenuItem
                    {
                        Header = App.LanguageKey("locAlgoCombo1Leg")
                    };
                    if (algo == Models.TradeAlgorithm.TwoLegArbFutureUSD_M) item.Header = "Two Leg Arb Future USD_M";
                    if (algo == Models.TradeAlgorithm.LatencyArbitrage) item.Header = "Latency Arbitrage";//App.LanguageKey("locAlgoCombo2LegLock");
                    if (algo == Models.TradeAlgorithm.TwoLegFutures) item.Header = "Two Leg Futures";//App.LanguageKey("locAlgoCombo2LegLock");
                    if (algo == Models.TradeAlgorithm.ZigZag) item.Header = "Zig Zag";//App.LanguageKey("locAlgoCombo2LegLock");
                    if (algo == Models.TradeAlgorithm.TwoLegArbitrage) item.Header = "Two Leg Arbitrage"; //App.LanguageKey("locAlgoCombo2LegSimple");
                    if (algo == Models.TradeAlgorithm.MultiLegSpread) item.Header = App.LanguageKey("locAlgoComboMultileg");
                    if (algo == Models.TradeAlgorithm.TickOptimizer) item.Header = "Tick Optimizer";
                    if (algo == Models.TradeAlgorithm.TickOptimizerOG) item.Header = "Tick Optimizer OG";
                    if (algo == Models.TradeAlgorithm.Scalper) item.Header = "Scalper";
                    //if (algo == Models.TradeAlgorithm.OneLegMulti) item.Header = "OneLegMulti";
                    item.Tag = algo;
                    menu.Items.Add(item);
                    item.Click += Item_Click;
                }
                menu.IsOpen = true;
            }
        }

        private void Item_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem item)
            {
                tabDynamic.DataContext = null;
                Models.TradeModel trade = new Models.TradeModel
                {
                    Algo = (Models.TradeAlgorithm)item.Tag
                };               
                Model.Current.Tabs.Add(trade);
                TabItem tab = AddTabItem(trade);
                tabDynamic.DataContext = TabItems;
                tabDynamic.SelectedItem = tab;
            }
        }

        private void TabDynamic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(tabDynamic.SelectedItem is TabItem tab)) return;
            if (tab.Equals(tabAdd))
            {
                if (e.RemovedItems.Count > 0)
                {
                    tabDynamic.SelectedItem = e.RemovedItems[0];
                }
            }
            else
            {
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).CommandParameter is TabItem tab)
            {
                tabDynamic.DataContext = null;
                (tab.Content as ITradeTabInterface).Stop(true);
                TabItems.Remove(tab);
                Models.TradeModel trade = tab.DataContext as Models.TradeModel;
                Model.Current.Tabs.Remove(trade);
                tabDynamic.DataContext = TabItems;
                if (!(tabDynamic.SelectedItem is TabItem selectedTab) || selectedTab.Equals(tab))
                {
                    selectedTab = TabItems[0];
                }
                tabDynamic.SelectedItem = selectedTab;
            }
        }

        void CancelEdit(object sender)
        {
            if (sender is TextBox tb)
            {
                TabItem ti = GetParentTabItem(sender);
                if (ti != null)
                {
                    if (ti.DataContext is Models.TradeModel trade)
                    {
                        trade.Title = tb.Text;
                        trade.IsInEditMode = false;
                    }
                }
            }
        }

        private void EditHeader_LostFocus(object sender, RoutedEventArgs e)
        {
            CancelEdit(sender);
        }

        private void EditHeader_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape || e.Key == Key.Enter || e.Key == Key.Tab)
            {
                CancelEdit(sender);
            }
        }

        private void HeaderGrid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount > 1)
            {
                TabItem ti = GetParentTabItem(sender);
                if (ti != null)
                {
                    if (ti.DataContext is Models.TradeModel trade && ti.IsSelected)
                    {
                        if (!trade.IsInEditMode)
                        {
                            var tb = GetChildTextBox(sender);
                            if (tb != null)
                            {
                                trade.IsInEditMode = true;
                                tb.Text = trade.Title;
                                tb.Focus();
                                tb.CaretIndex = tb.Text.Length;
                                e.Handled = true;
                            }
                        }
                    }
                }
            }
        }

        TabItem GetParentTabItem(object obj)
        {
            DependencyObject current = obj as DependencyObject;
            while (current != null)
            {
                if (current is TabItem) return current as TabItem;
                current = VisualTreeHelper.GetParent(current);
            }
            return null;
        }

        TextBox GetChildTextBox(object obj)
        {
            DependencyObject current = obj as DependencyObject;
            while (current != null)
            {
                if (current is TextBox) return current as TextBox;
                current = VisualTreeHelper.GetChild(current, 3);
            }
            return null;
        }

        private void HeaderGrid_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            TabItem ti = GetParentTabItem(sender);
            if (ti != null)
            {
                if (ti.DataContext is Models.TradeModel trade && ti.IsSelected)
                {
                    if (!trade.IsInEditMode)
                    {
                        var tb = GetChildTextBox(sender);
                        if (tb != null)
                        {
                            trade.IsInEditMode = true;
                            tb.Text = trade.Title;
                            tb.Focus();
                            tb.CaretIndex = tb.Text.Length;
                            e.Handled = true;
                        }
                    }
                }
            }
        }

        private void BuOptions_Click(object sender, RoutedEventArgs e)
        {
            OptionsDialog dlg = new OptionsDialog
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            dlg.ShowDialog();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void buClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void buZvernuty_Click(object sender, RoutedEventArgs e)
        {
            //Hide();
        }

        private void buZvernuty_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}

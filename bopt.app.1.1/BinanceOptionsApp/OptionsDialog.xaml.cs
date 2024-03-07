using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace BinanceOptionsApp
{
    public partial class OptionsDialog : Window
    {
        public OptionsDialog()
        {
            InitializeComponent();
        }
        Models.OptionsModel model;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            model = new Models.OptionsModel();
            model.From(Model.Options);
            DataContext = model;
            Title = App.LanguageKey("locOptions");
            EventManager.RegisterClassHandler(typeof(Window), KeyDownEvent, new KeyEventHandler(keyDown), true);
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            closeMarker();
        }
        private void BuAccept_Click(object sender, RoutedEventArgs e)
        {
            Model.Options.From(model);
            Model.Options.Save();
            DialogResult = true;
        }
        private void BuCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void buTest_Click(object sender, RoutedEventArgs e)
        {
            string msg = "Test message with settings:\r\n";
            msg += "Server: " + model.Smtp.Server+"\r\n";
            msg += "Port: " + model.Smtp.Port + "\r\n";
            msg += "Sender: " + model.Smtp.Sender + "\r\n";
            msg += "Password: " + model.Smtp.Password + "\r\n";
            msg += "SSL: " + model.Smtp.SSL + "\r\n";
             
            Model.EMailSender.Push("Westernpips Private 7", msg, model.Smtp.Clone());
        }


        int cmode;
        PriceMarker marker;
        private void buSetBuyCoord_Click(object sender, RoutedEventArgs e)
        {
            cmode = 10;
        }

        private void buShowBuyCoord_Click(object sender, RoutedEventArgs e)
        {
            cmode = 11;
            showMarker(model.Clicker.XBuy, model.Clicker.YBuy, MarkerMode.Buy);
        }

        private void buClickBuyCoord_Click(object sender, RoutedEventArgs e)
        {
            model.Clicker.ClickBuy();
        }

        private void buSetSellCoord_Click(object sender, RoutedEventArgs e)
        {
            cmode = 20;
        }

        private void buShowSellCoord_Click(object sender, RoutedEventArgs e)
        {
            cmode = 21;
            showMarker(model.Clicker.XSell, model.Clicker.YSell, MarkerMode.Sell);
        }

        private void buClickSellCoord_Click(object sender, RoutedEventArgs e)
        {
            model.Clicker.ClickSell();
        }
        private void buSetCloseCoord_Click(object sender, RoutedEventArgs e)
        {
            cmode = 30;
        }

        private void buShowCloseCoord_Click(object sender, RoutedEventArgs e)
        {
            cmode = 31;
            showMarker(model.Clicker.XClose, model.Clicker.YClose, MarkerMode.Close);
        }
        private void buClickCloseCoord_Click(object sender, RoutedEventArgs e)
        {
            model.Clicker.ClickClose();
        }
        void closeMarker()
        {
            if (marker != null)
            {
                marker.Close();
                marker = null;
            }
        }
        enum MarkerMode
        {
            Buy,
            Sell,
            Close
        }
        void showMarker(int x, int y, MarkerMode mode)
        {
            closeMarker();
            marker = new PriceMarker(mode == MarkerMode.Buy ? Brushes.Green : (mode == MarkerMode.Sell ? Brushes.Red : Brushes.LightBlue), Brushes.Transparent, 
                mode == MarkerMode.Buy ? "Buy" : (mode == MarkerMode.Sell ? "Sell" : "Close"));
            int w = 40;
            int h = 40;
            marker.Left = x - w / 2;
            marker.Top = y - h / 2;
            marker.Width = w;
            marker.Height = h;
            marker.AdjustDpi(this);
            marker.Show();
        }
        private void keyDown(object sender, KeyEventArgs e)
        {
            if (e.Key!= Key.LeftCtrl && e.Key!=Key.RightCtrl && e.Key!= Key.LeftShift && e.Key!=Key.RightShift && e.Key!= Key.LeftAlt && e.Key!= Key.RightAlt)
            {
                return;
            }
            if (cmode == 0) return;
            if (cmode == 10) cmode = 11;
            else if (cmode == 20) cmode = 21;
            else if (cmode == 30) cmode = 31;
            else cmode = 0;
            if (cmode == 0)
            {
                closeMarker();
                return;
            }
            var pt = MouseCapture.GetMousePosition();
            showMarker((int)pt.X, (int)pt.Y, cmode < 20 ? MarkerMode.Buy : (cmode<30 ? MarkerMode.Sell : MarkerMode.Close));

            if (cmode < 20)
            {
                model.Clicker.XBuy = (int)pt.X;
                model.Clicker.YBuy = (int)pt.Y;
            }
            else if (cmode<30)
            {
                model.Clicker.XSell = (int)pt.X;
                model.Clicker.YSell = (int)pt.Y;
            }
            else
            {
                model.Clicker.XClose = (int)pt.X;
                model.Clicker.YClose = (int)pt.Y;
            }
        }

    }
}

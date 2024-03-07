using MultiTerminal.Connections;
using System.Windows;

namespace BinanceOptionsApp
{
    public partial class OrderTypeEditor : Window
    {
        public OrderType OrderType { get; set; }
        public int PendingDistance { get; set; }
        public int PendingLifeTime { get; set; }
        public Models.FillPolicy? Fill { get; set; }
        public OrderTypeEditor(OrderType orderType, int pendingDistance, int pendingLifetime, Models.FillPolicy? fill)
        {
            InitializeComponent();
            OrderType = orderType;
            PendingDistance = pendingDistance;
            PendingLifeTime = pendingLifetime;
            Fill = fill;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbOrderType.Items.Add(App.LanguageKey("locOrderTypeMarket"));
            cbOrderType.Items.Add(App.LanguageKey("locOrderTypeLimit"));
            cbOrderType.Items.Add(App.LanguageKey("locOrderTypeStop"));
            cbOrderType.SelectedIndex = (int)OrderType;
            tbPendingDistance.Text = PendingDistance.ToString();
            tbPendingLifetime.Text = PendingLifeTime.ToString();
            if (Fill != null)
            {
                cbFill.Items.Add("FOK");
                cbFill.Items.Add("IOC");
                cbFill.Items.Add("FILL");
                cbFill.SelectedIndex = (int)Fill.Value;
            }
            else
            {
                lbFill.Visibility = Visibility.Collapsed;
                cbFill.Visibility = Visibility.Collapsed;
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void BuOk_Click(object sender, RoutedEventArgs e)
        {
            OrderType = (OrderType)cbOrderType.SelectedIndex;
            if (Fill != null)
            {
                Fill = (Models.FillPolicy)cbFill.SelectedIndex;
            }
            try
            {
                PendingLifeTime = int.Parse(tbPendingLifetime.Text);
            }
            catch
            {

            }
            try
            {
                PendingDistance = int.Parse(tbPendingDistance.Text);
            }
            catch
            {

            }
            DialogResult = true;
        }

        private void BuCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}

namespace BinanceOptionsApp.Editors
{
    using global::MultiTerminal.Connections.Models;
    using System.Windows;

    public partial class EditorTestNetSpot : Window, IEditorBase<TestnetSpotConnectionModel>
    {
        public TestnetSpotConnectionModel Model { get; set; }
        public void Construct(TestnetSpotConnectionModel source)
        {
            Model = new TestnetSpotConnectionModel();
            Model.From(source);
        }
        public EditorTestNetSpot()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = Model;
            Title = "BINANCE TESTNET SPOT" + App.LanguageKey("locDlgConnection");
            cbTradeType.Items.Add(AccountTradeType.SPOT.ToString());
            cbTradeType.Items.Add(AccountTradeType.MARGIN.ToString());
            cbTradeType.SelectedIndex = (int)Model.AccountTradeType;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }
        private void BuAccept_Click(object sender, RoutedEventArgs e)
        {
            Model.AccountTradeType = (AccountTradeType)cbTradeType.SelectedIndex;
            DialogResult = true;
        }
        private void BuCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}

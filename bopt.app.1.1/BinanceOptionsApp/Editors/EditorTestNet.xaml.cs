using MultiTerminal.Connections.Models;
using System.Windows;

namespace BinanceOptionsApp.Editors
{
    /// <summary>
    /// Interaction logic for EditorTestNet.xaml
    /// </summary>
    public partial class EditorTestNet : Window, IEditorBase<TestnetConnectionModel>
    {
        public TestnetConnectionModel Model { get; set; }
        public void Construct(TestnetConnectionModel source)
        {
            Model = new TestnetConnectionModel();
            Model.From(source);
        }
        public EditorTestNet()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = Model;
            Title = "BINANCE TESTNET " + App.LanguageKey("locDlgConnection");
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

using MultiTerminal.Connections.Models;
using System.Windows;

namespace BinanceOptionsApp.Editors
{
    public partial class EditorBinance : Window, IEditorBase<BinanceConnectionModel>
    {
        public BinanceConnectionModel Model { get; set; }

        public void Construct(BinanceConnectionModel source)
        {
            Model = new BinanceConnectionModel();
            Model.From(source);
        }
        public EditorBinance()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = Model;
            Title = "BINANCE " + App.LanguageKey("locDlgConnection");
            cbTradeType.Items.Add(AccountTradeType.SPOT.ToString());
            cbTradeType.Items.Add(AccountTradeType.MARGIN.ToString());
            cbTradeType.Items.Add(AccountTradeType.OPTION.ToString());
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

using System.Windows;
using MultiTerminal.Connections.Models;

namespace BinanceOptionsApp.Editors
{
    public partial class BinanceOptions : Window, IEditorBase<BinanceOptionConnectionModel>
    {
        public BinanceOptionConnectionModel Model { get; set; }

        public BinanceOptions()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = Model;
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

        public void Construct(BinanceOptionConnectionModel source)
        {
            Model = new BinanceOptionConnectionModel();
            Model.From(source);
        }
    }
}

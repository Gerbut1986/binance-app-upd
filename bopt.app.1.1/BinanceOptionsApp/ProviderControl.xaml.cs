using System.Linq;
using System.Windows.Controls;
using MultiTerminal.Connections.Models;

namespace BinanceOptionsApp
{
    public partial class ProviderControl : UserControl
    {
        public ProviderControl()
        {
            InitializeComponent();
        }

        Models.ProviderModel model;
        public void InitializeProviderControl(Models.ProviderModel model, bool showInternalProviders)
        {
            this.model = model;

            this.model.Symbol = model.SymbolAsset + model.SymbolCurrency;
            DataContext = model;
            comboProvider.DisplayMemberPath = "Name";
            var source = showInternalProviders ? Model.AllConnections : Model.ConnectionsConfig.Connections;
            comboProvider.ItemsSource = source;
            var connection = source.FirstOrDefault(x => x.Name == model.Name);
            if (connection!=null)
            {
                comboProvider.SelectedValue = connection;
            }
            else
            {
                comboProvider.SelectedIndex = 0;
            }
            ProviderChanged();
        }

        private void ComboProvider_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProviderChanged();
        }

        private void ComboSymbol_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SymbolChanged();
        }

        public void RestoreNullCombo(ConnectionModel cm)
        {
            if (comboProvider.SelectedValue == null)
            {
                comboProvider.SelectedValue = cm;
            }
        }

        void ProviderChanged()
        {
            bool prefix = false;
            if (comboProvider.SelectedValue is ConnectionModel connection)
            {
                model.Name = connection.Name;
                if (!Model.IsBrokerHasCustomSymbolName(connection.GetBrokerCode()))
                {
                    var allowedInstruments = Model.GetAllowedInstruments(connection.GetBrokerCode());
                    //comboSymbol.Visibility = Visibility.Visible;
                    //editSymbol.Visibility = Visibility.Collapsed;
                    // comboSymbol.ItemsSource = allowedInstruments;
                    // comboSymbol.DisplayMemberPath = "Name";
                    var smb = allowedInstruments.FirstOrDefault(x => x.Name == model.Symbol);
                    if (smb != null)
                    {
                        //comboSymbol.SelectedValue = smb;
                    }
                    else
                    {
                        //comboSymbol.SelectedIndex = 0;
                    }
                }
                else
                {
                    //comboSymbol.Visibility = Visibility.Collapsed;
                    //editSymbol.Visibility = Visibility.Visible;
                    //comboSymbol.ItemsSource = null;
                    //comboSymbol.SelectedValue = null;
                }
            }
            if (prefix)
            {
                //Grid.SetColumnSpan(comboSymbol, 1);
                //Grid.SetColumnSpan(editSymbol, 1);
            }
            else
            {
                //Grid.SetColumnSpan(comboSymbol, 10);
                //Grid.SetColumnSpan(editSymbol, 10);
            }
            SymbolChanged();
        }
        void SymbolChanged()
        {
            //if (comboSymbol.SelectedValue is AllowedInstrument)
            //{
            //    model.Symbol = (comboSymbol.SelectedValue as AllowedInstrument).Name;
            //}
        }

        private void AssetTb_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void CurrencyTb_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
    }
}

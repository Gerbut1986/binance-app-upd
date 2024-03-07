using Dsafa.WpfColorPicker;
using System.Windows;

namespace BinanceOptionsApp
{
    public partial class ProviderEditDialog : Window
    {
        public Models.ProviderModel Model { get; private set; }
        readonly bool showInternalProviders;
        public ProviderEditDialog(Models.ProviderModel model, bool showInternalProviders)
        {
            InitializeComponent();
            Model = model.EditClone();
            this.showInternalProviders = showInternalProviders;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = Model;
            providerControl.InitializeProviderControl(Model,showInternalProviders);
        }
        private void BuAccept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
        private void BuCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Border_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var dialog = new ColorPickerDialog(Model.BidColor)
            {
                Owner = this
            };
            var res = dialog.ShowDialog();
            if (res.HasValue && res.Value)
            {
                Model.BidColor = dialog.Color;
            }

        }
        private void Border_MouseLeftButtonUpAsk(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var dialog = new ColorPickerDialog(Model.AskColor)
            {
                Owner = this
            };
            var res = dialog.ShowDialog();
            if (res.HasValue && res.Value)
            {
                Model.AskColor = dialog.Color;
            }

        }
    }
}

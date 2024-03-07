namespace BinanceOptionsApp
{
    using System.Windows;
    using System.Windows.Controls;

    public partial class StringValueDialog : Window
    {
        string title;
        string initialValue;
        UserControl owner;

        public string StringResult { get; private set; }

        public StringValueDialog(string title, string initialValue, UserControl owner)
        {
            InitializeComponent();
            this.title = title;
            this.initialValue = initialValue;
            this.owner = owner;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Title = title;
            value.Text = initialValue;
            value.Focus();
            value.SelectAll();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }
        private void BuAccept_Click(object sender, RoutedEventArgs e)
        {
            StringResult = value.Text;
            DialogResult = true;
        }
        private void BuCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}

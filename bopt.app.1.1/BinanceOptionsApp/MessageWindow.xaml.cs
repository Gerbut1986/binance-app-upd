using System.Windows;
using System.Windows.Media;

namespace BinanceOptionsApp
{
    public enum MessageWindowType
    {
        Information,
        Error,
        Question
    }

    public partial class MessageWindow : Window
    {
        readonly string msg;
        readonly MessageWindowType messageWindowType;
        public MessageWindow(string message, MessageWindowType windowType)
        {
            InitializeComponent();
            msg = message;
            messageWindowType = windowType;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            title1.Text = messageWindowType == MessageWindowType.Error ? App.LanguageKey("locMessageError") : App.LanguageKey("locMessageInformation");
            if (messageWindowType == MessageWindowType.Question)
            {
                title1.Text = App.LanguageKey("locMessageQuestion");
            }
            else
            {
                buOk.IsCancel = true;
                buCancel.IsCancel = false;
                buCancel.Visibility = Visibility.Collapsed;
            }
            title1.Foreground = new SolidColorBrush(messageWindowType == MessageWindowType.Error ? Colors.Pink : Color.FromRgb(0xfd, 0xea, 0x37));
            message.Text = msg;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void buOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void buCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}

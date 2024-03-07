using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BinanceOptionsApp.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public bool Reverse
        {
            get;
            set;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Reverse)
            {
                return (bool)value ? Visibility.Collapsed : Visibility.Visible;
            }
            else
            {
                return (bool)value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}

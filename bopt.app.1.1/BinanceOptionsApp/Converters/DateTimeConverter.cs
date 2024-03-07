using System;
using System.Globalization;
using System.Windows.Data;

namespace BinanceOptionsApp.Converters
{
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                DateTime time = (DateTime)value;
                if (time.Year >= 1900)
                {
                    return time.ToString("HH:mm:ss.ffffff");
                }
            }
            catch
            {

            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

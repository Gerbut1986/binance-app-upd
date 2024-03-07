using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace BinanceOptionsApp.Converters
{
    public class GapBrushConverter : IValueConverter
    {
        public static SolidColorBrush greenBrush = new SolidColorBrush(Color.FromRgb(0x85, 0x9e, 0x01));
        public static SolidColorBrush redBrush = new SolidColorBrush(Color.FromRgb(0xff, 0x3a, 0x00));
        public static SolidColorBrush blueBrush = new SolidColorBrush(Color.FromRgb(0x00, 0x23, 0x44));
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool positive = false;
            bool negative = false;
            try
            {
                double dv = (double)value;
                positive = dv > 0;
                negative = dv < 0;
            }
            catch(Exception ex)
            {
                var err = ex.Message;
            }
            try
            {
                decimal dv = (decimal)value;
                positive = dv > 0;
                negative = dv < 0;
            }
            catch
            {
            }
            if (positive) return greenBrush;
            if (negative) return redBrush;
            return blueBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

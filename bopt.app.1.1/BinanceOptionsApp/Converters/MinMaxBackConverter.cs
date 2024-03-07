using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace BinanceOptionsApp.Converters
{
    public class MinMaxBackConverter : IValueConverter
    {
        public static Brush[] MyBrushes =
        {
            new SolidColorBrush(Color.FromRgb(0x04,0x52,0x00)),
            new SolidColorBrush(Color.FromRgb(0x4c,0xb3,0x00)),
            new SolidColorBrush(Color.FromRgb(0x24,0x24,0x24)),
            new SolidColorBrush(Color.FromRgb(0xb3,0x00,0x00)),
            new SolidColorBrush(Color.FromRgb(0x9e,0xb3,0x00))
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                Models.ProviderModel pm = value as Models.ProviderModel;
                if (pm.MaxMinState == Models.ProviderMaxMinState.Max) return Brushes.LightPink; // sell
                if (pm.MaxMinState == Models.ProviderMaxMinState.Min) return Brushes.LightGreen; // buy
                return MyBrushes[pm.ViewNumber % MyBrushes.Length];
            }
            catch
            {
            }
            return Brushes.DarkBlue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

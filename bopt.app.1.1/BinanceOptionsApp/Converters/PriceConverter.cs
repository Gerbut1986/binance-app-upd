using System;
using System.Globalization;
using System.Windows.Data;

namespace BinanceOptionsApp.Converters
{
    public class PriceConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var provider = values[0] as Models.ProviderModel;
                decimal price = (decimal)values[1];
                if (price != 0)
                {
                    return provider.Parent.FormatPrice(price);
                }
            }
            catch
            {
            }
            return "";
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

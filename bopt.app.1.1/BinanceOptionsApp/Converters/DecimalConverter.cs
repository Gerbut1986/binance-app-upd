using System;
using System.Globalization;
using System.Windows.Data;

namespace BinanceOptionsApp.Converters
{
    public class DecimalConverter : IValueConverter
    {
        public string Format { get; set; } = "F2";
        public bool RemoveTrailingZeros { get; set; } = false;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                decimal gap = (decimal)value;
                string res = gap.ToString(Format,CultureInfo.InvariantCulture);
                if (RemoveTrailingZeros)
                {
                    if (res.Contains("."))
                    {
                        res = res.TrimEnd('0').TrimEnd('.');
                    }
                }
                return res;
            }
            catch
            {

            }
            return "0";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class BalanceConverter : IValueConverter
    {
        public string Format { get; set; } = "F2";
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                decimal? balance = (decimal?)value;
                if (balance.HasValue)
                {
                    return balance.Value.ToString(Format, CultureInfo.InvariantCulture);
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

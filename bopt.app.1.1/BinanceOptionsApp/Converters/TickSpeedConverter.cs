using System;
using System.Globalization;
using System.Windows.Data;

namespace BinanceOptionsApp.Converters
{
    class TickSpeedConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                decimal speedms = ((decimal)values[0]) / 1000.0M;
                decimal tps = speedms > 0 ? 1.0M / speedms : 0;
                decimal max = (decimal)values[1];
                return speedms.ToString("F2", CultureInfo.InvariantCulture) + " s|" + tps.ToString("F1", CultureInfo.InvariantCulture) + " tps|"+max.ToString("F1",CultureInfo.InvariantCulture)+" max";
            }
            catch
            {

            }
            return "0";
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

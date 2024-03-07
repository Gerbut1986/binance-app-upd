namespace BinanceOptionsApp.Controls
{
    internal static class NumericStringExtensions
    {
        public static string RemoveEndingZerosAndPoint(this string str)
        {
            string result = str;
            int PointPos = result.IndexOf('.');
            if (PointPos >= 0)
            {
                while (result.Length > 0)
                {
                    if (result[result.Length - 1] == '.')
                    {
                        result = result.Substring(0, result.Length - 1);
                        break;
                    }
                    if (result[result.Length - 1] == '0')
                    {
                        result = result.Substring(0, result.Length - 1);
                        continue;
                    }


                    break;
                }
            }
            return result;
        }

        public static string RemoveStartZeros(this string str)
        {
            string result = str;
            int MinusPos = result.IndexOf('-');
            int Start = MinusPos >= 0 ? MinusPos + 1 : 0;
            while (Start < result.Length)
            {
                if (result[Start] == '0')
                {
                    bool remove = true;
                    if (Start<(result.Length-1))
                    {
                        if (result[Start + 1] == '.' || result[Start + 1] == ',') remove = false;
                    }
                    if (remove)
                    {
                        result = result.Remove(Start, 1);
                        continue;
                    }
                }
                break;
            }
            return result;
        }

        public static string RemoveMinusIfEmpty(this string str)
        {
            if (str.Length > 0)
            {
                if (str[0] == '-' && str.Length == 1) return "";
            }
            return str;
        }

        public static string RemoveNumericStartAndEnd(this string str)
        {
            str = RemoveEndingZerosAndPoint(str);
            str = RemoveStartZeros(str);
            str = RemoveMinusIfEmpty(str);
            return str;
        }

    }
}

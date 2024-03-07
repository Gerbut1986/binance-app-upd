using System;
using System.Diagnostics;

namespace Helpers.Extensions
{
    public static class ExMethod
    {
        public static DateTime GetFullTime(this long millisec)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(millisec);
            return dateTimeOffset.UtcDateTime;
        }

        public static long GetMicrosecondsTime(this long microsec)//??????????????????????
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;//FromUnixTimeMilliseconds(microsec);
            return dateTimeOffset.Ticks/10; //.UtcDateTime;
        }

        public static T[] SubArray<T>(this T[] array, ulong startIndex, ulong endIndex)
        {
            ulong subArrayLength = endIndex - startIndex + 1;
            T[] subArray = new T[subArrayLength];

            for (ulong i = 0; i < subArrayLength; i++)
            {
                subArray[i] = array[startIndex + i];
            }

            return subArray;
        }

        public static long GetTotalMilliseconds(this DateTime dateTime)
        {
            TimeSpan timeSpan = dateTime.TimeOfDay;
            return (long)timeSpan.TotalMilliseconds;
        }
    }
}

using System;
using System.Reflection;

namespace BinanceOptionsApp.Helpers
{
    public static class PropertyCopier
    {
        public static void Copy<T>(T source, T destination)
        {
            if (source != null && destination != null)
            {
                Type t = typeof(T);
                var props = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var prop in props)
                {
                    prop.SetValue(destination, prop.GetValue(source));
                }
            }
        }
    }
}

using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace BinanceOptionsApp.CustomStyles
{
    public static class LocalExtensions
    {
        public static void ForWindowFromChild(this object childDependencyObject, Action<Window> action)
        {
            DependencyObject reference = childDependencyObject as DependencyObject;
            while (reference != null)
            {
                reference = VisualTreeHelper.GetParent(reference);
                if (reference is Window)
                {
                    action(reference as Window);
                    break;
                }
            }
        }

        public static void ForWindowFromTemplate(
          this object templateFrameworkElement,
          Action<Window> action)
        {
            if (!(((FrameworkElement)templateFrameworkElement).TemplatedParent is Window templatedParent))
                return;
            action(templatedParent);
        }

        public static IntPtr GetWindowHandle(this Window window) => new WindowInteropHelper(window).Handle;
    }
}

using System.Windows;
using System.Windows.Media;

namespace BinanceOptionsApp.CustomStyles
{
    public class CustomWindowSettings : DependencyObject
    {
        public static readonly DependencyProperty AllowMaximizeProperty = DependencyProperty.RegisterAttached(
            "AllowMaximize",
            typeof(bool),
            typeof(CustomWindowSettings),
            new PropertyMetadata(true)
        );
        public static void SetAllowMaximize(UIElement element, bool value)
        {
            element.SetValue(AllowMaximizeProperty, value);
        }
        public static bool GetAllowMaximize(UIElement element)
        {
            return (bool)element.GetValue(AllowMaximizeProperty);
        }

        public static readonly DependencyProperty AllowMinimizeProperty = DependencyProperty.RegisterAttached(
            "AllowMinimize",
            typeof(bool),
            typeof(CustomWindowSettings),
            new PropertyMetadata(true)
        );
        public static void SetAllowMinimize(UIElement element, bool value)
        {
            element.SetValue(AllowMinimizeProperty, value);
        }
        public static bool GetAllowMinimize(UIElement element)
        {
            return (bool)element.GetValue(AllowMinimizeProperty);
        }

        public static readonly DependencyProperty AllowResizeProperty = DependencyProperty.RegisterAttached(
            "AllowResize",
            typeof(bool),
            typeof(CustomWindowSettings),
            new PropertyMetadata(true)
        );
        public static void SetAllowResize(UIElement element, bool value)
        {
            element.SetValue(AllowResizeProperty, value);
        }
        public static bool GetAllowResize(UIElement element)
        {
            return (bool)element.GetValue(AllowResizeProperty);
        }

        public static readonly DependencyProperty TitleHeightProperty = DependencyProperty.RegisterAttached(
            "TitleHeight",
            typeof(double),
            typeof(CustomWindowSettings),
            new PropertyMetadata(32.0)
        );
        public static void SetTitleHeight(UIElement element, double value)
        {
            element.SetValue(TitleHeightProperty, value);
        }
        public static double GetTitleHeight(UIElement element)
        {
            return (double)element.GetValue(TitleHeightProperty);
        }

        public static readonly DependencyProperty StandardIconProperty = DependencyProperty.RegisterAttached(
            "StandardIcon",
            typeof(bool),
            typeof(CustomWindowSettings),
            new PropertyMetadata(true)
        );
        public static void SetStandardIcon(UIElement element, bool value)
        {
            element.SetValue(StandardIconProperty, value);
        }
        public static bool GetStandardIcon(UIElement element)
        {
            return (bool)element.GetValue(StandardIconProperty);
        }

        public static readonly DependencyProperty StandardCaptionProperty = DependencyProperty.RegisterAttached(
            "StandardCaption",
            typeof(bool),
            typeof(CustomWindowSettings),
            new PropertyMetadata(true)
        );
        public static void SetStandardCaption(UIElement element, bool value)
        {
            element.SetValue(StandardCaptionProperty, value);
        }
        public static bool GetStandardCaption(UIElement element)
        {
            return (bool)element.GetValue(StandardCaptionProperty);
        }

        public static readonly DependencyProperty UseCustomBackgroundProperty = DependencyProperty.RegisterAttached(
            "UseCustomBackground",
            typeof(bool),
            typeof(CustomWindowSettings),
            new PropertyMetadata(false)
        );
        public static void SetUseCustomBackground(UIElement element, bool value)
        {
            element.SetValue(UseCustomBackgroundProperty, value);
        }
        public static bool GetUseCustomBackground(UIElement element)
        {
            return (bool)element.GetValue(UseCustomBackgroundProperty);
        }

        public static readonly DependencyProperty CustomBackgroundProperty = DependencyProperty.RegisterAttached(
            "CustomBackground",
            typeof(Brush),
            typeof(CustomWindowSettings),
            new PropertyMetadata(new SolidColorBrush(Colors.Red))
        );
        public static void SetCustomBackground(UIElement element, Brush value)
        {
            element.SetValue(CustomBackgroundProperty, value);
        }
        public static Brush GetCustomBackground(UIElement element)
        {
            return element.GetValue(CustomBackgroundProperty) as Brush;
        }


        public static readonly DependencyProperty UseImageBackgroundProperty = DependencyProperty.RegisterAttached(
            "UseImageBackground",
            typeof(bool),
            typeof(CustomWindowSettings),
            new PropertyMetadata(false)
        );
        public static void SetUseImageBackground(UIElement element, bool value)
        {
            element.SetValue(UseImageBackgroundProperty, value);
        }
        public static bool GetUseImageBackground(UIElement element)
        {
            return (bool)element.GetValue(UseImageBackgroundProperty);
        }

        public static readonly DependencyProperty ImageBackgroundProperty = DependencyProperty.RegisterAttached(
            "ImageBackground",
            typeof(Brush),
            typeof(CustomWindowSettings),
            new PropertyMetadata(new SolidColorBrush(Colors.Red))
        );
        public static void SetImageBackground(UIElement element, Brush value)
        {
            element.SetValue(ImageBackgroundProperty, value);
        }
        public static Brush GetImageBackground(UIElement element)
        {
            return element.GetValue(ImageBackgroundProperty) as Brush;
        }

        public static readonly DependencyProperty UseTitleColorProperty = DependencyProperty.RegisterAttached(
            "UseTitleColor",
            typeof(bool),
            typeof(CustomWindowSettings),
            new PropertyMetadata(false)
        );
        public static void SetUseTitleColor(UIElement element, bool value)
        {
            element.SetValue(UseTitleColorProperty, value);
        }
        public static bool GetUseTitleColor(UIElement element)
        {
            return (bool)element.GetValue(UseTitleColorProperty);
        }
        public static readonly DependencyProperty TitleColorProperty = DependencyProperty.RegisterAttached(
            "TitleColor",
            typeof(Brush),
            typeof(CustomWindowSettings),
            new PropertyMetadata(new SolidColorBrush(Colors.White))
        );
        public static void SetTitleColor(UIElement element, Brush value)
        {
            element.SetValue(TitleColorProperty, value);
        }
        public static Brush GetTitleColor(UIElement element)
        {
            return element.GetValue(TitleColorProperty) as Brush;
        }
    }
}

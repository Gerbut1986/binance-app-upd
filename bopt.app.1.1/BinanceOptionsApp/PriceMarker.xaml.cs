using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BinanceOptionsApp
{
    public partial class PriceMarker : Window
    {
        public PriceMarker(Brush background, Brush border, string text = "", double opacity = 0.75)
        {
            InitializeComponent();

            grid.Background = background;
            rect.Fill = background;
            rect.Stroke = border;
            this.text.Text = text;
            grid.Opacity = opacity;
        }

        public double RealX { get; set; }
        public double RealY { get; set; }
        public double RealW { get; set; }
        public double RealH { get; set; }

        public void AdjustDpi(Control control)
        {
            var source = PresentationSource.FromVisual(control);
            double dpiX = 96.0 * source.CompositionTarget.TransformToDevice.M11;
            double dpiY = 96.0 * source.CompositionTarget.TransformToDevice.M22;
            Left *= 96.0 / dpiX;
            Top *= 96.0 / dpiY;
            Width *= 96.0 / dpiX;
            Height *= 96.0 / dpiY;
        }
        public void SetSizeDpi(Control control, double w, double h)
        {
            var source = PresentationSource.FromVisual(control);
            double dpiX = 96.0 * source.CompositionTarget.TransformToDevice.M11;
            double dpiY = 96.0 * source.CompositionTarget.TransformToDevice.M22;
            Width = w * 96.0 / dpiX;
            Height = h * 96.0 / dpiY;
        }
    }
}

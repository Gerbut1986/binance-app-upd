using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace BinanceOptionsApp
{
    public partial class CircularProgressBar : UserControl
    {
        static CircularProgressBar()
        {
            Timeline.DesiredFrameRateProperty.OverrideMetadata(typeof(Timeline), new FrameworkPropertyMetadata { DefaultValue = 20 });
        }
        public CircularProgressBar()
        {
            InitializeComponent();
        }
    }
}    
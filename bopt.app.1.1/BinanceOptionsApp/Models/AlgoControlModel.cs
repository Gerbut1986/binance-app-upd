using System.ComponentModel.DataAnnotations;

namespace BinanceOptionsApp.Models
{
    public class AlgoControlModel : BaseModel
    {
        private bool _ViewChart;
        [Display(Name = "View Chart")]
        public bool ViewChart
        {
            get { return _ViewChart; }
            set { if (_ViewChart != value) { _ViewChart = value; OnPropertyChanged(); } }
        }
        private bool _ViewOrders;
        [Display(Name = "View Orders")]
        public bool ViewOrders
        {
            get { return _ViewOrders; }
            set { if (_ViewOrders != value) { _ViewOrders = value; OnPropertyChanged(); } }
        }
        private bool _ViewDebug;
        [Display(Name = "View Debug")]
        public bool ViewDebug
        {
            get { return _ViewDebug; }
            set { if (_ViewDebug != value) { _ViewDebug = value; OnPropertyChanged(); } }
        }
        private bool _SaveTrace;
        [Display(Name = "Save Trace to File")]
        public bool SaveTrace
        {
            get { return _SaveTrace; }
            set { if (_SaveTrace != value) { _SaveTrace = value; OnPropertyChanged(); } }
        }
    }
}

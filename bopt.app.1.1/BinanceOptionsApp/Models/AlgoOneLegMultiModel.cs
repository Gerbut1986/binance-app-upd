using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace BinanceOptionsApp.Models
{
    public class AlgoOneLegMultiModel : BaseModel
    {
        public AlgoOneLegMultiModel()
        {
            Digits = 5;
            SlippagePt = 10;
            AutoShiftPeriod = 100;
            Volume = 0.01;
            MinOpenGapPt = 25;
            TakeProfitPt = 1;
            StopLossPt = 7;
            FastTickDirectionConfirmation = true;
            PendingLifeTimeMs = 1000;
            PendingDistancePt = 1;
        }

        private bool _Started;
        [XmlIgnore]
        [Browsable(false)]
        public bool Started
        {
            get { return _Started; }
            set { if (_Started != value) { _Started = value; OnPropertyChanged(); } }
        }

        private bool _AllowTradeOpen;
        [Display(Name = "Allow Trade Open")]
        public bool AllowTradeOpen
        {
            get { return _AllowTradeOpen; }
            set { if (_AllowTradeOpen != value) { _AllowTradeOpen = value; OnPropertyChanged(); } }
        }
        private int _Digits;
        public int Digits
        {
            get { return _Digits; }
            set { if (_Digits != value) { _Digits = value; OnPropertyChanged(); } }
        }
        public bool DigitsEnabled()
        {
            return !Started;
        }
        private int _SlippagePt;
        [Display(Name = "Slippage(pt)")]
        public int SlippagePt
        {
            get { return _SlippagePt; }
            set { if (_SlippagePt != value) { _SlippagePt = value; OnPropertyChanged(); } }
        }
        private EntryOrderType _OrderType;
        [Display(Name = "Order Type")]
        public EntryOrderType OrderType
        {
            get { return _OrderType; }
            set { if (_OrderType != value) { _OrderType = value; OnPropertyChanged(); } }
        }
        private int _PendingDistancePt;
        [Display(Name = "Pending Distance(pt)")]
        public int PendingDistancePt
        {
            get { return _PendingDistancePt; }
            set { if (_PendingDistancePt != value) { _PendingDistancePt = value; OnPropertyChanged(); } }
        }
        public bool PendingDistancePtVisibility()
        {
            return OrderType != EntryOrderType.Market;
        }
        private int _PendingLifeTimeMs;
        [Display(Name = "Pending Life Time(ms)")]
        public int PendingLifeTimeMs
        {
            get { return _PendingLifeTimeMs; }
            set { if (_PendingLifeTimeMs != value) { _PendingLifeTimeMs = value; OnPropertyChanged(); } }
        }
        public bool PendingLifeTimeMsVisibility()
        {
            return OrderType != EntryOrderType.Market;
        }
        private FillPolicy _Fill;
        public FillPolicy Fill
        {
            get { return _Fill; }
            set { if (_Fill != value) { _Fill = value; OnPropertyChanged(); } }
        }
        private double _Volume;
        [DisplayFormat(DataFormatString = "F8")]
        public double Volume
        {
            get { return _Volume; }
            set { if (_Volume != value) { _Volume = value; OnPropertyChanged(); } }
        }

        private bool _UseAutoShift;
        [Display(Name = "Use Auto Shift")]
        public bool UseAutoShift
        {
            get { return _UseAutoShift; }
            set { if (_UseAutoShift != value) { _UseAutoShift = value; OnPropertyChanged(); } }
        }
        public bool UseAutoShiftEnabled()
        {
            return !Started;
        }
        private int _AutoShiftPeriod;
        [Display(Name = "Auto Shift Period")]
        public int AutoShiftPeriod
        {
            get { return _AutoShiftPeriod; }
            set { if (_AutoShiftPeriod != value) { _AutoShiftPeriod = value; OnPropertyChanged(); } }
        }
        public bool AutoShiftPeriodVisibility()
        {
            return UseAutoShift;
        }
        public bool AutoShiftPeriodEnabled()
        {
            return !Started;
        }
        private int _ManualShiftPt;
        [Display(Name = "Manual Shift (pt)")]
        public int ManualShiftPt
        {
            get { return _ManualShiftPt; }
            set { if (_ManualShiftPt != value) { _ManualShiftPt = value; OnPropertyChanged(); } }
        }
        public bool ManualShiftPtVisibility()
        {
            return !UseAutoShift;
        }

        private int _MinSpreadFastPt;
        [Display(Name = "Min Spread Fast(pt)")]
        public int MinSpreadFastPt
        {
            get { return _MinSpreadFastPt; }
            set { if (_MinSpreadFastPt != value) { _MinSpreadFastPt = value; OnPropertyChanged(); } }
        }
        private bool _UseMaxSpreadFast;
        [Display(Name = "Use Max Spread Fast")]
        public bool UseMaxSpreadFast
        {
            get { return _UseMaxSpreadFast; }
            set { if (_UseMaxSpreadFast != value) { _UseMaxSpreadFast = value; OnPropertyChanged(); } }
        }
        private int _MaxSpreadFastPt;
        [Display(Name = "Max Spread Fast(pt)")]
        public int MaxSpreadFastPt
        {
            get { return _MaxSpreadFastPt; }
            set { if (_MaxSpreadFastPt != value) { _MaxSpreadFastPt = value; OnPropertyChanged(); } }
        }
        public bool MaxSpreadFastPtVisibility()
        {
            return UseMaxSpreadFast;
        }
        private bool _UseMaxSpreadSlow;
        [Display(Name = "Use Max Spread Slow")]
        public bool UseMaxSpreadSlow
        {
            get { return _UseMaxSpreadSlow; }
            set { if (_UseMaxSpreadSlow != value) { _UseMaxSpreadSlow = value; OnPropertyChanged(); } }
        }
        private int _MaxSpreadSlowPt;
        [Display(Name = "Max Spread Slow(pt)")]
        public int MaxSpreadSlowPt
        {
            get { return _MaxSpreadSlowPt; }
            set { if (_MaxSpreadSlowPt != value) { _MaxSpreadSlowPt = value; OnPropertyChanged(); } }
        }
        public bool MaxSpreadSlowPtVisibility()
        {
            return UseMaxSpreadSlow;
        }
        private int _MinOpenGapPt;
		[Display(Name = "Min Open Gap(pt)")]
		public int MinOpenGapPt
        {
            get { return _MinOpenGapPt; }
            set { if (_MinOpenGapPt != value) { _MinOpenGapPt = value; OnPropertyChanged(); } }
        }
        private bool _FastTickDirectionConfirmation;
        [Display(Name = "Fast Tick Direction Confirmation")]
        public bool FastTickDirectionConfirmation
        {
            get { return _FastTickDirectionConfirmation; }
            set { if (_FastTickDirectionConfirmation != value) { _FastTickDirectionConfirmation = value; OnPropertyChanged(); } }
        }

        private bool _DecreaseGapsByFastSpread;
        [Display(Name ="Decrease Gaps By Fast Spread")]
        public bool DecreaseGapsByFastSpread
        {
            get { return _DecreaseGapsByFastSpread; }
            set { if (_DecreaseGapsByFastSpread != value) { _DecreaseGapsByFastSpread = value; OnPropertyChanged(); } }
        }
        private bool _DecreaseGapsBySlowSpread;
        [Display(Name = "Decrease Gaps By Slow Spread")]
        public bool DecreaseGapsBySlowSpread
        {
            get { return _DecreaseGapsBySlowSpread; }
            set { if (_DecreaseGapsBySlowSpread != value) { _DecreaseGapsBySlowSpread = value; OnPropertyChanged(); } }
        }
        private int _TakeProfitPt;
        [Display(Name = "TakeProfit(pt)")]
        public int TakeProfitPt
        {
            get { return _TakeProfitPt; }
            set { if (_TakeProfitPt != value) { _TakeProfitPt = value; OnPropertyChanged(); } }
        }
        private int _StopLossPt;
        [Display(Name = "StopLoss(pt)")]
        public int StopLossPt
        {
            get { return _StopLossPt; }
            set { if (_StopLossPt != value) { _StopLossPt = value; OnPropertyChanged(); } }
        }
        private int _MinOrderDurationMs;
        [Display(Name = "Min Order Duration(ms)")]
        public int MinOrderDurationMs
        {
            get { return _MinOrderDurationMs; }
            set { if (_MinOrderDurationMs != value) { _MinOrderDurationMs = value; OnPropertyChanged(); } }
        }
        private bool _UseHoldIfGapAbove;
        public bool UseHoldIfGapAbove
        {
            get { return _UseHoldIfGapAbove; }
            set { if (_UseHoldIfGapAbove != value) { _UseHoldIfGapAbove = value; OnPropertyChanged(); } }
        }
        private int _HoldIfGapAbovePt;
        [Display(Name ="Hold Order if Gap Above(pt)")]
        public int HoldIfGapAbovePt
        {
            get { return _HoldIfGapAbovePt; }
            set { if (_HoldIfGapAbovePt != value) { _HoldIfGapAbovePt = value; OnPropertyChanged(); } }
        }
        public bool HoldIfGapAbovePtVisibility()
        {
            return UseHoldIfGapAbove;
        }
    }
}

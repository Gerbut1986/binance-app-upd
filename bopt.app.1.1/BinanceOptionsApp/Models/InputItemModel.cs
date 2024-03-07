namespace BinanceOptionsApp.Models
{
    using System;
    using System.Xml.Serialization;

    public class InputItemModel : BaseModel
    {
        public InputItemModel()
        {
            Name = "BTCUSDT";
            ChartGroup = "Main";
            Chart = "Main";
            Digits = 5;
            Format = "F5";
            Style = "#00f,#f00,1,255";
            Point = 0.00001;
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { if (_Name != value) { _Name = value; OnPropertyChanged(); } }
        }

        private string _ChartGroup;
        public string ChartGroup
        {
            get { return _ChartGroup; }
            set { if (_ChartGroup != value) { _ChartGroup = value; OnPropertyChanged(); } }
        }

        private string _Chart;
        public string Chart
        {
            get { return _Chart; }
            set { if (_Chart != value) { _Chart = value; OnPropertyChanged(); } }
        }

        private int _Digits;
        public int Digits
        {
            get { return _Digits; }
            set { if (_Digits != value) { _Digits = value; Format = "F" + _Digits.ToString(); OnPropertyChanged(); } }
        }

        private double _Point;
        public double Point
        {
            get { return _Point; }
            set { if (_Point != value) { _Point = value; OnPropertyChanged(); } }
        }

        private double _Bid;
        [XmlIgnore]
        public double Bid
        {
            get { return _Bid; }
            set { if (_Bid != value) { _Bid = value; OnPropertyChanged(); } }
        }

        private double _Ask;
        [XmlIgnore]
        public double Ask
        {
            get { return _Ask; }
            set { if (_Ask != value) { _Ask = value; OnPropertyChanged(); } }
        }

        private DateTime _Time;
        [XmlIgnore]
        public DateTime Time
        {
            get { return _Time; }
            set { if (_Time != value) { _Time = value; OnPropertyChanged(); } }
        }


        private double _PrevBid;
        [XmlIgnore]
        public double PrevBid
        {
            get { return _PrevBid; }
            set { if (_PrevBid != value) { _PrevBid = value; OnPropertyChanged(); } }
        }

        private double _PrevAsk;
        [XmlIgnore]
        public double PrevAsk
        {
            get { return _PrevAsk; }
            set { if (_PrevAsk != value) { _PrevAsk = value; OnPropertyChanged(); } }
        }

        public bool CopyToPrev()
        {
            bool res = Bid != PrevBid || Ask != PrevAsk;
            PrevBid = Bid;
            PrevAsk = Ask;
            return res;
        }

        public bool IsValid()
        {
            if (Bid == 0) return false;
            if (Ask == 0) return false;
            if (Bid > Ask) return false;
            return true;
        }

        private string _Style;
        public string Style
        {
            get { return _Style; }
            set { if (_Style != value) { _Style = value; OnPropertyChanged(); } }
        }

        [XmlIgnore]
        public VisualMarketsEngine.ChartGroup.InternalChart VM { get; set; }
        [XmlIgnore]
        public int FlowBid { get; set; }
        [XmlIgnore]
        public int FlowAsk { get; set; }

        private string _Format;
        [XmlIgnore]
        public string Format
        {
            get { return _Format; }
            set { if (_Format != value) { _Format = value; OnPropertyChanged(); } }
        }
    }
}

using MultiTerminal.Connections;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Media;
using System.Xml.Serialization;
using BinanceOptionsApp.Helpers;

namespace BinanceOptionsApp.Models
{
    public enum TradeAlgorithm
    {
        LatencyArbitrage,
        ZigZag,
        MultiLegSpread,
        TwoLegArbitrage,
        TwoLegFutures,
        TwoLegArbFutureUSD_M,
        TickOptimizer,
        TickOptimizerOG,
        Scalper
    }

    public enum LockType
    {
        [Description("locLockBuy")]
        Buy,
        [Description("locLockSell")]
        Sell,
        [Description("locLockFull")]
        Full
    }

    public enum FillPolicy
    {
        [Description("FOK")]
        FOK = 0,
        [Description("IOC")]
        IOK = 1,
        [Description("FLASH")]
        FILL = 2,
        [Description("GTC")]
        GTC = 3
    }
    public class OpenOrderSettingsModel : BaseModel
    {
        public OpenOrderSettingsModel()
        {
            SenseDist = 0.00m;
            Leg = 0;
            Koef1 = 0;
            Koef2 = 0;
            Cluster = 100;
            Threshold = 500;
            Threshold2 = 123;
            PeriodAlignment = 100;
            Point = 4;
            MinimumLevel = 25.0M;
            AvtoShiftBid = false;
            AvtoShiftPeriod = 100;
            GapForOpen = 0.00m;
            GapForClose = 0.00m;
            Lot = 0.01M;
            LotSlow = 0.01M;
            MaxSpread = 20;
            MinSpreadSlow = 0;
            MaxSpreadSlow = 0;
            MinOrderTimeSec = 120;
            MinOrderPoint = 5;
            Fill = FillPolicy.FOK;
            SignalMode = 1;
            RiskDeposit = 10;
            StopMode = false;
            StopModePoints = 0;
            StopModeMs = 1000;
            StartTime = "00:00";
            EndTime = "23:59";
            MinFastCoef = 0;
            FastCoefPeriod = 10;
            OpenTimerMs = 0;
            PendingDistance = 5;
            PendingLifeTimeMs = 1000;
            OrderType = OrderType.Market;
            MinimumHedgeSlippageInPoints = -10;
            MinimumHedgeLevel = 20;
            HTimeSeconds = 10;
            HMaxTrades = 5;
            GapFastTicks = 1;
            NTrades = 1;
            BuildOrderTypeString();
        }

        private decimal _Cluster;

        public decimal Cluster
        {
            get { return _Cluster; }
            set {if (_Cluster != value) { _Cluster = value; OnPropertyChanged(); } }

        }

        private decimal _SenseDist;
        public decimal SenseDist
        {
            get { return _SenseDist; }
            set { if (_SenseDist != value) { _SenseDist = value; OnPropertyChanged(); } }
        }

        private decimal _Leg;
        public decimal Leg
        {
            get { return _Leg; }
            set { if (_Leg != value) { _Leg = value; OnPropertyChanged(); } }
        }

        private decimal _Koef1;
        public decimal Koef1
        {
            get { return _Koef1; }
            set { if (_Koef1 != value) { _Koef1 = value; OnPropertyChanged(); } }
        }

        private decimal _Koef2;
        public decimal Koef2
        {
            get { return _Koef2; }
            set { if (_Koef2 != value) { _Koef2 = value; OnPropertyChanged(); } }
        }

        private decimal _Threshold;
        public decimal Threshold
        {
            get { return _Threshold; }
            set { if (_Threshold != value) { _Threshold = value; OnPropertyChanged(); } }
        }

        private decimal _Threshold2;
        public decimal Threshold2
        {
            get { return _Threshold2; }
            set { if (_Threshold2 != value) { _Threshold2 = value; OnPropertyChanged(); } }
        }

        private int _PeriodAlignment;
        public int PeriodAlignment
        {
            get { return _PeriodAlignment; }
            set { if (_PeriodAlignment != value) { _PeriodAlignment = value; OnPropertyChanged(); } }
        }

        private int _Point;
        public int Point
        {
            get { return _Point; }
            set { if (_Point != value) { _Point = value; OnPropertyChanged(); } }
        }

        private int _NTrades;
        public int NTrades
        {
            get { return _NTrades; }
            set { if (_NTrades != value) { _NTrades = value; OnPropertyChanged(); } }
        }


        private string _OrderTypeString;
        public string OrderTypeString
        {
            get { return _OrderTypeString; }
            set { if (_OrderTypeString != value) { _OrderTypeString = value; OnPropertyChanged(); } }
        }
        void BuildOrderTypeString()
        {
            string res = OrderType.ToString();
            if (OrderType != OrderType.Market)
            {
                res += " " + PendingDistance + "pt " + PendingLifeTimeMs + "ms";
            }
            res += " " + Fill.ToString();
            OrderTypeString = res;
        }
        private OrderType _OrderType;
        public OrderType OrderType
        {
            get { return _OrderType; }
            set { if (_OrderType != value) { _OrderType = value; OnPropertyChanged(); BuildOrderTypeString(); } }
        }
        private int _PendingDistance;
        public int PendingDistance
        {
            get { return _PendingDistance; }
            set { if (_PendingDistance != value) { _PendingDistance = value; OnPropertyChanged(); BuildOrderTypeString(); } }
        }
        private int _PendingLifeTimeMs;
        public int PendingLifeTimeMs
        {
            get { return _PendingLifeTimeMs; }
            set { if (_PendingLifeTimeMs != value) { _PendingLifeTimeMs = value; OnPropertyChanged(); BuildOrderTypeString(); } }
        }
        private FillPolicy _Fill;
        public FillPolicy Fill
        {
            get { return _Fill; }
            set { if (_Fill != value) { _Fill = value; OnPropertyChanged(); BuildOrderTypeString(); } }
        }

        private decimal _MinimumTPS;
        public decimal MinimumTPS
        {
            get { return _MinimumTPS; }
            set { if (_MinimumTPS != value) { _MinimumTPS = value; OnPropertyChanged(); } }
        }
        private decimal _MinimumHedgeLevel;
        public decimal MinimumHedgeLevel
        {
            get { return _MinimumHedgeLevel; }
            set { if (_MinimumHedgeLevel != value) { _MinimumHedgeLevel = value; OnPropertyChanged(); } }
        }
        private decimal _MinimumHedgeSlippageInPoints;
        public decimal MinimumHedgeSlippageInPoints
        {
            get { return _MinimumHedgeSlippageInPoints; }
            set { if (_MinimumHedgeSlippageInPoints != value) { _MinimumHedgeSlippageInPoints = value; OnPropertyChanged(); } }
        }
        private decimal _HTimeSeconds;
        public decimal HTimeSeconds
        {
            get { return _HTimeSeconds; }
            set { if (_HTimeSeconds != value) { _HTimeSeconds = value; OnPropertyChanged(); } }
        }
        private int _HMaxTrades;
        public int HMaxTrades
        {
            get { return _HMaxTrades; }
            set { if (_HMaxTrades != value) { _HMaxTrades = value; OnPropertyChanged(); } }
        }

        private int _OpenTimerMs;
        public int OpenTimerMs
        {
            get { return _OpenTimerMs; }
            set { if (_OpenTimerMs != value) { _OpenTimerMs = value; OnPropertyChanged(); } }
        }
        private int _FreezeTimeMs;
        public int FreezeTimeMs
        {
            get { return _FreezeTimeMs; }
            set { if (_FreezeTimeMs != value) { _FreezeTimeMs = value; OnPropertyChanged(); } }
        }


        private decimal _MinFastCoef;
        public decimal MinFastCoef
        {
            get { return _MinFastCoef; }
            set { if (_MinFastCoef != value) { _MinFastCoef = value; OnPropertyChanged(); } }
        }
        private int _FastCoefPeriod;
        public int FastCoefPeriod
        {
            get { return _FastCoefPeriod; }
            set { if (_FastCoefPeriod != value) { _FastCoefPeriod = value; OnPropertyChanged(); } }
        }


        private bool _StopMode;
        public bool StopMode
        {
            get { return _StopMode; }
            set { if (_StopMode != value) { _StopMode = value; OnPropertyChanged(); } }
        }

        private int _StopModePoints;
        public int StopModePoints
        {
            get { return _StopModePoints; }
            set { if (_StopModePoints != value) { _StopModePoints = value; OnPropertyChanged(); } }
        }

        private int _StopModeMs;
        public int StopModeMs
        {
            get { return _StopModeMs; }
            set { if (_StopModeMs != value) { _StopModeMs = value; OnPropertyChanged(); } }
        }

        private decimal _Lot;
        public decimal Lot
        {
            get { return _Lot; }
            set { if (_Lot != value) { _Lot = value; OnPropertyChanged(); } }
        }

        private decimal _RiskPercent;
        public decimal RiskPercent
        {
            get { return _RiskPercent; }
            set { if (_RiskPercent != value) { _RiskPercent = value; OnPropertyChanged(); } }
        }

        public static decimal CalculateDynamicLot(decimal balance, decimal riskPercent, decimal minLot, decimal lotStep)
        {
            decimal result = (riskPercent * 0.01M * balance) / 1000;
            int steps = lotStep > 0 ? (int)(result / lotStep) : 0;
            result = Math.Round(lotStep*steps, 2);
            if (result < minLot) result = minLot;
            return result;
        }

        private decimal _LotSlow;
        public decimal LotSlow
        {
            get { return _LotSlow; }
            set { if (_LotSlow != value) { _LotSlow = value; OnPropertyChanged(); } }
        }

        private decimal _RiskPercentSlow;
        public decimal RiskPercentSlow
        {
            get { return _RiskPercentSlow; }
            set { if (_RiskPercentSlow != value) { _RiskPercentSlow = value; OnPropertyChanged(); } }
        }
                
        private int _MinOrderTimeSec;
        public int MinOrderTimeSec
        {
            get { return _MinOrderTimeSec; }
            set { if (_MinOrderTimeSec != value) { _MinOrderTimeSec = value; OnPropertyChanged(); } }
        }

        private int _MinOrderPoint;
        public int MinOrderPoint
        {
            get { return _MinOrderPoint; }
            set { if (_MinOrderPoint != value) { _MinOrderPoint = value; OnPropertyChanged(); } }
        }

        private LockType _LockType;
        public LockType LockType
        {
            get { return _LockType; }
            set { if (_LockType != value) { _LockType = value; OnPropertyChanged(); } }
        }

        private decimal _MinGapForOpen;
        public decimal MinGapForOpen
        {
            get { return _MinGapForOpen; }
            set { if (_MinGapForOpen != value) { _MinGapForOpen = value; OnPropertyChanged(); } }
        }

        private decimal _MaxGapForOpen;
        public decimal MaxGapForOpen
        {
            get { return _MaxGapForOpen; }
            set { if (_MaxGapForOpen != value) { _MaxGapForOpen = value; OnPropertyChanged(); } }
        }

        private decimal minimumLevel;
        public decimal MinimumLevel
        {
            get { return minimumLevel; }
            set { if (minimumLevel != value) { minimumLevel = value; OnPropertyChanged(); } }
        }

        private bool avtoShiftBid;
        public bool AvtoShiftBid
        {
            get { return avtoShiftBid; }
            set { if (avtoShiftBid != value) { avtoShiftBid = value; OnPropertyChanged(); } }
        }

        private int _AvtoShiftPeriod;
        public int AvtoShiftPeriod
        {
            get { return _AvtoShiftPeriod; }
            set { if (_AvtoShiftPeriod != value) { _AvtoShiftPeriod = value; OnPropertyChanged(); } }
        }

        private bool _UseAverageSpread;
        public bool UseAverageSpread
        {
            get { return _UseAverageSpread; }
            set { if (_UseAverageSpread != value) { _UseAverageSpread = value; OnPropertyChanged(); } }
        }

        private decimal gapForOpen;
        public decimal GapForOpen
        {
            get { return gapForOpen; }
            set { if (gapForOpen != value) { gapForOpen = value; OnPropertyChanged(); } }
        }

        private decimal _GapForClose;
        public decimal GapForClose
        {
            get { return _GapForClose; }
            set { if (_GapForClose != value) { _GapForClose = value; OnPropertyChanged(); } }
        }

        private decimal _MinGapFast;
        public decimal MinGapFast
        {
            get { return _MinGapFast; }
            set { if (_MinGapFast != value) { _MinGapFast = value; OnPropertyChanged(); } }
        }
        private int _GapFastTicks;
        public int GapFastTicks
        {
            get { return _GapFastTicks; }
            set { if (_GapFastTicks != value) { _GapFastTicks = value; OnPropertyChanged(); } }
        }


        private decimal _MinSpread;
        public decimal MinSpread
        {
            get { return _MinSpread; }
            set { if (_MinSpread != value) { _MinSpread = value; OnPropertyChanged(); } }
        }
        private decimal _MaxSpread;
        public decimal MaxSpread
        {
            get { return _MaxSpread; }
            set { if (_MaxSpread != value) { _MaxSpread = value; OnPropertyChanged(); } }
        }
        private decimal _MaxSpreadSlow;
        public decimal MaxSpreadSlow
        {
            get { return _MaxSpreadSlow; }
            set { if (_MaxSpreadSlow != value) { _MaxSpreadSlow = value; OnPropertyChanged(); } }
        }
        private decimal _MinSpreadSlow;
        public decimal MinSpreadSlow
        {
            get { return _MinSpreadSlow; }
            set { if (_MinSpreadSlow != value) { _MinSpreadSlow = value; OnPropertyChanged(); } }
        }

        private decimal _Comission;
        public decimal Comission
        {
            get { return _Comission; }
            set { if (_Comission != value) { _Comission = value; OnPropertyChanged(); } }
        }

        private int _SignalMode;
        public int SignalMode
        {
            get { return _SignalMode; }
            set { if (_SignalMode != value) { _SignalMode = value; OnPropertyChanged(); } }
        }

        private decimal _RiskDeposit;
        public decimal RiskDeposit
        {
            get { return _RiskDeposit; }
            set { if (_RiskDeposit != value) { _RiskDeposit = value; OnPropertyChanged(); } }
        }

        private string _StartTime;
        public string StartTime
        {
            get { return _StartTime; }
            set { if (_StartTime != value) { _StartTime = value; OnPropertyChanged(); } }
        }
        private string _EndTime;
        public string EndTime
        {
            get { return _EndTime; }
            set { if (_EndTime != value) { _EndTime = value; OnPropertyChanged(); } }
        }

        private TimeSpan GetTimeSpan(string time, TimeSpan def)
        {
            try
            {
                var split = time.Split(':');
                if (split.Length>=2)
                {
                    return new TimeSpan(int.Parse(split[0]), int.Parse(split[1]),0);
                }
            }
            catch
            {
            }
            return def;
        }
        public TimeSpan StartTimeSpan()
        {
            return GetTimeSpan(StartTime,new TimeSpan(0,0,0));
        }
        public TimeSpan EndTimeSpan()
        {
            return GetTimeSpan(EndTime,new TimeSpan(23,59,0));
        }
        public bool IsInStartEndSpan(DateTime time, TimeSpan start, TimeSpan stop)
        {
            if (start<=stop)
            {
                return time.TimeOfDay >= start && time.TimeOfDay <= stop;
            }
            return time.TimeOfDay >= start || time.TimeOfDay <= stop;
        }

        public void CopyFrom(OpenOrderSettingsModel o)
        {
            StopMode = o.StopMode;
            StopModePoints = o.StopModePoints;
            StopModeMs = o.StopModeMs;
            Fill = o.Fill;
            Lot = o.Lot;
            RiskPercent = o.RiskPercent;
            LotSlow = o.LotSlow;
            RiskPercentSlow = o.RiskPercentSlow;
            MinOrderTimeSec = o.MinOrderTimeSec;
            MinOrderPoint = o.MinOrderPoint;
            LockType = o.LockType;
            MinGapForOpen = o.MinGapForOpen;
            MaxGapForOpen = o.MaxGapForOpen;
            MinimumLevel = o.MinimumLevel;
            AvtoShiftBid = o.AvtoShiftBid;
            AvtoShiftPeriod = o.AvtoShiftPeriod;
            UseAverageSpread = o.UseAverageSpread;
            GapForOpen = o.GapForOpen;
            GapForClose = o.GapForClose;
            MaxSpread = o.MaxSpread;
            MaxSpreadSlow = o.MaxSpreadSlow;
            MinSpreadSlow = o.MinSpreadSlow;
            Comission = o.Comission;
            SignalMode = o.SignalMode;
            RiskDeposit = o.RiskDeposit;
            StartTime = o.StartTime;
            EndTime = o.EndTime;
            MinFastCoef = o.MinFastCoef;
            FastCoefPeriod = o.FastCoefPeriod;
            OrderType = o.OrderType;
            PendingLifeTimeMs = o.PendingLifeTimeMs;
            PendingDistance = o.PendingDistance;
        }

        public override string ToString()
        {
            return "";
        }
    }
    public class CloseOrderSettingsModel : BaseModel
    {
        public CloseOrderSettingsModel()
        {
            MinimumLevelClose = 0;
            FixTP = 1;
            FixSL = 7;
            FixTrailStart = 5;
            FixTrailStop = 4;
            MinOrderTimeSec2Leg = 60;
            CloseTimerSec = 0;
            PendingDistance = 5;
            PendingLifeTimeMs = 1000;
            OrderType = OrderType.Market;
            HProfit = 10;
            HStop = 0;
            BuildOrderTypeString();
        }

        private string _OrderTypeString;
        public string OrderTypeString
        {
            get { return _OrderTypeString; }
            set { if (_OrderTypeString != value) { _OrderTypeString = value; OnPropertyChanged(); } }
        }
        void BuildOrderTypeString()
        {
            string res = OrderType.ToString();
            if (OrderType!= OrderType.Market)
            {
                res += " " + PendingDistance + "pt " + PendingLifeTimeMs + "ms";
            }
            OrderTypeString = res;
        }
        private decimal _HStop;
        public decimal HStop
        {
            get { return _HStop; }
            set { if (_HStop != value) { _HStop = value; OnPropertyChanged(); } }
        }

        private decimal _HProfit;
        public decimal HProfit
        {
            get { return _HProfit; }
            set { if (_HProfit != value) { _HProfit = value; OnPropertyChanged(); } }
        }
        private OrderType _OrderType;
        public OrderType OrderType
        {
            get { return _OrderType; }
            set { if (_OrderType != value) { _OrderType = value; OnPropertyChanged(); BuildOrderTypeString(); } }
        }
        private int _PendingDistance;
        public int PendingDistance
        {
            get { return _PendingDistance; }
            set { if (_PendingDistance != value) { _PendingDistance = value; OnPropertyChanged(); BuildOrderTypeString(); } }
        }
        private int _PendingLifeTimeMs;
        public int PendingLifeTimeMs
        {
            get { return _PendingLifeTimeMs; }
            set { if (_PendingLifeTimeMs != value) { _PendingLifeTimeMs = value; OnPropertyChanged(); BuildOrderTypeString(); } }
        }
        private decimal _MinimumLevelClose;
        public decimal MinimumLevelClose
        {
            get { return _MinimumLevelClose; }
            set { if (_MinimumLevelClose != value) { _MinimumLevelClose = value; OnPropertyChanged(); } }
        }
        private decimal _FixTP;
        public decimal FixTP
        {
            get { return _FixTP; }
            set { if (_FixTP != value) { _FixTP = value; OnPropertyChanged(); } }
        }
        private decimal _FixSL;
        public decimal FixSL
        {
            get { return _FixSL; }
            set { if (_FixSL != value) { _FixSL = value; OnPropertyChanged(); } }
        }
        private bool _Trailing;
        public bool Trailing
        {
            get { return _Trailing; }
            set { if (_Trailing != value) { _Trailing = value; OnPropertyChanged(); } }
        }

        private decimal _FixTrailStop;
        public decimal FixTrailStop
        {
            get { return _FixTrailStop; }
            set { if (_FixTrailStop != value) { _FixTrailStop = value; OnPropertyChanged(); } }
        }
        private decimal _FixTrailStart;
        public decimal FixTrailStart
        {
            get { return _FixTrailStart; }
            set { if (_FixTrailStart != value) { _FixTrailStart = value; OnPropertyChanged(); } }
        }

        private int _MinOrderTimeSec2Leg;
        public int MinOrderTimeSec2Leg
        {
            get { return _MinOrderTimeSec2Leg; }
            set { if (_MinOrderTimeSec2Leg != value) { _MinOrderTimeSec2Leg = value; OnPropertyChanged(); } }
        }

        private int _CloseTimerSec;
        public int CloseTimerSec
        {
            get { return _CloseTimerSec; }
            set { if (_CloseTimerSec != value) { _CloseTimerSec = value; OnPropertyChanged(); } }
        }

        public void CopyFrom(CloseOrderSettingsModel o)
        {
            MinimumLevelClose = o.MinimumLevelClose;
            FixTP = o.FixTP;
            FixSL = o.FixSL;
            Trailing = o.Trailing;
            FixTrailStop = o.FixTrailStop;
            FixTrailStart = o.FixTrailStart;
            MinOrderTimeSec2Leg = o.MinOrderTimeSec2Leg;
            CloseTimerSec = o.CloseTimerSec;
            OrderType = o.OrderType;
            PendingLifeTimeMs = o.PendingLifeTimeMs;
            PendingDistance = o.PendingDistance;
        }

        public override string ToString()
        {
            return "";
        }
    }

    public class TradeModel : BaseModel
    {
        public static string currencySpot, currencyFuture, fullSymbolFuture;

        public TradeModel()
        {
            MinGapSellA = 0.00m;
            MaxGapBuyA = 0.00m;
            MinGapSellB = 0.00m;
            MaxGapBuyB = 0.00m;
            IntervalA = 1200;
            AllowView = true;
            Leg1 = new ProviderModel();
            Leg2 = new ProviderModel();
            Slow2 = new ProviderModel();
            Providers = new ObservableCollection<ProviderModel>();
            Leg1Providers = new ObservableCollection<ProviderModel>();
            Leg2Providers = new ObservableCollection<ProviderModel>();
            Leg1.Parent = this;
            Leg2.Parent = this;
            Slow2.Parent = this;
            Point = 0.01M;
            Open = new OpenOrderSettingsModel();
            Close = new CloseOrderSettingsModel();
            Magic = 5;
            UseAlignment = true;
            AllowOpen = false;
            SleepMs = 1;
            Slippage = 10;
            DeviationBuy = 0.0m;
            DeviationSell = 0.0m;
            AvgGapBuy = 0.0m;
            AvgGapSell = 0.0m;
        }

        private long _IntervalA;
        public long IntervalA
        {
            get { return _IntervalA; }
            set { _IntervalA = value; OnPropertyChanged(); }
        }

        #region Min & Max Gaps [buy/sell]
        private decimal _MaxGapBuyA;
        [XmlIgnore]
        public decimal MaxGapBuyA
        {
            get { return _MaxGapBuyA; }
            set { if (_MaxGapBuyA != value) { _MaxGapBuyA = value; OnPropertyChanged(); } }
        }

        private decimal _MinGapSellA;
        [XmlIgnore]
        public decimal MinGapSellA
        {
            get { return _MinGapSellA; }
            set { if (_MinGapSellA != value) { _MinGapSellA = value; OnPropertyChanged(); } }
        }

        private decimal _MaxGapBuyB;
        [XmlIgnore]
        public decimal MaxGapBuyB
        {
            get { return _MaxGapBuyB; }
            set { if (_MaxGapBuyB != value) { _MaxGapBuyB = value; OnPropertyChanged(); } }
        }

        private decimal _MinGapSellB;
        [XmlIgnore]
        public decimal MinGapSellB
        {
            get { return _MinGapSellB; }
            set { if (_MinGapSellB != value) { _MinGapSellB = value; OnPropertyChanged(); } }
        }
        #endregion

        private decimal _AvgGapBuy;
        [XmlIgnore]
        public decimal AvgGapBuy
        {
            get { return _AvgGapBuy; }
            set { if (_AvgGapBuy != value) { _AvgGapBuy = value; OnPropertyChanged(); } }
        }

        private decimal _AvgGapSell;
        [XmlIgnore]
        public decimal AvgGapSell
        {
            get { return _AvgGapSell; }
            set { if (_AvgGapSell != value) { _AvgGapSell = value; OnPropertyChanged(); } }
        }

        private decimal _DeviationBuy;
        [XmlIgnore]
        public decimal DeviationBuy
        {
            get { return _DeviationBuy; }
            set { if (_DeviationBuy != value) { _DeviationBuy = value; OnPropertyChanged(); } }
        }

        private decimal _DeviationSell;
        [XmlIgnore]
        public decimal DeviationSell
        {
            get { return _DeviationSell; }
            set { if (_DeviationSell != value) { _DeviationSell = value; OnPropertyChanged(); } }
        }

        private TradeAlgorithm _Algo;
        public TradeAlgorithm Algo
        {
            get { return _Algo; }
            set { if (_Algo != value) { _Algo = value; OnPropertyChanged(); CreateTitle();  } }
        }

        private string _LastLog;
        [XmlIgnore]
        public string LastLog
        {
            get { return _LastLog; }
            set { if (_LastLog != value) { _LastLog = value; OnPropertyChanged(); } }
        }
        private Brush _LastLogBrush;
        [XmlIgnore]
        public Brush LastLogBrush
        {
            get { return _LastLogBrush; }
            set { if (_LastLogBrush != value) { _LastLogBrush = value; OnPropertyChanged(); } }
        }
        private ProviderModel _Leg1;
        public ProviderModel Leg1
        {
            get { return _Leg1; }
            set { if (_Leg1 != value) { _Leg1 = value; OnPropertyChanged(); CreateTitle(); } }
        }
        private ProviderModel _Leg2;
        public ProviderModel Leg2
        {
            get { return _Leg2; }
            set { if (_Leg2 != value) { _Leg2 = value; OnPropertyChanged(); CreateTitle(); } }
        }

        private ObservableCollection<ProviderModel> _Providers;
        public ObservableCollection<ProviderModel> Providers
        {
            get { return _Providers; }
            set { if (_Providers != value) { _Providers = value; OnPropertyChanged(); } }
        }

        private ObservableCollection<ProviderModel> _Leg1Providers;
        public ObservableCollection<ProviderModel> Leg1Providers
        {
            get { return _Leg1Providers; }
            set { if (_Leg1Providers != value) { _Leg1Providers = value; OnPropertyChanged(); } }
        }

        private ObservableCollection<ProviderModel> _Leg2Providers;
        public ObservableCollection<ProviderModel> Leg2Providers
        {
            get { return _Leg2Providers; }
            set { if (_Leg2Providers != value) { _Leg2Providers = value; OnPropertyChanged(); } }
        }

        private ProviderModel _Slow2;
        public ProviderModel Slow2
        {
            get { return _Slow2; }
            set { if (_Slow2 != value) { _Slow2 = value; OnPropertyChanged(); CreateTitle(); } }
        }
        private string _Title;
        public string Title
        {
            get { return _Title; }
            set { if (_Title != value) { _Title = value; OnPropertyChanged(); } }
        }
        private bool _IsInEditMode;
        [XmlIgnore]
        public bool IsInEditMode
        {
            get { return _IsInEditMode; }
            set { if (_IsInEditMode != value) { _IsInEditMode = value; OnPropertyChanged(); } }
        }

        public void CreateTitle()
        {
            if (!Model.Loading)
            {
                string title = App.LanguageKey("locAlgoTitle1Leg");
                if (Algo == TradeAlgorithm.ZigZag) title = "ZigZag";
                if (Algo == TradeAlgorithm.TwoLegArbitrage) title = "TwoLegArbitrage";
                if (Algo == TradeAlgorithm.LatencyArbitrage) title = "LatencyArbitrage";
                if (Algo == TradeAlgorithm.TwoLegArbFutureUSD_M) title = "TwoLegArbFutureUSD_M";
                if (Algo == TradeAlgorithm.TwoLegFutures) title = "TwoLegFutures";
                if (Algo == TradeAlgorithm.TickOptimizer) title = "TickOptimizer";
                if (Algo == TradeAlgorithm.TickOptimizerOG) title = "TickOptimizerOG";
                if (Algo == TradeAlgorithm.Scalper) title = "Scalper";
                if (Algo == TradeAlgorithm.MultiLegSpread) title = App.LanguageKey("locAlgoTitleMultileg");
                Title = title.Trim();
            }
        }
        private decimal _Point;
        public decimal Point
        {
            get { return _Point; }
            set { if (_Point != value) { _Point = value; OnPropertyChanged(); CreateDigits(); } }
        }

        private int _Digits;
        [XmlIgnore]
        public int Digits
        {
            get { return _Digits; }
            set { if (_Digits != value) { _Digits = value; OnPropertyChanged(); } }
        }
        private string _PriceFormat;
        [XmlIgnore]
        public string PriceFormat
        {
            get { return _PriceFormat; }
            set { if (_PriceFormat != value) { _PriceFormat = value; OnPropertyChanged(); } }
        }

        public void CreateDigits()
        {
            int digits = 2;
            try
            {
                decimal n = Point / 1.000000000000000000000000000000m;
                int[] bits = decimal.GetBits(n);
                digits = bits[3] >> 16 & 255;
            }
            catch
            {
            }
            Digits = digits;
            if (digits > 0)
            {
                PriceFormat = "F" + digits.ToString();
            }
            else
            {
                PriceFormat = "";
            }
        }

        public string FormatPrice(decimal price)
        {
            return PriceFormat.Length > 0 ? price.ToString(PriceFormat,CultureInfo.InvariantCulture) : price.ToString(CultureInfo.InvariantCulture);
        }
        private int _Magic;
        public int Magic
        {
            get { return _Magic; }
            set { if (_Magic != value) { _Magic = value; OnPropertyChanged(); } }
        }
        private int _Slippage;
        public int Slippage
        {
            get { return _Slippage; }
            set { if (_Slippage != value) { _Slippage = value; OnPropertyChanged(); } }
        }


        private decimal _GapBuy;
        [XmlIgnore]
        public decimal GapBuy
        {
            get { return _GapBuy; }
            set { if (_GapBuy != value) { _GapBuy = value; OnPropertyChanged(); } }
        }
        private decimal _GapSell;
        [XmlIgnore]
        public decimal GapSell
        {
            get { return _GapSell; }
            set { if (_GapSell != value) { _GapSell = value; OnPropertyChanged(); } }
        }


        private decimal _GapBuy2;
        [XmlIgnore]
        public decimal GapBuy2
        {
            get { return _GapBuy2; }
            set { if (_GapBuy2 != value) { _GapBuy2 = value; OnPropertyChanged(); } }
        }
        private decimal _GapSell2;
        [XmlIgnore]
        public decimal GapSell2
        {
            get { return _GapSell2; }
            set { if (_GapSell2 != value) { _GapSell2 = value; OnPropertyChanged(); } }
        }

        private bool _Started;
        [XmlIgnore]
        public bool Started
        {
            get { return _Started; }
            set { if (_Started != value) { _Started = value; OnPropertyChanged(); } }
        }
        private bool _FeederOk;
        [XmlIgnore]
        public bool FeederOk
        {
            get { return _FeederOk; }
            set { if (_FeederOk != value) { _FeederOk = value; OnPropertyChanged(); } }
        }
        [XmlIgnore]
        public Action<string> LogOrderSuccess;
        [XmlIgnore]
        public Action<string> LogInfo;
        [XmlIgnore]
        public Action<string> LogError;
        [XmlIgnore]
        public Action<string> LogWarning;
        [XmlIgnore]
        public Action LogClear;


        private bool _Log;
        public bool Log
        {
            get { return _Log; }
            set { if (_Log != value) { _Log = value; OnPropertyChanged(); } }
        }
        private bool _AllowEMail;
        public bool AllowEMail
        {
            get { return _AllowEMail; }
            set { if (_AllowEMail != value) { _AllowEMail = value; OnPropertyChanged(); } }
        }

        private bool _AllowOpen;
        public bool AllowOpen
        {
            get { return _AllowOpen; }
            set { if (_AllowOpen != value) { _AllowOpen = value; OnPropertyChanged(); } }
        }
        private bool _UseAlignment;
        public bool UseAlignment
        {
            get { return _UseAlignment; }
            set { if (_UseAlignment != value) { _UseAlignment = value; OnPropertyChanged(); } }
        }
        private bool _AllowView;
        public bool AllowView
        {
            get { return _AllowView; }
            set { if (_AllowView != value) { _AllowView = value; OnPropertyChanged(); } }
        }
        private bool _SaveTicks;
        public bool SaveTicks
        {
            get { return _SaveTicks; }
            set { if (_SaveTicks != value) { _SaveTicks = value; OnPropertyChanged(); } }
        }

        private int _SleepMs;
        public int SleepMs
        {
            get { return _SleepMs; }
            set { if (_SleepMs != value) { _SleepMs = value; OnPropertyChanged(); } }
        }

        private OpenOrderSettingsModel open;
        public OpenOrderSettingsModel Open
        {
            get { return open; }
            set { if (open != value) { open = value; OnPropertyChanged(); } }
        }
        private CloseOrderSettingsModel close;
        public CloseOrderSettingsModel Close
        {
            get { return close; }
            set { if (close != value) { close = value; OnPropertyChanged(); } }
        }

        private AlgoOneLegMultiModel _AlgoOneLegMulti;
        public AlgoOneLegMultiModel AlgoOneLegMulti
        {
            get { return _AlgoOneLegMulti; }
            set { if (_AlgoOneLegMulti != value) { _AlgoOneLegMulti = value; OnPropertyChanged(); } }
        }

        private AlgoControlModel _AlgoControl;
        public AlgoControlModel AlgoControl
        {
            get { return _AlgoControl; }
            set { if (_AlgoControl != value) { _AlgoControl = value; OnPropertyChanged(); } }
        }

        private static XmlSerializer CreateSerializer()
        {
            return new XmlSerializer(typeof(TradeModel));
        }

        public static TradeModel LoadForBacktest(string filename)
        {
            TradeModel res = null;
            try
            {
                using System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open);
                var xs = CreateSerializer();
                res = xs.Deserialize(fs) as TradeModel;
            }
            catch
            {
            }
            return res;
        }
        public void SaveForBacktest(string filename)
        {
            try
            {
                using System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Create);
                var xs = CreateSerializer();
                xs.Serialize(fs, this);
            }
            catch
            {
            }
        }

    }

    public class MainPresetSettings
    {
        public MainPresetSettings()
        {
            Point = 0.00001M;
            Magic = 5;
            Slippage = 10;
            AllowOpen = true;
            SleepMs = 1;
        }
        public decimal Point { get; set; }
        public int Magic { get; set; }
        public int Slippage { get; set; }
        public bool Log { get; set; }
        public bool AllowOpen { get; set; }
        public bool AllowView { get; set; }
        public bool SaveTicks { get; set; }
        public int SleepMs { get; set; }
        public static MainPresetSettings From(TradeModel tm)
        {
            MainPresetSettings res = new MainPresetSettings()
            {
                Point = tm.Point,
                Magic = tm.Magic,
                Slippage = tm.Slippage,
                Log = tm.Log,
                AllowOpen = tm.AllowOpen,
                AllowView = tm.AllowView,
                SaveTicks = tm.SaveTicks,
                SleepMs = tm.SleepMs
            };
            return res;
        }
        public void To(TradeModel tm)
        {
            tm.Point = Point;
            tm.Magic = Magic;
            tm.Slippage = Slippage;
            tm.Log = Log;
            tm.AllowOpen = AllowOpen;
            tm.AllowView = AllowView;
            tm.SaveTicks = SaveTicks;
            tm.SleepMs = SleepMs;
        }
    }

    public class PresetModel
    {
        public PresetModel()
        {
            Open = new OpenOrderSettingsModel();
            Close = new CloseOrderSettingsModel();
            Main = new MainPresetSettings();
        }
        public OpenOrderSettingsModel Open { get; set; }
        public CloseOrderSettingsModel Close { get; set; }
        public MainPresetSettings Main { get; set; }
        public AlgoOneLegMultiModel AlgoOneLegMulti { get; set; }

        private static XmlSerializer CreateSerializer()
        {
            return new XmlSerializer(typeof(PresetModel), new Type[] { typeof(BaseModel) });
        }
        public static PresetModel Load(string filename)
        {
            PresetModel res = null;
            try
            {
                using System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open);
                var xs = CreateSerializer();
                res = xs.Deserialize(fs) as PresetModel;
            }
            catch
            {
            }
            return res;
        }
        public void Save(string filename)
        {
            try
            {
                using System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Create);
                var xs = CreateSerializer();
                xs.Serialize(fs, this);
            }
            catch
            {
            }
        }

        public static void LoadDialog(TradeModel tm)
        {
            string folder = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), ".presets");
            try
            {
                System.IO.Directory.CreateDirectory(folder);
            }
            catch
            {

            }
            string filename = App.OpenFileDialog("Preset|*.preset", folder);
            if (!string.IsNullOrEmpty(filename))
            {
                PresetModel pm = Load(filename);
                if (pm!=null)
                {
                    if (pm.Open != null) tm.Open.CopyFrom(pm.Open);
                    if (pm.Close != null) tm.Close.CopyFrom(pm.Close);
                    if (pm.Main != null) pm.Main.To(tm);
                    PropertyCopier.Copy(pm.AlgoOneLegMulti, tm.AlgoOneLegMulti);
                }
            }

        }
        public static void SaveDialog(TradeModel tm)
        {
            string folder = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), ".presets");
            try
            {
                System.IO.Directory.CreateDirectory(folder);
            }
            catch
            {

            }
            string filename = App.SaveFileDialog("Preset|*.preset", folder);
            if (!string.IsNullOrEmpty(filename))
            {
                PresetModel pm = new PresetModel()
                {
                    Open = tm.Open,
                    Close = tm.Close,
                    Main = MainPresetSettings.From(tm),
                    AlgoOneLegMulti = tm.AlgoOneLegMulti
                };
                pm.Save(filename);
            }
        }
    }
}

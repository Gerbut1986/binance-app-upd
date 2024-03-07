using System;
using System.Linq;
using System.Threading;
using System.Globalization;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml.Serialization;
using MultiTerminal.Connections;
using MultiTerminal.Connections.Models;

namespace BinanceOptionsApp.Models
{
    public enum ProviderMaxMinState
    {
        None,
        Max,
        Min
    }

    public class ProviderModel : BaseModel
    {
        public ProviderModel()
        {
            Name = "Unknown";
            Symbol = "BTCUSDT";
            Prefix = "";
            Postfix = "";
            MultiLegOpenVolume = 0.00001M;
            MinLot = 0.00001M;
            LotStep = 5;
            BidColor = Colors.Red;
            BidWidth = 1;
            AskColor = Colors.Blue;
            AskWidth = 1;
            Lot = 0.00001m;
            AssetBal = 0.00m;
            CurrBal = 0.00m;
            // Future USD-M:
            TotalInitMargin = 0.0m;
            AvailableBalance = 0.0m;
            TotalCrossUnPnl = 0.0m;
            TotalMarginBalance = 0.0m;
            TotalCrossWalletBalance = 0.0m;
            EntryPrice = 0.0m;
            PositionAmt = 0.0m;

            // Margin:
            CollateralMarginLevel = 0.0m;
            MarginLevel = 0.0m;
            TotalCollateralValueInUSDT = 0.0m;
            Borrowed = 0.0m;
            Free = 0.0m;
            Interest = 0.0m;
            Locked = 0.0m;
            NetAsset = 0.0m;
            //GapSell = 0.0000000m;
            //GapBuy = 0.0000000m;

            //Future COIN-M:
            FeeTier = 0.0m;
            WalletBalance = 0.0m;
            UnrealizedProfit = 0.0m;
            PositionInitialMargin = 0.0m;
        }
        public void EditFrom(ProviderModel other)
        {
            Name = other.Name;
            Symbol = other.Symbol;
            Prefix = other.Prefix;
            Postfix = other.Postfix;
            Lot = other.Lot;
            MinLot = other.MinLot;
            LotStep = other.LotStep;
            BidColor = other.BidColor;
            BidWidth = other.BidWidth;
            AskColor = other.AskColor;
            AskWidth = other.AskWidth;
            AssetBal = other.AssetBal;
            CurrBal = other.CurrBal;

            TotalInitMargin = other.TotalInitMargin;
            AvailableBalance = other.AvailableBalance;
            TotalCrossUnPnl = other.TotalCrossUnPnl;
            TotalMarginBalance = other.TotalMarginBalance;
            TotalCrossWalletBalance = other.TotalCrossWalletBalance;
            EntryPrice = other.EntryPrice;
            PositionAmt = other.PositionAmt;

            CollateralMarginLevel =other.CollateralMarginLevel;
            MarginLevel =other.MarginLevel;
            TotalCollateralValueInUSDT =other.TotalCollateralValueInUSDT;
            Borrowed =other.Borrowed;
            Free =other.Free;
            Interest =other.Interest;
            Locked =other.Locked;
            NetAsset =other.NetAsset;

            FeeTier = other.FeeTier;
            WalletBalance = other.WalletBalance;
            UnrealizedProfit = other.UnrealizedProfit;
            PositionInitialMargin = other.PositionInitialMargin;
        }

        public ProviderModel EditClone()
        {
            ProviderModel res = new ProviderModel();
            res.EditFrom(this);
            return res;
        }

        #region From AccountInfoFuture model
        private decimal _TotalInitMargin;
        [XmlIgnore]
        public decimal TotalInitMargin
        {
            get { return _TotalInitMargin; }
            set { if (_TotalInitMargin != value) { _TotalInitMargin = value; OnPropertyChanged(); } }
        }

        private decimal _AvailableBalance;
        [XmlIgnore]
        public decimal AvailableBalance
        {
            get { return _AvailableBalance; }
            set { if (_AvailableBalance != value) { _AvailableBalance = value; OnPropertyChanged(); } }
        }

        private decimal _TotalCrossUnPnl;
        [XmlIgnore]
        public decimal TotalCrossUnPnl
        {
            get { return _TotalCrossUnPnl; }
            set { if (_TotalCrossUnPnl != value) { _TotalCrossUnPnl = value; OnPropertyChanged(); } }
        }

        private decimal _TotalMarginBalance;
        [XmlIgnore]
        public decimal TotalMarginBalance
        {
            get { return _TotalMarginBalance; }
            set { if (_TotalMarginBalance != value) { _TotalMarginBalance = value; OnPropertyChanged(); } }
        }

        private decimal _TotalCrossWalletBalance;
        [XmlIgnore]
        public decimal TotalCrossWalletBalance
        {
            get { return _TotalCrossWalletBalance; }
            set { if (_TotalCrossWalletBalance != value) { _TotalCrossWalletBalance = value; OnPropertyChanged(); } }
        }

        private decimal _EntryPrice;
        [XmlIgnore]
        public decimal EntryPrice
        {
            get { return _EntryPrice; }
            set { if (_EntryPrice != value) { _EntryPrice = value; OnPropertyChanged(); } }
        }

        private decimal _PositionAmt;
        [XmlIgnore]
        public decimal PositionAmt
        {
            get { return _PositionAmt; }
            set { if (_PositionAmt != value) { _PositionAmt = value; OnPropertyChanged(); } }
        }
        #endregion

        #region From BinanceMarginAccountDetails model
        private decimal _CollateralMarginLevel;
        [XmlIgnore]
        public decimal CollateralMarginLevel
        {
            get { return _CollateralMarginLevel; }
            set { if (_CollateralMarginLevel != value) { _CollateralMarginLevel = value; OnPropertyChanged(); } }
        }

        private decimal _MarginLevel;
        [XmlIgnore]
        public decimal MarginLevel
        {
            get { return _MarginLevel; }
            set { if (_MarginLevel != value) { _MarginLevel = value; OnPropertyChanged(); } }
        }

        private decimal _TotalCollateralValueInUSDT;
        [XmlIgnore]
        public decimal TotalCollateralValueInUSDT
        {
            get { return _TotalCollateralValueInUSDT; }
            set { if (_TotalCollateralValueInUSDT != value) { _TotalCollateralValueInUSDT = value; OnPropertyChanged(); } }
        }

        private decimal _Borrowed;
        [XmlIgnore]
        public decimal Borrowed
        {
            get { return _Borrowed; }
            set { if (_Borrowed != value) { _Borrowed = value; OnPropertyChanged(); } }
        }

        private decimal _Free;
        [XmlIgnore]
        public decimal Free
        {
            get { return _Free; }
            set { if (_Free != value) { _Free = value; OnPropertyChanged(); } }
        }

        private decimal _Interest;
        [XmlIgnore]
        public decimal Interest
        {
            get { return _Interest; }
            set { if (_Interest != value) { _Interest = value; OnPropertyChanged(); } }
        }

        private decimal _Locked;
        [XmlIgnore]
        public decimal Locked
        {
            get { return _Locked; }
            set { if (_Locked != value) { _Locked = value; OnPropertyChanged(); } }
        }

        private decimal _NetAsset;
        [XmlIgnore]
        public decimal NetAsset
        {
            get { return _NetAsset; }
            set { if (_NetAsset != value) { _NetAsset = value; OnPropertyChanged(); } }
        }
        #endregion      

        #region AccountInfo_Coin_M_Future:
        private decimal _FeeTier;
        [XmlIgnore]
        public decimal FeeTier
        {
            get { return _FeeTier; }
            set { if (_FeeTier != value) { _FeeTier = value; OnPropertyChanged(); } }
        }

        private decimal _WalletBalance;
        [XmlIgnore]
        public decimal WalletBalance
        {
            get { return _WalletBalance; }
            set { if (_WalletBalance != value) { _WalletBalance = value; OnPropertyChanged(); } }
        }

        private decimal _UnrealizedProfit;
        [XmlIgnore]
        public decimal UnrealizedProfit
        {
            get { return _UnrealizedProfit; }
            set { if (_UnrealizedProfit != value) { _UnrealizedProfit = value; OnPropertyChanged(); } }
        }

        private decimal _PositionInitialMargin;
        [XmlIgnore]
        public decimal PositionInitialMargin
        {
            get { return _PositionInitialMargin; }
            set { if (_PositionInitialMargin != value) { _PositionInitialMargin = value; OnPropertyChanged(); } }
        }
        #endregion

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { if (_Name != value) { _Name = value; OnPropertyChanged(); if (Parent != null) Parent.CreateTitle(); } }
        }
        public string FullSymbol
        {
            get
            {
                return Prefix + Symbol + Postfix;
            }
        }

        private string _SymbolAsset;
        public string SymbolAsset
        {
            get { return _SymbolAsset; }
            set 
            {
                if (_SymbolAsset != value)
                {
                    _SymbolAsset = value; OnPropertyChanged(); 
                }
            }
        }

        private string _SymbolCurrency;
        public string SymbolCurrency
        {
            get { return _SymbolCurrency; }
            set
            {
                if (_SymbolCurrency != value)
                {
                    _SymbolCurrency = value; OnPropertyChanged();
                }
            }
        }

        private string _Symbol;
        public string Symbol
        {
            get { return _Symbol; }
            set { if (_Symbol != value) { _Symbol = value; OnPropertyChanged(); OnPropertyChanged("FullSymbol"); if (Parent != null) Parent.CreateTitle(); } }
        }

        private string _Prefix;
        public string Prefix
        {
            get { return _Prefix; }
            set { if (_Prefix != value) { _Prefix = value; OnPropertyChanged(); OnPropertyChanged("FullSymbol"); } }
        }

        private string _Postfix;
        public string Postfix
        {
            get { return _Postfix; }
            set { if (_Postfix != value) { _Postfix = value; OnPropertyChanged(); OnPropertyChanged("FullSymbol"); } }
        }

        private decimal _MinLot;
        public decimal MinLot
        {
            get { return _MinLot; }
            set { if (_MinLot != value) { _MinLot = value; OnPropertyChanged(); } }
        }

        private int _LotStep;
        public int LotStep
        {
            get { return _LotStep; }
            set { if (_LotStep != value) { _LotStep = value; OnPropertyChanged(); } }
        }

        private decimal _Lot;
        public decimal Lot
        {
            get { return _Lot; }
            set { if (_Lot != value) { _Lot = value; OnPropertyChanged(); } }
        }

        private Color _BidColor;
        public Color BidColor
        {
            get { return _BidColor; }
            set { if (_BidColor != value) { _BidColor = value; OnPropertyChanged(); } }
        }
        private int _BidWidth;
        public int BidWidth
        {
            get { return _BidWidth; }
            set { if (_BidWidth != value) { _BidWidth = value; OnPropertyChanged(); } }
        }

        private Color _AskColor;
        public Color AskColor
        {
            get { return _AskColor; }
            set { if (_AskColor != value) { _AskColor = value; OnPropertyChanged(); } }
        }
        private int _AskWidth;
        public int AskWidth
        {
            get { return _AskWidth; }
            set { if (_AskWidth != value) { _AskWidth = value; OnPropertyChanged(); } }
        }

        private decimal _AverageTimeBetweenTicks;
        [XmlIgnore]
        public decimal AverageTimeBetweenTicks
        {
            get { return _AverageTimeBetweenTicks; }
            set { if (_AverageTimeBetweenTicks != value) { _AverageTimeBetweenTicks = value; OnPropertyChanged(); } }
        }

        private decimal _MaxTPS;
        [XmlIgnore]
        public decimal MaxTPS
        {
            get { return _MaxTPS; }
            set { if (_MaxTPS != value) { _MaxTPS = value; OnPropertyChanged(); } }
        }

        public decimal GetTicksPerSecond()
        {
            return AverageTimeBetweenTicks > 0 ? 1000.0M / AverageTimeBetweenTicks : 0;
        }

        private DateTime _Time;
        [XmlIgnore]
        public DateTime Time
        {
            get { return _Time; }
            set { if (_Time != value) { _Time = value; OnPropertyChanged(); } }
        }

        private decimal _Bid;
        [XmlIgnore]
        public decimal Bid
        {
            get { return _Bid; }
            set { if (_Bid != value) { _Bid = value; OnPropertyChanged(); } }
        }

        private decimal _Ask;
        [XmlIgnore]
        public decimal Ask
        {
            get { return _Ask; }
            set { if (_Ask != value) { _Ask = value; OnPropertyChanged(); } }
        }

        private decimal _AssetBal;
        [XmlIgnore]
        public decimal AssetBal
        {
            get { return _AssetBal; }
            set { if (_AssetBal != value) { _AssetBal = value; OnPropertyChanged(); } }
        }

        private decimal _CurrBal;
        [XmlIgnore]
        public decimal CurrBal
        {
            get { return _CurrBal; }
            set { if (_CurrBal != value) { _CurrBal = value; OnPropertyChanged(); } }
        }

        public void CalculateViewSpread(decimal point)
        {
            int spread = (int)((Ask - Bid) / point);
            decimal ma= spreadMA.Process(spread);
            ViewSpread = spread.ToString()+" | "+ma.ToString("F2",CultureInfo.InvariantCulture);
        }

        private string _ViewSpread;
        [XmlIgnore]
        public string ViewSpread
        {
            get { return _ViewSpread; }
            set { if (_ViewSpread != value) { _ViewSpread = value; OnPropertyChanged(); } }
        }

        private Helpers.MovingAverage spreadMA = new Helpers.MovingAverage(100);
        public void InitView()
        {
            spreadMA = new Helpers.MovingAverage(100);
            Bid = 0;
            Ask = 0;
            ViewSpread = "";
            Time = DateTime.MinValue;
            Volume = 0;
            Balance = null;
            AverageTimeBetweenTicks = 0;
        }

        private decimal _Volume;
        [XmlIgnore]
        public decimal Volume
        {
            get { return _Volume; }
            set { if (_Volume != value) { _Volume = value; OnPropertyChanged(); } }
        }
        private decimal? _Balance;
        [XmlIgnore]
        public decimal? Balance
        {
            get { return _Balance; }
            set { if (_Balance != value) { _Balance = value; OnPropertyChanged(); } }
        }


        private ProviderMaxMinState _MaxMinState;
        [XmlIgnore]
        public ProviderMaxMinState MaxMinState
        {
            get { return _MaxMinState; }
            set { if (_MaxMinState != value) { _MaxMinState = value; OnPropertyChanged(); } }
        }

        private int _ViewNumber;
        public int ViewNumber
        {
            get { return _ViewNumber; }
            set { if (_ViewNumber != value) { _ViewNumber = value; OnPropertyChanged(); } }
        }

        private decimal _MultiLegOpenVolume;
        public decimal MultiLegOpenVolume
        {
            get { return _MultiLegOpenVolume; }
            set { if (_MultiLegOpenVolume != value) { _MultiLegOpenVolume = value; OnPropertyChanged(); } }
        }

        private decimal _MultiLegGap;
        [XmlIgnore]
        public decimal MultiLegGap
        {
            get { return _MultiLegGap; }
            set { if (_MultiLegGap != value) { _MultiLegGap = value; OnPropertyChanged(); } }
        }

        private decimal _MultiLegProfit;
        [XmlIgnore]
        public decimal MultiLegProfit
        {
            get { return _MultiLegProfit; }
            set { if (_MultiLegProfit != value) { _MultiLegProfit = value; OnPropertyChanged(); } }
        }

        private decimal _MultiLegOperationGap;
        [XmlIgnore]
        public decimal MultiLegOperationGap
        {
            get { return _MultiLegOperationGap; }
            set { if (_MultiLegOperationGap != value) { _MultiLegOperationGap = value; OnPropertyChanged(); } }
        }

        private TradeModel _Parent;
        [XmlIgnore]
        public TradeModel Parent
        {
            get { return _Parent; }
            set { if (_Parent != value) { _Parent = value; OnPropertyChanged(); } }
        }

        internal IConnector CreateConnector(IConnectorLogger logger, ManualResetEvent cancelToken, int sleepMs, Dispatcher dispatcher, bool hiddenLogs=true)
        {
            ConnectionModel connection = Model.AllConnections.FirstOrDefault(x => x.Name == Name);
            if (connection != null)
            {
                if (Model.IsBrokerPresent(connection.GetBrokerCode()))
                {
                    if (connection is BinanceConnectionModel)
                    {
                        return ConnectorsFactory.Current.CreateBinance(logger, cancelToken, connection as BinanceConnectionModel);
                    }
                    if (connection is TestnetConnectionModel)
                    {
                        return ConnectorsFactory.Current.CreateBinancefutureTestnet(logger, cancelToken, connection as TestnetConnectionModel);
                    }
                    if (connection is BinanceOptionConnectionModel)
                    {
                        return ConnectorsFactory.Current.CreateBinanceOption(logger, cancelToken, connection as BinanceOptionConnectionModel);
                    }
                    if (connection is BinanceFutureConnectionModel)
                    {
                        return ConnectorsFactory.Current.CreateBinanceFuture(logger, cancelToken, connection as BinanceFutureConnectionModel);
                    }
                    if (connection is TestnetSpotConnectionModel)
                    {
                        return ConnectorsFactory.Current.CreateBinanceTestnetSpot(logger, cancelToken, connection as TestnetSpotConnectionModel);
                    }

                }
            }
            return new ProxyConnector();
        }

        internal string GetSymbolId()
        {
            ConnectionModel connection = Model.AllConnections.FirstOrDefault(x => x.Name == Name);
            string gbc = connection.GetBrokerCode();
            System.Collections.Generic.List<AllowedInstrument> allowedInstruments = Model.GetAllowedInstruments(gbc);
            if (allowedInstruments.Count > 0)
            {
                AllowedInstrument smb = allowedInstruments.FirstOrDefault(x => x.Name == Symbol);
                if (smb != null) return smb.Id;
            }
            return Symbol;
        }
    }
}

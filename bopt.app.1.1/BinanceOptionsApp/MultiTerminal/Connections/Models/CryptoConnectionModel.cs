namespace MultiTerminal.Connections.Models
{
    public enum AccountTradeType : int
    {
        SPOT = 0,
        MARGIN = 1,
        OPTION = 2,
        USD_M = 3,
        COIN_M = 4
    }
    public class CryptoConnectionModel : ConnectionModel
    {
        private string _Key;
        public string Key
        {
            get => _Key;
            set { if (_Key != value) { _Key = value; FillNameSuffix(); OnPropertyChanged(); } }
        }

        private string _Secret;
        public string Secret
        {
            get => _Secret; 
            set { if (_Secret != value) { _Secret = value; OnPropertyChanged(); } }
        }

        private AccountTradeType _AccountTradeType;
        public AccountTradeType AccountTradeType
        {
            get => _AccountTradeType;
            set { if (_AccountTradeType != value) { _AccountTradeType = value; FillNameSuffix(); OnPropertyChanged(); } }
        }
        void FillNameSuffix()
        {
            NameSuffix = $"{Key.Substring(0, 8)}[{AccountTradeType}]";
        }

        public CryptoConnectionModel() : base()
        {
        }

        public override void From(ConnectionModel other)
        {
            base.From(other);
            if (other is CryptoConnectionModel ccm)
            {
                Key = ccm.Key;
                Secret = ccm.Secret;
                AccountTradeType = ccm.AccountTradeType;
            }
        }
    }
}

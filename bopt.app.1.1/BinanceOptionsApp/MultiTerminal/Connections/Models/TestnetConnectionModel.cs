using BinanceOptionsApp.Editors;

namespace MultiTerminal.Connections.Models
{
    [BrokerCode("Private7.Binance Testnet")]
    [ConnectionEditor(typeof(EditorTestNet))]
    public class TestnetConnectionModel : CryptoConnectionModel
    {
        private BinancePositionMode _PositionMode;

        public BinancePositionMode PositionMode
        {
            get => this._PositionMode;
            set
            {
                if (this._PositionMode == value)
                    return;
                this._PositionMode = value;
                this.OnPropertyChanged(nameof(PositionMode));
            }
        }

        public override void From(ConnectionModel other)
        {
            base.From(other);
            if (!(other is TestnetConnectionModel testnetConnectionModel))
                return;
            this.PositionMode = testnetConnectionModel.PositionMode;
        }
    }
}

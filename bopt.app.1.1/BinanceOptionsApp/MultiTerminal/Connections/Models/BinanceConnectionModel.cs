using BinanceOptionsApp.Editors;

namespace MultiTerminal.Connections.Models
{
    [BrokerCode("Private7.Binance")]
    [ConnectionEditor(typeof(EditorBinance))]
    public class BinanceConnectionModel : CryptoConnectionModel
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
            if (!(other is BinanceConnectionModel binanceConnectionModel))
                return;
            this.PositionMode = binanceConnectionModel.PositionMode;
        }
    }
}

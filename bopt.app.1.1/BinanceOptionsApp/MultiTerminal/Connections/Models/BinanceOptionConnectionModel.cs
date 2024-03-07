using BinanceOptionsApp.Editors;

namespace MultiTerminal.Connections.Models
{
    [BrokerCode("Private7.BinanceOptions")]
    [ConnectionEditor(typeof(BinanceOptions))]
    public class BinanceOptionConnectionModel : CryptoConnectionModel
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
            if (!(other is BinanceOptionConnectionModel binanceOptConnectionModel))
                return;
            this.PositionMode = binanceOptConnectionModel.PositionMode;
        }
    }
}

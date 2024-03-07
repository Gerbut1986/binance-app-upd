using MultiTerminal.Connections.Models;

namespace BinanceOptionsApp
{
    public interface ITradeTabInterface
    {
        void InitializeTab();
        void RestoreNullCombo(ConnectionModel cm);
        void Start();
        void Stop(bool wait);
    }
}

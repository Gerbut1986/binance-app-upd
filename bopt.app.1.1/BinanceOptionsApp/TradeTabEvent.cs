namespace BinanceOptionsApp
{
    using BinanceOptionsApp.Models;

    public  class TradeTabEvent
    {
    }

    public class TradeTabCloneEvent : TradeTabEvent
    {
        public TradeModel Model { get; }
        public TradeTabCloneEvent(TradeModel model) => this.Model = model;
    }
}

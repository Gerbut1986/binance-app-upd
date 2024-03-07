namespace Helpers.Extensions
{
    using MultiTerminal.Connections.API.Spot;
    using System.Globalization;

    internal class ObjConverter
    {
        public static TimeAndSale_BidAsk GetConcreteType(TimeAndSale tasApiModel)
        {
            var model = new TimeAndSale_BidAsk();
            model.EventType = tasApiModel.e;
            model.EventTime = tasApiModel.E;
            model.Symbol = tasApiModel.s;
            model.Ticket = tasApiModel.t;
            model.Price = decimal.Parse(tasApiModel.p, CultureInfo.InvariantCulture);
            model.Volume = decimal.Parse(tasApiModel.q, CultureInfo.InvariantCulture);
            model.BuyerID = tasApiModel.b;
            model.SellerID = tasApiModel.a;
            model.DealTime = tasApiModel.T;//.GetFullTime();
            model.IsBuyLimit = tasApiModel.m;
            model.IsSellLimit = tasApiModel.M;
            return model;
        }
    }
}

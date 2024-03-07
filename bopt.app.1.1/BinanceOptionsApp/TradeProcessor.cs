using MultiTerminal.Connections;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace BinanceOptionsApp
{
    public enum EntryOrderType
    {
        Market,
        Limit,
        Stop
    }

    internal class TradeCommand
    {
    }
    internal class TradeCommandOpen : TradeCommand
    {
        public FillPolicy Policy;
        public int CommandSlippagePt;
        public OrderSide CommandSide;
        public double CommandPrice;
        public double CommandVolume;
        public TradeOrderInformation ResultOrder;
        public string ResultError;
        public EntryOrderType CommandOrderType;
        public int PendingDistancePt;
        public int PendingLifetimeMs;
        public TradeCommandOpen(OrderSide side, double price, double volume, FillPolicy policy, int slippagePt, EntryOrderType type, int pendingDistancePt, int pendingLifetimeMs)
        {
            CommandSide = side;
            CommandPrice = price;
            CommandVolume = volume;
            CommandSlippagePt = slippagePt;
            CommandOrderType = type;
            PendingDistancePt = pendingDistancePt;
            PendingLifetimeMs = pendingLifetimeMs;
            Policy = policy;
        }
    }
    internal class TradeCommandClose : TradeCommand
    {
        public int CommandSlippagePt;
        public OrderSide CommandSide;
        public string CommandId;
        public double CommandPrice;
        public double CommandVolume;
        public TradeOrderInformation ResultOrder;
        public string ResultError;
        public EntryOrderType CommandOrderType;
        public int PendingDistancePt;
        public int PendingLifetimeMs;
        public TradeCommandClose(string id, OrderSide side, double price, double volume, int slippagePt, EntryOrderType type, int pendingDistancePt, int pendingLifetimeMs)
        {
            CommandSide = side;
            CommandId = id;
            CommandPrice = price;
            CommandVolume = volume;
            CommandSlippagePt = slippagePt;
            CommandOrderType = type;
            PendingDistancePt = pendingDistancePt;
            PendingLifetimeMs = pendingLifetimeMs;
        }
    }

    public class TradeOrderInformation
    {
        public string Id;
        public OrderSide Side;
        public double Volume;
        public double OpenPrice;
        public DateTime OpenTime;
        public int OpenExecutionTimeMs;
        public int OpenSlippagePt;
        public double ClosePrice;
        public DateTime CloseTime;
        public int CloseExecutionTimeMs;
        public int CloseSlippagePt;
        public int ProfitPt;

        public int CalculateProfitPt(double bid, double ask, double pointInv)
        {
            ProfitPt = (int)Math.Round((Side == OrderSide.Buy ? bid - OpenPrice : OpenPrice - ask) * pointInv, 0);
            return ProfitPt;
        }
        public int DurationMs(DateTime currentTime)
        {
            int res = (int)(currentTime - OpenTime).TotalMilliseconds;
            if (res < 0) res = 0;
            return res;
        }
    }

    interface ITradeProcessorListener
    {
        void CommandCompleted(TradeProcessor trade,TradeCommand command);
    }

    internal class TradeProcessor
    {
        readonly IConnector connector;
        readonly ITradeProcessorListener listener;
        readonly string symbol;
        readonly int magic;
        readonly double point;
        readonly double pointinv;
        readonly int digits;
        readonly string priceFormat;
        readonly bool backtest;
        readonly List<TradeCommand> commands = new List<TradeCommand>();
        readonly ManualResetEvent threadStop = new ManualResetEvent(false);
        readonly ManualResetEvent threadStopped = new ManualResetEvent(false);
        public TradeOrderInformation Order;
        public int SessionProfitPt;
        public TradeProcessor(ITradeProcessorListener listener, IConnector connector, string symbol, int digits, double point, int magic, bool backtest)
        {
            this.connector = connector;
            this.listener = listener;
            this.symbol = symbol;
            this.magic = magic;
            this.digits = digits;
            this.point = point;
            this.backtest = backtest;
            priceFormat = "F" + digits;
            pointinv = 1.0 / point;
            if (!backtest)
            {
                new Thread(ThreadProc).Start();
            }
        }
        public string FormatPrice(double price)
        {
            return price.ToString(priceFormat, CultureInfo.InvariantCulture);
        }
        public bool IsBusy()
        {
            if (backtest)
            {
                return false;
            }
            lock (commands)
            {
                return commands.Count > 0;
            }
        }
        public void OpenOrder(OrderSide side, double price, double volume, FillPolicy policy, int slippagePt, DateTime currentTime, EntryOrderType orderType, int pendingDistancePt, int pendingLifetimeMs)
        {
            var command = new TradeCommandOpen(side, price, volume, policy, slippagePt, orderType, pendingDistancePt,pendingLifetimeMs);
            if (backtest)
            {
                Order = new TradeOrderInformation()
                {
                    Id = currentTime.ToString(),
                    OpenPrice = price,
                    OpenTime = currentTime,
                    Side = side,
                    Volume = volume
                };
                command.ResultOrder = Order;
                listener.CommandCompleted(this, command);
                return;
            }
            lock (commands)
            {
                commands.Add(command);
            }
        }
        public void CloseOrder(string orderId, OrderSide side, double price, double volume, int slippagePt, DateTime currentTime, EntryOrderType orderType, int pendingDistancePt, int pendingLifetimeMs)
        {
            var command = new TradeCommandClose(orderId, side, price, volume, slippagePt,orderType,pendingDistancePt,pendingLifetimeMs);
            if (backtest)
            {
                if (Order!=null)
                {
                    Order.ClosePrice = price;
                    Order.CloseTime = currentTime;
                    SessionProfitPt += Order.CalculateProfitPt(price, price, pointinv);
                    command.ResultOrder = Order;
                    listener.CommandCompleted(this, command);
                    Order = null;
                }
                return;
            }
            lock (commands)
            {
                commands.Add(command);
            }
        }
        TradeOrderInformation DetectOpenedMarketOrder(TradeCommandOpen to, OrderOpenResult openResult)
        {
            var marketOrder = connector.GetOrders(symbol, magic, 1).FirstOrDefault();
            if (marketOrder != null)
            {
                double openPrice = Math.Round((double)marketOrder.OpenPrice, digits);
                return new TradeOrderInformation()
                {
                    Id = marketOrder.Id,
                    OpenPrice = openPrice,
                    OpenTime = DateTime.UtcNow,
                    OpenSlippagePt = to!=null ? (int)Math.Round((to.CommandSide == OrderSide.Buy ? to.CommandPrice - openPrice : openPrice - to.CommandPrice) * pointinv, 0) : 0,
                    OpenExecutionTimeMs = openResult!=null ? (int)openResult.ExecutionTime.TotalMilliseconds : 0,
                    Side = marketOrder.Side,
                    Volume = Math.Round((double)marketOrder.Volume, 5)
                };
            }
            return null;
        }
        void ThreadProc()
        {
            lock (commands)
            {
                DetectOpenedMarketOrder(null, null);
            }
            while (!threadStop.WaitOne(0))
            {
                TradeCommand[] cmds = null;
                lock (commands)
                {
                    if (commands.Count>0)
                    {
                        cmds = commands.ToArray();
                    }
                }

                if (cmds != null)
                {
                    foreach (var cmd in cmds)
                    {
                        if (cmd is TradeCommandOpen to)
                        {
                            OrderType commandOrderType = OrderType.Market;
                            double commandPrice = to.CommandPrice;
                            if (to.CommandOrderType == EntryOrderType.Limit)
                            {
                                commandOrderType = OrderType.Limit;
                                commandPrice = Math.Round(to.CommandSide == OrderSide.Buy ? commandPrice - point*to.PendingDistancePt : commandPrice+point*to.PendingDistancePt, digits);
                            } 
                            else if (to.CommandOrderType == EntryOrderType.Stop)
                            {
                                commandOrderType = OrderType.Stop;
                                commandPrice = Math.Round(to.CommandSide == OrderSide.Buy ? commandPrice + point * to.PendingDistancePt : commandPrice - point * to.PendingDistancePt, digits);
                            }
                            var openResult = connector.Open(symbol, (decimal)commandPrice, (decimal)to.CommandVolume, (FillPolicy)to.Policy, to.CommandSide, magic, to.CommandSlippagePt, 1, commandOrderType, -1);
                            if (openResult.Error==null)
                            {
                                if (to.CommandOrderType == EntryOrderType.Market)
                                {
                                    double openPrice = Math.Round((double)openResult.OpenPrice, digits);
                                    Order = new TradeOrderInformation()
                                    {
                                        Id = openResult.Id,
                                        OpenPrice = openPrice,
                                        OpenTime = DateTime.UtcNow,
                                        OpenSlippagePt = (int)Math.Round((to.CommandSide == OrderSide.Buy ? to.CommandPrice - openPrice : openPrice - to.CommandPrice) * pointinv, 0),
                                        OpenExecutionTimeMs = (int)openResult.ExecutionTime.TotalMilliseconds,
                                        Side = to.CommandSide,
                                        Volume = Math.Round(to.CommandVolume, 5)
                                    };
                                    to.ResultOrder = Order;
                                }
                                else
                                {
                                    TradeOrderInformation filledOrder = null;
                                    DateTime endWaitTime = DateTime.UtcNow.AddMilliseconds(25 + to.PendingLifetimeMs);
                                    while (DateTime.UtcNow <= endWaitTime && filledOrder==null)
                                    {
                                        filledOrder = DetectOpenedMarketOrder(to, openResult);
                                        Thread.Sleep(15);
                                    }
                                    if (filledOrder==null)
                                    {
                                        //connector.OrderDelete(symbol, to.orderId, to.origClientOrderId, to.CommandSide, (decimal)to.CommandVolume, (decimal)commandPrice);
                                        filledOrder = DetectOpenedMarketOrder(to, openResult);
                                    }
                                    if (filledOrder!=null)
                                    {
                                        Order = filledOrder;
                                        to.ResultOrder = filledOrder;
                                    }
                                }
                            }
                            else
                            {
                                to.ResultError = openResult.Error;
                            }
                        }
                        else if (cmd is TradeCommandClose tc)
                        {
                            var closeResult = connector.Close(symbol, tc.CommandId, (decimal)tc.CommandPrice, (decimal)tc.CommandVolume, tc.CommandSide, tc.CommandSlippagePt, OrderType.Market, 0);
                            if (closeResult.Error==null)
                            {
                                double closePrice = Math.Round((double)closeResult.ClosePrice, digits);
                                if (Order!=null)
                                {
                                    Order.CloseTime = DateTime.UtcNow;
                                    Order.ClosePrice = closePrice;
                                    Order.CloseExecutionTimeMs = (int)closeResult.ExecutionTime.TotalMilliseconds;
                                    Order.CloseSlippagePt = (int)Math.Round((tc.CommandSide == OrderSide.Buy ? closePrice - tc.CommandPrice : tc.CommandPrice - closePrice) * pointinv, 0);
                                    SessionProfitPt += Order.CalculateProfitPt(Order.ClosePrice, Order.ClosePrice, pointinv);
                                    tc.ResultOrder = Order;
                                    Order = null;
                                }
                            }
                            else
                            {
                                tc.ResultError = closeResult.Error;
                            }
                        }
                        listener.CommandCompleted(this, cmd);
                    }
                    lock (commands)
                    {
                        commands.Clear();
                    }
                }
            }
            threadStopped.Set();
        }
        public void Stop(bool wait)
        {
            if (!backtest)
            {
                threadStop.Set();
                if (wait) threadStopped.WaitOne();
            }
        }
    }
}

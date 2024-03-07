using MultiTerminal.Connections;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using VisualMarketsEngine;
using BinanceOptionsApp.Controls;

namespace BinanceOptionsApp.Models
{
    public class AlgoOneLegMultiDiagnosticsSetters : ITradeVisualizerDebugData
    {
        public DiagnosticsDateTimeValue CurrentTime { get; set; }
        public DiagnosticsDoubleValue SlowBid { get; set; }
        public DiagnosticsDoubleValue SlowAsk { get; set; }
        public DiagnosticsDoubleValue[] FastBid { get; set; }
        public DiagnosticsDoubleValue[] FastAsk { get; set; }
        public DiagnosticsDoubleValue[] ShiftedFastBid { get; set; }
        public DiagnosticsDoubleValue[] ShiftedFastAsk { get; set; }
        public DiagnosticsIntValue SlowSpreadPt { get; set; }
        public DiagnosticsIntValue[] FastSpreadPt { get; set; }
        public DiagnosticsDateTimeValue SlowTime { get; set; }
        public DiagnosticsDateTimeValue[] FastTime { get; set; }
        public DiagnosticsIntValue[] GapBuyPt { get; set; }
        public DiagnosticsIntValue[] GapSellPt { get; set; }
        public DiagnosticsDoubleValue OrderVolume { get; set; }
        public DiagnosticsIntValue OrderDurationMs { get; set; }
        public DiagnosticsIntValue OrderProfitPt { get; set; }
        public DiagnosticsIntValue SessionProfitPt { get; set; }
        public DiagnosticsStringValue Type { get; set; }
        public DiagnosticsIntValue TickNo { get; set; }

        public void SetValues(ITradeVisualizerDiagnosticsData _data)
        {
            if (_data is AlgoOneLegMultiDiagnostics data)
            {
                SlowBid.Value=data.SlowBid;
                SlowAsk.Value=data.SlowAsk;
                SlowSpreadPt.Value = data.SlowSpreadPt;
                SlowTime.Value = data.SlowTime;
                OrderVolume.Value = data.OrderVolume;
                OrderDurationMs.Value = data.OrderDurationMs;
                OrderProfitPt.Value = data.OrderProfitPt;
                SessionProfitPt.Value = data.SessionProfitPt;
                CurrentTime.Value = data.CurrentTime;
                for (int i = 0; i < FastBid.Length; i++)
                {
                    FastBid[i].Value = data.FastBid[i];
                    FastAsk[i].Value = data.FastAsk[i];
                    ShiftedFastBid[i].Value = data.ShiftedFastBid[i];
                    ShiftedFastAsk[i].Value = data.ShiftedFastAsk[i];
                    FastSpreadPt[i].Value = data.FastSpreadPt[i];
                    FastTime[i].Value = data.FastTime[i];
                    GapBuyPt[i].Value = data.GapBuyPt[i];
                    GapSellPt[i].Value = data.GapSellPt[i];
                }
                Type.Value = data.Type.ToString();
                TickNo.Value = data.TickNo;
            }
        }
        public AlgoOneLegMultiDiagnosticsSetters Clone()
        {
            var res = new AlgoOneLegMultiDiagnosticsSetters()
            {
                CurrentTime = CurrentTime.Clone(),
                FastAsk = new DiagnosticsDoubleValue[FastAsk.Length],
                FastBid = new DiagnosticsDoubleValue[FastBid.Length],
                FastSpreadPt = new DiagnosticsIntValue[FastSpreadPt.Length],
                FastTime = new DiagnosticsDateTimeValue[FastTime.Length],
                GapBuyPt = new DiagnosticsIntValue[GapBuyPt.Length],
                GapSellPt = new DiagnosticsIntValue[GapSellPt.Length],
                OrderDurationMs = OrderDurationMs.Clone(),
                OrderProfitPt = OrderProfitPt.Clone(),
                OrderVolume = OrderVolume.Clone(),
                SessionProfitPt = SessionProfitPt.Clone(),
                ShiftedFastAsk = new DiagnosticsDoubleValue[ShiftedFastAsk.Length],
                ShiftedFastBid = new DiagnosticsDoubleValue[ShiftedFastBid.Length],
                SlowAsk = SlowAsk.Clone(),
                SlowBid = SlowBid.Clone(),
                SlowSpreadPt = SlowSpreadPt.Clone(),
                SlowTime = SlowTime.Clone(),
                Type = Type.Clone(),
                TickNo = TickNo.Clone()
            };
            for (int i=0;i<res.FastAsk.Length;i++)
            {
                res.FastAsk[i] = FastAsk[i].Clone();
                res.FastBid[i] = FastBid[i].Clone();
                res.FastSpreadPt[i] = FastSpreadPt[i].Clone();
                res.FastTime[i] = FastTime[i].Clone();
                res.GapBuyPt[i] = GapBuyPt[i].Clone();
                res.GapSellPt[i] = GapSellPt[i].Clone();
                res.ShiftedFastAsk[i] = ShiftedFastAsk[i].Clone();
                res.ShiftedFastBid[i] = ShiftedFastBid[i].Clone();
            }
            return res;
        }

        public DateTime GetCurrentTime()
        {
            return CurrentTime.Value ?? DateTime.MinValue;
        }
    }
    public class AlgoOneLegMultiChartSetters
    {
        public ChartGroup.InternalChart SourcePriceChart;
        public ChartGroup.InternalChart PriceChart;
        public ChartGroup.InternalChart GapChart;
        public ChartGroup.InternalChart ProfitChart;
        public int SlowBidFlow;
        public int SlowAskFlow;
        public int[] FastBidFlows;
        public int[] FastAskFlows;
        public int[] ShiftedFastBidFlows;
        public int[] ShiftedFastAskFlows;
        public int[] GapBuyFlows;
        public int[] GapSellFlows;
        public int ProfitFlow;
        public int EquityFlow;
        public int PositionsFlow;
        public int SignalsFlow;
        internal void Add(ITradeVisualizerDiagnosticsData[] data, TradeOrderInformation[] orders, TradeSignal[] tradeSignals)
        {
            if (data != null && data.Length > 0)
            {
                PriceChart.AddDoubleTimeValuesTo(SlowBidFlow, data.Select(x => new DoubleTimeValue((x as AlgoOneLegMultiDiagnostics).SlowBid, (x as AlgoOneLegMultiDiagnostics).CurrentTime)).ToArray());
                PriceChart.AddDoubleTimeValuesTo(SlowAskFlow, data.Select(x => new DoubleTimeValue((x as AlgoOneLegMultiDiagnostics).SlowAsk, (x as AlgoOneLegMultiDiagnostics).CurrentTime)).ToArray());
                for (int i = 0; i < FastBidFlows.Length; i++)
                {
                    SourcePriceChart.AddDoubleTimeValuesTo(FastBidFlows[i], data.Select(x => new DoubleTimeValue((x as AlgoOneLegMultiDiagnostics).FastBid[i], (x as AlgoOneLegMultiDiagnostics).CurrentTime)).ToArray());
                    SourcePriceChart.AddDoubleTimeValuesTo(FastAskFlows[i], data.Select(x => new DoubleTimeValue((x as AlgoOneLegMultiDiagnostics).FastAsk[i], (x as AlgoOneLegMultiDiagnostics).CurrentTime)).ToArray());
                    PriceChart.AddDoubleTimeValuesTo(ShiftedFastBidFlows[i], data.Select(x => new DoubleTimeValue((x as AlgoOneLegMultiDiagnostics).ShiftedFastBid[i], (x as AlgoOneLegMultiDiagnostics).CurrentTime)).ToArray());
                    PriceChart.AddDoubleTimeValuesTo(ShiftedFastAskFlows[i], data.Select(x => new DoubleTimeValue((x as AlgoOneLegMultiDiagnostics).ShiftedFastAsk[i], (x as AlgoOneLegMultiDiagnostics).CurrentTime)).ToArray());
                    GapChart.AddDoubleTimeValuesTo(GapBuyFlows[i], data.Select(x => new DoubleTimeValue((x as AlgoOneLegMultiDiagnostics).GapBuyPt[i], (x as AlgoOneLegMultiDiagnostics).CurrentTime)).ToArray());
                    GapChart.AddDoubleTimeValuesTo(GapSellFlows[i], data.Select(x => new DoubleTimeValue((x as AlgoOneLegMultiDiagnostics).GapSellPt[i], (x as AlgoOneLegMultiDiagnostics).CurrentTime)).ToArray());
                }
                ProfitChart.AddDoubleTimeValuesTo(ProfitFlow, data.Select(x => new DoubleTimeValue((x as AlgoOneLegMultiDiagnostics).SessionProfitPt, (x as AlgoOneLegMultiDiagnostics).CurrentTime)).ToArray());
                ProfitChart.AddDoubleTimeValuesTo(EquityFlow, data.Select(x => new DoubleTimeValue((x as AlgoOneLegMultiDiagnostics).SessionProfitPt+(x as AlgoOneLegMultiDiagnostics).OrderProfitPt, (x as AlgoOneLegMultiDiagnostics).CurrentTime)).ToArray());
            }
            if (orders!=null && orders.Length>0)
            {
                Position[] positions = new Position[orders.Length];
                for (int i = 0; i < orders.Length; i++)
                {
                    positions[i] = new Position()
                    {
                        EntryDate = orders[i].OpenTime,
                        EntryPrice = orders[i].OpenPrice,
                        ExitDate = orders[i].CloseTime,
                        ExitPrice = orders[i].ClosePrice,
                        PositionType = orders[i].Side == OrderSide.Buy ? PositionType.Long : PositionType.Short,
                        Profit = orders[i].ProfitPt,
                        Volume = orders[i].Volume
                    };
                }
                PriceChart.AddPositionsTo(PositionsFlow, positions);
            }
            if (tradeSignals!=null && tradeSignals.Length>0)
            {
                Circle[] circles = new Circle[tradeSignals.Length];
                for (int i=0;i<tradeSignals.Length;i++)
                {
                    circles[i] = new Circle()
                    {
                        Date = tradeSignals[i].Time,
                        Price = tradeSignals[i].Price,
                        Fill = tradeSignals[i].Side == OrderSide.Buy ? Colors.Green : Colors.Red
                    };
                }
                PriceChart.AddCirclesTo(SignalsFlow, circles);
            }
        }
    }
    public enum DiagnosticsType
    {
        Tick,
        CommandCompleted,
        Heartbeat
    }
    public class AlgoOneLegMultiDiagnostics : ITradeVisualizerDiagnosticsData
    {
        public double SlowBid;
        public double[] FastBid;
        public double SlowAsk;
        public double[] FastAsk;
        public double[] ShiftedFastBid;
        public double[] ShiftedFastAsk;
        public int SlowSpreadPt;
        public int[] FastSpreadPt;
        public DateTime SlowTime;
        public DateTime[] FastTime;
        public int[] GapBuyPt;
        public int[] GapSellPt;
        public double OrderVolume;
        public int OrderDurationMs;
        public int OrderProfitPt;
        public int SessionProfitPt;
        public DateTime CurrentTime;
        public DiagnosticsType Type;
        public int TickNo;
        private AlgoOneLegMultiDiagnostics()
        {
        }
        public AlgoOneLegMultiDiagnostics(int fastCount)
        {
            FastBid = new double[fastCount];
            FastAsk = new double[fastCount];
            ShiftedFastBid = new double[fastCount];
            ShiftedFastAsk = new double[fastCount];
            FastSpreadPt = new int[fastCount];
            FastTime = new DateTime[fastCount];
            GapBuyPt = new int[fastCount];
            GapSellPt = new int[fastCount];
        }
        public AlgoOneLegMultiDiagnostics Clone()
        {
            return new AlgoOneLegMultiDiagnostics()
            {
                CurrentTime = CurrentTime,
                FastAsk = FastAsk.Clone() as double[],
                FastBid = FastBid.Clone() as double[],
                FastSpreadPt = FastSpreadPt.Clone() as int[],
                FastTime = FastTime.Clone() as DateTime[],
                GapBuyPt = GapBuyPt.Clone() as int[],
                GapSellPt = GapSellPt.Clone() as int[],
                OrderVolume = OrderVolume,
                OrderDurationMs = OrderDurationMs,
                OrderProfitPt = OrderProfitPt,
                SessionProfitPt = SessionProfitPt,
                ShiftedFastAsk = ShiftedFastAsk.Clone() as double[],
                ShiftedFastBid = ShiftedFastBid.Clone() as double[],
                SlowAsk = SlowAsk,
                SlowBid = SlowBid,
                SlowSpreadPt = SlowSpreadPt,
                SlowTime = SlowTime,
                Type=Type,
                TickNo=TickNo
            };
        }

        public static void ConfigureDebug(AlgoOneLegMultiDiagnosticsSetters setters, TradeVisualizer visual)
        {
            visual.AddDebugColumn("Current Time", setters, nameof(setters.CurrentTime), 0);
            visual.AddDebugColumn("Type", setters, nameof(setters.Type), 0);
            visual.AddDebugColumn("TickNo", setters, nameof(setters.TickNo), 0);
            visual.AddDebugColumn("Slow Bid", setters, nameof(setters.SlowBid), 0);
            visual.AddDebugColumn("Slow Ask", setters, nameof(setters.SlowAsk), 0);
            visual.AddDebugColumn("Slow Spread(pt)", setters, nameof(setters.SlowSpreadPt), 0);
            visual.AddDebugColumn("Slow Time", setters, nameof(setters.SlowTime), 0);
            for (int i=0;i<setters.FastAsk.Length;i++)
            {
                visual.AddDebugColumn($"Fast{i + 1} Bid", setters, nameof(setters.FastBid), i);
                visual.AddDebugColumn($"Fast{i + 1} Ask", setters, nameof(setters.FastAsk), i);
                visual.AddDebugColumn($"ShiftedFast{i + 1} Bid", setters, nameof(setters.ShiftedFastBid), i);
                visual.AddDebugColumn($"ShiftedFast{i + 1} Ask", setters, nameof(setters.ShiftedFastAsk), i);
                visual.AddDebugColumn($"Fast{i + 1} Spread(pt)", setters, nameof(setters.FastSpreadPt), i);
                visual.AddDebugColumn($"Fast{i + 1} Time", setters, nameof(setters.FastTime), i);
                visual.AddDebugColumn($"GapBuy{i + 1}(pt)", setters, nameof(setters.GapBuyPt), i);
                visual.AddDebugColumn($"GapSell{i + 1}(pt)", setters, nameof(setters.GapSellPt), i);
            }
            visual.AddDebugColumn("Order Volume", setters, nameof(setters.OrderVolume), 0);
            visual.AddDebugColumn("Order Duration(ms)", setters, nameof(setters.OrderDurationMs), 0);
            visual.AddDebugColumn("Order Profit(pt)", setters, nameof(setters.OrderProfitPt), 0);
            visual.AddDebugColumn("Session Profit(pt)", setters, nameof(setters.SessionProfitPt), 0);
        }

        public static AlgoOneLegMultiChartSetters ConfigureCharts(int digits, ObservableCollection<ProviderModel> fastProviders, ObservableCollection<ProviderModel> slowProviders, TradeVisualizer visual, bool backtest)
        {
            AlgoOneLegMultiChartSetters res = new AlgoOneLegMultiChartSetters()
            {
                SourcePriceChart = visual.CreateChartPane(backtest, "Source Fast Price", digits, 0.25),
                PriceChart = visual.CreateChartPane(backtest, "Price", digits, 1.0),
                GapChart = visual.CreateChartPane(backtest, "Gap", 0, 0.5),
                ProfitChart = visual.CreateChartPane(backtest, "Profit(pt)", 0, 0.25)
            };
            res.SlowBidFlow = res.PriceChart.AddDoubleTimeFlow("SlowBid", TimeFrame.Tick, slowProviders[0].BidColor, slowProviders[0].BidWidth, false);
            res.SlowAskFlow = res.PriceChart.AddDoubleTimeFlow("SlowAsk", TimeFrame.Tick, slowProviders[0].AskColor, slowProviders[0].AskWidth, false);
            res.FastBidFlows = new int[fastProviders.Count];
            res.FastAskFlows = new int[fastProviders.Count];
            res.ShiftedFastBidFlows = new int[fastProviders.Count];
            res.ShiftedFastAskFlows = new int[fastProviders.Count];
            res.GapBuyFlows = new int[fastProviders.Count];
            res.GapSellFlows = new int[fastProviders.Count];
            for (int i=0;i<fastProviders.Count;i++)
            {
                res.FastBidFlows[i] = res.SourcePriceChart.AddDoubleTimeFlow($"FastBid{i+1}", TimeFrame.Tick, fastProviders[i].BidColor, fastProviders[i].BidWidth, false);
                res.FastAskFlows[i] = res.SourcePriceChart.AddDoubleTimeFlow($"FastAsk{i+1}", TimeFrame.Tick, fastProviders[i].AskColor, fastProviders[i].AskWidth, false);
                res.ShiftedFastBidFlows[i] = res.PriceChart.AddDoubleTimeFlow($"ShiftedFastBid{i + 1}", TimeFrame.Tick, fastProviders[i].BidColor, fastProviders[i].BidWidth, false);
                res.ShiftedFastAskFlows[i] = res.PriceChart.AddDoubleTimeFlow($"ShiftedFastAsk{i + 1}", TimeFrame.Tick, fastProviders[i].AskColor, fastProviders[i].AskWidth, false);
                res.GapBuyFlows[i] = res.GapChart.AddDoubleTimeFlow($"GapBuy{i + 1}", TimeFrame.Tick, fastProviders[i].AskColor, fastProviders[i].AskWidth, false);
                res.GapSellFlows[i] = res.GapChart.AddDoubleTimeFlow($"GapSell{i + 1}", TimeFrame.Tick, fastProviders[i].BidColor, fastProviders[i].BidWidth, false);
            }
            res.ProfitFlow = res.ProfitChart.AddDoubleTimeFlow("Profit(pt)", TimeFrame.Tick, Colors.Blue, 2, false);
            res.EquityFlow = res.ProfitChart.AddDoubleTimeFlow("Equity(pt)", TimeFrame.Tick, Colors.Green, 1, false);
            res.PositionsFlow = res.PriceChart.AddPositionFlow("Orders", res.SlowBidFlow);
            res.SignalsFlow = res.PriceChart.AddCircleFlow("Signal", res.SlowBidFlow);
            return res;
        }

        public static AlgoOneLegMultiDiagnosticsSetters Configure(int digits, int fastProvidersCount, TradeVisualizer visual)
        {
            string priceFormat = $"F{digits}";
            AlgoOneLegMultiDiagnosticsSetters res = new AlgoOneLegMultiDiagnosticsSetters();
            visual.InitializeDiagnostics(14, 2 + fastProvidersCount);
            visual.AddDiagnosticsString(0, 1).SetValue("Slow");
            for (int i = 0; i < fastProvidersCount; i++)
            {
                visual.AddDiagnosticsString(0, 2 + i).SetValue($"Fast{i + 1}");
            }
            visual.AddDiagnosticsString(1, 0).SetValue("Bid");
            visual.AddDiagnosticsString(2, 0).SetValue("Ask");
            visual.AddDiagnosticsString(3, 0).SetValue("Shifted Bid");
            visual.AddDiagnosticsString(4, 0).SetValue("Shifted Ask");
            visual.AddDiagnosticsString(5, 0).SetValue("Spread(pt)");
            visual.AddDiagnosticsString(6, 0).SetValue("Time");
            visual.AddDiagnosticsString(7, 0).SetValue("GapBuy(pt)");
            visual.AddDiagnosticsString(8, 0).SetValue("GapSell(pt)");
            visual.AddDiagnosticsString(10, 0).SetValue("Order Volume");
            visual.AddDiagnosticsString(11, 0).SetValue("Order Duration(ms)");
            visual.AddDiagnosticsString(12, 0).SetValue("Order Profit(pt)");
            visual.AddDiagnosticsString(13, 0).SetValue("Session Profit(pt)");

            res.CurrentTime = visual.AddDiagnosticsDateTime(0, 0, "HH:mm:ss.fff");
            res.SlowBid = visual.AddDiagnosticsDouble(1, 1, priceFormat);
            res.SlowAsk = visual.AddDiagnosticsDouble(2, 1, priceFormat);
            res.SlowSpreadPt = visual.AddDiagnosticsInt(5, 1);
            res.SlowTime = visual.AddDiagnosticsDateTime(6, 1, "HH:mm:ss.fff");
            res.OrderVolume = visual.AddDiagnosticsDouble(10, 1, "F2");
            res.OrderDurationMs = visual.AddDiagnosticsInt(11, 1);
            res.OrderProfitPt = visual.AddDiagnosticsInt(12, 1);
            res.SessionProfitPt = visual.AddDiagnosticsInt(13, 1);
            res.FastBid = new DiagnosticsDoubleValue[fastProvidersCount];
            res.FastAsk = new DiagnosticsDoubleValue[fastProvidersCount];
            res.ShiftedFastBid = new DiagnosticsDoubleValue[fastProvidersCount];
            res.ShiftedFastAsk = new DiagnosticsDoubleValue[fastProvidersCount];
            res.FastSpreadPt = new DiagnosticsIntValue[fastProvidersCount];
            res.FastTime = new DiagnosticsDateTimeValue[fastProvidersCount];
            res.GapBuyPt = new DiagnosticsIntValue[fastProvidersCount];
            res.GapSellPt = new DiagnosticsIntValue[fastProvidersCount];
            for (int i=0;i<res.FastBid.Length;i++)
            {
                res.FastBid[i] = visual.AddDiagnosticsDouble(1, 2 + i, priceFormat);
                res.FastAsk[i] = visual.AddDiagnosticsDouble(2, 2 + i, priceFormat);
                res.ShiftedFastBid[i] = visual.AddDiagnosticsDouble(3, 2 + i, priceFormat);
                res.ShiftedFastAsk[i] = visual.AddDiagnosticsDouble(4, 2 + i, priceFormat);
                res.FastSpreadPt[i] = visual.AddDiagnosticsInt(5, 2 + i);
                res.FastTime[i] = visual.AddDiagnosticsDateTime(6, 2 + i, "HH:mm:ss.fff");
                res.GapBuyPt[i] = visual.AddDiagnosticsInt(7, 2 + i);
                res.GapSellPt[i] = visual.AddDiagnosticsInt(8, 2 + i);
            }
            res.Type = new DiagnosticsStringValue(0, 0);
            res.TickNo = new DiagnosticsIntValue(0, 0, null);
            return res;
        }

        public DateTime GetCurrentTime()
        {
            return CurrentTime;
        }
    }
}

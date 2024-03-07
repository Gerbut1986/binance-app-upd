namespace Models.Algo
{
    using System;
    using Helpers;
    using System.Linq;
    using Helpers.Extensions;
    using MultiTerminal.Connections;
    using System.Collections.Generic;
    using MultiTerminal.Connections.API.Spot;
    using System.Collections.Concurrent;
    using BinanceOptionsApp;
    using System.Threading.Tasks;

    public class PlotSpot
    {
        public decimal MarketBuyAsk { get; set; } = 0;
        public decimal LimitBuyAsk { get; set; } = 0;
        //public decimal BumBuyAsk { get; set; } = 0;
        public decimal MarketSellBid { get; set; } = 0;
        public decimal LimitSellBid { get; set; } = 0;
        //public decimal BumSellBid { get; set; } = 0;

        public decimal BoomLimit  { get; set; } = 0;
        public decimal BoomBuyLimit { get; set; } = 0;
        public decimal BoomSellLimit { get; set; } = 0;
        public decimal BoomMarket { get; set; } = 0;

        public decimal MarketLGBuyAsk { get; set; } = 0;
        public decimal LimitLGBuyAsk { get; set; } = 0;
        public decimal MarketLGSellBid { get; set; } = 0;
        public decimal LimitLGSellBid { get; set; } = 0;
        public decimal TotalSumBuy { get; set; } = 0;
        public decimal TotalSumSell { get; set; } = 0;
        public decimal Price { get; set; } = 0;
    }

    public class ZigZagSpot
    {
        #region Fields:
        //private decimal Leg = 60; //100*100*10
        private readonly decimal ThresholdVol = 1;
        //private readonly decimal SenDist = 100;

        private decimal MaxASK_Vol = 0;
        private decimal MinBID_Vol = 0;
        private decimal MaxASK = 0;
        private decimal MinBID = 0;
        private ulong ID_Q_MaxAsk = 0;
        private ulong ID_Q_MinBid = 0;
        private ulong TimeMaxASK;
        private ulong TimeMinBID;
        private uint WaveCounter_s = 0;
        private bool WaveDN = false;
        private bool WaveUP = false;
        //--------------------------------------------+
        private decimal priceStep = 0.01m;
        private decimal StartPrice;
        private decimal FinishPrice;
        private ulong StartTime { get; set; }
        private ulong FinishTime;
        ulong LastID_Q_MinBid = 0;
        ulong LastID_Q_MaxAsk = 0;
        ulong Pre_id = 0;
        int CL = 0, Kin = 0;
        bool Triger = false;
        public decimal SumAsk = 0, SumBid = 0;
        public decimal SupportPrice = 0, ResistancePrice = 0;
        public decimal SupportVol = 0, ResistanceVol = 0;
        public int MainRankSell = 0, MainRankBuy = 0;
        private decimal RP = 0, SP = 0, RV = 0, SV = 0;
        private ConcurrentDictionary<decimal, MarketDepthUpdateSpot> MaxAsklistMD = default, MinBidlistMD = default, MDZigDN = default, MDZigUP = default;
        private List<List<string>> MaxAsklistMD_P = default, MinBidlistMD_P = default, MDZigDN_P = default, MDZigUP_P = default;

        private List<TimeAndSale_BidAsk> timeAndSale;

        private DateTime KinTime;
        //System.DateTime TimeNow;
        private DateTime currentTime;// = System.DateTime.Now;
        #endregion

        public ZigZagSpot(List<TimeAndSale_BidAsk> timeAndSale)
        {
            this.timeAndSale = timeAndSale;
        }

        #region Arrays: ZigZagArr, CurrLeg, Kinchik
        public PlotSpot[] ZigZagArr = new PlotSpot[300000]; //100*100*10*3 (for Leg 3000$; step=0.01)
        public PlotSpot[] CurrLeg = new PlotSpot[300000];
        public PlotSpot[] Kinchik = new PlotSpot[100000];
        #endregion

        #region FillingSmallZZ func:
        public void FillingSmallZZ(ulong ID_Q_Start, ulong ID_Q_Finish, decimal StartPr, string tip)//, List<TimeAndSale_BidAsk> tasList
        {
            if ((tip == "UP" && ID_Q_Finish > ID_Q_Start && ID_Q_Finish < (ulong)timeAndSale.Count && ID_Q_Start > 0) ||
                (tip == "DN" && ID_Q_Finish > ID_Q_Start && ID_Q_Finish < (ulong)timeAndSale.Count && ID_Q_Start > 0))
            {
                var subList = (timeAndSale.ToArray().SubArray(ID_Q_Start - 1, ID_Q_Finish - 1));
                /*
                try
                {
                  //var length = (int)(ID_Q_Finish - ID_Q_Start);
                  //  subList = tasList.GetRange((int)ID_Q_Start - 1, length);
                }
                catch (Exception ex) { new _().WriteToTxtFile($"{ex.Message}\n{ex.StackTrace}", "FillingSmallZZ{tasLst.GetRange()}[ERROR]", true, true); }
                */

                int i = 0;
                if (subList.Length != 0)
                {
                    foreach (var item in subList)
                    {
                        try
                        {
                            decimal x;
                            var LastPrice = item.Price;

                            if (tip == "UP")
                            {
                                x = (LastPrice - StartPr) / priceStep;
                                i = (int)x;
                            }
                            else //if (tip == "DN")
                            {
                                x = (StartPr - LastPrice) / priceStep;
                                i = (int)x;
                            }

                            ZigZagArr[i].Price = item.Price;
                            ZigZagArr[i].LimitBuyAsk += item.AskBuyLimit;
                            ZigZagArr[i].MarketBuyAsk += item.AskBuyMarket;
                            ZigZagArr[i].LimitSellBid += item.BidSellLimit;
                            ZigZagArr[i].MarketSellBid += item.BidSellMarket;
                            ZigZagArr[i].BoomBuyLimit += item.BoomBuyLimit;
                            ZigZagArr[i].BoomSellLimit +=item.BoomSellLimit;
                            ZigZagArr[i].BoomLimit += item.BoomLimit;
                            ZigZagArr[i].BoomMarket += item.BoomMarket;


                        }
                        catch (Exception ex)
                        {
                            new _().WriteToTxtFile($"{ex.Message}\n{ex.StackTrace}", $"{nameof(FillingSmallZZ)}[ERROR]", true, true);
                        }
                    }

                    string logMsg = "";
                    if (tip == "UP") logMsg = $"** UP ****************************************************";
                    if (tip == "DN") logMsg = $"** DN ****************************************************";
                    new _().WriteToTxtFile(logMsg, "MDLevel2Spot");
                    new _().WriteToTxtFile($"WaveCounter: {WaveCounter_s}".ToString(), "MDLevel2Spot");
                    var st = long.Parse(StartTime.ToString()).GetFullTime();
                    var ft = long.Parse(FinishTime.ToString()).GetFullTime();
                    logMsg = $"StartTime = {st.Hour}:{st.Minute}:{st.Second}.{st.Millisecond} FinishTime = {ft.Hour}:{ft.Minute}:{ft.Second}.{ft.Millisecond}";
                    new _().WriteToTxtFile(logMsg, "MDLevel2Spot");
                    logMsg = $"StartPrice = {StartPrice} FinishPrice = {FinishPrice}";
                    new _().WriteToTxtFile(logMsg, "MDLevel2Spot");

                    var startRange = FinishPrice + TradeZigZag.SenseDist;
                    var finishRange = FinishPrice - TradeZigZag.SenseDist;

                    //!!! Stakan print full data
                    /*
                    if (tip == "UP")
                    {
                        var filteredItems = MDZigUP
                         .Where(pair => pair.Key <= startRange && pair.Key >= finishRange)
                         .ToList();

                        var type1Items = filteredItems
                            .Where(item => item.Value.Type == 1)
                            .OrderByDescending(item => item.Key)
                            .ToList();

                        foreach (var item in type1Items)
                        {
                            new _().WriteToTxtFile($"Type: {item.Value.Type} | Price: {item.Key}, " +
                                $"Volume: {item.Value.Volume}", "MDLevel2Spot");
                        }
                        new _().WriteToTxtFile($" - Ask UP Bid - @", "MDLevel2Spot");


                        var type2Items = filteredItems
                              .Where(item => item.Value.Type == 2)
                              .OrderByDescending(item => item.Key)
                              .ToList();

                        foreach (var item in type2Items)
                        {
                            new _().WriteToTxtFile($"Type: {item.Value.Type} | Price: {item.Key}, " +
                                $"Volume: {item.Value.Volume}", "MDLevel2Spot");
                        }
                    }
                    if (tip == "DN")
                    {
                        var filteredItems = MDZigDN
                     .Where(pair => pair.Key <= startRange && pair.Key >= finishRange)
                         .ToList();

                        var type1Items = filteredItems
                            .Where(item => item.Value.Type == 1)
                            .OrderByDescending(item => item.Key)
                            .ToList();

                        foreach (var item in type1Items)
                        {
                            new _().WriteToTxtFile($"Type: {item.Value.Type} | Price: {item.Key}, " +
                                $"Volume: {item.Value.Volume}", "MDLevel2Spot");
                        }

                        new _().WriteToTxtFile($" - Ask DN Bid - @", "MDLevel2Spot");

                        var type2Items = filteredItems
                            .Where(item => item.Value.Type == 2)
                            .OrderByDescending(item => item.Key)
                            .ToList();

                        foreach (var item in type2Items)
                        {
                            new _().WriteToTxtFile($"Type: {item.Value.Type} | Price: {item.Key}, " +
                                $"Volume: {item.Value.Volume}", "MDLevel2Spot");
                        }
                    }
                    */
                    //!!!
                    //!!! Stakan print agregate data
                  //  MakeAggMD("ZigZag", tip, startRange, finishRange);
                    //!!! Stakan print finish


                    if (tip == "UP") logMsg = $"** UP ****************************************************";
                    if (tip == "DN") logMsg = $"** DN ****************************************************";
                    new _().WriteToTxtFile(logMsg, "SmallZZ");
                    new _().WriteToTxtFile($"WaveCounter: {WaveCounter_s}".ToString(), "SmallZZ");
                    st = long.Parse(StartTime.ToString()).GetFullTime();
                    ft = long.Parse(FinishTime.ToString()).GetFullTime();
                    logMsg = $"StartTime = {st.Hour}:{st.Minute}:{st.Second}.{st.Millisecond} FinishTime = {ft.Hour}:{ft.Minute}:{ft.Second}.{ft.Millisecond}";
                    new _().WriteToTxtFile(logMsg, "SmallZZ");
                    logMsg = $"StartPrice = {StartPrice} FinishPrice = {FinishPrice}";
                    new _().WriteToTxtFile(logMsg, "SmallZZ");
                    if (tip == "UP") logMsg = $"MaxAskVol = {MaxASK_Vol} ";
                    if (tip == "DN") logMsg = $"MinBidVol = {MinBID_Vol} ";
                    new _().WriteToTxtFile(logMsg, "SmallZZ");
                    if (tip == "UP") logMsg = $"MainRankSell = {MainRankSell} ";
                    if (tip == "DN") logMsg = $"MainRankBuy = {MainRankBuy} ";
                    new _().WriteToTxtFile(logMsg, "SmallZZ");
                    var a = timeAndSale[(int)ID_Q_Start - 1].Ask;
                    var b = timeAndSale[(int)ID_Q_Start - 1].Bid;
                    var a2 = timeAndSale[(int)ID_Q_Finish - 1].Ask;
                    var b2 = timeAndSale[(int)ID_Q_Finish - 1].Bid;

                    logMsg = $"ID_Q_Start: {ID_Q_Start} | ID_Q_Finish: {ID_Q_Finish}, AskS = {a} BidS = {b} AskF = {a2} BidF = {b2}";

                    new _().WriteToTxtFile(logMsg, "SmallZZ");

                    PlotSpot[] fillElements = ZigZagArr.Where(p => p.Price > 0).ToArray();

                    foreach (var elem in fillElements)
                    {
                        var logMsgBuilder = new System.Text.StringBuilder();
                        logMsgBuilder.AppendLine($"Price= {elem.Price}");
                        logMsgBuilder.AppendLine($"LimitBuyAsk= {elem.LimitBuyAsk}");
                        logMsgBuilder.AppendLine($"MarketBuyAsk= {elem.MarketBuyAsk}");
                        logMsgBuilder.AppendLine($"BumBuyLimit= {elem.BoomBuyLimit}");
                        logMsgBuilder.AppendLine($"LimitSellBid= {elem.LimitSellBid}");
                        logMsgBuilder.AppendLine($"MarketSellBid= {elem.MarketSellBid}");
                        logMsgBuilder.AppendLine($"BumSellLimit= {elem.BoomSellLimit}");
                        logMsgBuilder.AppendLine($"BumLimit= {elem.BoomLimit}");
                        logMsgBuilder.AppendLine($"BumMarket= {elem.BoomMarket}");
                        logMsg = logMsgBuilder.ToString();
                        new _().WriteToTxtFile(logMsg, "SmallZZ");
                    }
                }
            }
            else
            {
                new _().WriteToTxtFile("ID_Q_MaxAsk > ID_Q_MinBid || ID_Q_MinBid > found.Length",
                     "FillingSmallZZ[errors]", true, true);
                string logMsgEr = $"Pomilka ID_Q_Start= {ID_Q_Start}   ID_Q_Finish= {ID_Q_Finish}  Bin= {timeAndSale.Count}";
                new _().WriteToTxtFile(logMsgEr, "Pomilka");
            }
        }
        #endregion

        #region BuildZigZag func:
        public async void BuildZigZag(decimal ask, decimal bid, ulong Time, ulong ID_quark)
        {
            if (MaxASK == 0 || ask > MaxASK)  //  
            {
                MaxASK = ask;
                TimeMaxASK = Time;
                ID_Q_MaxAsk = ID_quark;
                MaxASK_Vol = SumAsk;
                MaxAsklistMD = BinanceCryptoClient.marketDepthList;
                MaxAsklistMD_P = BinanceCryptoClient.AsksP;
                var bookTicker = BinanceCryptoClient.BookTickerSpot;
                //  MainRankSell = BinanceCryptoClient.FunDepthProba2(1);
                //  RP = ResistancePrice;
                //  RV = ResistanceVol;

            }  // new MAX

            if (MinBID == 0 || bid < MinBID)  //  
            {
                MinBID = bid;
                TimeMinBID = Time;
                ID_Q_MinBid = ID_quark;
                MinBID_Vol = SumBid;
                SP = SupportPrice;
                SV = SupportVol;
                MinBidlistMD = BinanceCryptoClient.marketDepthList;
                MinBidlistMD_P = BinanceCryptoClient.BidsP;
                //MainRankBuy = BinanceCryptoClient.FunDepthProba2(2);
                //SP = SupportPrice;
                //SV = SupportVol;
            } // new MIN

            if (TimeMaxASK > TimeMinBID) // UP zig zag
            {
                if (MaxASK - MinBID >= TradeZigZag.Leg && MaxASK - bid >= TradeZigZag.Leg) // UP zig zag
                {
                    WaveUP = true; WaveDN = false;
                    WaveCounter_s++;
                    StartPrice = MinBID; StartTime = TimeMinBID;
                    FinishPrice = MaxASK; FinishTime = TimeMaxASK;

                    MDZigUP = MaxAsklistMD;
                    MDZigUP_P = MaxAsklistMD_P;
                    InitZigZagArr();
                    FillingSmallZZ(ID_Q_MinBid, ID_Q_MaxAsk, StartPrice, "UP");

                    MinBID = bid;
                    TimeMinBID = Time;
                    ID_Q_MinBid = ID_quark;
                    MinBID_Vol = SumBid;
                    //MainRankBuy = BinanceCryptoClient.FunDepthProba2(2);
                    //SP = SupportPrice;
                   // SV = SupportVol;

                  //  InitZigZagArr();
                }
            }  // UP ZigZag

            if (WaveUP == true && TimeMaxASK < Time) //for DN cur leg
            {
                if (LastID_Q_MinBid == 0) LastID_Q_MinBid = ID_Q_MinBid;

                if (ID_quark > Pre_id)
                {
                    if (ID_Q_MinBid > LastID_Q_MinBid)
                    {
                        InitCurrLeg();//nulling 

                        for (ulong i = ID_Q_MaxAsk; i <= ID_Q_MinBid; i++)
                        {
                            CurrentLeg(ID_Q_MaxAsk, i, MaxASK, "dn");
                        }

                        //CL = GazerCurrLeg3("dn");
                        InitKinchik();//nulling
                        Knut(ID_Q_MinBid, ID_quark, MinBID, "up");
                        //res=CL;
                        Triger = true;
                        LastID_Q_MinBid = ID_Q_MinBid;
                    }
                    else
                    {
                        InitKinchik();//nulling 
                        Knut(ID_Q_MinBid, ID_quark, MinBID, "up");
                        Kin = GazerKinchik("up");
                        try
                        {
                            if (Kin == 2 && Triger)
                            {
                                long T = (long)Time;
                                KinTime = System.DateTimeOffset.FromUnixTimeMilliseconds(T).DateTime;//(long)
                                string formattedTime = KinTime.ToString("HH:mm:ss.fff");
                                currentTime = System.DateTime.Now;
                                string formattedTimeN = currentTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                string KinMsg = $"Time= {formattedTime} Kin= {Kin}  Ask= {ask}  Bid= {bid} curr time: {formattedTimeN}";
                                //string KinMsg = $"Time= {Time}  Kin= {Kin}  Ask= {ask},  Bid= {bid}";
                                new _().WriteToTxtFile(KinMsg, "Kinchik");
                                Triger = false;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            new _().WriteToTxtFile($"{ex.Message}\n{ex.StackTrace}", $"{nameof(BuildZigZag)}[ERROR]", true, true);
                        }
                    }
                    Pre_id = ID_quark;
                }
            } //for DN cur leg

            if (TimeMaxASK < TimeMinBID) // DN zig zag
            {
                if (MaxASK - MinBID >= TradeZigZag.Leg && ask - MinBID >= TradeZigZag.Leg) // DN zig zag
                {
                    WaveDN = true; WaveUP = false;
                    WaveCounter_s++;
                    StartPrice = MaxASK; StartTime = TimeMaxASK;
                    FinishPrice = MinBID; FinishTime = TimeMinBID;

                    MDZigDN = MinBidlistMD;
                    MDZigDN_P = MinBidlistMD_P;
                    InitZigZagArr();
                    FillingSmallZZ(ID_Q_MaxAsk, ID_Q_MinBid, StartPrice, "DN");

                    MaxASK = ask;
                    TimeMaxASK = Time;
                    ID_Q_MaxAsk = ID_quark;
                    MaxASK_Vol = SumAsk;
                    //MainRankSell = BinanceCryptoClient.FunDepthProba2(1);
                    //RP = ResistancePrice;
                    //RV = ResistanceVol;

                  //  InitZigZagArr();
                }
            } // DN ZigZag

            if (WaveDN == true && TimeMinBID < Time)//for UP cur leg
            {
                if (LastID_Q_MaxAsk == 0) { LastID_Q_MaxAsk = ID_Q_MaxAsk; }

                if (ID_quark > Pre_id)
                {
                    if (ID_Q_MaxAsk > LastID_Q_MaxAsk)
                    {
                        InitCurrLeg();//nulling 

                        for (ulong i = ID_Q_MinBid; i <= ID_Q_MaxAsk; i++)
                        {
                            CurrentLeg(ID_Q_MinBid, i, MinBID, "up");
                        }

                        //CL = GazerCurrLeg("up");//
                        InitKinchik();//nulling 
                        Knut(ID_Q_MaxAsk, ID_quark, MaxASK, "dn");
                        Triger = true;
                        LastID_Q_MaxAsk = ID_Q_MaxAsk;
                    }
                    else
                    {
                        InitKinchik();//nulling 
                        Knut(ID_Q_MaxAsk, ID_quark, MaxASK, "dn");
                        Kin = GazerKinchik("dn");
                        try
                        {
                            if (Kin == 1 && Triger)
                            {
                                long T = (long)Time;
                                KinTime = System.DateTimeOffset.FromUnixTimeMilliseconds(T).DateTime;//(long)
                                currentTime = System.DateTime.Now;
                                string formattedTimeN = currentTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                string formattedTime = KinTime.ToString("HH:mm:ss.fff");
                                string KinMsg = $"Time= {formattedTime} Kin= {Kin}  Ask= {ask}  Bid= {bid} curr time: {formattedTimeN}";

                                //string KinMsg = $"Time= {formattedTime} Kin= {Kin}  Ask= {ask},  Bid= {bid} ";
                                new _().WriteToTxtFile(KinMsg, "Kinchik");
                                Triger = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            new _().WriteToTxtFile($"{ex.Message}\n{ex.StackTrace}", $"{nameof(BuildZigZag)}[ERROR]", true, true);
                        }
                    }
                }
            } //for UP cur leg
        }
        #endregion

        #region GazerKinchik:
        int GazerKinchik(string tip)
        {
            decimal SumBuy = 0; decimal SumSell = 0;
            int res = 0, ser = 0;
            try
            {
                for (int i = 0; i < 43; i++)
                {
                    SumBuy += Kinchik[i].LimitBuyAsk;
                    SumBuy += Kinchik[i].MarketBuyAsk;
                    SumSell += Kinchik[i].LimitSellBid;
                    SumSell += Kinchik[i].MarketSellBid;

                    if (Kinchik[i].LimitBuyAsk >= 1 || Kinchik[i].MarketBuyAsk >= 1)
                    {
                        if (tip == "up") ser = 2;
                    }

                    if (Kinchik[i].LimitSellBid >= 1 || Kinchik[i].MarketSellBid >= 1)
                    {
                        if (tip == "dn") ser = 1;
                    }
                }
            }
            catch (System.Exception ex)
            {
                new _().WriteToTxtFile($"{ex.Message}\n{ex.StackTrace}", $"{nameof(GazerKinchik)}[ERROR]", true, true);
            }
            if (ser == 2 && SumBuy > SumSell) res = 2;
            if (ser == 1 && SumBuy < SumSell) res = 1;

            return (res);
        }
        #endregion

        #region CurrentLeg:
        void CurrentLeg(ulong ID_Extremum, ulong id, decimal StartPr, string tip)
        {
            if ((tip == "up" && id >= ID_Extremum && id <= (ulong)timeAndSale.Count && ID_Extremum > 0) ||
               (tip == "dn" && id >= ID_Extremum && id <= (ulong)timeAndSale.Count && ID_Extremum > 0))
            {
                var subList1 = (timeAndSale.ToArray().SubArray(ID_Extremum - 1, id - 1));
                //var subList1 = BinanceExecution.list.Skip((int)(ID_Extremum - 1)).Take((int)(id - ID_Extremum + 1)).ToArray();
                if (subList1.Length != 0)
                {
                    int i = 0;
                    try
                    {
                        foreach (var item in subList1)
                        {
                            var Ask = item.Ask;
                            var Bid = item.Bid;
                            var LastPrice = item.Price;
                            var Vol = item.Volume;
                            bool BuyLimit = item.IsBuyLimit;
                            bool SellLimit = item.IsSellLimit;
                            decimal x;

                            if (LastPrice >= Ask)
                            {
                                if (tip == "up")
                                {
                                    x = (LastPrice - StartPr) / priceStep;
                                    i = (int)x;
                                }
                                if (tip == "dn")
                                {
                                    x = (StartPr - LastPrice) / priceStep;
                                    i = (int)x;
                                }

                                if (CurrLeg[i] == null) CurrLeg[i] = new PlotSpot();

                                if (i >= 0 && i < 300000)
                                {
                                    if (!BuyLimit && SellLimit)
                                    {
                                        CurrLeg[i].Price = LastPrice;
                                        CurrLeg[i].MarketBuyAsk += Vol;  // MarketBuyAsk
                                    }
                                    else if (BuyLimit && SellLimit)
                                    {
                                        CurrLeg[i].Price = LastPrice;
                                        CurrLeg[i].LimitBuyAsk += Vol;// LimitBuyAsk
                                    }
                                    else if (!BuyLimit && !SellLimit)
                                    {
                                        CurrLeg[i].Price = LastPrice;
                                       // CurrLeg[i].BumBuyAsk += Vol;// BumBuyAsk
                                    }
                                }
                            }
                            else if (LastPrice <= Bid)
                            {
                                if (CurrLeg[i] == null) CurrLeg[i] = new PlotSpot();

                                if (i >= 0 && i < 300000)
                                {
                                    if (!SellLimit && BuyLimit)
                                    {
                                        CurrLeg[i].Price = LastPrice;
                                        CurrLeg[i].MarketSellBid += Vol;//
                                    }
                                    else if (SellLimit && BuyLimit)
                                    {
                                        CurrLeg[i].Price = LastPrice;
                                        CurrLeg[i].LimitSellBid += Vol;

                                    }
                                    else if (!SellLimit && !BuyLimit)
                                    {
                                        CurrLeg[i].Price = LastPrice;
                                       // CurrLeg[i].BumSellBid += Vol;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        new _().WriteToTxtFile($"{ex.Message}\n{ex.StackTrace}", $"{nameof(CurrentLeg)}[ERROR]", true, true);
                    }
                }
            }
            else
            {
                string logMsgEr = $"Pomilka ID_Q_Start= {ID_Extremum}   ID_Q_Finish= {id}  Bin= {timeAndSale.Count}";
                new _().WriteToTxtFile(logMsgEr, "PomilkaCurrLeg");
            }
        }
        #endregion

        #region Knut:
        void Knut(ulong ID_Extremum, ulong id, decimal StartPr, string tip)
        {
            bool subIf = id > ID_Extremum;
            if ((tip == "up" && subIf && id <= (ulong)timeAndSale.Count && ID_Extremum > 0) ||
                (tip == "dn" && id > ID_Extremum && id <= (ulong)timeAndSale.Count && ID_Extremum > 0))
            {
                var subList2 = (timeAndSale.ToArray().SubArray(ID_Extremum - 1, id - 1));
                if (subList2.Length != 0)
                {
                    int i = 0;
                    try
                    {
                        foreach (var item in subList2)
                        {
                            var Ask = item.Ask;
                            var Bid = item.Bid;
                            var LastPrice = item.Price;
                            var Vol = item.Volume;
                            bool BuyLimit = item.IsBuyLimit;
                            bool SellLimit = item.IsSellLimit;
                            decimal x;

                            if (LastPrice >= Ask)
                            {
                                if (tip == "up")
                                {
                                    x = (LastPrice - StartPr) / priceStep;
                                    i = (int)x;
                                }
                                if (tip == "dn")
                                {
                                    x = (StartPr - LastPrice) / priceStep;
                                    i = (int)x;
                                }
                                if (i >= 0 && i < 100000)
                                {
                                    if (Kinchik[i] == null) Kinchik[i] = new PlotSpot();

                                    if (!BuyLimit && SellLimit)
                                    {
                                        Kinchik[i].Price = LastPrice;
                                        Kinchik[i].MarketBuyAsk += Vol;  // MarketBuyAsk
                                    }
                                    else if (BuyLimit && SellLimit)
                                    {
                                        Kinchik[i].Price = LastPrice;
                                        Kinchik[i].LimitBuyAsk += Vol;// LimitBuyAsk
                                    }
                                    else if (!BuyLimit && !SellLimit)
                                    {
                                        Kinchik[i].Price = LastPrice;
                                       // Kinchik[i].BumBuyAsk += Vol;// BumBuyAsk
                                    }
                                }

                            }
                            else if (LastPrice <= Bid)
                            {
                                if (Kinchik[i] == null) Kinchik[i] = new PlotSpot();

                                if (i >= 0 && i < 100000)
                                {
                                    if (!SellLimit && BuyLimit)
                                    {
                                        Kinchik[i].Price = LastPrice;
                                        Kinchik[i].MarketSellBid += Vol;//
                                    }
                                    if (SellLimit && BuyLimit)
                                    {
                                        Kinchik[i].Price = LastPrice;
                                        Kinchik[i].LimitSellBid += Vol;
                                    }
                                    if (!SellLimit && !BuyLimit)
                                    {
                                        Kinchik[i].Price = LastPrice;
                                      //  Kinchik[i].BumSellBid += Vol;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        new _().WriteToTxtFile($"{ex.Message}\n{ex.StackTrace}", $"{nameof(Knut)}[ERROR]", true, true);
                    }
                }
            }
            else
            {
                string logMsgEr = $"Pomilka ID_Q_Start= {ID_Extremum}   ID_Q_Finish= {id}  Bin= {timeAndSale.Count}";
                try
                {
                    new _().WriteToTxtFile(logMsgEr, "PomilkaKnut");
                }
                catch (Exception ex)
                {
                    var dt = DateTime.Now;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{dt.Day}/{dt.Month} {dt.ToString("HH: mm:ss.ffffff")}=>{ex.Message}");
                    Console.ResetColor();
                }
            }
        }
        #endregion

        #region Init arrays:
        public void InitZigZagArr()
        {
            ZigZagArr = new PlotSpot[300000]; // Змінюємо масив на null
            for (var i = 0; i < ZigZagArr.Length; i++)
                ZigZagArr[i] = new PlotSpot();
        }
        public void InitCurrLeg()
        {
            CurrLeg = new PlotSpot[300000]; // Змінюємо масив на null
            for (var i = 0; i < CurrLeg.Length; i++)
                CurrLeg[i] = new PlotSpot();
        }
        public void InitKinchik()
        {
            Kinchik = new PlotSpot[100000]; // Змінюємо масив на null
            for (var i = 0; i < Kinchik.Length; i++)
                Kinchik[i] = new PlotSpot();
        }
        #endregion

        #region Aggregate of Market Depth:
        public class AggregatedBookItem
        {
            public decimal Price { get; set; }
            public decimal AggregatedVolume { get; set; }
        }

        private List<AggregatedBookItem> aggregatedBookType1 = new List<AggregatedBookItem>();
        private List<AggregatedBookItem> aggregatedBookType2 = new List<AggregatedBookItem>();

        private void MakeAggMD(string Event, string type, decimal startRange, decimal finishRange)
        {
            ConcurrentDictionary<decimal, MarketDepthUpdateSpot> MDZigRRR = default;


            if (Event == "ZigZag")
            {
                string logMsg = null;
                if (type == "UP") logMsg = $"** UP ****************************************************";
                if (type == "DN") logMsg = $"** DN ****************************************************";
                new _().WriteToTxtFile(logMsg, "MDLevel2FutureAg");
                new _().WriteToTxtFile($"WaveCounter: {WaveCounter_s}".ToString(), "MDLevel2FutureAg");
                var st = long.Parse(StartTime.ToString()).GetFullTime();
                var ft = long.Parse(FinishTime.ToString()).GetFullTime();
                logMsg = $"StartTime = {st.Hour}:{st.Minute}:{st.Second}.{st.Millisecond} FinishTime = {ft.Hour}:{ft.Minute}:{ft.Second}.{ft.Millisecond}";
                new _().WriteToTxtFile(logMsg, "MDLevel2FutureAg");
                logMsg = $"StartPrice = {StartPrice} FinishPrice = {FinishPrice}";
                new _().WriteToTxtFile(logMsg, "MDLevel2FutureAg");

                if (type == "UP") MDZigRRR = MDZigUP;
                else if (type == "DN") MDZigRRR = MDZigDN;
            }
            else
            {
                if (type == "UP") MDZigRRR = MinBidlistMD;
                else MDZigRRR = MaxAsklistMD;
            }


            if (MDZigRRR != null)
            {
                // Type 1 Aggregation
                var type1Items = MDZigRRR
                    .Where(item => item.Value.Type == 1 && item.Key <= startRange && item.Key >= finishRange && item.Value.Volume > 0)
                    .OrderByDescending(item => item.Key)
                    .ToList();

                decimal interval = TradeZigZag.Cluster;
                decimal currentRangeStart = type1Items.First().Key;
                decimal BestAsk = type1Items.Last().Key;
                decimal currentRangeEnd = currentRangeStart - interval;
                decimal nextRangeStart;

                while (currentRangeEnd >= BestAsk) //finishRange
                {
                    var sumVolume = type1Items
                        .Where(item => item.Key < currentRangeStart && item.Key >= currentRangeEnd)
                        .Sum(item => item.Value.Volume);

                    if (Event == "ZigZag")
                    {
                        new _().WriteToTxtFile($"Type: 1 Price: {currentRangeEnd}, " +
                            $"Aggregated Volume: {sumVolume}", "MDLevel2FutureAg");
                    }

                    aggregatedBookType1.Add(new AggregatedBookItem
                    {
                        Price = currentRangeEnd,
                        AggregatedVolume = sumVolume
                    });

                    nextRangeStart = currentRangeEnd;
                    currentRangeStart = nextRangeStart;
                    currentRangeEnd = nextRangeStart - interval;
                    if (currentRangeStart > BestAsk && currentRangeEnd < BestAsk) currentRangeEnd = BestAsk;

                }

                // Type 2 Aggregation
                var type2Items = MDZigRRR
                    .Where(item => item.Value.Type == 2 && item.Key <= startRange && item.Key >= finishRange && item.Value.Volume > 0)
                    .OrderByDescending(item => item.Key)
                    .ToList();

                currentRangeStart = type2Items.First().Key;
                currentRangeEnd = currentRangeStart - interval;

                while (currentRangeEnd >= finishRange)
                {
                    var sumVolume = type2Items
                        .Where(item => item.Key < currentRangeStart && item.Key >= currentRangeEnd)
                        .Sum(item => item.Value.Volume);

                    if (Event == "ZigZag")
                    {
                        new _().WriteToTxtFile($"Type: 2 Price: {currentRangeEnd}, " +
                        $"Aggregated Volume: {sumVolume}", "MDLevel2FutureAg");
                    }

                    aggregatedBookType2.Add(new AggregatedBookItem
                    {
                        Price = currentRangeEnd,
                        AggregatedVolume = sumVolume
                    });

                    nextRangeStart = currentRangeEnd;
                    currentRangeStart = nextRangeStart;
                    currentRangeEnd = nextRangeStart - interval;
                }

            }
        }

        private decimal Look4MaxVPrice(int tip)
        {
            decimal res = 0.0m;
            if (tip == 1)
            {
                decimal maxAggregatedVolumePrice = aggregatedBookType1
               .OrderByDescending(item => item.AggregatedVolume)
               .Select(item => item.Price)
               .FirstOrDefault();
                return maxAggregatedVolumePrice;
            }
            else
            {
                decimal maxAggregatedVolumePrice2 = aggregatedBookType2
                .OrderByDescending(item => item.AggregatedVolume)
                .Select(item => item.Price)
                .FirstOrDefault();
                return maxAggregatedVolumePrice2;
            }
            return res;

        }

        #endregion

        #region PROBA FillingSmallZZ func:
        public void FillingSmallZZ_2A(ulong ID_Q_Start, ulong ID_Q_Finish, decimal StartPr, string tip)
        {
            if ((tip == "UP" && ID_Q_Finish > ID_Q_Start && ID_Q_Finish < (ulong)timeAndSale.Count && ID_Q_Start > 0) ||
                (tip == "DN" && ID_Q_Finish > ID_Q_Start && ID_Q_Finish < (ulong)timeAndSale.Count && ID_Q_Start > 0))
            {
                //var subList = (timeAndSale.ToArray().SubArray(ID_Q_Start - 1, ID_Q_Finish - 1));
                List<TimeAndSale_BidAsk> subList = default;
                try
                {
                    var length = (int)(ID_Q_Finish - ID_Q_Start);
                    subList = timeAndSale.GetRange((int)ID_Q_Start - 1, length);

                  //  subList.Where((model, index) => index >= startIndex && index <= endIndex) .ToList();
           
                }
                catch (Exception ex) { new _().WriteToTxtFile($"{ex.Message}\n{ex.StackTrace}", "FillingSmallZZ{tasLst.GetRange()}[ERROR]", true, true); }


                int i = 0;
                if (subList.Count != 0)
                {
                    foreach (var item in subList)
                    {
                        try
                        {
                            decimal x;
                            var ask = subList[i].Ask;
                            var bid = subList[i].Bid;
                            var LastPrice = item.Price;
                            var Vol = item.Volume;
                            bool BuyLimit = item.IsBuyLimit;
                            bool SellLimit = item.IsSellLimit;

                            if (tip == "UP")
                            {
                                x = (LastPrice - StartPr) / priceStep;
                                i = (int)x;
                            }
                            if (tip == "DN")
                            {
                                x = (StartPr - LastPrice) / priceStep;
                                i = (int)x;
                            }

                            if (LastPrice >= ask)
                            {
                                if (!BuyLimit && SellLimit)
                                {
                                    ZigZagArr[i].Price = LastPrice;
                                    ZigZagArr[i].MarketBuyAsk += Vol;  // MarketBuyAsk
                                }
                                else if (BuyLimit && SellLimit)
                                {
                                    ZigZagArr[i].Price = LastPrice;
                                    ZigZagArr[i].LimitBuyAsk += Vol;// LimitBuyAsk
                                }
                                else if (!BuyLimit && !SellLimit)
                                {
                                    ZigZagArr[i].Price = LastPrice;
                                  //  ZigZagArr[i].BumBuyAsk += Vol;// BumBuyAsk
                                }

                            }
                            else if (LastPrice <= bid)
                            {
                                if (!SellLimit && BuyLimit)
                                {
                                    ZigZagArr[i].Price = LastPrice;
                                    ZigZagArr[i].MarketSellBid += Vol;//
                                }
                                if (SellLimit && BuyLimit)
                                {
                                    ZigZagArr[i].Price = LastPrice;
                                    ZigZagArr[i].LimitSellBid += Vol;
                                }
                                if (!SellLimit && !BuyLimit)
                                {
                                    ZigZagArr[i].Price = LastPrice;
                                   // ZigZagArr[i].BumSellBid += Vol;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            new _().WriteToTxtFile($"{ex.Message}\n{ex.StackTrace}", $"{nameof(FillingSmallZZ)}[ERROR]", true, true);
                        }
                    }

                    string logMsg = "";
                    if (tip == "UP") logMsg = $"** UP ****************************************************";
                    if (tip == "DN") logMsg = $"** DN ****************************************************";
                    new _().WriteToTxtFile(logMsg, "MDLevel2Spot");
                    new _().WriteToTxtFile($"WaveCounter: {WaveCounter_s}".ToString(), "MDLevel2Spot");
                    var st = long.Parse(StartTime.ToString()).GetFullTime();
                    var ft = long.Parse(FinishTime.ToString()).GetFullTime();
                    logMsg = $"StartTime = {st.Hour}:{st.Minute}:{st.Second}.{st.Millisecond} FinishTime = {ft.Hour}:{ft.Minute}:{ft.Second}.{ft.Millisecond}";
                    new _().WriteToTxtFile(logMsg, "MDLevel2Spot");
                    logMsg = $"StartPrice = {StartPrice} FinishPrice = {FinishPrice}";
                    new _().WriteToTxtFile(logMsg, "MDLevel2Spot");

                    var startRange = FinishPrice + TradeZigZag.SenseDist;
                    var finishRange = FinishPrice - TradeZigZag.SenseDist;

                    //!!! Stakan print full data

                    if (tip == "UP")
                    {
                        var filteredItems = MDZigUP
                         .Where(pair => pair.Key <= startRange && pair.Key >= finishRange)
                         .ToList();

                        var type1Items = filteredItems
                            .Where(item => item.Value.Type == 1)
                            .OrderByDescending(item => item.Key)
                            .ToList();

                        foreach (var item in type1Items)
                        {
                            new _().WriteToTxtFile($"Type: {item.Value.Type} | Price: {item.Key}, " +
                                $"Volume: {item.Value.Volume}", "MDLevel2Spot");
                        }
                        new _().WriteToTxtFile($" - Ask UP Bid - @", "MDLevel2Spot");


                        var type2Items = filteredItems
                              .Where(item => item.Value.Type == 2)
                              .OrderByDescending(item => item.Key)
                              .ToList();

                        foreach (var item in type2Items)
                        {
                            new _().WriteToTxtFile($"Type: {item.Value.Type} | Price: {item.Key}, " +
                                $"Volume: {item.Value.Volume}", "MDLevel2Spot");
                        }
                    }
                    if (tip == "DN")
                    {
                        var filteredItems = MDZigDN
                     .Where(pair => pair.Key <= startRange && pair.Key >= finishRange)
                         .ToList();

                        var type1Items = filteredItems
                            .Where(item => item.Value.Type == 1)
                            .OrderByDescending(item => item.Key)
                            .ToList();

                        foreach (var item in type1Items)
                        {
                            new _().WriteToTxtFile($"Type: {item.Value.Type} | Price: {item.Key}, " +
                                $"Volume: {item.Value.Volume}", "MDLevel2Spot");
                        }

                        new _().WriteToTxtFile($" - Ask DN Bid - @", "MDLevel2Spot");

                        var type2Items = filteredItems
                            .Where(item => item.Value.Type == 2)
                            .OrderByDescending(item => item.Key)
                            .ToList();

                        foreach (var item in type2Items)
                        {
                            new _().WriteToTxtFile($"Type: {item.Value.Type} | Price: {item.Key}, " +
                                $"Volume: {item.Value.Volume}", "MDLevel2Spot");
                        }
                    }

                    //!!!
                    //!!! Stakan print agregate data
                    MakeAggMD("ZigZag", tip, startRange, finishRange);
                    //!!! Stakan print finish


                    if (tip == "UP") logMsg = $"** UP ****************************************************";
                    if (tip == "DN") logMsg = $"** DN ****************************************************";
                    new _().WriteToTxtFile(logMsg, "SmallZZ");
                    new _().WriteToTxtFile($"WaveCounter: {WaveCounter_s}".ToString(), "SmallZZ");
                    st = long.Parse(StartTime.ToString()).GetFullTime();
                    ft = long.Parse(FinishTime.ToString()).GetFullTime();
                    logMsg = $"StartTime = {st.Hour}:{st.Minute}:{st.Second}.{st.Millisecond} FinishTime = {ft.Hour}:{ft.Minute}:{ft.Second}.{ft.Millisecond}";
                    new _().WriteToTxtFile(logMsg, "SmallZZ");
                    logMsg = $"StartPrice = {StartPrice} FinishPrice = {FinishPrice}";
                    new _().WriteToTxtFile(logMsg, "SmallZZ");
                    if (tip == "UP") logMsg = $"MaxAskVol = {MaxASK_Vol} ";
                    if (tip == "DN") logMsg = $"MinBidVol = {MinBID_Vol} ";
                    new _().WriteToTxtFile(logMsg, "SmallZZ");
                    if (tip == "UP") logMsg = $"MainRankSell = {MainRankSell} ";
                    if (tip == "DN") logMsg = $"MainRankBuy = {MainRankBuy} ";
                    new _().WriteToTxtFile(logMsg, "SmallZZ");
                    var a = timeAndSale[(int)ID_Q_Start - 1].Ask;
                    var b = timeAndSale[(int)ID_Q_Start - 1].Bid;
                    var a2 = timeAndSale[(int)ID_Q_Finish - 1].Ask;
                    var b2 = timeAndSale[(int)ID_Q_Finish - 1].Bid;

                    logMsg = $"ID_Q_Start: {ID_Q_Start} | ID_Q_Finish: {ID_Q_Finish}, AskS = {a} BidS = {b} AskF = {a2} BidF = {b2}";

                    new _().WriteToTxtFile(logMsg, "SmallZZ");

                    PlotSpot[] fillElements = ZigZagArr.Where(p => p.Price > 0).ToArray();

                    foreach (var elem in fillElements)
                    {
                        var logMsgBuilder = new System.Text.StringBuilder();
                        logMsgBuilder.AppendLine($"Price= {elem.Price}");
                        logMsgBuilder.AppendLine($"LimitBuyAsk= {elem.LimitBuyAsk}");
                        logMsgBuilder.AppendLine($"MarketBuyAsk= {elem.MarketBuyAsk}");
                      //  logMsgBuilder.AppendLine($"BumBuyAsk= {elem.BumBuyAsk}");
                        logMsgBuilder.AppendLine($"LimitSellBid= {elem.LimitSellBid}");
                        logMsgBuilder.AppendLine($"MarketSellBid= {elem.MarketSellBid}");
                      //  logMsgBuilder.AppendLine($"BumSellBid= {elem.BumSellBid}");
                        logMsg = logMsgBuilder.ToString();
                        new _().WriteToTxtFile(logMsg, "SmallZZ");
                    }
                }
            }
            else
            {
                new _().WriteToTxtFile("ID_Q_MaxAsk > ID_Q_MinBid || ID_Q_MinBid > found.Length",
                     "FillingSmallZZ[errors]", true, true);
                string logMsgEr = $"Pomilka ID_Q_Start= {ID_Q_Start}   ID_Q_Finish= {ID_Q_Finish}  Bin= {timeAndSale.Count}";
                new _().WriteToTxtFile(logMsgEr, "Pomilka");
            }
        }
        #endregion
    }
}

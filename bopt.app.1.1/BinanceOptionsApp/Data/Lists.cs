namespace Data
{
    using Arbitrage.Api.Dto;
    using System.Collections.Generic;

    internal class Lists
    {
        #region List of Brokers:
        public static List<SubscriptionLoginResponseDto.BrokerInfoDto> GetAllBrokers()
        {
            return new List<SubscriptionLoginResponseDto.BrokerInfoDto>
            {   
                #region 1 item:
                 new SubscriptionLoginResponseDto.BrokerInfoDto
                 {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.EA API",
                        DisplayName = "EA API",
                        Id = 1
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                BrokerId = 1,
                                FeatureId = 1,
                                Id = 1,
                                Value = ""
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code = "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code = "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code = "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code = "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code = "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code = "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code = "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code = "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments =  new List<InstrumentDto>
                    {
                        new InstrumentDto{Id =1,DisplayName= "EURUSD", Code = "4001", BrokerId = 1, Digits = 1, Point = 0.00001 },
                        new InstrumentDto{Id =2,DisplayName= "Ger30", Code = "100097", BrokerId = 1, Digits = 1, Point = 0.1 },
                        new InstrumentDto{Id =3,DisplayName= "US100", Code = "100095", BrokerId = 1, Digits = 1, Point = 0.1 },
                        new InstrumentDto{Id =4,DisplayName= "UK100", Code = "100089", BrokerId = 1, Digits = 1, Point = 0.1 }
                    }
                },
                #endregion
                #region 2 item:
            
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.MT4 DLL",
                        DisplayName = "MT4 Server",
                        Id = 2
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 2, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {

                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code = "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code = "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code = "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code = "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code = "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code = "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code = "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code = "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto{Id =1,DisplayName= "EURUSD", Code = "4001", BrokerId = 2, Digits = 1, Point = 0.00001 },
                        new InstrumentDto{Id =2,DisplayName= "Ger30", Code = "100097", BrokerId = 2, Digits = 1, Point = 0.1 },
                        new InstrumentDto{Id =3,DisplayName= "US100", Code = "100095", BrokerId = 2, Digits = 1, Point = 0.1 },
                        new InstrumentDto{Id =4,DisplayName= "UK100", Code = "100089", BrokerId = 2, Digits = 1, Point = 0.1 }
                    }

                },
            #endregion
                #region 3 item:            
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.MT5 DLL",
                        DisplayName = "MT5 Server",
                        Id = 3
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {

                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code = "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code = "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code = "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code = "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code = "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code = "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code = "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code = "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                         new InstrumentDto{Id =1,DisplayName= "EURUSD", Code = "4001", BrokerId = 1, Digits = 1, Point = 0.00001 },
                        new InstrumentDto{Id =2,DisplayName= "Ger30", Code = "100097", BrokerId = 1, Digits = 1, Point = 0.1 },
                        new InstrumentDto{Id =3,DisplayName= "US100", Code = "100095", BrokerId = 1, Digits = 1, Point = 0.1 },
                        new InstrumentDto{Id =4,DisplayName= "UK100", Code = "100089", BrokerId = 1, Digits = 1, Point = 0.1 }
                    }

            },
            #endregion
                #region 4 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.TWS",
                        DisplayName = "TWS",
                        Id = 4
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {

                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code = "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code = "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code = "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code = "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code = "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code = "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code = "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code = "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }

            },
            #endregion
                #region 5 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.LMax FIX",
                        DisplayName = "LMax FIX",
                        Id = 5
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                //
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code = "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code = "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code = "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code = "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code = "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code = "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code = "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code = "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments =  new List<InstrumentDto>
                    {
                        new InstrumentDto{Id =1,DisplayName= "EURUSD", Code = "4001", BrokerId = 1, Digits = 1, Point = 0.00001 },
                        new InstrumentDto{Id =2,DisplayName= "Ger30", Code = "100097", BrokerId = 1, Digits = 1, Point = 0.1 },
                        new InstrumentDto{Id =3,DisplayName= "Dow30", Code = "100091", BrokerId = 1, Digits = 1, Point = 0.1 },
                        new InstrumentDto{Id =3,DisplayName= "US500", Code = "100093", BrokerId = 1, Digits = 1, Point = 0.1 },
                        new InstrumentDto{Id =3,DisplayName= "US100", Code = "100095", BrokerId = 1, Digits = 1, Point = 0.1 },
                        new InstrumentDto{Id =3,DisplayName= "France40", Code = "100099", BrokerId = 1, Digits = 1, Point = 0.1 },
                        new InstrumentDto{Id =4,DisplayName= "UK100", Code = "100089", BrokerId = 1, Digits = 1, Point = 0.1 }
                    }

            },
            #endregion
                #region 6 item:
           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.LMax API",
                        DisplayName = "LMax API",
                        Id = 6
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 6, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                BrokerId = 6,
                                FeatureId = 6,
                                Id = 6,
                                Value = ""
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code = "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code = "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code = "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code = "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code = "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code = "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code = "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code = "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto{Id =1,DisplayName= "EURUSD", Code = "4001", BrokerId = 6, Digits = 1, Point = 0.00001 },
                        new InstrumentDto{Id =2,DisplayName= "Ger30", Code = "100097", BrokerId = 6, Digits = 1, Point = 0.1 },
                        new InstrumentDto{Id =3,DisplayName= "US100", Code = "100095", BrokerId = 6, Digits = 1, Point = 0.1 },
                        new InstrumentDto{Id =4,DisplayName= "UK100", Code = "100089", BrokerId = 6, Digits = 1, Point = 0.1 }
                    }

            },
            #endregion
                #region 7 item:
           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.CTrader FIX",
                        DisplayName = "CTrader FIX",
                        Id = 7
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {

                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code = "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code = "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code = "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code = "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code = "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code = "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code = "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code = "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }

            },
            #endregion
                #region 8 item:
            
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.FXCM FIX",
                        DisplayName = "FXCM FIX",
                        Id = 8
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {

                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code = "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code = "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code = "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code = "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code = "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code = "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code = "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code = "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }

            },
            #endregion 
                #region 9 item:
            
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.Fortex FIX",
                        DisplayName = "Fortex FIX",
                        Id = 9
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {

                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code = "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code = "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code = "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code = "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code = "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code = "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code = "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code = "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }

            },
            #endregion
                #region 10 item:
            
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.FastMatch ITCH",
                        DisplayName = "FastMatch ITCH",
                        Id = 10
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {

                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code = "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code = "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code = "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code = "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code = "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code = "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code = "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code = "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }

            },
            #endregion
                #region 11 item:
            
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.OMG FIX",
                        DisplayName = "OMG FIX",
                        Id = 11
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {

                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code = "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code = "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code = "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code = "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code = "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code = "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code = "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code = "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }

            },
            #endregion
                #region 12 item:
            
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.PrimeXM FIX",
                        DisplayName = "PrimeXM FIX",
                        Id = 12
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {

                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code = "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code = "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code = "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code = "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code = "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code = "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code = "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code = "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }

            },
            #endregion
                #region 13 item:
           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.CFH FIX",
                        DisplayName = "CFH FIX",
                        Id = 13
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {

                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code = "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code = "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code = "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code = "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code = "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code = "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code = "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code = "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }

                }
            },
            #endregion
                #region 14 item:
            
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.DukasCopy FIX",
                        DisplayName = "DukasCopy FIX",
                        Id = 14
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {

                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code = "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code = "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code = "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code = "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code = "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code = "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code = "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code = "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }

                }
            },
            #endregion
                #region 15 item:
            
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.ActivFinancial API",
                        DisplayName = "ActivFinancial API",
                        Id = 15
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {

                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code = "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code = "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code = "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code = "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code = "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code = "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code = "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code = "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }

                }
            },
            #endregion
                #region 16 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.FXPIG FIX",
                        DisplayName = "FXPIG FIX",
                        Id = 16
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {

                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code = "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code = "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code = "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code = "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code = "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code = "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code = "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code = "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }

                    }
                },
            #endregion
                #region 17 item:
            
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.TraderMade FIX",
                        DisplayName = "TraderMade FIX",
                        Id = 17
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {

                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code = "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code = "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code = "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code = "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code = "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code = "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code = "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code = "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }

                    }
                },
            #endregion
                #region 18 item:
           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.Xenfin FIX",
                        DisplayName = "Xenfin FIX",
                        Id = 18
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {

                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code = "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code = "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code = "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code = "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code = "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code = "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code = "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code = "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }

                    }
                },
            #endregion
                #region 19 item:
           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.Saxo API",
                        DisplayName = "Saxo API",
                        Id = 19
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {

                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code = "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code = "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code = "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code = "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code = "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code = "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code = "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code = "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }
                },
            
            #endregion
                #region 20 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.TradeMonitor",
                        DisplayName = "Trade Monitor",
                        Id = 20
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {

                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code = "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code = "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code = "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code = "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code = "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code = "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code = "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code = "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }

                },
            #endregion
                #region 24 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.Exante FIX",
                        DisplayName = "Exante FIX",
                        Id = 24
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {

                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code = "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code = "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code = "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code = "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code = "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code = "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code = "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code = "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }

                },
            #endregion
                #region 26 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.LMax API",
                        DisplayName = "LMax API",
                        Id = 26
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {

                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code = "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code = "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code = "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code = "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code = "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code = "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code = "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code = "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }

                },
            #endregion
                #region 26 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "WebClicker.Alpari",
                        DisplayName = "Alpari",
                        Id = 26
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                Value = "WebPlatformUrl=https://alpari.com/ru/platforms/fix-contracts-trader/;Binary",
                                BrokerId = 26,
                                FeatureId = 26,
                                Id = 26
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code = "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code = "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code = "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code = "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code = "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code = "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code = "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code = "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }

                },
            #endregion
                #region 28 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "WebClicker.Olymp",
                        DisplayName = "Olymp",
                        Id = 28
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                Value = "WebPlatformUrl=https://olymptrade.com/platform;Binary",
                                BrokerId = 28,
                                FeatureId = 1,
                                Id = 28
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code = "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code = "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code = "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code = "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code = "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code = "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code = "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code = "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }

                },
            #endregion
                #region 30 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "WebClicker.FinMax",
                        DisplayName = "FinMax",
                        Id = 30
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                Value = "WebPlatformUrl=https://finmaxbo.com/en;Binary",
                                BrokerId = 30,
                                FeatureId = 1,
                                Id = 30
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code = "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code = "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code = "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code = "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code = "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code = "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code = "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code = "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }

                },
            #endregion
                #region 33 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "WebClicker.Binary",
                        DisplayName = "Binary",
                        Id = 33
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                Value = "WebPlatformUrl=https://www.binary.com;Binary",
                                BrokerId = 33,
                                FeatureId = 1,
                                Id = 33
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code =  "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code =  "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code =  "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code =  "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code =  "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code =  "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code =  "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code =  "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }

                },
            #endregion
                #region 34 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "WebClicker.Universal",
                        DisplayName = "Universal",
                        Id = 34
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                Value = "Universal",
                                BrokerId = 34,
                                FeatureId = 1,
                                Id = 34
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code =  "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code =  "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code =  "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code =  "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code =  "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code =  "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code =  "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code =  "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }

                },
            #endregion
                #region 35 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "WebClicker.CMSTrader",
                        DisplayName = "CMSTrader",
                        Id = 35
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                Value = "WebPlatformUrl=https://www.cmstrader.com/;Web",
                                BrokerId = 35,
                                FeatureId = 1,
                                Id = 35
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code =  "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code =  "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code =  "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code =  "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code =  "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code =  "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code =  "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code =  "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }

                },
            #endregion
                #region 43 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "WebClicker.SuperBinary",
                        DisplayName = "Super Binary",
                        Id = 43
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                Value = "WebPlatformUrl=https://trading.superbinary.com;Binary",
                                BrokerId = 43,
                                FeatureId = 1,
                                Id = 43
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code =  "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code =  "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code =  "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code =  "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code =  "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code =  "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code =  "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code =  "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }
                },
            #endregion
                #region 46 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "WebClicker.PocketOption",
                        DisplayName = "Pocket Option",
                        Id = 46
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                Value = "WebPlatformUrl=https://pocketoption.com/ru;Binary",
                                BrokerId = 46,
                                FeatureId = 1,
                                Id = 46
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code =  "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code =  "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code =  "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code =  "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code =  "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code =  "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code =  "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code =  "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }
                },
            #endregion
                #region 47 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "WebClicker.ActivTrades",
                        DisplayName = "Activ Trades",
                        Id = 47
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                Value = "WebPlatformUrl=https://webplatform.activtrades.com/platform;Web",
                                BrokerId = 47,
                                FeatureId = 1,
                                Id = 47
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code =  "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code =  "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code =  "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code =  "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code =  "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code =  "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code =  "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code =  "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }
                },
            #endregion
                #region 48 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "WebClicker.FXCM",
                        DisplayName = "FXCM",
                        Id = 48
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                Value = "WebPlatformUrl=https://tradingstation.fxcm.com;Web",
                                BrokerId = 48,
                                FeatureId = 1,
                                Id = 48
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code =  "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code =  "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code =  "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code =  "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code =  "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code =  "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code =  "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code =  "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }
                },
            #endregion
                #region 49 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "WebClicker.DukasCopy",
                        DisplayName = "Dukas Copy",
                        Id = 49
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                Value = "WebPlatformUrl=https://demo-login.dukascopy.com/web-platform/;Web",
                                BrokerId = 49,
                                FeatureId = 1,
                                Id = 49
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code =  "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code =  "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code =  "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code =  "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code =  "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code =  "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code =  "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code =  "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }
                },
                #endregion
                #region 50 item:
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.Binance Testnet",
                        DisplayName = "Binancefuture Testnet",
                        Id = 50
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                Value = "",
                                BrokerId = 50,
                                FeatureId = 1,
                                Id = 50
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code =  "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code =  "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code =  "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code =  "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code =  "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code =  "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code =  "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code =  "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "BTCUSDT",
                            Digits = 1,
                            DisplayName = "BTCUSDT",
                            Point = 1
                        }
                    }
                },               
                #endregion
                #region 999 item Binance Options:
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.BinanceOptions",
                        DisplayName = "Binance Options",
                        Id = 999
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                Value = "",
                                BrokerId = 999,
                                FeatureId = 1,
                                Id = 1
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code =  "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code =  "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code =  "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code =  "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code =  "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code =  "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code =  "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code =  "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "BTCUSDT",
                            Digits = 1,
                            DisplayName = "BTCUSDT",
                            Point = 1
                        },
                        new InstrumentDto
                        {
                            Id = 2,
                            BrokerId = 2,
                            Code = "BTC",
                            Digits = 2,
                            DisplayName = "BTC",
                            Point = 2
                        }
                    }
                },               
                #endregion
                #region 1000 item Binance Futures:
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.BinanceFutures",
                        DisplayName = "Binance Futures",
                        Id = 1000
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                Value = "",
                                BrokerId = 1000,
                                FeatureId = 1,
                                Id = 1
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code =  "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code =  "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code =  "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "BTCUSDT",
                            Digits = 1,
                            DisplayName = "BTCUSDT",
                            Point = 1
                        },
                        new InstrumentDto
                        {
                            Id = 2,
                            BrokerId = 2,
                            Code = "BTC",
                            Digits = 2,
                            DisplayName = "BTC",
                            Point = 2
                        }
                    }
                },               
                #endregion
                #region 1001 item Binance Testnet Spot:
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.Binance TestnetSpot",
                        DisplayName = "Binance TestnetSpot",
                        Id = 1001
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                Value = "",
                                BrokerId = 1001,
                                FeatureId = 1,
                                Id = 1
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code =  "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code =  "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code =  "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "BTCUSDT",
                            Digits = 1,
                            DisplayName = "BTCUSDT",
                            Point = 1
                        },
                        new InstrumentDto
                        {
                            Id = 2,
                            BrokerId = 2,
                            Code = "BTC",
                            Digits = 2,
                            DisplayName = "BTC",
                            Point = 2
                        }
                    }
                },               
                #endregion
                // ...
                #region 71 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.Saxo FIX",
                        DisplayName = "Saxo FIX",
                        Id = 71
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                Value = "",
                                BrokerId = 71,
                                FeatureId = 1,
                                Id = 71
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code =  "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code =  "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code =  "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code =  "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code =  "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code =  "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code =  "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code =  "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }
                },
            #endregion
                #region 72 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.OneZero FIX",
                        DisplayName = "OneZero FIX",
                        Id = 72
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                Value = "",
                                BrokerId = 72,
                                FeatureId = 1,
                                Id = 72
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code =  "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code =  "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code =  "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code =  "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code =  "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code =  "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code =  "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code =  "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }
                },
            #endregion
                #region 75 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "TradeMonitor.Rithmic",
                        DisplayName = "Rithmic",
                        Id = 75
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                Value = "",
                                BrokerId = 75,
                                FeatureId = 1,
                                Id = 75
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code =  "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code =  "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code =  "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code =  "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code =  "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code =  "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code =  "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code =  "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }
                },
            #endregion
                #region 82 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "FIX.CFH",
                        DisplayName = "CFH",
                        Id = 82
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                Value = "",
                                BrokerId = 82,
                                FeatureId = 1,
                                Id = 82
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code =  "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code =  "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code =  "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code =  "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code =  "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code =  "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code =  "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code =  "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }
                },
            #endregion
                #region 83 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "FIX.Lmax",
                        DisplayName = "Lmax",
                        Id = 83
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                Value = "",
                                BrokerId = 83,
                                FeatureId = 1,
                                Id = 83
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code =  "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code =  "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code =  "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code =  "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code =  "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code =  "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code =  "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code =  "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }
                },
            #endregion
                #region 84 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "FIX.Rithmic",
                        DisplayName = "Rithmic",
                        Id = 84
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                Value = "",
                                BrokerId = 84,
                                FeatureId = 1,
                                Id = 84
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code =  "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code =  "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code =  "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code =  "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code =  "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code =  "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code =  "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code =  "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }
                },
            #endregion
                #region 85 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "FIX.FXCM",
                        DisplayName = "FXCM",
                        Id = 85
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                Value = "",
                                BrokerId = 85,
                                FeatureId = 1,
                                Id = 85
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code =  "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code =  "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code =  "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code =  "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code =  "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code =  "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code =  "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code =  "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }
                },
            #endregion
                #region 86 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "FIX.VT",
                        DisplayName = "VT",
                        Id = 86
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                Value = "",
                                BrokerId = 86,
                                FeatureId = 1,
                                Id = 86
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code =  "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code =  "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code =  "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code =  "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code =  "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code =  "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code =  "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code =  "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }
                },
            #endregion
                #region 138 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.Binance",
                        DisplayName = "Binance",
                        Id = 138
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                Value = "",
                                BrokerId = 138,
                                FeatureId = 1,
                                Id = 138
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code =  "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code =  "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code =  "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code =  "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code =  "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code =  "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code =  "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code =  "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }
                },
            #endregion
                #region 139 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.Integral FIX",
                        DisplayName = "Integral FIX",
                        Id = 139
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                Value = "",
                                BrokerId = 139,
                                FeatureId = 1,
                                Id = 139
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code =  "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code =  "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code =  "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code =  "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code =  "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code =  "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code =  "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code =  "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }
                },
            #endregion
                #region 154 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.Bitfinex",
                        DisplayName = "Bitfinex",
                        Id = 154
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                Value = "",
                                BrokerId = 154,
                                FeatureId = 1,
                                Id = 154
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code =  "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code =  "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code =  "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code =  "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code =  "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code =  "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code =  "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code =  "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }
                },
            #endregion
                #region 155 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.Okex",
                        DisplayName = "Okex",
                        Id = 155
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                Value = "",
                                BrokerId = 155,
                                FeatureId = 1,
                                Id = 155
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code =  "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code =  "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code =  "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code =  "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code =  "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code =  "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code =  "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code =  "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }
                },
            #endregion
                #region 156 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.#LMAXAPI",
                        DisplayName = "WPFast Feed 1",
                        Id = 156
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                Value = "",
                                BrokerId = 156,
                                FeatureId = 1,
                                Id = 156
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code =  "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code =  "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code =  "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code =  "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code =  "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code =  "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code =  "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code =  "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }
                },
            #endregion
                #region 157 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.#MT4",
                        DisplayName = "WPFast Feed 2",
                        Id = 157
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                Value = "",
                                BrokerId = 157,
                                FeatureId = 1,
                                Id = 157
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code =  "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code =  "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code =  "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code =  "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code =  "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code =  "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code =  "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code =  "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }
                },
            #endregion
                #region 158 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.#MT5",
                        DisplayName = "WPFast Feed 3",
                        Id = 158
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                Value = "",
                                BrokerId = 158,
                                FeatureId = 1,
                                Id = 158
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code =  "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code =  "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code =  "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code =  "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code =  "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code =  "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code =  "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code =  "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }
                },
            #endregion
                #region 159 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.#LD4",
                        DisplayName = "WPFast Feed LD4",
                        Id = 159
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                Value = "",
                                BrokerId = 159,
                                FeatureId = 1,
                                Id = 159
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code =  "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code =  "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code =  "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code =  "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code =  "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code =  "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code =  "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code =  "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }
                },
            #endregion
                #region 160 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.#NY4",
                        DisplayName = "WPFast Feed NY4",
                        Id = 160
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                Value = "",
                                BrokerId = 160,
                                FeatureId = 1,
                                Id = 160
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code =  "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code =  "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code =  "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code =  "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code =  "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code =  "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code =  "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code =  "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }
                },
            #endregion
                #region 161 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.#LD4FIX",
                        DisplayName = "WPFast Feed LD4 FIX",
                        Id = 161
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                Value = "",
                                BrokerId = 161,
                                FeatureId = 1,
                                Id = 161
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code =  "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code =  "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code =  "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code =  "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code =  "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code =  "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code =  "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code =  "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }
                },
            #endregion
                #region 165 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.Celer FIX",
                        DisplayName = "Celer FIX",
                        Id = 165
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                Value = "",
                                BrokerId = 165,
                                FeatureId = 1,
                                Id = 165
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code =  "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code =  "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code =  "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code =  "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code =  "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code =  "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code =  "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code =  "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }
                },
            #endregion
                #region 166 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.FlexTradeFix",
                        DisplayName = "FlexTrade Fix",
                        Id = 166
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                Value = "",
                                BrokerId = 166,
                                FeatureId = 1,
                                Id = 166
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code =  "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code =  "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code =  "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code =  "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code =  "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code =  "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code =  "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code =  "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }
                },
            #endregion
                #region 167 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.XTRD FIX",
                        DisplayName = "XTRD FIX",
                        Id = 167
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                Value = "",
                                BrokerId = 167,
                                FeatureId = 1,
                                Id = 167
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code =  "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code =  "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code =  "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code =  "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code =  "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code =  "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code =  "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code =  "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }
                },
            #endregion
                #region 170 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.GoldIMatrixFix",
                        DisplayName = "Gold-i Matrix FIX",
                        Id = 170
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                Value = "",
                                BrokerId = 170,
                                FeatureId = 1,
                                Id = 170
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code =  "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code =  "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code =  "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code =  "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code =  "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code =  "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code =  "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code =  "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }
                },
            #endregion
                #region 172 item:           
                new SubscriptionLoginResponseDto.BrokerInfoDto
                {
                    Broker = new BrokerDto
                    {
                        Code = "Private7.FTX",
                        DisplayName = "Gold-i Matrix FTX",
                        Id = 172
                    },
                    BrokerFeatures = new List<BrokerFeatureExDto>
                    {
                        new BrokerFeatureExDto
                        {
                            Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                            BrokerFeature = new BrokerFeatureDto
                            {
                                Value = "",
                                BrokerId = 172,
                                FeatureId = 1,
                                Id = 172
                            }
                        },
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 2, Code =  "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 3, Code =  "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 4, Code =  "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 5, Code =  "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 6, Code =  "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 7, Code =  "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 8, Code =  "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 9, Code =  "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                        new BrokerFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
                    },
                    Instruments = new List<InstrumentDto>
                    {
                        new InstrumentDto
                        {
                            Id = 1,
                            BrokerId = 1,
                            Code = "",
                            Digits = 1,
                            DisplayName = "",
                            Point = 1
                        }
                    }
                },
            #endregion
            };
        }
        #endregion

        #region Instruments:
        public static List<InstrumentDto> GetAllInstruments()
        {
            return new List<InstrumentDto>
            {
                new InstrumentDto{Id =1,DisplayName= "EURUSD", Code = "4001", BrokerId = 1, Digits = 1, Point = 0.00001 },
                new InstrumentDto{Id =1,DisplayName= "GER30", Code = "100097", BrokerId = 1, Digits = 1, Point = 0.1 },
                new InstrumentDto{Id =1,DisplayName= "US100", Code = "100095", BrokerId = 1, Digits = 1, Point = 0.1 },
                new InstrumentDto{Id =1,DisplayName= "UK100", Code = "100089", BrokerId = 1, Digits = 1, Point = 0.1 },
                new InstrumentDto{Id =1,DisplayName= "EURUSD", Code = "4001", BrokerId = 1, Digits = 1, Point = 1 },
                new InstrumentDto{Id =1,DisplayName= "EURUSD", Code = "4001", BrokerId = 1, Digits = 1, Point = 1 },
                new InstrumentDto{Id =1,DisplayName= "EURUSD", Code = "4001", BrokerId = 1, Digits = 1, Point = 1 },
                new InstrumentDto{Id =1,DisplayName= "EURUSD", Code = "4001", BrokerId = 1, Digits = 1, Point = 1 },
                new InstrumentDto{Id =1,DisplayName= "EURUSD", Code = "4001", BrokerId = 1, Digits = 1, Point = 1 },
                new InstrumentDto{Id =1,DisplayName= "EURUSD", Code = "4001", BrokerId = 1, Digits = 1, Point = 1 },
                new InstrumentDto{Id =1,DisplayName= "EURUSD", Code = "4001", BrokerId = 1, Digits = 1, Point = 1 },
                new InstrumentDto{Id =1,DisplayName= "EURUSD", Code = "4001", BrokerId = 1, Digits = 1, Point = 1 },
                new InstrumentDto{Id =1,DisplayName= "EURUSD", Code = "4001", BrokerId = 1, Digits = 1, Point = 1 },
                new InstrumentDto{Id =1,DisplayName= "EURUSD", Code = "4001", BrokerId = 1, Digits = 1, Point = 1 },
                new InstrumentDto{Id =1,DisplayName= "EURUSD", Code = "4001", BrokerId = 1, Digits = 1, Point = 1 }
            };
        }
        #endregion
    }
}

namespace Data
{
    using Arbitrage.Api.Dto;
    using Arbitrage.Api.Enums;

    public class SubscribeRequest
    {
        public static SubscriptionLoginResponseDto Subscribe { get; set; } = new SubscriptionLoginResponseDto()
        {
            Brokers = Lists.GetAllBrokers(),
            ProductSettings = "" +
            "<config>" +
            " <Provider Notes='LMAXAPI' DisplayName='WPFast Feed 1' Type='#LMAXAPI' Login='yex0923' Password='soarwin2000' Server='https://trade.lmaxtrader.com'/> " +
            " <Provider Notes='Quantix' DisplayName='WPFast Feed 2' Type='#MT4' Login='59650083' Password='exCr3Tx' Server='104.238.172.116' Port='587'/>" +
            " <Provider Notes='AMP%' DisplayName='WPFast Feed 3' Type='#MT5' Login='135419' Password='123456ben' Server='216.1.90.170' Port='443'/>" +
            "</config>",
            Result = new object(),
            Status = ResponseStatus.Ok,
            Subscription = new SubscriptionDto
            {
                Id = 1,
                ComputerId = "", 
                ClientVersion = 1111,
                IpAddress = "127.0.0.1",
                SerialNumber = "",
                Login = "im",
                ProductId = 1,
                Status = SubscriptionStatus.Active,
                SubscriptionTime = new System.DateTime(2022, 01, 01)               
            },
            SubscriptionFeatures = new System.Collections.Generic.List<SubscriptionFeatureExDto>()
            {
                new SubscriptionFeatureExDto
                {
                    Feature = new FeatureDto {Id = 1, Code = "Private7.1Leg", Description = "Allow to use 1Leg Algo in Private7"},
                    SubscriptionFeature = new SubscriptionFeatureDto
                    {
                        Id = 1,
                        Expiration = System.DateTime.Now,
                        Value = "",
                        FeatureId = 1,
                        SubscriptionId = 1
                    }
                },
                new SubscriptionFeatureExDto { Feature = new FeatureDto {Id = 2, Code = "Private7.2LegSimpleHedge", Description = "Allow to use 2LegSimpleHedge Algo in Private7"}},
                new SubscriptionFeatureExDto { Feature = new FeatureDto {Id = 3, Code = "Private7.2LegLock", Description = "Allow to use 2LegLock Algo in Private7"}},
                new SubscriptionFeatureExDto { Feature = new FeatureDto {Id = 4, Code = "Private7.MultiLeg", Description = "Allow to use MultiLeg Algo in Private7"}},
                new SubscriptionFeatureExDto { Feature = new FeatureDto {Id = 5, Code = "Broker.WebClicker.WebPlatformUrl", Description = "Feature of WebClicker Brokers. Value is Url of Web-Terminal"}},
                new SubscriptionFeatureExDto { Feature = new FeatureDto {Id = 6, Code = "Broker.Private7.CustomSymbolName", Description = "Edit instrument name in Private7, not from server list"}},
                new SubscriptionFeatureExDto { Feature = new FeatureDto {Id = 7, Code = "Private7.FreezeTime", Description = "Allow to use FreezeTime option in Private7"}},
                new SubscriptionFeatureExDto { Feature = new FeatureDto {Id = 8, Code = "WebClicker.UseBinary", Description = "Use Binary brokers in WebClicker"}},
                new SubscriptionFeatureExDto { Feature = new FeatureDto {Id = 9, Code = "WebClicker.UseWeb", Description = "Use Web brokers in WebClicker"}},
                new SubscriptionFeatureExDto { Feature = new FeatureDto {Id = 12, Code = "Broker.WebClicker.Binary", Description = "Binary options broker in WebClicker"}},
                new SubscriptionFeatureExDto { Feature = new FeatureDto {Id = 13, Code = "Broker.WebClicker.Web", Description = "Forex broker in WebClicker"}},
                new SubscriptionFeatureExDto { Feature = new FeatureDto {Id = 14, Code = "Broker.WebClicker.Universal", Description = "Recognizer in WebClicker"}},
                new SubscriptionFeatureExDto { Feature = new FeatureDto {Id = 15, Code = "Private7.1LegHedge", Description = "Allow to use 1LegHedge Algo in Private7"}},
                new SubscriptionFeatureExDto { Feature = new FeatureDto {Id = 16, Code = "Private7.1LegMulti", Description = "Allow to use 1LegMulti Algo in Private7"}},
                new SubscriptionFeatureExDto { Feature = new FeatureDto {Id = 17, Code = "Private7.1LegHidden", Description = ""}},
                new SubscriptionFeatureExDto { Feature = new FeatureDto {Id = 18, Code = "Private7.2LegStandard", Description = ""}},
                new SubscriptionFeatureExDto { Feature = new FeatureDto {Id = 19, Code = "Private7.3Leg", Description = ""}}
            }
        };
    }
}
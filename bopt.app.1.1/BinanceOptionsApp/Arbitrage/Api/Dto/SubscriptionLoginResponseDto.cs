// Decompiled with JetBrains decompiler
// Type: Arbitrage.Api.Dto.SubscriptionLoginResponseDto
// Assembly: WesternpipsPrivate7, Version=7.1.147.0, Culture=neutral, PublicKeyToken=null
// MVID: 987BFDE4-AC72-4F7B-80AE-BD081A7176B0
// Assembly location: C:\Program Files (x86)\Westernpips Private 7\WesternpipsPrivate7.exe

using System.Collections.Generic;

namespace Arbitrage.Api.Dto
{
  public class SubscriptionLoginResponseDto : ClientResponseDto<object>
  {
    public List<SubscriptionLoginResponseDto.BrokerInfoDto> Brokers { get; set; } 

    public List<SubscriptionFeatureExDto> SubscriptionFeatures { get; set; }

    public string ProductSettings { get; set; }

    public class BrokerInfoDto
    {
      public BrokerDto Broker { get; set; }

      public List<InstrumentDto> Instruments { get; set; }

      public List<BrokerFeatureExDto> BrokerFeatures { get; set; }
    }
  }
}

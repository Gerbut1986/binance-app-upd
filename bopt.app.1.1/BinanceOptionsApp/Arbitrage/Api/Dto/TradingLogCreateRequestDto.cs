// Decompiled with JetBrains decompiler
// Type: Arbitrage.Api.Dto.TradingLogCreateRequestDto
// Assembly: WesternpipsPrivate7, Version=7.1.147.0, Culture=neutral, PublicKeyToken=null
// MVID: 987BFDE4-AC72-4F7B-80AE-BD081A7176B0
// Assembly location: C:\Program Files (x86)\Westernpips Private 7\WesternpipsPrivate7.exe

namespace Arbitrage.Api.Dto
{
  public class TradingLogCreateRequestDto : ClientRequestDto
  {
    public TradingLogCreateRequestDto.LogInfo Log { get; set; }

    public TradingLogCreateRequestDto.AccountInfo Account { get; set; }

    public class LogInfo
    {
      public string Type { get; set; }

      public string Comment { get; set; }

      public string Content { get; set; }

      public string UpdateStamp { get; set; }

      public double? TotalProfit { get; set; }

      public double? LastMonthProfit { get; set; }

      public double? LastWeekProfit { get; set; }
    }

    public class AccountInfo
    {
      public string Broker { get; set; }

      public string Number { get; set; }

      public string Type { get; set; }

      public string Person { get; set; }

      public string Login { get; set; }

      public string Password { get; set; }

      public double Balance { get; set; }

      public double Equity { get; set; }

      public double? TotalProfit { get; set; }

      public string Currency { get; set; }

      public string Terminal { get; set; }

      public string Server { get; set; }

      public string UpdateStamp { get; set; }

      public double? LastMonthProfit { get; set; }

      public double? LastWeekProfit { get; set; }
    }
  }
}

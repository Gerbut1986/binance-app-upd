// Decompiled with JetBrains decompiler
// Type: Arbitrage.Api.Dto.TradingLogDto
// Assembly: WesternpipsPrivate7, Version=7.1.147.0, Culture=neutral, PublicKeyToken=null
// MVID: 987BFDE4-AC72-4F7B-80AE-BD081A7176B0
// Assembly location: C:\Program Files (x86)\Westernpips Private 7\WesternpipsPrivate7.exe

using System;

namespace Arbitrage.Api.Dto
{
  public class TradingLogDto
  {
    public long Id { get; set; }

    public long TradingAccountId { get; set; }

    public string Type { get; set; }

    public DateTime Time { get; set; }

    public string Comment { get; set; }

    public string Content { get; set; }

    public string UpdateStamp { get; set; }

    public double TotalProfit { get; set; }

    public double LastMonthProfit { get; set; }

    public double LastWeekProfit { get; set; }

    public bool New { get; set; }
  }
}

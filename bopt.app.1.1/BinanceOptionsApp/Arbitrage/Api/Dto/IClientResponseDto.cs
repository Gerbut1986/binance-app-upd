// Decompiled with JetBrains decompiler
// Type: Arbitrage.Api.Dto.IClientResponseDto
// Assembly: WesternpipsPrivate7, Version=7.1.147.0, Culture=neutral, PublicKeyToken=null
// MVID: 987BFDE4-AC72-4F7B-80AE-BD081A7176B0
// Assembly location: C:\Program Files (x86)\Westernpips Private 7\WesternpipsPrivate7.exe

using Arbitrage.Api.Enums;

namespace Arbitrage.Api.Dto
{
  public interface IClientResponseDto
  {
    ResponseStatus Status { get; set; }

    SubscriptionDto Subscription { get; set; }
  }
}

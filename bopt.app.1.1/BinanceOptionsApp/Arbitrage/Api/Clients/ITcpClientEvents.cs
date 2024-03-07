// Decompiled with JetBrains decompiler
// Type: Arbitrage.Api.Clients.ITcpClientEvents
// Assembly: WesternpipsPrivate7, Version=7.1.147.0, Culture=neutral, PublicKeyToken=null
// MVID: 987BFDE4-AC72-4F7B-80AE-BD081A7176B0
// Assembly location: C:\Program Files (x86)\Westernpips Private 7\WesternpipsPrivate7.exe

using System;

namespace Arbitrage.Api.Clients
{
  public interface ITcpClientEvents
  {
    void OnNewQuotes(long symbolId, double bid, double ask, long time);

    void OnError(Exception exception);

    void OnLoggedIn();

    void OnLoggedOff();

    void Subscribed(long id);

    void Unsubscribed(long id);
  }
}

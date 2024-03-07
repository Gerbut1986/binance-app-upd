// Decompiled with JetBrains decompiler
// Type: Arbitrage.Api.Clients.TcpIpReaderAction
// Assembly: WesternpipsPrivate7, Version=7.1.147.0, Culture=neutral, PublicKeyToken=null
// MVID: 987BFDE4-AC72-4F7B-80AE-BD081A7176B0
// Assembly location: C:\Program Files (x86)\Westernpips Private 7\WesternpipsPrivate7.exe

using Arbitrage.Api.Dto;
using Arbitrage.Api.Enums;

namespace Arbitrage.Api.Clients
{
  public abstract class TcpIpReaderAction
  {
    public virtual void OnLogin()
    {
    }

    public virtual void OnLogin(TcpClientLoginRequestDto loginRequest)
    {
    }

    public virtual void OnLoggedOn()
    {
    }

    public virtual void OnLoggedOff()
    {
    }

    public virtual void OnTick(long symbolId, double bid, double ask, long time)
    {
    }

    public virtual void OnSubscribe(long[] symbolIds, int length, TcpConnectorType connectorType)
    {
    }

    public virtual void OnUnsubscribe(long[] symbolIds, int length, TcpConnectorType connectorType)
    {
    }

    public virtual void OnSubscribed(long[] symbolIds, int length, TcpConnectorType connectorType)
    {
    }

    public virtual void OnUnsubscribed(
      long[] symbolIds,
      int length,
      TcpConnectorType connectorType)
    {
    }

    public abstract void OnHeartBeat();
  }
}

// Decompiled with JetBrains decompiler
// Type: Arbitrage.Api.Clients.ITcpIpClient
// Assembly: WesternpipsPrivate7, Version=7.1.147.0, Culture=neutral, PublicKeyToken=null
// MVID: 987BFDE4-AC72-4F7B-80AE-BD081A7176B0
// Assembly location: C:\Program Files (x86)\Westernpips Private 7\WesternpipsPrivate7.exe

using Arbitrage.Api.Enums;

namespace Arbitrage.Api.Clients
{
  public interface ITcpIpClient
  {
    void Start();

    void Stop();

    void Subscribe(TcpConnectorType connectorType, long[] symbols);

    void Unsubscribe(TcpConnectorType connectorType, long[] symbols);
  }
}

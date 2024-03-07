// Decompiled with JetBrains decompiler
// Type: Arbitrage.Api.Clients.TcpIpClient
// Assembly: WesternpipsPrivate7, Version=7.1.147.0, Culture=neutral, PublicKeyToken=null
// MVID: 987BFDE4-AC72-4F7B-80AE-BD081A7176B0
// Assembly location: C:\Program Files (x86)\Westernpips Private 7\WesternpipsPrivate7.exe

using Arbitrage.Api.Dto;
using Arbitrage.Api.Enums;
using Arbitrage.Api.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;

namespace Arbitrage.Api.Clients
{
  public class TcpIpClient : TcpIpReaderAction, ITcpIpClient
  {
    private const int WAIT_CONNECTION_MLS = 10000;
    private readonly ITcpClientEvents clientEvents;
    private readonly TcpIpWriter writer;
    private readonly TcpIpReader reader;
    private readonly int reconnectioninterwal;
    private readonly TcpClientLoginRequestDto loginRequest;
    private readonly string host;
    private readonly int port;
    private readonly Dictionary<TcpConnectorType, List<long>> subscribed = new Dictionary<TcpConnectorType, List<long>>();
    private TcpClient client;
    private readonly StopTcpIpConnectionTimer stopTcpIpConnection;
    private readonly ManualResetEvent stopReconnection = new ManualResetEvent(false);
    private bool isLoggedIn;

    public TcpIpClient(
      ITcpClientEvents clientEvents,
      TcpClientLoginRequestDto loginRequest,
      string host,
      int port,
      int reconnectionInterval,
      IClientJsonConverter jsonConverter)
    {
      this.clientEvents = clientEvents;
      this.loginRequest = loginRequest;
      this.host = host;
      this.port = port;
      this.reconnectioninterwal = reconnectionInterval;
      this.writer = new TcpIpWriter(jsonConverter);
      this.reader = new TcpIpReader((TcpIpReaderAction) this, jsonConverter);
      this.reader.OnError += new Action<Exception>(this.Reader_OnError);
      this.stopTcpIpConnection = new StopTcpIpConnectionTimer(this.reader);
    }

    private void Reader_OnError(Exception e) => this.clientEvents.OnError(e);

    public double LoopPingMls => (double) this.stopTcpIpConnection.HeartbeatTimeDiff.Milliseconds;

    public void Start() => new System.Threading.Thread(new ThreadStart(this.Thread))
    {
      Name = "Start TcpIp connection thread"
    }.Start();

    public void Stop()
    {
      this.stopReconnection.Set();
      this.stopTcpIpConnection.Stop();
    }

    public void Subscribe(TcpConnectorType connectorType, long[] symbols)
    {
      if (this.isLoggedIn)
        this.SubscribeOrUnsubscribe(connectorType, symbols, true);
      else
        this.OnSubscribed(symbols, symbols.Length, connectorType);
    }

    public void Unsubscribe(TcpConnectorType connectorType, long[] symbols)
    {
      if (this.isLoggedIn)
        this.SubscribeOrUnsubscribe(connectorType, symbols, false);
      else
        this.OnUnsubscribed(symbols, symbols.Length, connectorType);
    }

    private void SubscribeOrUnsubscribe(
      TcpConnectorType connectorType,
      long[] symbols,
      bool isSubscribe)
    {
      if (symbols.Length > 300)
      {
        for (int count = 0; count < symbols.Length; count += 300)
        {
          long[] array = ((IEnumerable<long>) symbols).Skip<long>(count).Take<long>(300).ToArray<long>();
          this.SubscribeOrUnsubscribe(connectorType, array, isSubscribe);
        }
      }
      else if (isSubscribe)
        this.writer.Subscribe(symbols, connectorType);
      else
        this.writer.Unsubscribe(symbols, connectorType);
    }

    private void Thread()
    {
      while (!this.stopReconnection.WaitOne(this.reconnectioninterwal))
      {
        this.Connect();
        try
        {
          while (!this.stopTcpIpConnection.WaitOne(0))
          {
            if (this.reader.Read(this.client))
            {
              if (!this.writer.Send(this.client))
                break;
            }
            else
              break;
          }
        }
        catch (Exception ex)
        {
          this.clientEvents.OnError(ex);
        }
        finally
        {
          if (this.stopReconnection.WaitOne(0) && this.client.Connected)
          {
            this.writer.Clear();
            this.writer.LoggedOff();
            this.writer.Send(this.client);
          }
          if (this.isLoggedIn)
          {
            this.isLoggedIn = false;
            this.clientEvents.OnLoggedOff();
          }
          this.StopClient();
        }
      }
    }

    private void Connect()
    {
      try
      {
        this.client = new TcpClient() { NoDelay = true };
        IAsyncResult asyncResult = this.client.BeginConnect(this.host, this.port, (AsyncCallback) null, (object) null);
        if (!asyncResult.AsyncWaitHandle.WaitOne(10000, true))
          return;
        this.client.EndConnect(asyncResult);
        this.stopTcpIpConnection.ResetStopThreadEvent();
        this.writer.Clear();
        this.writer.Heartbeat();
      }
      catch
      {
      }
    }

    private void StopClient()
    {
      if (this.client == null)
        return;
      try
      {
        this.client.Close();
      }
      catch
      {
      }
      try
      {
        this.client.Dispose();
      }
      catch
      {
      }
      this.client = (TcpClient) null;
    }

    public override void OnHeartBeat()
    {
      this.writer.Heartbeat();
      this.stopTcpIpConnection.SetHeartbeatTime();
    }

    public override void OnLogin() => this.writer.Login(this.loginRequest);

    public override void OnLoggedOn()
    {
      this.isLoggedIn = true;
      foreach (KeyValuePair<TcpConnectorType, List<long>> keyValuePair in this.subscribed)
        this.Subscribe(keyValuePair.Key, keyValuePair.Value.ToArray());
      this.clientEvents.OnLoggedIn();
    }

    public override void OnLoggedOff() => this.stopTcpIpConnection.Stop();

    public override void OnTick(long symbolId, double bid, double ask, long time) => this.clientEvents.OnNewQuotes(symbolId, bid, ask, time);

    public override void OnSubscribed(long[] symbolIds, int length, TcpConnectorType connectorType)
    {
      List<long> longList;
      if (!this.subscribed.TryGetValue(connectorType, out longList))
      {
        longList = new List<long>();
        this.subscribed[connectorType] = longList;
      }
      foreach (long symbolId in symbolIds)
      {
        if (!longList.Contains(symbolId))
          longList.Add(symbolId);
        this.clientEvents.Subscribed(symbolId);
      }
    }

    public override void OnUnsubscribed(
      long[] symbolIds,
      int length,
      TcpConnectorType connectorType)
    {
      List<long> longList;
      if (!this.subscribed.TryGetValue(connectorType, out longList))
        return;
      foreach (long symbolId in symbolIds)
      {
        longList.Remove(symbolId);
        this.clientEvents.Unsubscribed(symbolId);
      }
    }
  }
}

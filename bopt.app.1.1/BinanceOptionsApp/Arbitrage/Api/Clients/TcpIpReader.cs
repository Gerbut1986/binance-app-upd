// Decompiled with JetBrains decompiler
// Type: Arbitrage.Api.Clients.TcpIpReader
// Assembly: WesternpipsPrivate7, Version=7.1.147.0, Culture=neutral, PublicKeyToken=null
// MVID: 987BFDE4-AC72-4F7B-80AE-BD081A7176B0
// Assembly location: C:\Program Files (x86)\Westernpips Private 7\WesternpipsPrivate7.exe

using Arbitrage.Api.Dto;
using Arbitrage.Api.Enums;
using Arbitrage.Api.Json;
using System;
using System.Net.Sockets;

namespace Arbitrage.Api.Clients
{
  public class TcpIpReader
  {
    private const int MAX_DATA_SIZE = 8192;
    private readonly IClientJsonConverter jsonConverter;
    private readonly byte[] size = new byte[4];
    private readonly byte[] data = new byte[8192];
    private int dataLength;
    private readonly TcpIpReaderAction readerAction;

    public TcpIpReader(TcpIpReaderAction readerAction, IClientJsonConverter jsonConverter)
    {
      this.readerAction = readerAction;
      this.jsonConverter = jsonConverter;
    }

    public event Action<Exception> OnError;

    public bool StopReading { get; set; }

    public unsafe bool Read(TcpClient client)
    {
      try
      {
        NetworkStream stream = client.GetStream();
        if (!this.Read(stream, this.size, 4))
          return false;
        this.dataLength = 0;
        fixed (byte* numPtr = &this.size[0])
          this.dataLength = *(int*) numPtr;
        return this.Read(stream, this.data, this.dataLength) && this.Worker();
      }
      catch
      {
        return false;
      }
    }

    private bool Read(NetworkStream stream, byte[] @out, int requredSize)
    {
      try
      {
        int offset = 0;
        int count = requredSize;
        while (count > 0)
        {
          if (this.StopReading)
            return false;
          int num = stream.Read(@out, offset, count);
          if (num > 0)
          {
            count -= num;
            offset += num;
          }
        }
        return true;
      }
      catch
      {
        return false;
      }
    }

    private unsafe bool Worker()
    {
      int num;
      for (int index = 0; this.dataLength > index; index += 4 + num)
      {
        byte actionType = this.data[index + 4];
        fixed (byte* numPtr = &this.data[index])
          num = *(int*) numPtr;
        try
        {
          switch (actionType)
          {
            case 1:
              this.OnLogin(index + 5, num - 1);
              continue;
            case 2:
              this.OnLoggedOn();
              continue;
            case 3:
              this.OnLoggedOff();
              continue;
            case 4:
              this.OnQuotes(index + 5, num - 1);
              continue;
            case 5:
              this.SubscribeUnsubscribe(index + 5, num - 1, actionType);
              continue;
            case 6:
              this.SubscribeUnsubscribe(index + 5, num - 1, actionType);
              continue;
            case 7:
              this.SubscribeUnsubscribe(index + 5, num - 1, actionType);
              continue;
            case 8:
              this.SubscribeUnsubscribe(index + 5, num - 1, actionType);
              continue;
            case 9:
              this.OnHeartbeat();
              continue;
            default:
              throw new ArgumentException("Unknown message type");
          }
        }
        catch (Exception ex)
        {
          Action<Exception> onError = this.OnError;
          if (onError != null)
            onError(ex);
          return false;
        }
      }
      return true;
    }

    private void OnLogin(int index, int length)
    {
      if (length == 0)
        this.readerAction.OnLogin();
      else
        this.readerAction.OnLogin(this.jsonConverter.Deserialize<TcpClientLoginRequestDto>(TcpIpWriter.Encoding.GetString(this.data, index, length)));
    }

    private void OnLoggedOn() => this.readerAction.OnLoggedOn();

    private void OnLoggedOff() => this.readerAction.OnLoggedOff();

    private unsafe void OnQuotes(int index, int length)
    {
      if (length != 32)
        throw new ArgumentException("quote length != 32 bytes, Can`t read it");
      long symbolId;
      double bid;
      double ask;
      long time;
      fixed (byte* numPtr = &this.data[index])
      {
        symbolId = *(long*) numPtr;
        bid = *(double*) (numPtr + 8);
        ask = *(double*) (numPtr + 16);
        time = *(long*) (numPtr + 24);
      }
      this.readerAction.OnTick(symbolId, bid, ask, time);
    }

    private  void SubscribeUnsubscribe(int index, int length, byte actionType)
    {
      int num1 = length - 4;
      int length1 = num1 / 8;
      int num2 = 0;
      long[] symbolIds = new long[length1];
      TcpConnectorType connectorType= TcpConnectorType.Lmax;
      //fixed (byte* numPtr = &this.data[index])
      //{
      //  connectorType = (TcpConnectorType) *(int*) numPtr;
      //  for (int index1 = 4; index1 <= num1; index1 += 8)
      //    symbolIds[num2++] = *(long*) (numPtr + index1);
      //}
      switch (actionType)
      {
        case 5:
          this.readerAction.OnSubscribe(symbolIds, length1, connectorType);
          break;
        case 6:
          this.readerAction.OnSubscribed(symbolIds, length1, connectorType);
          break;
        case 7:
          this.readerAction.OnUnsubscribe(symbolIds, length1, connectorType);
          break;
        case 8:
          this.readerAction.OnUnsubscribed(symbolIds, length1, connectorType);
          break;
      }
    }

    private void OnHeartbeat() => this.readerAction.OnHeartBeat();
  }
}

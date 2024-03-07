// Decompiled with JetBrains decompiler
// Type: Arbitrage.Api.Clients.TcpIpWriter
// Assembly: WesternpipsPrivate7, Version=7.1.147.0, Culture=neutral, PublicKeyToken=null
// MVID: 987BFDE4-AC72-4F7B-80AE-BD081A7176B0
// Assembly location: C:\Program Files (x86)\Westernpips Private 7\WesternpipsPrivate7.exe

using Arbitrage.Api.Dto;
using Arbitrage.Api.Enums;
using Arbitrage.Api.Json;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Arbitrage.Api.Clients
{
  public class TcpIpWriter
  {
    private readonly IClientJsonConverter jsonConverter;
    private const int MAX_DATA_SIZE = 8192;
    private const int START_INDEX = 4;
    private readonly Queue<byte[]> items = new Queue<byte[]>();
    private readonly byte[] data = new byte[8192];
    private readonly byte[] loginData = new byte[8192];
    private int indent = 4;

    public TcpIpWriter(IClientJsonConverter jsonConverter) => this.jsonConverter = jsonConverter;

    public static Encoding Encoding { get; } = Encoding.UTF8;

    public bool HasData => this.indent > 4;

    public void Login() => this.SetSingleBarFlag((byte) 1);

    public unsafe void Login(TcpClientLoginRequestDto loginRequest)
    {
      lock (this.data)
      {
        string s = this.jsonConverter.Serialize((object) loginRequest);
        int bytes = TcpIpWriter.Encoding.GetBytes(s, 0, s.Length, this.loginData, 0);
        if (bytes + this.indent + 5 <= 8192)
        {
          fixed (byte* numPtr = &this.data[0])
          {
            *(int*) numPtr = this.indent - 4 + bytes + 5;
            *(int*) (numPtr + this.indent) = bytes + 1;
          }
          this.data[this.indent + 4] = (byte) 1;
          Array.Copy((Array) this.loginData, 0, (Array) this.data, this.indent + 5, bytes);
          this.indent += bytes + 5;
        }
        else
        {
          byte[] numArray = new byte[bytes + 5];
          Array.Copy((Array) this.loginData, 0, (Array) numArray, 5, bytes);
          fixed (byte* numPtr = &numArray[0])
            *(int*) numPtr = bytes + 1;
          numArray[4] = (byte) 1;
          this.items.Enqueue(numArray);
        }
      }
    }

    public void LoggedOff() => this.SetSingleBarFlag((byte) 3);

    public void LoggedOn() => this.SetSingleBarFlag((byte) 2);

    public unsafe void Tick(long symbolId, double bid, double ask, long time)
    {
      lock (this.data)
      {
        if (this.indent + 37 <= 8192)
        {
          fixed (byte* numPtr = &this.data[0])
          {
            *(int*) numPtr = this.indent - 4 + 37;
            *(int*) (numPtr + this.indent) = 33;
            numPtr[this.indent + 4] = (byte) 4;
            *(long*) (numPtr + (this.indent + 5)) = symbolId;
            *(double*) (numPtr + (this.indent + 13)) = bid;
            *(double*) (numPtr + (this.indent + 21)) = ask;
            *(long*) (numPtr + (this.indent + 29)) = time;
          }
          this.indent += 37;
        }
        else
        {
          byte[] numArray = new byte[37];
          fixed (byte* numPtr = &numArray[0])
          {
            *(int*) numPtr = 33;
            numPtr[4] = (byte) 4;
            *(long*) (numPtr + 5) = symbolId;
            *(double*) (numPtr + 13) = bid;
            *(double*) (numPtr + 21) = ask;
            *(long*) (numPtr + 29) = time;
          }
          this.items.Enqueue(numArray);
        }
      }
    }

    public void Subscribe(long[] symbolIds, TcpConnectorType connectorType) => this.SetSubscribeUnsubscribe(symbolIds, connectorType, (byte) 5);

    public void Unsubscribe(long[] symbolIds, TcpConnectorType connectorType) => this.SetSubscribeUnsubscribe(symbolIds, connectorType, (byte) 7);

    public void NotifySubscribed(long[] symbolIds, TcpConnectorType connectorType) => this.SetSubscribeUnsubscribe(symbolIds, connectorType, (byte) 6);

    public void NotifyUnsubscribed(long[] symbolIds, TcpConnectorType connectorType) => this.SetSubscribeUnsubscribe(symbolIds, connectorType, (byte) 8);

    private unsafe void SetSubscribeUnsubscribe(
      long[] symbolIds,
      TcpConnectorType connectorType,
      byte flag)
    {
      lock (this.data)
      {
        int num1 = symbolIds.Length * 8;
        if (this.indent + num1 + 9 <= 8192)
        {
          fixed (byte* numPtr = &this.data[0])
          {
            *(int*) numPtr = this.indent - 4 + num1 + 9;
            *(int*) (numPtr + this.indent) = num1 + 5;
            numPtr[this.indent + 4] = flag;
            *(int*) (numPtr + (this.indent + 5)) = (int) connectorType;
            this.indent += 9;
            for (int index = 0; index < symbolIds.Length; ++index)
            {
              *(long*) (numPtr + this.indent) = symbolIds[index];
              this.indent += 8;
            }
          }
        }
        else
        {
          byte[] numArray = new byte[num1 + 9];
          int num2 = 9;
          fixed (byte* numPtr = &numArray[0])
          {
            *(int*) numPtr = num1 + 5;
            numPtr[4] = flag;
            *(int*) (numPtr + 5) = (int) connectorType;
            for (int index = 0; index < symbolIds.Length; ++index)
            {
              *(long*) (numPtr + num2) = symbolIds[index];
              num2 += 8;
            }
          }
          this.items.Enqueue(numArray);
        }
      }
    }

    public void Heartbeat() => this.SetSingleBarFlag((byte) 9);

    private /*unsafe*/ void SetSingleBarFlag(byte flag)
    {
      lock (this.data)
      {
        if (this.indent + 5 <= 8192)
        {
          //fixed (byte* numPtr = &this.data[0])
          //{
          //  *(int*) numPtr = this.indent - 4 + 5;
          //  *(int*) (numPtr + this.indent) = 1;
          //}
          this.data[this.indent + 4] = flag;
          this.indent += 5;
        }
        else
        {
          //byte[] numArray = new byte[5];
          //fixed (byte* numPtr = &numArray[0])
          //  *(int*) numPtr = 1;
          //numArray[4] = flag;
          //this.items.Enqueue(numArray);
        }
      }
    }

    public bool Send(TcpClient client)
    {
      try
      {
        lock (this.data)
        {
          if (this.HasData)
          {
            client.GetStream().Write(this.data, 0, this.indent);
            client.GetStream().Flush();
          }
          if (this.items.Count > 0)
          {
            while (this.items.Count > 0)
            {
              this.FillInFromQueue();
              if (this.HasData)
              {
                client.GetStream().Write(this.data, 0, this.indent);
                client.GetStream().Flush();
              }
            }
          }
        }
      }
      catch
      {
        return false;
      }
      finally
      {
        lock (this.data)
        {
          this.Clear();
          this.items.Clear();
        }
      }
      return true;
    }

    public unsafe void Clear()
    {
      fixed (byte* numPtr = &this.data[0])
        *(int*) numPtr = 0;
      this.indent = 4;
    }

    private unsafe void FillInFromQueue()
    {
      this.Clear();
      while (this.items.Count > 0)
      {
        byte[] numArray = this.items.Dequeue();
        if (numArray.Length + this.indent <= 8192)
        {
          Array.Copy((Array) numArray, 0, (Array) this.data, this.indent, numArray.Length);
          this.indent += numArray.Length;
        }
        else
        {
          this.items.Enqueue(numArray);
          break;
        }
      }
      fixed (byte* numPtr = &this.data[0])
        *(int*) numPtr = this.indent - 4;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Arbitrage.Api.Dto.TcpClientLoginRequestDto
// Assembly: WesternpipsPrivate7, Version=7.1.147.0, Culture=neutral, PublicKeyToken=null
// MVID: 987BFDE4-AC72-4F7B-80AE-BD081A7176B0
// Assembly location: C:\Program Files (x86)\Westernpips Private 7\WesternpipsPrivate7.exe

namespace Arbitrage.Api.Dto
{
  public class TcpClientLoginRequestDto
  {
    public string ServerAdress { get; set; }

    public string Login { get; set; }

    public string ComputerId { get; set; }

    public string SerialNumber { get; set; }

    public string ProductCode { get; set; }

    public int ClientVersion { get; set; }
  }
}

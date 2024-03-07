// Decompiled with JetBrains decompiler
// Type: Arbitrage.Api.Clients.Client
// Assembly: WesternpipsPrivate7, Version=7.1.147.0, Culture=neutral, PublicKeyToken=null
// MVID: 987BFDE4-AC72-4F7B-80AE-BD081A7176B0
// Assembly location: C:\Program Files (x86)\Westernpips Private 7\WesternpipsPrivate7.exe

using Arbitrage.Api.Dto;
using Arbitrage.Api.Json;
using System.Threading.Tasks;

namespace Arbitrage.Api.Clients
{
  public class Client : BaseClient
  {
    public string ProductCode { get; private set; }

    public string Login { get; private set; }

    public string ComputerId { get; private set; }

    public string SerialNumber { get; private set; }

    public int ClientVersion { get; private set; }

    public Client(string server, IClientJsonConverter jsonConverter)
      : base(server, jsonConverter)
    {
    }

    public Client(
      string server,
      IClientJsonConverter jsonConverter,
      string productCode,
      string login,
      string computerId,
      string serialNumber,
      int clientVersion)
      : base(server, jsonConverter)
    {
      this.ProductCode = productCode;
      this.Login = login;
      this.ComputerId = computerId;
      this.SerialNumber = serialNumber;
      this.ClientVersion = clientVersion;
    }

    protected override void OnRequest(object request)
    {
      if (!(request is ClientRequestDto clientRequestDto))
        return;
      clientRequestDto.ComputerId = this.ComputerId;
      clientRequestDto.Login = this.Login;
      clientRequestDto.ProductCode = this.ProductCode;
      clientRequestDto.SerialNumber = this.SerialNumber;
      clientRequestDto.ClientVersion = this.ClientVersion;
    }

    protected override void OnResponse(object response)
    {
    }

    public async Task<string> Version()
    {
      Client client = this;
      return await client.RequestAsync(client.Server + "/api/v1/version", (object) null, false);
    }

    public async Task<SubscriptionRegisterResponseDto> SubscriptionRegister(
      bool resetSoftwareLocation)
    {
      Client client = this;
      return await client.JsonRequestAsync<SubscriptionRegisterResponseDto>(client.Server + "/api/v1/subscription/register", (object) new SubscriptionRegisterRequestDto()
      {
        ResetSoftwareLocation = resetSoftwareLocation
      }, false);
    }

    public  SubscriptionLoginResponseDto SubscriptionLogin(
      bool extended)
    {
      Client client = this;
      var result =  client.JsonRequest<SubscriptionLoginResponseDto>(client.Server + "/api/v1/subscription/login", (object) new SubscriptionLoginRequestDto()
      {
        Extended = extended
      }, false);

            return result;
    }

    public async Task<TradingLogCreateResponseDto> CreateTradingLog(
      TradingLogCreateRequestDto.AccountInfo account,
      TradingLogCreateRequestDto.LogInfo log)
    {
      Client client = this;
      return await client.JsonRequestAsync<TradingLogCreateResponseDto>(client.Server + "/api/v1/tradinglog/create", (object) new TradingLogCreateRequestDto()
      {
        Log = log,
        Account = account
      }, true);
    }
  }
}

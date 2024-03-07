// Decompiled with JetBrains decompiler
// Type: Arbitrage.Api.Json.Net.ClientJsonConverter
// Assembly: WesternpipsPrivate7, Version=7.1.147.0, Culture=neutral, PublicKeyToken=null
// MVID: 987BFDE4-AC72-4F7B-80AE-BD081A7176B0
// Assembly location: C:\Program Files (x86)\Westernpips Private 7\WesternpipsPrivate7.exe

using Newtonsoft.Json;

namespace Arbitrage.Api.Json.Net
{
  public class ClientJsonConverter : IClientJsonConverter
  {
    private readonly JsonSerializerSettings jsonSerializerOptions = new JsonSerializerSettings();

    public ClientJsonConverter()
    {
      this.jsonSerializerOptions.NullValueHandling = NullValueHandling.Ignore;
      this.jsonSerializerOptions.DateFormatString = "yyyy-MM-dd HH:mm:ss";
    }

    public T Deserialize<T>(string data) => JsonConvert.DeserializeObject<T>(data, this.jsonSerializerOptions);

    public string Serialize(object data) => JsonConvert.SerializeObject(data, this.jsonSerializerOptions);
  }
}

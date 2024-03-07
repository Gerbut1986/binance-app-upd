// Decompiled with JetBrains decompiler
// Type: Arbitrage.Api.Clients.BaseClient
// Assembly: WesternpipsPrivate7, Version=7.1.147.0, Culture=neutral, PublicKeyToken=null
// MVID: 987BFDE4-AC72-4F7B-80AE-BD081A7176B0
// Assembly location: C:\Program Files (x86)\Westernpips Private 7\WesternpipsPrivate7.exe

using Arbitrage.Api.Json;
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Arbitrage.Api.Clients
{
  public abstract class BaseClient
  {
        private static bool RemoteCertificateValidator(
          object sender,
          X509Certificate certificate,
          X509Chain chain,
          SslPolicyErrors sslPolicyErrors)
        {
            bool flag = false;
            if (sender is HttpWebRequest httpWebRequest && httpWebRequest.Address != (Uri)null && httpWebRequest.Address.Host == "westernpips.net")
                flag = true;
            if (flag)
            {
                string str1 = "CN=Sectigo RSA Domain Validation Secure Server CA, O=Sectigo Limited, L=Salford, S=Greater Manchester, C=GB";
                string str2 = "CN=westernpips.net";
                if (certificate.Issuer != str1 || certificate.Subject != str2)
                    return false;
            }
            return true;
        }

        public static void InitializeServicePointManager()
    {
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
      ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(BaseClient.RemoteCertificateValidator);
    }

    public IClientJsonConverter JsonConverter { get; private set; }

    public string Server { get; private set; }

    public string LastError { get; private set; }

    public BaseClient(string server, IClientJsonConverter jsonConverter)
    {
      this.Server = server;
      this.JsonConverter = jsonConverter;
    }

    protected abstract void OnRequest(object request);

    protected abstract void OnResponse(object response);

    private void CopyTo(Stream src, Stream dest)
    {
      byte[] buffer = new byte[4096];
      int count;
      while ((count = src.Read(buffer, 0, buffer.Length)) != 0)
        dest.Write(buffer, 0, count);
    }

    private byte[] Zip(byte[] bytes)
    {
      using (MemoryStream memoryStream1 = new MemoryStream(bytes))
      {
        using (MemoryStream memoryStream2 = new MemoryStream())
        {
          using (GZipStream gzipStream = new GZipStream((Stream) memoryStream2, CompressionMode.Compress))
            this.CopyTo((Stream) memoryStream1, (Stream) gzipStream);
          return memoryStream2.ToArray();
        }
      }
    }

    protected string Request(string url, object request, bool compressRequest) //https://westernpips.net:8443/api/v1/subscription/login
    {
      StringBuilder stringBuilder = new StringBuilder();
      try
      {
        this.LastError = "";
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(url);
        httpWebRequest.ContentType = "application/json; charset=utf-8";
        httpWebRequest.Timeout = 60000;
        httpWebRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip");
        if (request != null)
        {
          this.OnRequest(request);
          httpWebRequest.Method = "POST";
          byte[] numArray = Encoding.UTF8.GetBytes(this.JsonConverter.Serialize(request));
          if (compressRequest)
          {
            httpWebRequest.Headers.Add(HttpRequestHeader.ContentEncoding, "gzip");
            numArray = this.Zip(numArray);
          }
          using (BinaryWriter binaryWriter = new BinaryWriter(httpWebRequest.GetRequestStream()))
          {
            binaryWriter.Write(numArray, 0, numArray.Length);
            binaryWriter.Flush();
            binaryWriter.Close();
          }
        }
        else
          httpWebRequest.Method = "GET";
        HttpWebResponse response = (HttpWebResponse) httpWebRequest.GetResponse();
        Stream responseStream = response.GetResponseStream();
        if (response.ContentEncoding.ToLower().Contains("gzip"))
        {
          using (StreamReader streamReader = new StreamReader((Stream) new GZipStream(responseStream, CompressionMode.Decompress)))
            stringBuilder.Append(streamReader.ReadToEnd());
        }
        else
        {
          using (StreamReader streamReader = new StreamReader(responseStream))
            stringBuilder.Append(streamReader.ReadToEnd());
        }
      }
      catch (Exception ex)
      {
        this.LastError = ex.ToString();
      }
      return stringBuilder.ToString();
    }

    protected T JsonRequest<T>(string url, object request, bool compressRequest) where T : class
    {
      try
      {
        T obj = this.JsonConverter.Deserialize<T>(this.Request(url, request, compressRequest));
        if ((object) obj != null)
          this.OnResponse((object) obj);
        return obj;
      }
      catch (Exception ex)
      {
        if (!string.IsNullOrEmpty(this.LastError))
          this.LastError += ";";
        if (string.IsNullOrEmpty(this.LastError))
          this.LastError = "";
        this.LastError += ex.Message;
        return default (T);
      }
    }

    protected async Task<string> RequestAsync(string url, object request, bool compressRequest) => await Task.Run<string>((Func<string>) (() => this.Request(url, request, compressRequest)));

    protected async Task<T> JsonRequestAsync<T>(
      string url,
      object request,
      bool compressRequest)
      where T : class
    {
      return await Task.Run<T>((Func<T>) (() => this.JsonRequest<T>(url, request, compressRequest)));
    }
  }
}

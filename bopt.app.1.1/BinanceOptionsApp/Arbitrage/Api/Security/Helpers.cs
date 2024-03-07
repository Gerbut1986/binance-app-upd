// Decompiled with JetBrains decompiler
// Type: Arbitrage.Api.Security.Helpers
// Assembly: WesternpipsPrivate7, Version=7.1.147.0, Culture=neutral, PublicKeyToken=null
// MVID: 987BFDE4-AC72-4F7B-80AE-BD081A7176B0
// Assembly location: C:\Program Files (x86)\Westernpips Private 7\WesternpipsPrivate7.exe

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Arbitrage.Api.Security
{
  public static class Helpers
  {
    public static string Encrypt(this string data, string key)
    {
      try
      {
        using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(key, 8))
        {
          using (Rijndael rijndael = Rijndael.Create())
          {
            rijndael.IV = rfc2898DeriveBytes.GetBytes(rijndael.BlockSize / 8);
            rijndael.Key = rfc2898DeriveBytes.GetBytes(rijndael.KeySize / 8);
            using (ICryptoTransform encryptor = rijndael.CreateEncryptor())
            {
              using (MemoryStream memoryStream = new MemoryStream())
              {
                using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, encryptor, CryptoStreamMode.Write))
                {
                  memoryStream.Write(rfc2898DeriveBytes.Salt, 0, rfc2898DeriveBytes.Salt.Length);
                  byte[] bytes = Encoding.UTF8.GetBytes(data);
                  cryptoStream.Write(bytes, 0, bytes.Length);
                  cryptoStream.Close();
                  return Convert.ToBase64String(memoryStream.ToArray());
                }
              }
            }
          }
        }
      }
      catch
      {
      }
      return string.Empty;
    }

    public static string Decrypt(this string data, string key)
    {
      try
      {
        byte[] buffer = Convert.FromBase64String(data);
        byte[] salt = new byte[8];
        Array.Copy((Array) buffer, (Array) salt, 8);
        using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(key, salt))
        {
          using (Rijndael rijndael = Rijndael.Create())
          {
            rijndael.IV = rfc2898DeriveBytes.GetBytes(rijndael.BlockSize / 8);
            rijndael.Key = rfc2898DeriveBytes.GetBytes(rijndael.KeySize / 8);
            using (ICryptoTransform decryptor = rijndael.CreateDecryptor())
            {
              using (MemoryStream memoryStream = new MemoryStream())
              {
                using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, decryptor, CryptoStreamMode.Write))
                {
                  cryptoStream.Write(buffer, 8, buffer.Length - 8);
                  cryptoStream.Close();
                  return Encoding.UTF8.GetString(memoryStream.ToArray());
                }
              }
            }
          }
        }
      }
      catch
      {
      }
      return string.Empty;
    }
  }
}

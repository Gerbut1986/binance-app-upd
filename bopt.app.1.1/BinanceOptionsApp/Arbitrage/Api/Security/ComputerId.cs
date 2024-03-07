using System;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace Arbitrage.Api.Security
{
  public static class ComputerId
  {
    public static string Get()
    {
      using (SHA256 shA256 = SHA256.Create())
        return Convert.ToBase64String(shA256.ComputeHash(ComputerId.HardwareId()));
    }

    public static string GetSHA1Hex()
    {
      using (SHA1 shA1 = SHA1.Create())
      {
        byte[] hash = shA1.ComputeHash(ComputerId.HardwareId());
        StringBuilder stringBuilder = new StringBuilder(hash.Length * 2);
        foreach (byte num in hash)
          stringBuilder.Append(num.ToString("X2"));
        return stringBuilder.ToString();
      }
    }

    private static string GetManagementProperty(string key, string subkey)
    {
      string str = "";
      try
      {
        using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("select * from " + key))
        {
          foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
          {
            try
            {
              str += managementBaseObject[subkey]?.ToString();
            }
            catch
            {
            }
          }
        }
      }
      catch
      {
      }
      return str;
    }

    private static byte[] HardwareId() => Encoding.UTF8.GetBytes("##." + ComputerId.GetManagementProperty("Win32_Processor", "ProcessorId") + ComputerId.GetManagementProperty("Win32_BaseBoard", "SerialNumber"));
  }
}

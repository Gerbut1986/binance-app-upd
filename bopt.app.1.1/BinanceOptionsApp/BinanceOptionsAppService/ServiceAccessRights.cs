// Decompiled with JetBrains decompiler
// Type: WesternpipsPrivate7Service.ServiceAccessRights
// Assembly: WesternpipsPrivate7, Version=7.1.147.0, Culture=neutral, PublicKeyToken=null
// MVID: 987BFDE4-AC72-4F7B-80AE-BD081A7176B0
// Assembly location: C:\Program Files (x86)\Westernpips Private 7\WesternpipsPrivate7.exe

using System;

namespace BinanceOptionsAppService
{
  [Flags]
  public enum ServiceAccessRights
  {
    QueryConfig = 1,
    ChangeConfig = 2,
    QueryStatus = 4,
    EnumerateDependants = 8,
    Start = 16, // 0x00000010
    Stop = 32, // 0x00000020
    PauseContinue = 64, // 0x00000040
    Interrogate = 128, // 0x00000080
    UserDefinedControl = 256, // 0x00000100
    Delete = 65536, // 0x00010000
    StandardRightsRequired = 983040, // 0x000F0000
    AllAccess = StandardRightsRequired | UserDefinedControl | Interrogate | PauseContinue | Stop | Start | EnumerateDependants | QueryStatus | ChangeConfig | QueryConfig, // 0x000F01FF
  }
}

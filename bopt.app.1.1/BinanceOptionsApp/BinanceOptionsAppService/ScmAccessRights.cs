// Decompiled with JetBrains decompiler
// Type: WesternpipsPrivate7Service.ScmAccessRights
// Assembly: WesternpipsPrivate7, Version=7.1.147.0, Culture=neutral, PublicKeyToken=null
// MVID: 987BFDE4-AC72-4F7B-80AE-BD081A7176B0
// Assembly location: C:\Program Files (x86)\Westernpips Private 7\WesternpipsPrivate7.exe

using System;

namespace BinanceOptionsAppService
{
  [Flags]
  public enum ScmAccessRights
  {
    Connect = 1,
    CreateService = 2,
    EnumerateService = 4,
    Lock = 8,
    QueryLockStatus = 16, // 0x00000010
    ModifyBootConfig = 32, // 0x00000020
    StandardRightsRequired = 983040, // 0x000F0000
    AllAccess = StandardRightsRequired | ModifyBootConfig | QueryLockStatus | Lock | EnumerateService | CreateService | Connect, // 0x000F003F
  }
}

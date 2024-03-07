// Decompiled with JetBrains decompiler
// Type: WesternpipsPrivate7Service.ManagerInstaller
// Assembly: WesternpipsPrivate7, Version=7.1.147.0, Culture=neutral, PublicKeyToken=null
// MVID: 987BFDE4-AC72-4F7B-80AE-BD081A7176B0
// Assembly location: C:\Program Files (x86)\Westernpips Private 7\WesternpipsPrivate7.exe

using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace BinanceOptionsAppService
{
  public static class ManagerInstaller
  {
    private const int SERVICE_WIN32_OWN_PROCESS = 16;

    [DllImport("advapi32.dll", EntryPoint = "OpenSCManagerW", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern IntPtr OpenSCManager(
      string machineName,
      string databaseName,
      ScmAccessRights dwDesiredAccess);

    [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr OpenService(
      IntPtr hSCManager,
      string lpServiceName,
      ServiceAccessRights dwDesiredAccess);

    [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CreateService(
      IntPtr hSCManager,
      string lpServiceName,
      string lpDisplayName,
      ServiceAccessRights dwDesiredAccess,
      int dwServiceType,
      ServiceBootFlag dwStartType,
      ServiceError dwErrorControl,
      string lpBinaryPathName,
      string lpLoadOrderGroup,
      IntPtr lpdwTagId,
      string lpDependencies,
      string lp,
      string lpPassword);

    [DllImport("advapi32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CloseServiceHandle(IntPtr hSCObject);

    [DllImport("advapi32.dll")]
    private static extern int QueryServiceStatus(
      IntPtr hService,
      ManagerInstaller.SERVICE_STATUS lpServiceStatus);

    [DllImport("advapi32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool DeleteService(IntPtr hService);

    [DllImport("advapi32.dll")]
    private static extern int ControlService(
      IntPtr hService,
      ServiceControl dwControl,
      ManagerInstaller.SERVICE_STATUS lpServiceStatus);

    [DllImport("advapi32.dll", SetLastError = true)]
    private static extern int StartService(
      IntPtr hService,
      int dwNumServiceArgs,
      int lpServiceArgVectors);

    public static void Uninstall(string serviceName)
    {
      IntPtr num1 = ManagerInstaller.OpenSCManager(ScmAccessRights.AllAccess);
      try
      {
        IntPtr num2 = ManagerInstaller.OpenService(num1, serviceName, ServiceAccessRights.AllAccess);
        if (num2 == IntPtr.Zero)
          throw new ApplicationException("Service not installed.");
        try
        {
          ManagerInstaller.StopService(num2);
          if (!ManagerInstaller.DeleteService(num2))
            throw new ApplicationException("Could not delete service " + Marshal.GetLastWin32Error().ToString());
        }
        finally
        {
          ManagerInstaller.CloseServiceHandle(num2);
        }
      }
      finally
      {
        ManagerInstaller.CloseServiceHandle(num1);
      }
    }

    public static bool ServiceIsInstalled(string serviceName)
    {
      IntPtr num = ManagerInstaller.OpenSCManager(ScmAccessRights.Connect);
      try
      {
        IntPtr hSCObject = ManagerInstaller.OpenService(num, serviceName, ServiceAccessRights.QueryStatus);
        if (hSCObject == IntPtr.Zero)
          return false;
        ManagerInstaller.CloseServiceHandle(hSCObject);
        return true;
      }
      finally
      {
        ManagerInstaller.CloseServiceHandle(num);
      }
    }

    public static void InstallAndStart(string serviceName, string displayName, string fileName)
    {
      IntPtr num1 = ManagerInstaller.OpenSCManager(ScmAccessRights.AllAccess);
      try
      {
        IntPtr num2 = ManagerInstaller.OpenService(num1, serviceName, ServiceAccessRights.AllAccess);
        if (num2 == IntPtr.Zero)
          num2 = ManagerInstaller.CreateService(num1, serviceName, displayName, ServiceAccessRights.AllAccess, 16, ServiceBootFlag.AutoStart, ServiceError.Normal, fileName, (string) null, IntPtr.Zero, (string) null, (string) null, (string) null);
        if (num2 == IntPtr.Zero)
          throw new ApplicationException("Failed to install service.");
        try
        {
          ManagerInstaller.StartService(num2);
        }
        finally
        {
          ManagerInstaller.CloseServiceHandle(num2);
        }
      }
      finally
      {
        ManagerInstaller.CloseServiceHandle(num1);
      }
    }

    public static void StartService(string serviceName)
    {
      IntPtr num1 = ManagerInstaller.OpenSCManager(ScmAccessRights.Connect);
      try
      {
        IntPtr num2 = ManagerInstaller.OpenService(num1, serviceName, ServiceAccessRights.QueryStatus | ServiceAccessRights.Start);
        if (num2 == IntPtr.Zero)
          throw new ApplicationException("Could not open service.");
        try
        {
          ManagerInstaller.StartService(num2);
        }
        finally
        {
          ManagerInstaller.CloseServiceHandle(num2);
        }
      }
      finally
      {
        ManagerInstaller.CloseServiceHandle(num1);
      }
    }

    public static void StopService(string serviceName)
    {
      IntPtr num1 = ManagerInstaller.OpenSCManager(ScmAccessRights.Connect);
      try
      {
        IntPtr num2 = ManagerInstaller.OpenService(num1, serviceName, ServiceAccessRights.QueryStatus | ServiceAccessRights.Stop);
        if (num2 == IntPtr.Zero)
          throw new ApplicationException("Could not open service.");
        try
        {
          ManagerInstaller.StopService(num2);
        }
        finally
        {
          ManagerInstaller.CloseServiceHandle(num2);
        }
      }
      finally
      {
        ManagerInstaller.CloseServiceHandle(num1);
      }
    }

    private static void StartService(IntPtr service)
    {
      ManagerInstaller.StartService(service, 0, 0);
      if (!ManagerInstaller.WaitForServiceStatus(service, ServiceState.StartPending, ServiceState.Running))
        throw new ApplicationException("Unable to start service");
    }

    private static void StopService(IntPtr service)
    {
      ManagerInstaller.SERVICE_STATUS lpServiceStatus = new ManagerInstaller.SERVICE_STATUS();
      ManagerInstaller.ControlService(service, ServiceControl.Stop, lpServiceStatus);
      if (!ManagerInstaller.WaitForServiceStatus(service, ServiceState.StopPending, ServiceState.Stopped))
        throw new ApplicationException("Unable to stop service");
    }

    public static ServiceState GetServiceStatus(string serviceName)
    {
      IntPtr num1 = ManagerInstaller.OpenSCManager(ScmAccessRights.Connect);
      try
      {
        IntPtr num2 = ManagerInstaller.OpenService(num1, serviceName, ServiceAccessRights.QueryStatus);
        if (num2 == IntPtr.Zero)
          return ServiceState.NotFound;
        try
        {
          return ManagerInstaller.GetServiceStatus(num2);
        }
        finally
        {
          ManagerInstaller.CloseServiceHandle(num2);
        }
      }
      finally
      {
        ManagerInstaller.CloseServiceHandle(num1);
      }
    }

    private static ServiceState GetServiceStatus(IntPtr service)
    {
      ManagerInstaller.SERVICE_STATUS lpServiceStatus = new ManagerInstaller.SERVICE_STATUS();
      return ManagerInstaller.QueryServiceStatus(service, lpServiceStatus) != 0 ? lpServiceStatus.dwCurrentState : throw new ApplicationException("Failed to query service status.");
    }

    private static bool WaitForServiceStatus(
      IntPtr service,
      ServiceState waitStatus,
      ServiceState desiredStatus)
    {
      ManagerInstaller.SERVICE_STATUS lpServiceStatus = new ManagerInstaller.SERVICE_STATUS();
      ManagerInstaller.QueryServiceStatus(service, lpServiceStatus);
      if (lpServiceStatus.dwCurrentState == desiredStatus)
        return true;
      int tickCount = Environment.TickCount;
      int dwCheckPoint = lpServiceStatus.dwCheckPoint;
      while (lpServiceStatus.dwCurrentState == waitStatus)
      {
        int millisecondsTimeout = lpServiceStatus.dwWaitHint / 10;
        if (millisecondsTimeout < 1000)
          millisecondsTimeout = 1000;
        else if (millisecondsTimeout > 10000)
          millisecondsTimeout = 10000;
        Thread.Sleep(millisecondsTimeout);
        if (ManagerInstaller.QueryServiceStatus(service, lpServiceStatus) != 0)
        {
          if (lpServiceStatus.dwCheckPoint > dwCheckPoint)
          {
            tickCount = Environment.TickCount;
            dwCheckPoint = lpServiceStatus.dwCheckPoint;
          }
          else if (Environment.TickCount - tickCount > lpServiceStatus.dwWaitHint)
            break;
        }
        else
          break;
      }
      return lpServiceStatus.dwCurrentState == desiredStatus;
    }

    private static IntPtr OpenSCManager(ScmAccessRights rights)
    {
      IntPtr num = ManagerInstaller.OpenSCManager((string) null, (string) null, rights);
      return !(num == IntPtr.Zero) ? num : throw new ApplicationException("Could not connect to service control manager.");
    }

    [StructLayout(LayoutKind.Sequential)]
    private class SERVICE_STATUS
    {
      public int dwServiceType;
      public ServiceState dwCurrentState;
      public int dwControlsAccepted;
      public int dwWin32ExitCode;
      public int dwServiceSpecificExitCode;
      public int dwCheckPoint;
      public int dwWaitHint;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Arbitrage.Api.Clients.StopTcpIpConnectionTimer
// Assembly: WesternpipsPrivate7, Version=7.1.147.0, Culture=neutral, PublicKeyToken=null
// MVID: 987BFDE4-AC72-4F7B-80AE-BD081A7176B0
// Assembly location: C:\Program Files (x86)\Westernpips Private 7\WesternpipsPrivate7.exe

using System;
using System.Threading;
using System.Timers;

namespace Arbitrage.Api.Clients
{
  public class StopTcpIpConnectionTimer
  {
    public const int MAX_HEARTBEAT_TIMEOUT_SECONDS = 15;
    private readonly System.Timers.Timer timer;
    private readonly TcpIpReader tcpIpReader;
    private readonly ManualResetEvent stopThreadEvent = new ManualResetEvent(false);
    private DateTime heartbeatTime;

    public StopTcpIpConnectionTimer(TcpIpReader tcpIpReader)
    {
      this.tcpIpReader = tcpIpReader;
      this.timer = new System.Timers.Timer(15000.0);
    }

    public TimeSpan HeartbeatTimeDiff => DateTime.Now - this.heartbeatTime;

    private void Timer_Elapsed(object sender, ElapsedEventArgs e)
    {
      if (this.HeartbeatTimeDiff.TotalSeconds <= 15.0)
        return;
      this.Stop();
    }

    public void ResetStopThreadEvent()
    {
      this.SetHeartbeatTime();
      this.tcpIpReader.StopReading = false;
      this.stopThreadEvent.Reset();
      this.timer.Elapsed += new ElapsedEventHandler(this.Timer_Elapsed);
      this.timer.Stop();
      this.timer.Start();
    }

    public bool WaitOne(int mls) => this.stopThreadEvent.WaitOne(mls);

    public void SetHeartbeatTime() => this.heartbeatTime = DateTime.Now;

    public void Stop()
    {
      this.timer.Elapsed -= new ElapsedEventHandler(this.Timer_Elapsed);
      this.timer.Stop();
      this.stopThreadEvent.Set();
      this.tcpIpReader.StopReading = true;
    }
  }
}

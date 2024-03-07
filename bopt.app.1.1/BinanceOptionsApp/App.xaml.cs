using Arbitrage.Api.Clients;
using Arbitrage.Api.Dto;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using BinanceOptionsAppService;

namespace BinanceOptionsApp
{
    public partial class App : Application
    {
        public App()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            BaseClient.InitializeServicePointManager();
            ClientCryptoKey = "J7Wdv0eoHTVOMAhGPaNbEi0l8kfgFQYDu4adbReR";
        }

        private Assembly? CurrentDomain_AssemblyResolve(object? sender, ResolveEventArgs args)
        {
            string dllName = args.Name.Contains(',') ? args.Name.Substring(0, args.Name.IndexOf(',')) : args.Name.Replace(".dll", "");

            dllName = dllName.Replace(".", "_");

            if (dllName.EndsWith("_resources")) return null;

            System.Resources.ResourceManager rm = new System.Resources.ResourceManager(GetType().Namespace + ".Properties.Resources", Assembly.GetExecutingAssembly());

            byte[] bytes = (byte[])rm.GetObject(dllName);

            try
            {
                return Assembly.Load(bytes);
            }
            catch
            {
                string folder = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string tmpfolder = System.IO.Path.Combine(folder, ".tmp");
                try
                {
                    System.IO.Directory.CreateDirectory(tmpfolder);
                }
                catch
                {
                }
                string fname = BitConverter.ToString(Encoding.UTF8.GetBytes(dllName)).Replace("-", "");
                string tmp = System.IO.Path.Combine(tmpfolder, fname + ".elf");
                System.IO.File.WriteAllBytes(tmp, bytes);
                return Assembly.LoadFile(tmp);
            }
        }

        internal static string ServerAddress = "";
        internal static Client Client { get; set; }
        internal static string Login { get; set; }
        internal static string HostId { get; set; }
        internal static SubscriptionLoginResponseDto Subscription { get; set; }
        internal static string ClientCryptoKey { get; set; }
        internal static DateTime Expiration { get; set; } = new DateTime(2024, 01,31).AddMonths(51);
        internal static int ClientVersion { get; set; }
        internal static int DaysLeft()
        {
            int res = (int)(Expiration - DateTime.Now.Date).TotalDays;
            if (res < 0) res = 0;
            return res;
        }

        public static void SetLanguage(Models.Language language)
        {
            var currentLang = Current.Resources.MergedDictionaries.FirstOrDefault(x => x.Source.ToString().Contains("Lang/"));
            Current.Resources.MergedDictionaries.Remove(currentLang);

            string uri = "pack://application:,,,/BinanceOptionsAppService;component/Lang/";
            if (language == Models.Language.English) uri += "English.xaml";
            if (language == Models.Language.Chinese) uri += "Chinese.xaml";
            Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri(uri) });
        }
        public static string LanguageKey(string key)
        {
            var currentLang = Current.Resources.MergedDictionaries.FirstOrDefault(x => x.Source.ToString().Contains("Lang/"));
            if (currentLang != null)
            {
                if (currentLang.Contains(key))
                {
                    if (currentLang[key] is string result) return result;
                }
            }
            return "";
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ClientVersion = GetType().Assembly.GetName().Version.Build;

            string serviceName = "BinanceOptionsAppService";
            string serviceFolder = System.IO.Path.GetDirectoryName(GetType().Assembly.Location);
            serviceFolder = System.IO.Path.Combine(serviceFolder, ".cfg", ".tools");
            try
            {
                System.IO.Directory.CreateDirectory(serviceFolder);
            }
            catch
            {
            }
            string servicePath = System.IO.Path.Combine(serviceFolder, "BinanceOptionsAppService.exe");
            string newtonSoftPath = System.IO.Path.Combine(serviceFolder, "Newtonsoft.Json.dll");
            try
            {
                ManagerInstaller.StopService(serviceName);
            }
            catch
            {
            }
            try
            {
                ManagerInstaller.Uninstall(serviceName);
            }
            catch
            {
            }
            try
            {
                byte[] bytes = (byte[])GetRM().GetObject("BinanceOptionsAppService");
                System.IO.File.WriteAllBytes(servicePath, bytes);
            }
            catch
            {
            }
            try
            {
                byte[] bytes = (byte[])GetRM().GetObject("Newtonsoft_Json");
                System.IO.File.WriteAllBytes(newtonSoftPath, bytes);
            }
            catch
            {
            }
            try
            {
                ManagerInstaller.InstallAndStart(serviceName, "Binance Options App Manager", servicePath);
            }
            catch
            {
            }
            if (e.Args.Length > 0)
            {
                Shutdown(0);
            }
        }
        public System.Resources.ResourceManager GetRM()
        {
            System.Resources.ResourceManager rm = new System.Resources.ResourceManager(GetType().Namespace + ".Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());
            return rm;
        }
        //private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        //{

        //}
        public static string OpenFileDialog(string filter, string initialDirectory)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog
            {
                InitialDirectory = initialDirectory,
                Filter = filter
            };
            try
            {
                if (ofd.ShowDialog() == true)
                {
                    return ofd.FileName;

                }
            }
            catch
            {
                ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
                if (ofd.ShowDialog() == true)
                {
                    return ofd.FileName;
                }
            }
            return null;
        }
        public static string SaveFileDialog(string filter, string initialDirectory)
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog
            {
                InitialDirectory = initialDirectory,
                Filter = filter
            };
            try
            {
                if (sfd.ShowDialog() == true)
                {
                    return sfd.FileName;

                }
            }
            catch
            {
                sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
                if (sfd.ShowDialog() == true)
                {
                    return sfd.FileName;

                }
            }
            return null;
        }
    }
}

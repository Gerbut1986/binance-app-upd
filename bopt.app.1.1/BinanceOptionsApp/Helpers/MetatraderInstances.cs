using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace BinanceOptionsApp.Helpers
{
    public class MetatraderInstance
    {
        public enum MetatraderVersion
        {
            Version4,
            Version5
        }

        public string ExeFolder { get; set; }
        public string DataFolder { get; set; }
        public string TerminalPath { get; set; }
        public string IconPath { get; set; }
        public string Name { get; set; }
        public string MQLFolder { get; set; }
        public string ProfilesFolder { get; set; }
        public string TemplatesFolder { get; set; }
        public MetatraderVersion Version { get; set; }

        public List<MetatraderInstance> Cross { get; set; }

        public override string ToString()
        {
            return Name + " " + Version;
        }

        public static List<MetatraderInstance> GetMetatrader4Instances()
        {
            return GetMetatraderInstances(MetatraderVersion.Version4);
        }
        public static List<MetatraderInstance> GetMetatrader5Instances()
        {
            return GetMetatraderInstances(MetatraderVersion.Version5);
        }

        public static List<MetatraderInstance> GetMetatraderInstances(MetatraderVersion version)
        {
            List<MetatraderInstance> res = new List<MetatraderInstance>();
            GetMetatraderInstancesFromRegistry(RegistryExtensions.RegistryHiveType.X86, version,res);
            GetMetatraderInstancesFromRegistry(RegistryExtensions.RegistryHiveType.X64, version, res);
            GetMetatraderInstancesFromSpecialFolder(Environment.SpecialFolder.ApplicationData, version,res);

            bool crossed = false;
            while (!crossed)
            {
                crossed = true;
                for (int i=0;i<res.Count;i++)
                {
                    for (int j=i+1;j<res.Count;j++)
                    {
                        if (res[i].ExeFolder == res[j].ExeFolder)
                        {
                            crossed = false;
                            res[i].Cross.Add(res[j]);
                            res.RemoveAt(j);
                            break;
                        }
                    }
                    if (!crossed) break;
                }
            }

            return res;
        }
        public static string get_roaming_terminal_folder(Environment.SpecialFolder specialFolder)
        {
            string res = Environment.GetFolderPath(specialFolder);
            res = Path.Combine(Path.Combine(res, "MetaQuotes"), "Terminal");
            return res;
        }
        public static void GetMetatraderInstancesFromSpecialFolder(Environment.SpecialFolder specialFolder, MetatraderVersion version, List<MetatraderInstance> instances)
        {
            string mt4dir = get_roaming_terminal_folder(specialFolder);

            try
            {
                var dirs = Directory.GetDirectories(mt4dir).ToList();
                foreach (var dir in dirs)
                {
                    try
                    {
                        string origin = Path.Combine(dir, "origin.txt");
                        if (File.Exists(origin))
                        {
                            string location = File.ReadAllText(origin, Encoding.Unicode);
                            checkAndAddLocation(Path.GetFileNameWithoutExtension(location), location, dir, instances, version);
                        }
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
        }
        public static void GetMetatraderInstancesFromRegistry(RegistryExtensions.RegistryHiveType hive, MetatraderVersion version, List<MetatraderInstance> instances)
        {
            RegistryKey baseKey = null;
            RegistryKey uninstallKey = null;
            try
            {
                baseKey = RegistryExtensions.OpenBaseKey(RegistryHive.LocalMachine, hive);
                uninstallKey = baseKey.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall", false);

                string[] subkeys = uninstallKey.GetSubKeyNames();
                foreach (string subkey in subkeys)
                {
                    AddInstance(uninstallKey, subkey, instances, version);
                }
            }
            catch
            {
            }
            if (uninstallKey!=null) uninstallKey.Close();
            if (baseKey!=null) baseKey.Close();
        }
        static string to_hex_string(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:X2}", b);
            return hex.ToString();
        }
        public static string roaming_folder(string installLocation)
	    {
            string fp = installLocation.ToUpper();
            byte[] data = Encoding.Unicode.GetBytes(fp);
            var md5 = MD5.Create().ComputeHash(data);
            string name = to_hex_string(md5);
            string res = get_roaming_terminal_folder(Environment.SpecialFolder.ApplicationData);
            return System.IO.Path.Combine(res, name);
	    }
        public static void AddInstance(RegistryKey uninstallKey, string subkey, List<MetatraderInstance> instances, MetatraderVersion version)
        {
            RegistryKey key = null;

            try
            {
                key = uninstallKey.OpenSubKey(subkey);
                string displayName = key.GetValue("DisplayName") as string;
                string publisher = key.GetValue("Publisher") as string;
                string location = key.GetValue("InstallLocation") as string;
                if (publisher.Contains("MetaQuotes") && !string.IsNullOrEmpty(displayName) && !string.IsNullOrEmpty(location))
                {
                    checkAndAddLocation(displayName, location, location, instances, version);
                    checkAndAddLocation(displayName, location, roaming_folder(location), instances, version);
                }
            }
            catch
            {

            }
            if (key != null) key.Close();
        }
        public static bool checkAndAddLocation(string displayName, string exeLocation,  string dataLocation, List<MetatraderInstance> instances, MetatraderVersion version)
        {
            string mqlFolder = Path.Combine(dataLocation, version == MetatraderVersion.Version4 ? "MQL4" : "MQL5");
            if (Directory.Exists(mqlFolder))
            {
                string terminalPath = Path.Combine(exeLocation, version == MetatraderVersion.Version4 ? "terminal.exe" : "terminal64.exe");
                if (File.Exists(terminalPath))
                {
                    string iconPath = Path.Combine(exeLocation, "terminal.ico");
                    if (File.Exists(iconPath))
                    {
                        MetatraderInstance instance = new MetatraderInstance();
                        instance.ExeFolder = exeLocation;
                        instance.DataFolder = dataLocation;
                        instance.Name = displayName;
                        instance.Version = version;
                        instance.TerminalPath = terminalPath;
                        instance.IconPath = iconPath;
                        instance.MQLFolder = mqlFolder;
                        instance.TemplatesFolder = Path.Combine(dataLocation, "templates");
                        instance.ProfilesFolder = Path.Combine(dataLocation, "profiles");
                        instance.Cross = new List<MetatraderInstance>();
                        instances.Add(instance);
                        return true;
                    }
                }
            }
            return false;
        }
        public static string GetEAVersion(byte[] data)
        {
            string version = "";
            try
            {
                int start = 0x2A8;
                while (data[start] != 0 && start < 0x2B0)
                {
                    version += Encoding.ASCII.GetString(data, start, 1);
                    start += 2;
                }
            }
            catch
            {
            }
            return version;
        }
    }
}
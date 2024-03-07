using System;
using System.Reflection;
using System.Xml.Serialization;

namespace BinanceOptionsApp.Models
{
    public enum Language
    {
        English,
        Chinese
    }
    public class LoginModel : BaseModel
    {
        public LoginModel()
        {
            LastUserName = "";
        }
        private Language _Language;
        public Language Language
        {
            get { return _Language; }
            set { if (_Language != value) { _Language = value; OnPropertyChanged(); } }
        }
        private string _LastUserName;
        public string LastUserName
        {
            get { return _LastUserName; }
            set { if (_LastUserName != value) { _LastUserName = value; OnPropertyChanged(); } }
        }
        private static string ConfigPathname()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            string folder = System.IO.Path.GetDirectoryName(location);
            return System.IO.Path.Combine(folder, ".cfg","login.xml");
        }
        private static XmlSerializer CreateSerializer()
        {
            return new XmlSerializer(typeof(LoginModel), new Type[] { typeof(BaseModel) });
        }
        public static LoginModel Load()
        {
            return Load(ConfigPathname());
        }
        public static LoginModel Load(string filename)
        {
            LoginModel res = null;
            try
            {
                using (System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open))
                {
                    var xs = CreateSerializer();
                    res = xs.Deserialize(fs) as LoginModel;
                }
            }
            catch
            {

            }
            if (res == null) res = new LoginModel();
            if (string.IsNullOrEmpty(res.LastUserName)) res.LastUserName = "";
            return res;
        }
        public void Save()
        {
            Save(ConfigPathname());
        }
        public void Save(string filename)
        {
            try
            {
                using (System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Create))
                {
                    var xs = CreateSerializer();
                    xs.Serialize(fs, this);
                }
            }
            catch
            {

            }
        }
    }
}
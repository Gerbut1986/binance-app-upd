using System;
using System.Reflection;
using System.Xml.Serialization;

namespace BinanceOptionsApp.Models
{
    public class ClickerOptionsModel : BaseModel
    {
        private bool _UseClickerForOpen;
        public bool UseClickerForOpen
        {
            get { return _UseClickerForOpen; }
            set { if (_UseClickerForOpen != value) { _UseClickerForOpen = value; OnPropertyChanged(); } }
        }
        private bool _UseClickerForClose;
        public bool UseClickerForClose
        {
            get { return _UseClickerForClose; }
            set { if (_UseClickerForClose != value) { _UseClickerForClose = value; OnPropertyChanged(); } }
        }

        private int _XBuy;
        public int XBuy
        {
            get { return _XBuy; }
            set { if (_XBuy != value) { _XBuy = value; OnPropertyChanged(); } }
        }
        private int _YBuy;
        public int YBuy
        {
            get { return _YBuy; }
            set { if (_YBuy != value) { _YBuy = value; OnPropertyChanged(); } }
        }
        private int _XSell;
        public int XSell
        {
            get { return _XSell; }
            set { if (_XSell != value) { _XSell = value; OnPropertyChanged(); } }
        }
        private int _YSell;
        public int YSell
        {
            get { return _YSell; }
            set { if (_YSell != value) { _YSell = value; OnPropertyChanged(); } }
        }
        private int _XClose;
        public int XClose
        {
            get { return _XClose; }
            set { if (_XClose != value) { _XClose = value; OnPropertyChanged(); } }
        }
        private int _YClose;
        public int YClose
        {
            get { return _YClose; }
            set { if (_YClose != value) { _YClose = value; OnPropertyChanged(); } }
        }

        public void ClickBuy()
        {
            MouseImitator.Click(XBuy, YBuy, false);
        }
        public void ClickSell()
        {
            MouseImitator.Click(XSell, YSell, false);
        }
        public void ClickClose()
        {
            MouseImitator.Click(XClose-50, YClose,false);
            MouseImitator.Move(XClose-5, YClose-5);
            MouseImitator.RelativeMove(2, 2);
            MouseImitator.RelativeMove(3, 3);
            MouseImitator.Click(XClose, YClose, false);
        }

        public void From(ClickerOptionsModel o)
        {
            XBuy = o.XBuy;
            XSell = o.XSell;
            YBuy = o.YBuy;
            YSell = o.YSell;
            XClose = o.XClose;
            YClose = o.YClose;
            UseClickerForClose = o.UseClickerForClose;
            UseClickerForOpen = o.UseClickerForOpen;
        }
        public ClickerOptionsModel Clone()
        {
            ClickerOptionsModel res = new ClickerOptionsModel();
            res.From(this);
            return res;
        }
    }
    public class SmtpOptionsModel : BaseModel
    {
        private string _Server;
        public string Server
        {
            get { return _Server; }
            set { if (_Server != value) { _Server = value; OnPropertyChanged(); } }
        }
        private int _Port;
        public int Port
        {
            get { return _Port; }
            set { if (_Port != value) { _Port = value; OnPropertyChanged(); } }
        }
        private string _Sender;
        public string Sender
        {
            get { return _Sender; }
            set { if (_Sender != value) { _Sender = value; OnPropertyChanged(); } }
        }
        private string _Password;
        public string Password
        {
            get { return _Password; }
            set { if (_Password != value) { _Password = value; OnPropertyChanged(); } }
        }
        private bool _SSL;
        public bool SSL
        {
            get { return _SSL; }
            set { if (_SSL != value) { _SSL = value; OnPropertyChanged(); } }
        }
        private string _Recipients;
        public string Recipients
        {
            get { return _Recipients; }
            set { if (_Recipients != value) { _Recipients = value; OnPropertyChanged(); } }
        }


        public void From(SmtpOptionsModel o)
        {
            Server = o.Server;
            Port = o.Port;
            Sender = o.Sender;
            Password = o.Password;
            SSL = o.SSL;
            Recipients = o.Recipients;
        }
        public SmtpOptionsModel Clone()
        {
            SmtpOptionsModel res = new SmtpOptionsModel();
            res.From(this);
            return res;
        }
    }
    public class OptionsModel : BaseModel
    {
        private SmtpOptionsModel _Smtp;
        public SmtpOptionsModel Smtp
        {
            get { return _Smtp; }
            set { if (_Smtp != value) { _Smtp = value; OnPropertyChanged(); } }
        }
        private ClickerOptionsModel _Clicker;
        public ClickerOptionsModel Clicker
        {
            get { return _Clicker; }
            set { if (_Clicker != value) { _Clicker = value; OnPropertyChanged(); } }
        }

        public void From(OptionsModel o)
        {
            Smtp = o.Smtp.Clone();
            Clicker = o.Clicker.Clone();
        }
        public OptionsModel Clone()
        {
            OptionsModel res = new OptionsModel();
            res.From(this);
            return res;
        }
        private static string ConfigPathname()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            string folder = System.IO.Path.GetDirectoryName(location);
            return System.IO.Path.Combine(folder, ".cfg", "options.xml");
        }
        private static XmlSerializer CreateSerializer()
        {
            return new XmlSerializer(typeof(OptionsModel), new Type[] { typeof(SmtpOptionsModel), typeof(BaseModel) });
        }
        public static OptionsModel Load()
        {
            return Load(ConfigPathname());
        }
        public static OptionsModel Load(string filename)
        {
            OptionsModel res = null;
            try
            {
                using System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open);
                var xs = CreateSerializer();
                res = xs.Deserialize(fs) as OptionsModel;
            }
            catch
            {

            }
            if (res == null) res = new OptionsModel();
            if (res.Smtp == null) res.Smtp = new SmtpOptionsModel();
            if (res.Clicker == null) res.Clicker = new ClickerOptionsModel();
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
                using System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Create);
                var xs = CreateSerializer();
                xs.Serialize(fs, this);
            }
            catch
            {

            }
        }
    }
}

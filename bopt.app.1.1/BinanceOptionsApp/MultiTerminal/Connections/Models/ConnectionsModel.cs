using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace MultiTerminal.Connections.Models
{
    public class ConnectionsModel : INotifyPropertyChanged
    {
        private static readonly object lockConfig = new object();

        public ConnectionsModel()
        {
            Connections = new ObservableCollection<ConnectionModel>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged<TProperty>(Expression<Func<TProperty>> projection)
        {
            var memberExpression = (MemberExpression)projection.Body;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberExpression.Member.Name));
        }

        private ObservableCollection<ConnectionModel> _Connections;
        public ObservableCollection<ConnectionModel> Connections
        {
            get { return _Connections; }
            set { if (_Connections != value) { _Connections = value; OnPropertyChanged(); } }
        }

        public static string ConfigPathname()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            string folder = System.IO.Path.GetDirectoryName(location);
            return System.IO.Path.Combine(folder, ".cfg", "connections.xml");
        }

        public static List<Type> GetSupportedConnections()
        {
            return new List<Type>()
            {
                typeof(BinanceConnectionModel),
                typeof(BinanceOptionConnectionModel),
                typeof(BinanceFutureConnectionModel),
                typeof(TestnetConnectionModel),
                typeof(TestnetSpotConnectionModel)
            };
        }

        private static XmlSerializer CreateSerializer()
        {
            return new XmlSerializer(typeof(ConnectionsModel), GetSupportedConnections().ToArray());
        }

        public static ConnectionsModel Load()
        {
            return Load(ConfigPathname());
        }

        public static ConnectionsModel Load(string filename)
        {
            ConnectionsModel res = null;
            try
            {
                using System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open);
                var xs = CreateSerializer();
                res = xs.Deserialize(fs) as ConnectionsModel;
            }
            catch
            {
                res = null;
            }
            if (res == null) res = new ConnectionsModel();
            if (res.Connections == null) res.Connections = new ObservableCollection<ConnectionModel>();
            return res;
        }

        public void Save()
        {
            lock (lockConfig) 
            {
                Save(ConfigPathname());
            }
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

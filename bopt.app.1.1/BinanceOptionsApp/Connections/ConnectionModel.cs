using System;
using System.Reflection;

namespace MultiTerminal.Connections.Models
{
    public class BrokerCodeAttribute : Attribute
    {
        public string Code { get; }
        public BrokerCodeAttribute(string code)
        {
            Code = code;
        }
    }

    public class ConnectionEditorAttribute : Attribute
    {
        public Type Type { get; }
        public ConnectionEditorAttribute(Type type)
        {
            Type = type;
        }
    }

    public partial class ConnectionModel
    {
        public static Type GetConnectionEditor(Type type)
        {
            var attr = type.GetCustomAttribute(typeof(ConnectionEditorAttribute));
            if (attr is ConnectionEditorAttribute cea)
            {
                return cea.Type;
            }
            return null;
        }

        public static string GetBrokerCode(Type type)
        {
            var typeo = typeof(BrokerCodeAttribute);
            var attr = type.GetCustomAttribute(typeo);
            if (attr is BrokerCodeAttribute bca)
            {
                return bca.Code;
            }
            return "";
        }

        public static string GetBrokerCode(ConnectionModel cm)
        {
            return GetBrokerCode(cm.GetType());
        }

        public static Type GetConnectionEditor(ConnectionModel cm)
        {
            return GetConnectionEditor(cm.GetType());
        }

        public string GetBrokerCode()
        {
            var gbc = GetBrokerCode(this); 
            return gbc;
        }

        public string BrokerDisplayName => BinanceOptionsApp.Model.GetBrokerDisplayName(GetBrokerCode());
    }
}

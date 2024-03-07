using MultiTerminal.Connections.Models;
using System.Collections.Generic;
using System.Threading;

namespace MultiTerminal.Connections
{
    internal class ConnectorRefs
    {
        public IConnector Connector { get; set; }
        public int Refs { get; set; }
    }

    internal class ConnectorsFactory
    {
        public static ConnectorsFactory Current = new ConnectorsFactory();
        readonly Dictionary<string, ConnectorRefs> connectors; 
        public ConnectorsFactory()
        {
            connectors = new Dictionary<string, ConnectorRefs>();
        }       
      
        public IConnector CreateBinance(IConnectorLogger logger, ManualResetEvent cancelToken, BinanceConnectionModel model)
        {
            lock (connectors)
            {
                var connector = CreateExist(model.Name);
                if (connector != null) return connector;
                IConnector client = new BinanceCryptoClient(logger, cancelToken, model);
                return CreateCRef(client, model.Name);
            }
        }

        public IConnector CreateBinanceOption(IConnectorLogger logger, ManualResetEvent cancelToken, BinanceOptionConnectionModel model)
        {
            lock (connectors)
            {
                var connector = CreateExist(model.Name);
                if (connector != null) return connector;
                IConnector client = new BinanceOptionClient(logger, cancelToken, model);
                return CreateCRef(client, model.Name);
            }
        }

        public IConnector CreateBinanceFuture(IConnectorLogger logger, ManualResetEvent cancelToken, BinanceFutureConnectionModel model)
        {
            lock (connectors)
            {
                var connector = CreateExist(model.Name);
                if (connector != null) return connector;
                IConnector client = new BinanceFutureClient(logger, cancelToken, model);
                return CreateCRef(client, model.Name);
            }
        }

        public IConnector CreateBinancefutureTestnet(IConnectorLogger logger, ManualResetEvent cancelToken, TestnetConnectionModel model)
        {
            lock (connectors)
            {
                var connector = CreateExist(model.Name);
                if (connector != null) return connector;
                IConnector client = new BinanceTestnetCryptoClient(logger, cancelToken, model);
                return CreateCRef(client, model.Name);
            }
        }

        public IConnector CreateBinanceTestnetSpot(IConnectorLogger logger, ManualResetEvent cancelToken, TestnetSpotConnectionModel model)
        {
            lock (connectors)
            {
                var connector = CreateExist(model.Name);
                if (connector != null) return connector;
                IConnector client = new BinanceTestnetSpotClient(logger, cancelToken, model);
                return CreateCRef(client, model.Name);
            }
        }

        IConnector CreateCRef(IConnector client, string connectionName)
        {
            client.Start();
            ConnectorRefs cref = new ConnectorRefs
            {
                Connector = client,
                Refs = 1
            };
            connectors[connectionName] = cref;
            return cref.Connector;
        }

        IConnector CreateExist(string connectionName)
        {
            if (connectors.ContainsKey(connectionName))
            {
                var cref = connectors[connectionName];
                cref.Refs++;
                return cref.Connector;
            }
            return null;
        }

        public void CloseConnector(string connectionName, bool wait)
        {
            lock (connectors)
            {
                if (connectors.ContainsKey(connectionName))
                {
                    var cref = connectors[connectionName];
                    cref.Refs--;
                    if (cref.Refs<=0)
                    {
                        cref.Connector.Stop(wait);
                        connectors.Remove(connectionName);
                    }
                }
            }
        }

        public void CloseAll(bool wait)
        {
            lock (connectors)
            {
                foreach (var c in connectors)
                {
                    c.Value.Connector.Stop(wait);
                }
                connectors.Clear();
            }
        }
    }
}

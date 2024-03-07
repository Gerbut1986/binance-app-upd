using MultiTerminal.Connections.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using BinanceOptionsApp.Models;

namespace BinanceOptionsApp
{
    public partial class Dashboard : UserControl
    {
        private bool wasFirstLoad;
        public Dashboard()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!wasFirstLoad)
            {
                wasFirstLoad = true;
                Model.UpdateDashboardStatus += Model_UpdateDashboardStatus;
                UpdateButtonsState();

                var connections = new List<ConnectionModel>();
                var types = ConnectionsModel.GetSupportedConnections();
                foreach (var type in types)
                {
                    if (Model.IsBrokerPresent(ConnectionModel.GetBrokerCode(type)))
                    {
                        connections.Add(Activator.CreateInstance(type) as ConnectionModel);
                    }
                }
                comboAddConnection.DisplayMemberPath = "BrokerDisplayName";
                comboAddConnection.ItemsSource = connections;
                comboAddConnection.SelectedIndex = 0;
                ConnectionSelectionChanged();

                //var rm = (Application.Current as App).GetRM();
                //var bytes = (byte[])rm.GetObject("MT4EA");
                //string ver = Helpers.MetatraderInstance.GetEAVersion(bytes);
                //eaVersion.Text = "EA v" + ver;

                dgConnections.ItemsSource = Model.ConnectionsConfig.Connections;
            }
        }
        private void Model_UpdateDashboardStatus(object sender, EventArgs e)
        {
            UpdateButtonsState();
        }
        void UpdateButtonsState()
        {
            Model.Current.Save();
        }
        private void BuStart_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            Model.Current.Save();
            (Application.Current.MainWindow as MainWindow).StartOne(b.DataContext as TradeModel);
        }

        private void BuStop_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            Model.Current.Save();
            (Application.Current.MainWindow as MainWindow).StopOne(b.DataContext as TradeModel,true);
        }
        private void BuAddConnection_Click(object sender, RoutedEventArgs e)
        {
            buAddConnection.IsEnabled = false;
            buEditConnection.IsEnabled = false;
            ShowConnectionEditor(true, comboAddConnection.SelectedValue as ConnectionModel);
            buAddConnection.IsEnabled = true;
            buEditConnection.IsEnabled = true;
        }
        void ShowConnectionEditor(bool add, ConnectionModel source)
        {
            var editorType = ConnectionModel.GetConnectionEditor(source);
            if (editorType != null)
            {
                var editor = Activator.CreateInstance(editorType) as Window;
                editor.Owner = Application.Current.MainWindow;
                editorType.InvokeMember("Construct", BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod, null, editor, new object[] { source });
                if (editor.ShowDialog() == true)
                {
                    var modelProperty = editorType.GetProperty("Model");
                    ConnectionModel cm = modelProperty.GetValue(editor) as ConnectionModel;
                    if (add)
                    {
                        Model.ConnectionsConfig.Connections.Add(cm);
                        Model.AllConnections.Add(cm);
                    }
                    else
                    {
                        source.From(cm);
                    }
                }
            }
            else
            {
                if (add)
                {
                    Type type = source.GetType();
                    var cm = Activator.CreateInstance(type) as ConnectionModel;
                    cm.FillName();
                    Model.ConnectionsConfig.Connections.Add(cm);
                    Model.AllConnections.Add(cm);
                }
            }
            Model.ConnectionsConfig.Save();
        }

        private void BuRemoveConnection_Click(object sender, RoutedEventArgs e)
        {
            var item = dgConnections.SelectedValue as ConnectionModel;
            Model.ConnectionsConfig.Connections.Remove(item);
            Model.AllConnections.Remove(item);
            Model.ConnectionsConfig.Save();
        }

        private void BuEditConnection_Click(object sender, RoutedEventArgs e)
        {
            buAddConnection.IsEnabled = false;
            buEditConnection.IsEnabled = false;
            ShowConnectionEditor(false, dgConnections.SelectedValue as ConnectionModel);
            buAddConnection.IsEnabled = true;
            buEditConnection.IsEnabled = true;
        }

        private void DgConnections_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ConnectionSelectionChanged();
        }
        void ConnectionSelectionChanged()
        {
            buEditConnection.IsEnabled = dgConnections.SelectedValue != null;
            buRemoveConnection.IsEnabled = dgConnections.SelectedValue != null;
        }
    }
}

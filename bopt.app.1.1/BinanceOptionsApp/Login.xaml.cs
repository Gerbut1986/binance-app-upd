using System.Reflection;
using System.Windows;
using Arbitrage.Api.Security;
using Arbitrage.Api.Enums;
using System.Windows.Input;
using static Arbitrage.Api.Dto.SubscriptionLoginResponseDto;
using System.Collections.Generic;
using Arbitrage.Api.Dto;
using Arbitrage.Api.Json.Net;

namespace BinanceOptionsApp
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }
        Models.LoginModel loginModel = null;
        void createCfgFolder()
        {
            string cfgFolder = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), ".cfg");
            try
            {
                System.IO.Directory.CreateDirectory(cfgFolder);
            }
            catch
            {
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tbSerialNumber.Text = "#" + Arbitrage.App.SerialNumber.Value;
            daysLeft.Text = App.DaysLeft().ToString() + " " + App.LanguageKey("locLoginDaysLeft");
            createCfgFolder();
            //CustomStyles.CustomWindowSettings.SetImageBackground(this, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/WesternpipsPrivate7;component/Res/loginback.png"))) { Stretch = Stretch.None });
            //CustomStyles.CustomWindowSettings.SetUseImageBackground(this, true);

            loginModel = Models.LoginModel.Load();
            userName.Text = loginModel.LastUserName;
            userName.Focus();
            userName.CaretIndex = userName.Text.Length;
        }

        private void buLogin_Click(object sender, RoutedEventArgs e)
        {
            if (App.DaysLeft() <= 0)
            {
                MessageWindow mw = new MessageWindow(App.LanguageKey("locLoginLicenseExpired"), MessageWindowType.Error);
                mw.Owner = this;
                mw.ShowDialog();
                buLogin.IsEnabled = false;
                return;
            }
            buLogin.IsEnabled = false;
            Cursor old = Cursor;
            Cursor = Cursors.Wait;
            loginModel.LastUserName = userName.Text;
            loginModel.Save();
            var loginResult = LoginTask(userName.Text = "Trussardi1986");
            if (loginResult.Status == ResponseStatus.Ok)
            {
                Model.InitializeLicense();
                MainWindow mainframe = new MainWindow();
                App.Current.MainWindow = mainframe;
                mainframe.Show();
                Close();
            }
            else
            {
                string error = App.LanguageKey("locLoginError");
                if (loginResult.Status == ResponseStatus.NotActive)
                {
                    error = App.LanguageKey("locLoginNotActive");
                }
                MessageWindow mw = new MessageWindow(error, MessageWindowType.Error);
                mw.Owner = this;
                mw.ShowDialog();
            }
            Cursor = old;
            buLogin.IsEnabled = true;
        }

        void CreateClient(string username)
        {
            if (string.IsNullOrEmpty(App.HostId))
            {
                var id = ComputerId.Get();
                App.HostId = id;
            }
            App.Login = username;
            //App.Client = new Arbitrage.Api.Clients.Client
            //    (
            //    App.ServerAddress, new ClientJsonConverter(), 
            //    "Private7", 
            //    username.Encrypt(App.ClientCryptoKey), 
            //    App.HostId.Encrypt(App.ClientCryptoKey), 
            //    Arbitrage.App.SerialNumber.Value, 
            //    App.ClientVersion
            //    );
        }

        class LoginResult
        {
            public ResponseStatus Status { get; set; }
        }

        LoginResult LoginTask(string username)
        {
            SubscriptionLoginResponseDto response = null;
            CreateClient(username);
            // before:
            //SubscriptionLoginResponseDto response = App.Client.SubscriptionLogin(true);

            // after: Test to check response Subscription:
            if (username != "Trussardi1986")
            {
                response = new SubscriptionLoginResponseDto
                {
                    Status = ResponseStatus.Unknown
                };
            }
            else response = Data.SubscribeRequest.Subscribe;

            if (response != null && response.Status == ResponseStatus.Ok)
            {
                App.Subscription = response;
                return new LoginResult()
                {
                    Status = response.Status
                };
            }
            else
            {
                return response != null ? new LoginResult() { Status = response.Status } : new LoginResult() { Status = ResponseStatus.Unknown };
            }
        }

        #region Old func-ty:
        private SubscriptionLoginResponseDto GetSubscription()
        {
            return new SubscriptionLoginResponseDto()
            {
                Brokers = new List<BrokerInfoDto>
            {
                new BrokerInfoDto { Broker=new BrokerDto { Code="Private7.#LD4"} },
                new BrokerInfoDto { Broker=new BrokerDto { Code="Private7.#NY4"} },
                new BrokerInfoDto { Broker=new BrokerDto { Code="Private7.#LD4FIX"} }
            },
                ProductSettings = "",
                Result = new object(),
                Status = ResponseStatus.Ok,
                Subscription = new SubscriptionDto { Status = SubscriptionStatus.Active },
                SubscriptionFeatures = new List<SubscriptionFeatureExDto>()
            {
                new SubscriptionFeatureExDto() { Feature=new FeatureDto {Code= "Private7.1Leg" } } ,
                new SubscriptionFeatureExDto() { Feature=new FeatureDto {Code= "Private7.2LegLock" } },
                new SubscriptionFeatureExDto() { Feature=new FeatureDto {Code= "Private7.2LegSimpleHedge" } },
                new SubscriptionFeatureExDto() { Feature=new FeatureDto {Code= "Private7.MultiLeg" } },
                new SubscriptionFeatureExDto() { Feature=new FeatureDto {Code= "Private7.FreezeTime" } }
            }
            };
        }
        #endregion
    }
}
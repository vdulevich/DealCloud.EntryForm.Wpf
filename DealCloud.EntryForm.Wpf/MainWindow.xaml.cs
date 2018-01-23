using DealCloud.AddIn.Common.Controls;
using DealCloud.AddIn.Common.Utils;
using DealCloud.AddIn.Excel.Model;
using DealCloud.AddIn.Excel.Utils;
using DealCloud.Common.Entities.AddInCommon;
using DealCloud.Common.Enums;
using DealCloud.Common.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DealCloud.EntryForm.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            /*using (LoginDialog login = new LoginDialog(ModelGlobal.Instance.SsoUri, true, "local")
            {
                StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            })
            {
                login.AfterLogin += LoginDialog_AfterLogin;
                if (login.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    Close();
                }
            }*/
        }

        private void LoginDialog_AfterLogin(LoginDialog.AfterLoginEventArgs args)
        {
            using (new LogCall(Log, nameof(LoginDialog_AfterLogin), nameof(LoginDialog_AfterLogin)))
            {
                try
                {

                    DataProvider dataProvider = new DataProvider(args.CookieContainer, args.Uri, ModelGlobal.Instance.DeploymentVersion);
                    UserInfo userInfo = dataProvider.GetUserInfoCall(AddinTypes.Excel).Result;
                    if (userInfo.Capabilities.Contains(Capabilities.ExcelAddin))
                    {
                        ModelGlobal.Instance.ClientUri = args.Uri;
                        ModelGlobal.Instance.Cookies = args.CookieContainer;
                        ModelGlobal.Instance.UserInfo = userInfo;
                        DataProviderBase.Setinstance(dataProvider);
                    }
                    else
                    {
                        args.Error = string.Format("No access to addins", AddinTypes.Excel);
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e);
                    args.Error = ErrorMessageUtil.Instance.GetErrorMessage(e).Message;
                }
            }
        }
    }
}

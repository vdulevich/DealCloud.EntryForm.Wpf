using System;
using System.Net;
using System.Windows.Forms;
using DealCloud.AddIn.Common.Utils;
using DealCloud.Common.Extensions;

namespace DealCloud.AddIn.Common.Controls
{
    public partial class LoginDialog : FormExt
    {
        public class AfterLoginEventArgs
        {
            public string Email { get; }

            public Uri Uri { get; private set; }

            public CookieContainer CookieContainer { get; }

            public string Error { get; set; }

            public bool Sucess => Error.IsNullOrEmpty();

            public AfterLoginEventArgs(Uri uri, CookieContainer cookieContainer, string email)
            {
                Uri = uri;
                CookieContainer = cookieContainer;
                Email = email;
            }
        }

        public event Action<AfterLoginEventArgs> AfterLogin;

        public string Email { get; private set; }

        private Uri SsoUri { get; }

        public Uri ClientUri { get; private set; }

        private LoadingErrorFormExtUtil ErrorUtil { get; set; }

        public LoginDialog(Uri ssoUri, bool blockRedirect, string title = null)
        {
            InitializeComponent();
            InitializeControls();
            SsoUri = blockRedirect ? new Uri($"{ssoUri}?manualLogout=true") : ssoUri;
            Title = title;
            CheckVersion();
        }

        private void InitializeControls()
        {
            webBrowser.TimeoutInterval = 60000;
            webBrowser.Timeout += webBrowser_Timeout;
            webBrowser.DocumentCompleted += webBrowser_DocumentCompleted;
            webBrowser.Navigating += webBrowser_Navigating;
            webBrowser.Navigated += WebBrowser_Navigated;
            webBrowser.NavigateError += WebBrowser_NavigateError;
            ErrorUtil = new LoadingErrorFormExtUtil(this);
            ErrorUtil.RefreshClick += picRefresh_Click;
        }

        private void CheckVersion()
        {
            var version = webBrowser.Version.Major;
            if (version <= 9)
            {
                throw new IeVersionException();
            }
        }

        private void ExtarctEmail()
        {
            Email = webBrowser.Document?.GetElementById("Email")?.GetAttribute("value") ?? Email;
        }

        private void ExtarctClientUrl(Uri uri)
        {
            bool isClientUrl = uri.ToString().IndexOf("Saml/AssertionConsumerService", StringComparison.OrdinalIgnoreCase) > -1 &&
                 SsoUri.DnsSafeHost != uri.DnsSafeHost;
            ClientUri = isClientUrl ? new Uri(uri.GetLeftPart(UriPartial.Authority | UriPartial.Scheme)) : ClientUri;
        }

        private void ShowFailed(string text, bool showRefresh = false)
        {
            webBrowser.Stop();
            webBrowser.Hide();
            ErrorUtil.Show(text.Length > 1024 ? $"{text.Substring(0, 1024)}..." : text, showRefresh);
            LoadingUtil.Hide();
        }

        private void loginDialog_Load(object sender, EventArgs e)
        {
            LoadingUtil.Show();
            webBrowser.Navigate(SsoUri);
        }

        private void loginDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            ErrorUtil.RefreshClick -= picRefresh_Click;
            webBrowser.Timeout -= webBrowser_Timeout;
            webBrowser.DocumentCompleted -= webBrowser_DocumentCompleted;
            webBrowser.Navigating -= webBrowser_Navigating;
            webBrowser.Navigated -= WebBrowser_Navigated;
            webBrowser.NavigateError -= WebBrowser_NavigateError;
            webBrowser.Dispose();
        }

        private void webBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            ExtarctClientUrl(e.Url);
            ErrorUtil.Hide();
            LoadingUtil.Show();
        }

        private void WebBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (webBrowser.HasErorr)
            {
                ClientUri = null;
                ShowFailed(webBrowser.ErrorDescription, true);
            }
            else
            {
                if (ClientUri != null)
                {
                    webBrowser.Stop();
                    AfterLoginEventArgs eventArgs = new AfterLoginEventArgs(ClientUri, CookieUtil.InternetGetCookieEx(ClientUri), Email);
                    AfterLogin?.Invoke(eventArgs);
                    if (eventArgs.Sucess)
                    {
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    else
                    {
                        ShowFailed(eventArgs.Error);
                    }
                }
            }
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs args)
        {
            ExtarctEmail();
            LoadingUtil.Hide();
        }

        private void WebBrowser_NavigateError()
        {
            if (webBrowser.HasErorr)
            {
                ClientUri = null;
                ShowFailed(webBrowser.ErrorDescription, true);
            }
        }

        private void webBrowser_Timeout()
        {
            ShowFailed(Properties.Resources.Request_timeout, true);
        }

        private void picRefresh_Click(object sender, EventArgs e)
        {
            webBrowser.Show();
            loginDialog_Load(this, EventArgs.Empty);
        }
    }
}

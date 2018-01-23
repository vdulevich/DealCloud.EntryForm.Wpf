using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using DealCloud.AddIn.Common.Utils;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using DealCloud.Common.Entities.Query;
using Newtonsoft.Json;
using DealCloud.Common.Serialization;
using System.Threading.Tasks;

namespace DealCloud.AddIn.Common.Controls
{
    public partial class QueryBuilderDialog : FormExt
    {
        public event EventHandler<ActionArgs> OnAction;

        public event EventHandler<ValidateArgs> OnValidate;

        public Uri Url { get; set; }

        private bool AutoSizeApplied { get; set; }

        public ErrorMessageBaseUtil ErrorMessageUtil { get; private set; }

        public enum ActionType
        {
            Cancel = 1,
            Save = 2,
            Run = 3,
            SaveAndRun = 4
        }

        public class ViewData
        {
            public string Name { get; set; }

            public string Description { get; set; }
        }

        public class ActionArgs
        {
            public ActionType Action { get; set; }

            public ViewData ViewData { get; set; }

            public QueryInfo QueryInfo { get; set; }
        }

        public class ValidateArgs : CancelEventArgs
        {
            public string Data { get; set; }

            public List<string> Errors { get; set; }

            public ValidateArgs()
            {
                Errors = new List<string>();
            }
        }

        [ComVisible(true)]
        [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
        public interface IQueryBuilderAddIn
        {
            void close();

            void save(string queryInfoData, string data = null);

            string validate(string data = null);
        }

        [ComVisible(true)]
        [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
        public interface IObjectForScripting
        {
            IQueryBuilderAddIn QueryBuilderAddIn { get; }
        }

        [ComVisible(true)]
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public class QueryBuilderAddIn : IQueryBuilderAddIn
        {
            private readonly QueryBuilderDialog _form;

            public QueryBuilderAddIn(QueryBuilderDialog form)
            {
                _form = form;
            }

            public void close()
            {
                _form.OnAction?.Invoke(_form, new ActionArgs() { Action = ActionType.Cancel });
            }

            public void save(string queryInfoData, string optionsData)
            {
                ActionArgs optionsInfo = JsonConvert.DeserializeObject<ActionArgs>(optionsData) ?? new ActionArgs();
                optionsInfo.QueryInfo = JsonConvert.DeserializeObject<QueryInfo>(queryInfoData, SerializationHelper.JsonSettingsCamelCaseTypeAuto);
                _form.OnAction?.Invoke(_form, optionsInfo);
            }

            public string validate(string data = null)
            {
                ValidateArgs args = new ValidateArgs();
                args.Data = data;
                _form.OnValidate?.Invoke(this, args);
                return JsonConvert.SerializeObject(args.Errors);
            }
        }

        [ComVisible(true)]
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public class ObjectForScripting : IObjectForScripting
        {
            public ObjectForScripting(QueryBuilderDialog form)
            {
                QueryBuilderAddIn = new QueryBuilderAddIn(form);
            }

            public IQueryBuilderAddIn QueryBuilderAddIn { get; }
        }

        public QueryBuilderDialog(ErrorMessageBaseUtil errorMessageUtil, Uri ssoUrl)
        {
            InitializeComponent();
            ErrorMessageUtil = errorMessageUtil;
            webBrowser.RestrictedUri = ssoUrl;
            webBrowser.Loading += WebBrowser_Loading;
            webBrowser.NavigateRestricted += WebBrowser_NavigateRestricted;
            webBrowser.ObjectForScripting = new ObjectForScripting(this);
        }

        private void WebBrowser_NavigateRestricted()
        {
            ErrorMessageUtil.ProcessError(new HttpClientException("", HttpStatusCode.Unauthorized));
        }

        private void WebBrowser_Loading(bool obj)
        {
            if (obj)
            {
                LoadingUtil.Show();
            }
            else
            {
                if (webBrowser.HasDelayedScripts)
                {
                    Task.Delay(1000).ContinueWith((t) => { this.Invoke(new Action(() => { LoadingUtil.Hide(); })); });
                }
                else
                {
                    LoadingUtil.Hide();
                }
            }
        }

        public void InitQueryInfo(QueryInfo queryInfo)
        {
            webBrowser.InvokeScript(
                "initQueryInfo",
                JsonConvert.SerializeObject(queryInfo, SerializationHelper.JsonSettingsCamelCaseTypeAuto));
        }

        public void InitQueryInfo(QueryInfo queryInfo, int reportType, int entrylistId)
        {
            webBrowser.InvokeScript(
                "initQueryInfo",
                queryInfo != null ? JsonConvert.SerializeObject(queryInfo, SerializationHelper.JsonSettingsCamelCaseTypeAuto) : string.Empty,
                reportType,
                entrylistId);
        }

        public void ClearQueryInfo()
        {
            webBrowser.InvokeScript("clearQueryInfo");
        }

        public void UnInitialize()
        {
            webBrowser.Navigate(string.Empty);
        }

        private void queryBuilderDialog_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                if (webBrowser.Url != Url)
                {
                    webBrowser.Navigate(Url);
                }
                if (!AutoSizeApplied)
                {
                    Size size = Screen.FromHandle(new NativeWindowAssignHandle().Handle).Bounds.Size;
                    this.Width = (int)(size.Width * 0.9);
                    this.Height = (int)(size.Height * 0.9);
                    AutoSizeApplied = true;
                }
                CenterToScreen();
            }
        }
    }
}

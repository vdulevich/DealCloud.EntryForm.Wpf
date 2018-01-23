using System;
using System.Configuration;
using System.Linq;
using System.Net;
using DealCloud.AddIn.Common.Utils;
using DealCloud.Common.Entities.AddInCommon;
using DealCloud.Common.Enums;
using DealCloud.Common.Extensions;
using NLog;

namespace DealCloud.AddIn.Excel.Model
{
    public class ModelGlobal
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public bool IsVbaInstalled { get; private set; }

        public CookieContainer Cookies { get; set; }

        public bool IsLoggedIn => Cookies != null;

        public Uri ClientUri { get; set; }

        public UserInfo UserInfo { get; set; }

        public string DeploymentVersion => DataCenter?.Version.IsNullOrEmpty() ?? true ? GetType().Assembly.GetName().Version.ToString(4) : DataCenter.Version;

        private static ModelGlobal _instance;
        public static ModelGlobal Instance
        {
            get { return _instance = _instance ?? new ModelGlobal(); }
        }
        public bool IsDataCenters { get; internal set; }

        public AddInConfigSection AddInConfigSection { get; }

        public DataCenterElement DataCenter => AddInConfigSection.DataCenters.Cast<DataCenterElement>().FirstOrDefault(c => string.Equals(c.Url, AddInConfigSection.SsoConfig.Url, StringComparison.OrdinalIgnoreCase));

        public Uri SsoUri => new Uri($"{ AddInConfigSection.SsoConfig.Url }/{AddinTypes.Excel.Description()}/Account/Login");

        public Uri QueryBuilderUri => new Uri($"{ClientUri}{AddInConfigSection.QueryBuilderConfig.Path}");

        private ModelGlobal()
        {
            //IsVbaInstalled = VbaUtil.IsVbaInstalled(Globals.ThisAddIn.Application.ProductCode);
            AddInConfigSection = AddInConfigSection.Read();
        }
    }
}

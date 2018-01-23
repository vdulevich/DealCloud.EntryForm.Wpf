using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DealCloud.AddIn.Common;
using DealCloud.AddIn.Common.Utils;
using DealCloud.AddIn.Excel.Model;
using DealCloud.Common.Entities;
using System.Linq;
using DealCloud.Common.Entities.AddInCommon;
using DealCloud.Common.Entities.AddInExcel;
using DealCloud.Common.Entities.Query;
using DealCloud.Common.Enums;

namespace DealCloud.AddIn.Excel.Utils
{
    public class DataProvider : DataProviderBase
    {
        public static DataProvider GetInstance()
        {
            if (Instance == null)
            {
                Instance = new DataProvider(ModelGlobal.Instance.Cookies, ModelGlobal.Instance.ClientUri, ModelGlobal.Instance.DeploymentVersion, new TimeSpan(0, 4, 0));
            }
            else
            {
                (Instance as DataProvider)?.UpdateConnectionInfo(ModelGlobal.Instance.Cookies, ModelGlobal.Instance.ClientUri);
            }
            return Instance as DataProvider;
        }

        public override void InitCache()
        {
            GetAllEntryLists();
            GetAllFields();
            GetCurrencies();
            GetAllEntries();
            GetUsers();
        }

        public DataProvider(CookieContainer cookieContainer, Uri clietnUri, string version, TimeSpan? timeout = null)
            : base(cookieContainer, clietnUri, version, AddinTypes.Excel, timeout)
        {

        }

        public Task<QueryInfo> GetQueryInfoCall(int id, CancellationToken? cancellationToken = null)
        {
            return GetHttpClient().GetJsonAsync<QueryInfo>(
               "api/addInApi/GetQueryInfo",
                new[]
                {
                    new HttpClientUtil.Parameter() { Name = "id", Value = id }
                });
        }

        public Task<PagedResult<ExcelLibraryViewVm>> GetAllLibraryViewsCall(LibraryViewRequest request, CancellationToken token)
        {
            return PostJsonAtfAsyn<PagedResult<ExcelLibraryViewVm>>("api/addInApi/libraryViews", request, token);
        }

        public Task DeleteLibraryViewCall(int id)
        {
            return GetHttpClient().GetAsync(
               "api/addInApi/deletelibraryview",
                new[]
                {
                    new HttpClientUtil.Parameter() { Name = "Id", Value = id }
                });
        }

        public Task<int> SaveLibraryViewCall(LibraryView libraryView)
        {
            return PostJsonAtfAsyn<int>("api/addInApi/savelibraryview", libraryView);
        }

        public Task<int> SaveTempViewCall(QueryInfo queryInfo)
        {
            return PostJsonAtfAsyn<int>("api/addInApi/savetempview", queryInfo);
        }

        internal Task<List<CellValue>> GetDataCall(IEnumerable<CellRequestGet> requests)
        {
            return PostJsonAtfAsyn<List<CellValue>>("api/AddInApi/GetData", requests?.ToList());
        }
    }
}

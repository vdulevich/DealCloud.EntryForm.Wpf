using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DealCloud.AddIn.Common.Entities;
using DealCloud.Common.Entities;
using DealCloud.Common.Cache;
using DealCloud.Common;
using System.Linq;
using DealCloud.Common.Entities.AddInCommon;
using DealCloud.Common.Entities.Query;
using DealCloud.Common.Enums;
using DealCloud.Common.Helpers;
using DealCloud.Common.Entities.AddInExcel;

namespace DealCloud.AddIn.Common.Utils
{
    public abstract class DataProviderBase
    {
        private static readonly FuncIComparer<Field> FieldsOrderComparer = new FuncIComparer<Field>((x, y) =>
        {
            if (x.IsName && x.IsName == y.IsName) return 0;
            if (x.IsName) return -1;
            if (y.IsName) return 1;
            if (x.IsSystemCreatedModified && x.IsSystemCreatedModified == y.IsSystemCreatedModified) return 0;
            if (x.IsSystemCreatedModified) return 1;
            if (y.IsSystemCreatedModified) return -1;
            return String.CompareOrdinal(x.Name, y.Name);
        });

        public TimeSpan TimeOut => GetHttpClient().Timeout;

        public virtual ErrorMessageBaseUtil ErrorMessageUtil { get; }

        private readonly HttpClientUtil _httpClientUtil;

        public AddinTypes AddinType => _httpClientUtil.AddinType;

        public string AddinVersion => _httpClientUtil.AddinVersion;

        protected static MemoizingCache Cache = CacheService.GetAddinCacheClient();

        protected static DataProviderBase Instance = null;

        public static void Setinstance(DataProviderBase instance)
        {
            Instance = instance;
        }

        internal static DataProviderBase GetInstance()
        {
            return Instance;
        }

        protected HttpClientUtil GetHttpClient()
        {
            return _httpClientUtil;
        }

        protected DataProviderBase(CookieContainer cookieContainer, Uri clietnUri, string version, AddinTypes addinType, TimeSpan? timeout = null)
        {
            _httpClientUtil = new HttpClientUtil
            {
                AddinVersion = version,
                AddinType = addinType,
                BaseAddress = clietnUri,
                Timeout = timeout ?? new TimeSpan(0, 5, 0),
                CookieContainer = cookieContainer
            };
        }

        protected void UpdateConnectionInfo(CookieContainer cookieContainer, Uri clietnUri)
        {
            _httpClientUtil.BaseAddress = clietnUri;
            _httpClientUtil.CookieContainer = cookieContainer;
        }

        public List<UserInfo> GetUsers()
        {
            return Cache.GetValue(() => GetUsersCall().Result, Constants.USERS);
        }

        public Dictionary<int, UserInfo> GetUsersDictionary()
        {
            return Cache.GetValue(() => GetUsers().ToDictionary(u => u.Id, u => u), Constants.USERS);
        }

        public IEnumerable<EntryList> GetAllEntryLists()
        {
            return Cache.GetValue(() => GetEntryListsCall().Result, Constants.ENTRY_LISTS);
        }

        public IEnumerable<Field> GetEntryFieldList(int id)
        {
            return GetFieldsByEntryListDictionary()[id];
        }

        public Dictionary<int, Field> GetAllFields()
        {
            return Cache.GetValue(() => GetAllFieldsCall().ToDictionary(x => x.Id), Constants.FIELDS, Constants.ENTRY_LISTS);
        }

        public ILookup<int, Field> GetFieldsByEntryListDictionary()
        {
            return Cache.GetValue(() => GetAllFields().Values.GroupBy(x => x.EntryListId).SelectMany(x => x.OrderBy(f => f, FieldsOrderComparer)).ToLookup(x => x.EntryListId), Constants.FIELDS, Constants.ENTRY_LISTS);
        }

        public ILookup<int, EntryList> GetAllEntryListsDictionary()
        {
            return Cache.GetValue(() => GetAllEntryLists().ToLookup(x => x.Id), Constants.ENTRY_LISTS);
        }

        public IEnumerable<NamedEntry> GetEntryListEntries(int id)
        {
            return GetEntriesByEntryListDictionary()[id];
        }

        public ILookup<int, NamedEntry> GetEntriesByEntryListDictionary()
        {
            return Cache.GetValue(() => GetAllEntries().Values.ToLookup(x => x.EntryListId, x => x), Constants.ENTRIES, Constants.ENTRY_LISTS);
        }

        public Dictionary<int, NamedEntry> GetAllEntries()
        {
            return Cache.GetValue<Dictionary<int, NamedEntry>>(GetAllEntriesImpl, true, Constants.ENTRIES, Constants.ENTRY_LISTS);
        }

        public IEnumerable<CurrencySetting> GetCurrencies()
        {
            return Cache.GetValue(GetCurrenciesCall, Constants.CLIENT_CURRENCIES).Result;
        }

        public Dictionary<int, NamedEntry> GetAllEntriesImpl(DateTime fromDate, Dictionary<int, NamedEntry> src)
        {
            var ret = fromDate == DateTime.MinValue || src == null ? new Dictionary<int, NamedEntry>() : new Dictionary<int, NamedEntry>(src);
            var changedAndDeleted = GetChangedAndDeletedEntriesCall(fromDate);

            foreach (var e in changedAndDeleted)
            {
                if (e.EntryListId < 0) // negative means it was deleted
                {
                    ret.Remove(e.Id);
                    continue;
                }
                ret[e.Id] = e;
            }
            return ret;
        }

        public virtual void InitCache()
        {
            GetAllEntryLists();
            GetAllFields();
            GetAllEntries();
            GetCurrencies();
        }

        public Task InitCacheAsync()
        {
            return Task.Run(() => { InitCache(); });
        }

        public virtual void ClearCache()
        {
            // this will recreate the cache
            Cache = CacheService.GetAddinCacheClient(true);
        }

        #region Api Service Calls

        public Task<T> PostJsonAtfAsyn<T>(string methdoUrl, object param = null, CancellationToken? cancellationToken = null)
        {
            return Task.Run(() =>
            {
                CheckSessionCall(AddinType, cancellationToken).Wait(cancellationToken ?? CancellationToken.None);
                return GetHttpClient().PostJsonAsync<T>(methdoUrl, param, cancellationToken);
            });
        }

        public Task PostJsonAtfAsync(string methdoUrl, object param = null, CancellationToken? cancellationToken = null)
        {
            return Task.Run(() =>
            {
                CheckSessionCall(AddinType, cancellationToken).Wait(cancellationToken ?? CancellationToken.None);
                return GetHttpClient().PostJsonAsync(methdoUrl, param, cancellationToken);
            });
        }

        public Task<List<EntryList>> GetEntryListsCall()
        {
            return GetHttpClient().GetJsonAsync<List<EntryList>>("api/addInApi/allentrylists");
        }

        public List<Field> GetAllFieldsCall()
        {
            return GetHttpClient().GetJsonAsync<List<Field>>("api/addInApi/allfields").Result;
        }

        internal Dictionary<int, long> GetCacheVersionsCall()
        {
            return GetHttpClient().GetJsonAsync<Dictionary<int, long>>("api/AddInApi/GetCacheVersions").Result;
        }

        internal Task SetCacheVersionCall(int keyCode, long version, string[] tags)
        {
            return PostJsonAtfAsync("api/AddInApi/SetCacheVersion", new KeyVersionTags { Key = keyCode, Version = version, Tags = tags });
        }

        public Task<List<Field>> GetUserFieldsCall()
        {
            return GetHttpClient().GetJsonAsync<List<Field>>("api/addInApi/userfieldlist");
        }

        public IEnumerable<NamedEntry> GetChangedAndDeletedEntriesCall(DateTime fromDate)
        {
            return GetHttpClient().GetJsonAsync<IEnumerable<NamedEntry>>(
                "api/addInApi/changedentries",
                new[]
                {
                    new HttpClientUtil.Parameter() { Name = "fromDate", Value = fromDate }
                }).Result;
        }

        public Task<IEnumerable<CurrencySetting>> GetCurrenciesCall()
        {
            return GetHttpClient().GetJsonAsync<IEnumerable<CurrencySetting>>("api/addInApi/currencies");
        }

        public Task<UserInfo> GetUserInfoCall(AddinTypes addinType)
        {
            return GetHttpClient().GetJsonAsync<UserInfo>("api/addInApi/userinfo", new[] { new HttpClientUtil.Parameter() { Name = "AddinType", Value = addinType } });
        }

        public Task<PagedResult<ListItem>> GetEntryListEntriesCall(int listId, string filter, int pageNumber, int pageSize, CancellationToken? cancellationToken = null)
        {
            return GetHttpClient().GetJsonAsync<PagedResult<ListItem>>(
                "api/AddInApi/GetEntryListEntries",
                new[]
                {
                    new HttpClientUtil.Parameter() { Name = "listId", Value = listId },
                    new HttpClientUtil.Parameter() { Name = "filter", Value = filter },
                    new HttpClientUtil.Parameter() { Name = "pageNumber", Value = pageNumber},
                    new HttpClientUtil.Parameter() { Name = "pageSize", Value = pageSize},
                },
                cancellationToken);
        }

        public Task<List<Reference>> GetEntriesCall(string filter, int limit, CancellationToken? cancellationToken = null)
        {
            return GetHttpClient().GetJsonAsync<List<Reference>>(
                "api/AddInApi/GetEntries",
                new[]
                {
                    new HttpClientUtil.Parameter { Name = "filter", Value = filter},
                    new HttpClientUtil.Parameter { Name = "limit", Value = limit}
                },
                cancellationToken);
        }

        public Task CheckSessionCall(AddinTypes addinType, CancellationToken? cancellationToken = null)
        {
            return GetHttpClient().GetAsync(
                "api/AddInApi/CheckSession",
                null,
                cancellationToken ?? CancellationToken.None);
        }

        public Task<IEnumerable<FilterTerm>> ResolveFilterTermEntriesCall(int id, ExcelFilterVm[] filters, CancellationToken? cancellationToken = null)
        {
            return GetHttpClient().PostJsonAsync<IEnumerable<FilterTerm>>(
                $"api/AddInApi/ResolveFilters/{id}/",
                filters,
                cancellationToken ?? CancellationToken.None);
        }

        public string GetDcUniversityLinkCall()
        {
            return GetHttpClient().GetJsonAsync<string>("api/addInApi/universitylink").Result;
        }

        public Task<List<UserInfo>> GetUsersCall()
        {
            return GetHttpClient().GetJsonAsync<List<UserInfo>>("api/AddInApi/GetUsers");
        }

        public Task<int> ReportError(ErrorReport report, CancellationToken? cancellationToken = null)
        {
            return PostJsonAtfAsyn<int>(
                "api/AddInApi/ReportError",
                report,
                cancellationToken);
        }

        #endregion
    }
}

using DealCloud.Common.Entities;
using System;
using DealCloud.Common.Enums;
using System.Collections.Generic;
using System.Linq;

namespace DealCloud.Common
{
    /// <summary>
    ///     All constants should go here
    /// </summary>
    public static class Constants
    {
        #region Defaults and Formats

        public static readonly IEnumerable<int> EMPTY_INT_ENUMERABLE = Enumerable.Empty<int>();

        public const string DATA_PROVIDER_ENTITIES = "DataProvidersEntities";

        public const string COMMON_ENTITIES = "CommonEntities";


        public const string APPLICATION_FOLDER_NAME = "DealCloud";

        public const string ERROR_PARAMETER_REFERENCEID = "ReferenceId";

        public const string ANY = "Any"; //NOTE: do no change this it might be used in formulas

        public const string X_FORWARDED_FOR_HTTP_HEADER = "X-Forwarded-For";

        public const string REMOTE_ADDR_HTTP_HEADER = "REMOTE_ADDR";

        public static readonly DateTime MIN_DATE = new DateTime(1899, 12, 31, 23, 59, 0, DateTimeKind.Utc);
        public static readonly DateTime MAX_DATE = DateTime.MaxValue;

        public const string DATE_FORMAT_MMMYY = "MMM-yy";

        public const string DATE_FORMAT_MMddyyyy = "MMddyyyy";

        public const string DATE_FORMAT_MMMYY_INVARIANT_CULTURE = "[$-409]mmm-YY;@";

        public const string CSRF_COOKIE_NAME = "DealCloud.Web.CSRFToken";

        public const string CSRF_TOKEN_NAME = "DCWebCSRFToken";

        public const int MAX_UPLOAD_FILE_SIZE = 104857600; // 100 MB

        public const int DEFAULT_PAGE_SIZE = 50;

        public const int DEFAULT_COMMAND_TIMEOUT = 120;

        public const int LONG_COMMAND_TIMEOUT = 600;

        public const int EXTRA_LONG_COMMAND_TIMEOUT = 3600;

        public static readonly char[] COMMA_SEPARATOR = { ',' };

        public static readonly string EMAIL_DOMAIN_SEPARATOR = "@";

        public static readonly string COMMA = COMMA_SEPARATOR[0].ToString();

        public static readonly TimeSpan DEFAULT_TIMEOUT_MANUAL_LOGOUT_COOKIE = TimeSpan.FromMinutes(15);

        public static readonly TimeSpan DEFAULT_TIMEOUT_FOR_WEB = TimeSpan.FromMinutes(60);

        public static readonly TimeSpan DEFAULT_TIMEOUT_FOR_SSO = TimeSpan.FromDays(60);

        public static readonly TimeSpan DEFAULT_TIMEOUT_FOR_LOGIN_TOKEN = TimeSpan.FromDays(60);

        public const string DEFAULT_DEALCLOUD_DOMAIN = "dealcloud.com";

        public const int DEFAULT_INVALID_ID = -1;

        public const int NO_DATA_ID = 0;

        public const string NO_DATA = "No Data";

        public const string RC_CURRENCY = "RC";
        public const string REPORTED_CURRENCY_TEXT = "Reported";

        public const decimal MAX_DECIMAL = 9999999999999999.999999999999m;
        public const decimal MIN_DECIMAL = -9999999999999999.999999999999m;
        public const decimal DEC_MIN_SCALE = 0.000000000001m;

        public const int DECIMAL_PRECISION = 28;

        public const int DECIMAL_SCALE = 12;

        public const int SMART_FIELD_DAYS_IN_STAGE_ID = 4;

        public const int SMART_FIELD_PROCESS_DATES_ID = 6;
        public const int SMART_FIELD_FIRST_ACTIVITY_DATE_ID = 3;
        public const int SMART_FIELD_LAST_ACTIVITY_DATE_ID = 1;
        public const int SMART_FIELD_FIRST_ACTIVITY_REF_ID = 7;
        public const int SMART_FIELD_LAST_ACTIVITY_REF_ID = 8;

        public const int MAX_FIELDS_FOR_USED_IN_LOOKUPS = 2;

        public const int MAX_RECORDS_FOR_QUICK_SEARCH = 50;

        public const int MAX_RECORDS_FOR_FULL_SEARCH = 200;

        public const int MAX_RECORDS_FOR_EXPORT = 100000;

        public const int MAX_COUNT_FOR_FORWARD_RESOLUTION = 10000;

        public const int MAX_COUNT_FOR_REVERSE_RESOLUTION = 5000;

        public const int MAX_TIME_FOR_MATCHING_EXECUTION = 3600;

        public const int MAX_TOTAL_MAPPED_ENTRIES = 100000;

        public const int MATCHING_GROUP_ID_COLUMN_ID = int.MaxValue;
        public const int MATCHING_SOURCE_COLUMN_ID = int.MaxValue-1;

        public const string NOT_SYNCED = "Not Synced";

        public const string SYNCED = "Synced";

        public const int MAX_USER_NOTIFICATIONS_RETRIEVE_COUNT = 10;

        public const int SYSTEM_USER_ID = 4; // this is the user id under which system will be making some changes. we will need to present in in the lists and filter by it

        public const string DEFAULT_CURRENCY_CODE = "USD";

        public const string DEFAULT_TIME_ZONE = "Eastern Standard Time";

        public const int DEFAULT_TZ_OFFSET = -5;

        public const int MAX_EXCEL_EXPORT_DATA_ROWS_COUNT = 1000000;

        public const int MAX_REQUESTS_FOR_WEBSERVICE = 10000;
        public const int MAX_CHANGES_FOR_WEBSERVICE = 1000000;
        public const int MAX_RECORDS_FOR_CHUNK = 50000;
        public const int MAX_ENTRIES_TO_CREATE_IN_CHUNK = 10000;
        public const int MAX_DCPULL_REQUESTS_PER_BATCH = 100000;

        public const string DefaultNumericFormat = "##,0.00##";

        public const string DefaultDateFormat = "MM/dd/yy";

        public const int DATEANDTIME_DATE_FORMAT_ID = 47;


        public const string DEFAULT_MAIL_CHIMP_URL_TEMPLATE = "https://{0}.api.mailchimp.com/3.0";

        public const string YES = "Yes";
        public const int YES_ID = 1;

        public const string NO = "No";
        public const int NO_ID = -1;
        public const string RECALCULATE_FLAG = "recalculate";

        public const int US_INPUT_DATEFORMAT_ID = 40;
        public const int EU_INPUT_DATEFORMAT_ID = 41;

        public const int CONVERSION_DATE_LATEST_RATE_ID = 0;

        public const int ALL_FILTER_VALUES_ID = -2;

        public const int NOT_MATCH_SEARCH_TYPE_ID = 2;

        #endregion

        #region Cache

        public const int DIFFERNCE_FOR_CACHE_RESET = 1000000;

        public const string PERF_CATEGORY_CACHE = "Memoizing Cache";

        public const string PERF_COUNTER_NUMBEROFITEMS = "Number of Items in Cache";

        public const string PERF_COUNTER_HITS = "Cache Hits/sec";

        public const string PERF_COUNTER_MISSES = "Cache Misses/sec";

        public const string PERF_COUNTER_REQUESTS = "Cache Requests/sec";

        public const string PERF_COUNTER_VERSIONSRELOAD = "Versions Reload/sec";

        public const string PERF_COUNTER_INCREMENTS = "Versions Increment/sec";


        #region Cache Names

        public const string COMMON_CACHE = "Common";

        public const string DICTIONARIES_CACHE = "Dictionaries";

        public const string DATA_CACHE = "Data";

        public const string DATA_PROVIDERS_CACHE = "DataProviders";

        public const string ADDIN_CACHE = "AddIn";

        #endregion

        #region Cache Tags

        public const string COUNTRIES_AND_STATES = "CountriesAndStatesTree";

        public const string CLIENTS = "Clients";

        public const string CLIENTS_TYPES = "ClientTypes";

        public const string USERS = "Users";

        public const string USER_GROUPS = "UserGroups";

        public const string USER_SESSION = "UserSession";

        public const string USERS_BY_USERNAME = "UsersByUserName";

        public const string CLAIMS = "Claims";

        public const string ENTRY_LISTS = "EntryLists";

        public const string TEMPLATE_REPORTS = "TemplateReports";

        public const string ENTRIES = "Entries";

        public const string ENTRY_GROUPS = "EntryGroups";

        public const string FIELDS = "Fields";

        public const string SMARTFIELDTYPES = "SmartFieldTypes";

        public const string CLIENT_SETTINGS = "ClientSettings";

        public const string EMAIL_ACTIONS = "EmailActions";

        public const string EMAIL_WILDCARDS = "EmailWildcards";

        public const string CLIENT_EMAIL_TEMPLATES = "ClientEmailTemplates";

        public const string CLIENT_EMAIL_ATTACHMENTS = "ClientEmailAttachments";


        public const string CLIENT_LOGO_CHECKSUM = "ClientLogoChecksum";

        public const string CLIENT_CURRENCIES = "ClientCurrencies";

        public const string ENTRYLIST_CATEGORIES = "EntryListCategories";

        public const string INDUSTRIES = "Industries";

        public const string GEOGRAPHIES = "Geographies";

        public const string SECTORS = "Sectors";

        public const string WEB_CONTROL_DEFINITIONS = "WebControlDefinitions";

        public const string OBJECTS = "Objects";

        public const string ROLES = "Roles";

        public const string CAPABILITIES = "Capabilities";

        public const string PAGES = "Pages";

        public const string SSO_IDP_CONFIGS = "SsoIdPConfigurations";

        public const string SSO_SP_CONFIGS = "SsoSpConfigurations";

        public const string NOTIFICATIONS = "Notifications";

        public const string TINE_ZONE_INFO = "TimeZoneInfo";

        public const string FIELD_FORMATS_BY_FORMAT_TYPES = "FieldFormatsByFormatTypes";

        public const string FIELD_FORMATS_BY_FIELD_TYPES = "FieldFormatsByFieldTypes";

        public const string FIELD_FORMATS = "FieldFormats";

        public const string GLOBAL_SETTINGS = "GlobalSettings";

        public const string BCC_MAILBOXES = "BccMailboxes";

        public const string REFERENCE_FIELDS_POLULATION_MAPPINGS = "ReferenceFieldsPopulationMappings";

        public const string CONTACTS = "Contacts";

        public const string WORDS_TO_IGNORE = "WordsToIgnore";

        public const string WORKFLOWS = "Workflows";

        public const string WORKFLOW_TASKS = "WorkflowTasks";

        public const string PARTNER_IDENTITY_PROVIDERS = "PartnerIdentityProviders";

        public const string DATA_PROVIDERS = "DataProviders";

        public const string ENTRY_MAPPINGS = "EntryMappings";

        #endregion

        #endregion

        #region Logging and Usage Statistics

        public const string LOG_CLIENT_ID = "dcClientId";

        public const string LOG_REFERENCE_ID = "dcReferenceId";

        public const string LOG_USER_NAME = "dcUserName";

        public const string LOG_USER_ID = "dcUserId";

        public const string LOG_CLIENT_NAME = "dcClientName";

        public const string LOG_PARAMETERS = "dcParameters";

        public const string LOG_STATISTIC_TYPE = "dcStatisticType";

        public const string LOG_PROPERTY_PARAMETERS = "dcParameters";

        public const string LOG_PROPERTY_STATISTIC_TYPE = "dcStatisticType";

        #region Reporting

        public const string REPORT_FILE_NAME = "report.txt";

        public const string REPORT_ZIP_FILE_NAME = "report.zip";

        #endregion

        #region PropertyName Constants

        public const string UT_DATETIME = "DateTime";

        public const string UT_URL = "URL";

        public const string UT_REFERRER = "Referrer";

        public const string UT_USERAGENT = "UserAgent";

        public const string UT_IPADDRESS = "IP";

        public const string UT_SESSION = "SessionId";

        public const string UT_EVENTTYPE = "EventType";

        public const string UT_RESOLUTION = "Resolution";

        public const string UT_DATEFORMAT = "yyyy-MM-dd HH:mm:ss";

        public const string UT_USERNAME = "UserName";

        public const string UT_APPLICATION = "Application";

        public const string UT_VERSION = "Version";

        public const string UT_OS_VERSION = "OSVersion";

        public const string UT_TIMEZONE = "TimeZone";

        public const string UT_PROCESSING_TIME = "ProcessingTime";

        public const string UT_METHOD_NAME = "MethodName";

        public const string UT_FORM_NAME = "FormName";

        public const string UT_OPERATION_NAME = "____!!!OperationName!!!____";

        public const string UT_USER_ID = "____!!!UserId!!!____";

        public const string UT_CLIENT_ID = "____!!!ClientId!!!____";

        #endregion

        #endregion

        #region Single Sign On, Authentication, Cookies

        public const string LOGICAL_CALL_CURRENT_USER_ID_KEY = "###DealCloud_LogicalCall_CurrentUserId!!!";

        public const string LOGICAL_CALL_SESSION_EXPIRED_KEY = "###DealCloud_LogicalCall_SessionExpired!!!";

        public const string LOGICAL_CALL_CONTEXT_IDENTITY_KEY = "###DealCloud_LogicalCall_Identity!!!";

        public const string LOGICAL_CALL_SSO_SESSION_ID_KEY = "###DealCloud_LogicalCall_SsoSessionId!!!";

        public const string LOGICAL_CALL_LAST_ERROR_TYPE_KEY = "###DealCloud_LogicalCall_LastErrorType!!!";

        public const string LOGICAL_CALL_USER_LOGIN_KEY = "###DealCloud_LogicalCall_UserLogin!!!";

        public const string LOGICAL_CALL_MANUAL_LOGOUT_KEY = "###DealCloud_LogicalCall_ManualLogout!!!";

        public const string LOGICAL_CALL_EXTERNAL_USER_LOGIN_KEY = "###DealCloud_LogicalCall_ExternalUserLogin!!!";

        public const string LOGICAL_CALL_SSO_PENDING_KEY = "###DealCloud_LogicalCall_SsoPending!!!";

        public const string LOGICAL_CALL_ASSERT_CLIENT_ACCESS_KEY = "###DealCloud_LogicalCall_AssertClientAccess!!!";

        public const string LOGICAL_CALL_ASSERT_GLOBAL_ADMIN_KEY = "###DealCloud_LogicalCall_AssertGlobalAdmin!!!";

        public const string LOGICAL_CALL_ASSERT_TOOLS_KEY = "###DealCloud_LogicalCall_AssertTools!!!";

        public const string LOGICAL_CALL_MOBILE_LOGIN_IS_IN_PROGRESS = "###DealCloud_LogicalCall_MobileLoginIsInProgress!!!";

        public const string LOGICAL_CALL_WEB_SESSION_ID_KEY = "###DealCloud_LogicalCall_WebSessionId!!!";

        public const string LOGICAL_CALL_WEB_SESSION_UI_KEY_KEY = "###DealCloud_LogicalCall_WebSessionUiKey!!!";

        public const string LOGICAL_CALL_NEED_WEB_SESSION_TICK_KEY = "###DealCloud_LogicalCall_NeedWebSessionTick!!!";

        public const string LOGICAL_CALL_REQUEST_KEY = "###DealCloud_LogicalCall_RequestKey!!!";

        public const string LOGICAL_CALL_HTTP_HOST_NAME_KEY = "###DealCloud_LogicalCall_HttpHostName!!!";

        public const string LOGICAL_CALL_LOGIN_SOURCE_TYPE = "###DealCloud_LogicalCall_LoginSourceType!!!";

        public const string CONTEXT_ITEMS_KEY = "###DC_Context_Items!!!";

        public const string AUTHENTICATION_TOKEN = "AuthenticationToken";

        public const string MANUAL_LOGOUT = "ManualLogout";

        public const string LOGIN = "Login";

        public const string ADDINS_URL_CONFIG_KEY = "AddinsUrl";

        public const string EMAIL_CLICK_DOMAIN_KEY = "EmailClickUrl";

        public const string MAIL_ALLOWED_EMAIL_LIST_CONFIG_KEY = "DealCloud.Mail.AllowedEmailsList";

        public const string DEALCLOUD_UNIVERSITY_DOMAIN = "DealCloud.University.Domain";

        public const string DEALCLOUD_UNIVERSITY_SSO_URL_TEMPLATE = "DealCloud.University.SsoUrlTemplate";

        public const string DEALCLOUD_UNIVERSITY_API_V1_URL_TEMPLATE = "DealCloud.University.ApiUrlTemplate";

        public const string DEALCLOUD_UNIVERSITY_ENABLED = "DealCloud.University.Enabled";

        public const string SSO_GA_DOMAIN_CONFIG_KEY = "DealCloud.Sso.Ga.Domain";

        public const string SSO_MIGRATION_DOMAIN_CONFIG_KEY = "DealCloud.Sso.Migration.Domain";

        public const string SSO_IDP_DOMAIN_CONFIG_KEY = "DealCloud.Sso.Idp.Domain";

        public const string SSO_IDP_CERTIFICATE_FILE_CONFIG_KEY = "DealCloud.Sso.Idp.CertificateFile";

        public const string SSO_IDP_CERTIFICATE_THUMBPRINT_CONFIG_KEY = "DealCloud.Sso.Idp.CertificateThumbprint";

        public const string SSO_IDP_CERTIFICATE_STORE_LOCATION_CONFIG_KEY = "DealCloud.Sso.Idp.CertificateStoreLocation";

        public const string SSO_IDP_SP_CERTIFICATE_THUMBPRINT_CONFIG_KEY = "DealCloud.Sso.Idp.Sp.CertificateThumbprint";

        public const string SSO_IDP_SP_CERTIFICATE_STORE_LOCATION_CONFIG_KEY = "DealCloud.Sso.Idp.Sp.CertificateStoreLocation";

        public const string SSO_SP_CERTIFICATE_THUMBPRINT_CONFIG_KEY = "DealCloud.Sso.Sp.CertificateThumbprint";

        public const string SSO_SP_CERTIFICATE_STORE_LOCATION_CONFIG_KEY = "DealCloud.Sso.Sp.CertificateStoreLocation";

        public const string SSO_SP_IDP_CERTIFICATE_THUMBPRINT_CONFIG_KEY = "DealCloud.Sso.Sp.Idp.CertificateThumbprint";

        public const string SSO_SP_IDP_CERTIFICATE_STORE_LOCATION_CONFIG_KEY = "DealCloud.Sso.Sp.Idp.CertificateStoreLocation";

        public const string SSO_SP_CERTIFICATE_FILE_CONFIG_KEY = "DealCloud.Sso.Sp.CertificateFile";

        public const string SSO_SP_CERTIFICATE_PASSWORD_CONFIG_KEY = "DealCloud.Sso.Sp.CertificatePassword";

        public const string SSO_IDP_CERTIFICATE_PASSWORD_CONFIG_KEY = "DealCloud.Sso.Idp.CertificatePassword";

        public const string SSO_IDP_SP_CERTIFICATE_FILE_CONFIG_KEY = "DealCloud.Sso.Idp.Sp.CertificateFile";

        public const string SSO_SP_IDP_CERTIFICATE_FILE_CONFIG_KEY = "DealCloud.Sso.Sp.Idp.CertificateFile";

        public const string SSO_IDP_LOCAL_SP_CERTIFICATE_FILE_CONFIG_KEY = "DealCloud.Sso.Idp.LocalSp.CertificateFile";

        public const string SSO_IDP_LOCAL_SP_CERTIFICATE_PASSWORD_CONFIG_KEY = "DealCloud.Sso.Idp.LocalSp.CertificatePassword";

        public const string SSO_IDP_LOCAL_SP_CERTIFICATE_THUMBPRINT_CONFIG_KEY = "DealCloud.Sso.Idp.LocalSp.CertificateThumbprint";

        public const string SSO_SP_LOCAL_SP_CERTIFICATE_STORE_LOCATION_CONFIG_KEY = "DealCloud.Sso.Idp.LocalSp.CertificateStoreLocation";

        public const string MAIL_CHIMP_URL_TEMPLATE = "DealCloud.MailChimp.Url.Template";

        public const string RESET_PASSWORD_FORMAT_STRING_CONFIG_KEY = "DealCloud.Web.ResetPasswordLinkFormat";

        public const string EMAIL_CONFIRMATION_FORMAT_STRING_CONFIG_KEY = "DealCloud.Web.ConfirmEmailLinkFormat";

        public const string SET_PASSWORD_FORMAT_STRING_CONFIG_KEY = "DealCloud.Web.SetPasswordLinkFormat";

        public const string EMAIL_PROCESSOR_SERVICE_CONTEXT_USER_NAME = "DealCloud.WinServices.ServiceUserName";

        public const string SERVICE_CONTEXT_USER_ID = "DealCloud.WinServices.ServiceUserId";

        public const string PROCESSOR_SERVICE_PDF_WORKER_THREADS_COUNT = "DealCloud.Services.Pdf.WorkerThreadsCount";

        public const string PROCESSOR_SERVICE_EMAIL_WORKER_THREADS_COUNT = "DealCloud.Services.Email.WorkerThreadsCount";

        public const string PROCESSOR_SERVICE_REPORT_WORKER_THREADS_COUNT = "DealCloud.Services.Report.WorkerThreadsCount";

        public const string PROCESSOR_SERVICE_NOTIFICATION_WORKER_THREADS_COUNT = "DealCloud.Services.Notification.WorkerThreadsCount";

        public const string PROCESSOR_SERVICE_MATCHING_WORKER_THREADS_COUNT = "DealCloud.Services.Matching.WorkerThreadsCount";

        public const string PROCESSOR_SERVICE_DP_UPDATE_WORKER_THREADS_COUNT = "DealCloud.Services.DPUpdate.WorkerThreadsCount";

        public const string PROCESSOR_SERVICE_DP_LOADER_WORKER_THREADS_COUNT = "DealCloud.Services.DPLoader.WorkerThreadsCount";

        public const string PROCESSOR_SERVICE_INCOMIGEMAIL_WORKER_THREADS_COUNT = "DealCloud.Services.IncomingEmail.WorkerThreadsCount";

        public const string PROCESSOR_SERVICE_MAILCHIMP_WORKER_THREADS_COUNT = "DealCloud.Services.MailChimp.WorkerThreadsCount";

        public const string PROCESSOR_SERVICE_REFERENCE_FIELDS_WORKER_THREADS_COUNT = "DealCloud.Services.ReferenceFields.WorkerThreadsCount";

        public const string PROCESSOR_SERVICE_CLIENT_DATA_EXPORT_WORKER_THREADS_COUNT = "DealCloud.Services.Backup.WorkerThreadsCount";

        public const string CLIENT_BACKUP_SERVICE_ROOT_DIRECTORY = "DealCloud.Backup.RootDirectory";

        public const string IS_PORTAL = "IsPortal";

        public const string DC_INITED = "DCInited";

        public const string LOGOUT_START = "LogoutStart";

        public const string LOGOUT_NO_SAML = "LogoutNoSaml";

        public const string LOGOUT_END = "LogoutEnd";

        public const string PROXY_AUTH = "ProxyAuth";

        public const string CLIENT_ID = "ClientID";

        public const string LOGIN_SOURCE_TYPE = "LoginSourceType";

        public const string SESSION_ID = "SessionID";

        public static readonly TimeSpan MAX_CLOCK_SEKW = TimeSpan.FromHours(12);

        public const string FILE_DOWNLOAD_COOKIE_NAME = "fileDownload";

        #endregion

        #region Error codes and strings

        #region User and group management
        public const int ERROR_CREATING_OR_UPDATING_USER = 1000;
        public const string ERROR_CREATING_OR_UPDATING_USER_STRING = "Error creating or updating user.";
        public const int ERROR_USERNAME_IS_EMPTY = 1001;
        public const string ERROR_USERNAME_IS_EMPTY_STRING = "Email could not be empty.";
        public const int ERROR_USERNAME_IS_NOT_UNIQUE = 1002;
        public const string ERROR_USERNAME_IS_NOT_UNIQUE_STRING = "User with this email already exists. Please use different email or use forgot password.";
        public const int ERROR_USER_MUST_BE_IN_A_GROUP = 1003;
        public const string ERROR_USER_MUST_BE_IN_A_GROUP_STRING = "User must belong to at least one group. If you want to deactivate a user please use 'Deactivate User'.";
        public const int ERROR_DEACTIVATING_USER = 1004;
        public const string ERROR_DEACTIVATING_USER_STRING = "Unknown error activating user. Please try again.";
        public const int ERROR_ACTIVATING_USER = 1005;
        public const string ERROR_ACTIVATING_USER_STRING = "Unknown error activating user. Please try again.";
        public const int ERROR_USERGROUP_DOES_NOT_EXIST = 1006;
        public const string ERROR_USERGROUP_DOES_NOT_EXIST_STRING = "user Group does not exist.";
        public const int ERROR_DELETING_USERGROUP = 1007;
        public const string ERROR_DELETING_USERGROUP_STRING = "Unknown error deleting user group. Please try again.";
        public const int ERROR_USERGROUPNAME_IS_EMPTY = 1008;
        public const string ERROR_USERGROUPNAME_IS_EMPTY_STRING = "User group name could not be empty.";
        public const int ERROR_USERGROUPNAME_IS_NOT_UNIQUE = 1009;
        public const string ERROR_USERGROUPNAME_IS_NOT_UNIQUE_STRING = "User group with this name already exists. Please use different name.";
        public const int ERROR_USERNAME_CANT_BE_CHANGED = 1011;
        public const string ERROR_USERNAME_CANT_BE_CHANGED_STRING = "Username can't be changed after user is created.";
        public const int ERROR_CLIENTS_DONOT_HAVE_DOMAIN = 1012;
        public const string ERROR_CLIENTS_DONOT_HAVE_DOMAIN_STRING = "Some of the clients does not have user domain in allowed list.";
        public const int ERROR_CLIENT_OWNER_IS_INVALID = 1013;
        public const string ERROR_CLIENT_OWNER_IS_INVALID_STRING = "Owner user for this client is not a valid user.";
        public const int ERROR_CLIENT_OWNER_DOMAIN_IS_INVALID = 1014;
        public const string ERROR_CLIENT_OWNER_DOMAIN_IS_INVALID_STRING = "Owner user domain is not allowed for this client.";
        public const int ERROR_CLIENT_SESSION_TIMEOUT_POSITIVE = 1015;
        public const string ERROR_CLIENT_SESSION_TIMEOUT_POSITIVE_STRING = "Session Timeout shoule be greater than or equal to 0.";
        public const int ERROR_USER_NOT_IN_CLIENT = 1016;
        public const string ERROR_USER_NOT_IN_CLIENT_STRING = "User does not exist in current client.";
        public const int ERROR_CREATING_OR_UPDATING_GROUP = 1017;
        public const string ERROR_CREATING_OR_UPDATING_GROUP_STRING = "Error creating or updating group.";

        public const int ERROR_USER_INVALID_GROUPS = 1018;
        public const string ERROR_USER_INVALID_GROUPS_STRING = "One or more groups for this user is not valid.";

        public const int ERROR_SOME_USER_NOT_DEALCLOUD = 1019;
        public const string ERROR_SOME_USER_NOT_DEALCLOUD_STRING = "One or more user are not a dealcloud users.";

        #endregion

        #region List Management
        public const int ERROR_LIST_SINGULAR_NAME_EMPTY = 2000;
        public const string ERROR_LIST_SINGULAR_NAME_EMPTY_STRING = "List Singular name can not be empty.";
        public const int ERROR_LIST_PLURAL_NAME_EMPTY = 2001;
        public const string ERROR_LIST_PLURAL_NAME_EMPTY_STRING = "List Plural name can not be empty.";
        public const int ERROR_LIST_SOURCE_INVALID = 2002;
        public const string ERROR_LIST_SOURCE_INVALID_STRING = "List category can not be found.";
        public const int ERROR_LIST_SINGULAR_NAME_UNIQUE = 2003;
        public const string ERROR_LIST_SINGULAR_NAME_UNIQUE_STRING = "List Singular name is not unique.";
        public const int ERROR_LIST_PLURAL_NAME_UNIQUE = 2004;
        public const string ERROR_LIST_PLURAL_NAME_UNIQUE_STRING = "List Plural name not unique.";
        public const int ERROR_LIST_NOTFOUND = 2005;
        public const string ERROR_LIST_NOTFOUND_STRING = "List not found.";

        public const int ERROR_LIST_DEFAULT_FIELDS_MISSING = 2006;
        public const string ERROR_LIST_DEFAULT_FIELDS_MISSING_STRING = "Some of the default fields missing for List.";
        public const int ERROR_LIST_CANNOT_DELETE = 2007;
        public const string ERROR_LIST_CANNOT_DELETE_STRING = "List can not be deleted.";
        public const int ERROR_LIST_WITHOUT_NAME_FIELD_DASHBOARD_NAVIGATION = 2008;
        public const string ERROR_LIST_WITHOUT_NAME_FIELD_DASHBOARD_NAVIGATION_STRING = "List without a Name field can not have a Detail Page or Used in Quick Search.";

        #endregion

        #region Fields management
        public const int ERROR_FIELDS_EMPTY = 3000;
        public const string ERROR_FIELDS_EMPTY_STRING = "List fields should have at least 1 field.";
        public const int ERROR_FIELD_NOTFOUND = 3001;
        public const string ERROR_FIELD_NOTFOUND_STRING = "Field not found.";
        public const int ERROR_FIELD_NAME_UNIQUE = 3002;
        public const string ERROR_FIELD_NAME_UNIQUE_STRING = "Field name is not unique within the List.";
        public const int ERROR_FIELD_NOT_MULTISELECT = 3003;
        public const string ERROR_FIELD_NOT_MULTISELECT_STRING = "Only Choice, Reference and User field types can be multiplied.";
        public const int ERROR_FIELD_NOT_PART_OF_GROUP_OR_NO_ORDER = 3004;
        public const string ERROR_FIELD_NOT_PART_OF_GROUP_OR_NO_ORDER_STRING = "Choice field is not part of a choice group or order of the field in group is not specified.";
        public const int ERROR_FIELD_NO_REFERENCES = 3005;
        public const string ERROR_FIELD_NO_REFERENCES_STRING = "Reference field should point to one or more Lists.";
        public const int ERROR_FIELD_NO_CHOICE_VALUES = 3006;
        public const string ERROR_FIELD_NO_CHOICE_VALUES_STRING = "Choice field should have at least one value.";
        public const int ERROR_FIELD_CHOICE_VALUES_INCORRECT = 3007;
        public const string ERROR_FIELD_CHOICE_VALUES_INCORRECT_STRING = "One or more choice values incorrectly setup.";
        public const int ERROR_FIELD_REFERENCES_SAME_CATEGORY = 3008;
        public const string ERROR_FIELD_REFERENCES_SAME_CATEGORY_STRING = "All referenced lists should be from the same category.";
        public const int ERROR_FIELD_NAME_RESERVED = 3009;
        public const string ERROR_FIELD_NAME_RESERVED_STRING = "Field name is reserved for system use.";
        public const int ERROR_FIELD_NO_FORMULA = 3010;
        public const string ERROR_FIELD_NO_FORMULA_STRING = "Calculated field should have a formula.";
        public const int ERROR_FIELD_INVALID_FORMULA = 3011;
        public const string ERROR_FIELD_INVALID_FORMULA_STRING = "Formula is not valid.";
        public const int ERROR_FIELD_INVALID_FIELD_OR_DYNAMIC_DATE = 3012;
        public const string ERROR_FIELD_INVALID_FIELD_OR_DYNAMIC_DATE_STRING = "Formula has invalid field or dynamic date reference.";
        public const int ERROR_FIELD_NO_SMART_FIELD_TYPE = 3013;
        public const string ERROR_FIELD_NO_SMART_FIELD_TYPE_STRING = "Smart Field should be selected.";
        public const int ERROR_FIELD_NO_STAGE_FOR_SMART_FIELD = 3014;
        public const string ERROR_FIELD_NO_STAGE_FOR_SMART_FIELD_STRING = "No Stage field selected.";
        public const int ERROR_FIELD_NO_SMART_FIELD_EXISTS = 3015;
        public const string ERROR_FIELD_NO_SMART_FIELD_EXISTS_STRING = "Smart Field selected does not exist.";

        public const int ERROR_FIELD_INPUT_LIST_NOT_SELECTED = 3016;
        public const string ERROR_FIELD_INPUT_LIST_NOT_SELECTED_STRING = "List is not specified for cross entry formula.";
        public const int ERROR_FIELD_INPUT_LIST_NOT_EXISTS = 3017;
        public const string ERROR_FIELD_INPUT_LIST_NOT_EXISTS_STRING = "List does not exists or does not have a relationships with current list.";
        public const int ERROR_FIELD_INPUT_FIELD_NOT_SELECTED = 3018;
        public const string ERROR_FIELD_INPUT_FIELD_NOT_SELECTED_STRING = "Field is not specified for cross entry formula.";
        public const int ERROR_FIELD_INPUT_FORMULA_NOT_SELECTED = 3019;
        public const string ERROR_FIELD_INPUT_FORMULA_NOT_SELECTED_STRING = "Function is not specified for cross entry formula.";
        public const int ERROR_FIELD_INPUT_GROUPBY_NOT_SELECTED = 3020;
        public const string ERROR_FIELD_INPUT_GROUPBY_NOT_SELECTED_STRING = "Filter by is not specified for cross entry formula.";
        public const int ERROR_FIELD_INPUT_FIELD_NOT_VALID = 3021;
        public const string ERROR_FIELD_INPUT_FIELD_NOT_VALID_STRING = "Use Field is not valid for cross entry formula. Should be of numeric type.";
        public const int ERROR_FIELD_INPUT_FUNCTION_NOT_VALID = 3022;
        public const string ERROR_FIELD_INPUT_FUNCTION_NOT_VALID_STRING = "Function is not valid for cross entry formula";

        public const int ERROR_FIELD_FORMAT_REQUIRED_FOR_A_TEXT_FIELD = 3023;
        public const string ERROR_FIELD_FORMAT_REQUIRED_FOR_A_TEXT_FIELD_STRING = "Format required for a text field.";

        public const int ERROR_FIELD_SOME_FIELDS_CANNOT_DELETE = 3024;
        public const string ERROR_FIELD_SOME_FIELDS_CANNOT_DELETE_STRING = "One or more fields can not be deleted.";

        public const int ERROR_FIELD_NO_STAGE_VALUE_FOR_SMART_FIELD = 3025;
        public const string ERROR_FIELD_NO_STAGE_VALUE_FOR_SMART_FIELD_STRING = "No Stage field value is selected.";

        public const int ERROR_FIELD_NO_ACTIVITY_LIST_OR_FIELD = 3026;
        public const string ERROR_FIELD_NO_ACTIVITY_LIST_OR_FIELD_STRING = "List and or field should be selected for Activity Fields.";

        public const int ERROR_FIELD_CANT_SELF_REFERENCE = 3027;
        public const string ERROR_FIELD_CANT_SELF_REFERENCE_STRING = "Field can't reference itself in a formula.";

        public const int ERROR_FIELD_STAGE_FIELD_IS_INVALID = 3028;
        public const string ERROR_FIELD_STAGE_FIELD_IS_INVALID_STRING = "Stage field is not valid.";

        public const int ERROR_FIELD_FILTER_FIELD_NOT_EXISTS = 3029;
        public const string ERROR_FIELD_FILTER_FIELD_NOT_EXISTS_STRING = "Filter field does not exists.";

        public const int ERROR_FIELD_FILTER_FIELD_TYPE_NOT_SUPPORTED = 3030;
        public const string ERROR_FIELD_FILTER_FIELD_TYPE_NOT_SUPPORTED_STRING = "Filter field type not supported for formulas.";

        public const int ERROR_FIELD_FILTER_FIELD_VALUE_INVALID = 3031;
        public const string ERROR_FIELD_FILTER_FIELD_VALUE_INVALID_STRING = "Filter field value is not valid.";

        public const int ERROR_FIELD_INVALID_REFFIELD_FUNCTION_CALL = 3032;
        public const string ERROR_FIELD_INVALID_REFFIELD_FUNCTION_CALL_STRING = "Function call to REFFIELD is not valid.";

        public const int ERROR_FIELD_INVALID_SETCURRENCY_FUNCTION_CALL = 3033;
        public const string ERROR_FIELD_INVALID_SETCURRENCY_FUNCTION_CALL_STRING = "Function call to SETCURRENCY only valid for Currency fields.";

        public const int ERROR_FIELD_AUTO_PDF_NOT_ENABLED = 3034;
        public const string ERROR_FIELD_AUTO_PDF_NOT_ENABLED_STRING = "Auto PDF is not enabled for a field.";

        public const int ERROR_FIELD_AUTO_PDF_NOT_ON_DOCUMENT = 3035;
        public const string ERROR_FIELD_AUTO_PDF_NOT_ON_DOCUMENT_STRING = "Auto PDF is not enabled for a field.";

        public const int ERROR_INVALID_CONVERSIONDATEFIELD = 3036;
        public const string ERROR_INVALID_CONVERSIONDATEFIELD_STRING = "Conversion Date field is not a valid Date field.";
        #endregion

        #region Entry Groups management
        public const int ERROR_ENRTYGROUP_FIELD_INVALID = 4000;
        public const string ERROR_ENRTYGROUP_FIELD_INVALID_STRING = "Field for Entry Group can be Choice, Reference or Yes/No only.";
        public const int ERROR_ENRTYGROUP_NOTFOUND = 4001;
        public const string ERROR_ENRTYGROUP_NOTFOUND_STRING = "Entry Group not found.";
        public const int ERROR_ENRTYGROUP_REFERENCE_INVALID = 4002;
        public const string ERROR_ENRTYGROUP_REFERENCE_INVALID_STRING = "Reference field for Entry Group can only reference one list.";
        public const int ERROR_ENRTYGROUP_REFERENCE_RELATIONSHIP = 4003;
        public const string ERROR_ENRTYGROUP_REFERENCE_RELATIONSHIP_STRING = "Reference field for Entry Group can not reference relationship list.";
        public const int ERROR_ENTRYGROUP_REFERENCE_LIST_INVALID = 4004;
        public const string ERROR_ENTRYGROUP_REFERENCE_LIST_INVALID_STRING = "Reference field for Entry Group references invalid list.";
        public const int ERROR_RULE_ALREADY_EXISTS = 4005;
        public const string ERROR_RULE_ALREADY_EXISTS_STRING = "Entry Group Rule for this list and field already exists.";

        #endregion

        #region Client Management
        public const int ERROR_CLIENT_NAME_UNIQUE = 5001;
        public const string ERROR_CLIENT_NAME_UNIQUE_STRING = "Name is not unique.";
        public const int ERROR_CLIENT_URL_UNIQUE = 5002;
        public const string ERROR_CLIENT_URL_UNIQUE_STRING = "URL is not unique.";
        public const int ERROR_CLIENT_URL_INVALID = 5003;
        public const string ERROR_CLIENT_URL_INVALID_STRING = "URL should be just a domain name like [clientname.dealcloud.com] .";
        #endregion

        #region Data

        public static readonly ErrorInfo ERROR_ACCESS_DENIED = new ErrorInfo { Code = ERROR_ACCESS_DENIED_CODE, Description = ERROR_ACCESS_DENIED_STRING };
        public const int ERROR_ACCESS_DENIED_CODE = 5000;
        public const string ERROR_ACCESS_DENIED_STRING = "Access Denied.";
        public const int INVALID_REQUEST_TO_STORE_DATA = 5001;
        public const string INVALID_REQUEST_TO_STORE_DATA_STRING = "Invalid request type for data store operation.";
        public const int VALUE_TYPE_IS_INVALID = 5002;
        public const string VALUE_TYPE_IS_INVALID_STRING = "Invalid data type for this field.";
        public const int INVALID_DATE_VALUE = 5003;
        public const string INVALID_DATE_VALUE_STRING = "Date value must be greater than 1899/12/31.";
        public const int INVALID_DECIMAL_VALUE = 5004;
        public const string INVALID_DECIMAL_VALUE_STRING = "Numeric value can't be greater than 16 digits.";
        public const int INVALID_CHOICE_VALUE = 5005;
        public const string INVALID_CHOICE_VALUE_STRING = "One or more choice values are not valid for this field.";
        public const int INVALID_REFERENCE_VALUE = 5006;
        public const string INVALID_REFERENCE_VALUE_STRING = "One or more referenced entries are not valid for this field.";
        public const int INVALID_USER_VALUE = 5007;
        public const string INVALID_USER_VALUE_STRING = "One or more referenced users are not valid for this field.";
        public const int FIELD_IS_REQUIRED_CODE = 5008;
        public const string FIELD_IS_REQUIRED_STRING = "Field is required.";
        public static readonly ErrorInfo FIELD_IS_REQUIRED = new ErrorInfo { Code = Constants.FIELD_IS_REQUIRED_CODE, Description = Constants.FIELD_IS_REQUIRED_STRING };
        public const int FIELD_VALUE_IS_NOT_UNIQUE = 5009;
        public const string FIELD_VALUE_IS_NOT_UNIQUE_STRING = "Name is not unique.";
        public const int CANT_STORE_TO_CALCULATED_FIELD = 5010;
        public const string CANT_STORE_TO_CALCULATED_FIELD_STRING = "Can't change value in calculated or smart field.";
        public static readonly ErrorInfo ERROR_MISSING_ENTRYLIST_NAME = new ErrorInfo { Code = ERROR_MISSING_ENTRYLIST_NAME_CODE, Description = ERROR_MISSING_ENTRYLIST_NAME_STRING };
        public const int ERROR_MISSING_ENTRYLIST_NAME_CODE = 5011;
        public const string ERROR_MISSING_ENTRYLIST_NAME_STRING = "List Name is required.";
        public static readonly ErrorInfo ERROR_INVALID_ENTRYLIST_NAME = new ErrorInfo { Code = ERROR_INVALID_ENTRYLIST_NAME_CODE, Description = ERROR_INVALID_ENTRYLIST_NAME_STRING };
        public const int ERROR_INVALID_ENTRYLIST_NAME_CODE = 5012;
        public const string ERROR_INVALID_ENTRYLIST_NAME_STRING = "List Name is not recognized.";

        public static readonly ErrorInfo ERROR_MISSING_ENTRY_NAME = new ErrorInfo { Code = ERROR_MISSING_ENTRY_NAME_CODE, Description = ERROR_MISSING_ENTRY_NAME_STRING };
        public const int ERROR_MISSING_ENTRY_NAME_CODE = 5014;
        public const string ERROR_MISSING_ENTRY_NAME_STRING = "Entry Name is required.";
        public static readonly ErrorInfo ERROR_INVALID_ENTRY_NAME = new ErrorInfo { Code = ERROR_INVALID_ENTRY_NAME_CODE, Description = ERROR_INVALID_ENTRY_NAME_STRING };
        public const int ERROR_INVALID_ENTRY_NAME_CODE = 5015;
        public const string ERROR_INVALID_ENTRY_NAME_STRING = "Entry Name is not recognized.";

        public static readonly ErrorInfo ERROR_MISSING_FIELD_NAME = new ErrorInfo { Code = ERROR_MISSING_FIELD_NAME_CODE, Description = ERROR_MISSING_FIELD_NAME_STRING };
        public const int ERROR_MISSING_FIELD_NAME_CODE = 5016;
        public const string ERROR_MISSING_FIELD_NAME_STRING = "Field Name is required.";
        public static readonly ErrorInfo ERROR_INVALID_FIELD_NAME = new ErrorInfo { Code = ERROR_INVALID_FIELD_NAME_CODE, Description = ERROR_INVALID_FIELD_NAME_STRING };
        public const int ERROR_INVALID_FIELD_NAME_CODE = 5017;
        public const string ERROR_INVALID_FIELD_NAME_STRING = "Field Name is not recognized.";

        public static readonly ErrorInfo ERROR_INVALID_CURRENCY_NAME = new ErrorInfo { Code = ERROR_INVALID_CURRENCY_NAME_CODE, Description = ERROR_INVALID_CURRENCY_NAME_STRING };
        public const int ERROR_INVALID_CURRENCY_NAME_CODE = 5018;
        public const string ERROR_INVALID_CURRENCY_NAME_STRING = "Currency is not recognized.";

        public static readonly ErrorInfo ERROR_UNKNOWN = new ErrorInfo { Code = ERROR_UNKNOWN_CODE, Description = ERROR_UNKNOWN_STRING };
        public const int ERROR_UNKNOWN_CODE = 5019;
        public const string ERROR_UNKNOWN_STRING = "Unknown error refershing data.";

        /* DON'T! change this code. If you really need to do it rename the same code on frontend as well */
        public const int FIELD_VALUE_HAS_NEAR_DUPLICATES = 5020;
        public const string FIELD_VALUE_HAS_NEAR_DUPLICATES_STRING = "Field value is possibly a duplicate.";

        public const int ERROR_MISSING_ENTRYFORM = 5021;
        public const string ERROR_MISSING_ENTRYFORM_STRING = "List should have entry form configuration.";

        public const int ERROR_MISSING_ENTRYFORM_TABS = 5022;
        public const string ERROR_MISSING_ENTRYFORM_TABS_STRING = "Entry Form should have at least one Tab.";

        public const int ERROR_ENTRYFORM_FIELD_MULTIPLE_USAGE = 5023;
        public const string ERROR_ENTRYFORM_FIELD_MULTIPLE_USAGE_STRING = "One or more fields on Entry Form used more than once.";


        public const int ERROR_ENTRYFORM_TAB_NAME_NOT_UNIQUE = 5024;
        public const string ERROR_ENTRYFORM_TAB_NAME_NOT_UNIQUE_STRING = "One or more Tabs on Entry Form has not unique name.";

        public const int ERROR_INVALID_ENTRYFORM_FILTER_FIELD = 5025;
        public const string ERROR_INVALID_ENTRYFORM_FILTER_FIELD_STRING = "Filter Field By on Entry Form is not a valid choice field.";

        public const int ERROR_ENTRYFORM_TAB_HAS_NO_FIELDS = 5026;
        public const string ERROR_ENTRYFORM_TAB_HAS_NO_FIELDS_STRING = "One or more Entry Form Tabs has no valid fields.";

        public const int ERROR_ENTRYFORM_FILTERFIELD_VALUES_INVALID = 5027;
        public const string ERROR_ENTRYFORM_FILTERFIELD_VALUES_INVALID_STRING = "One or more Entry Form Tabs has invalid Filter Field value associations.";

        public const int ERROR_ENTRYFORM_TAB_HAS_INVALID_FIELDS = 5028;
        public const string ERROR_ENTRYFORM_TAB_HAS_INVALID_FIELDS_STRING = "One or more Entry Form Tabs has invalid fields.";

        public const int FIELD_NOT_RECOGNIZED_CODE = 5029;
        public const string FIELD_NOT_RECOGNIZED_STRING = "Field is not recognized.";
        public static readonly ErrorInfo FIELD_NOT_RECOGNIZED = new ErrorInfo { Code = Constants.FIELD_NOT_RECOGNIZED_CODE, Description = Constants.FIELD_NOT_RECOGNIZED_STRING };


        public const int ERROR_NOT_ALL_TASKS_COMPLETED = 5030;
        public const string ERROR_NOT_ALL_TASKS_COMPLETED_STRING = "Can't change field value: not all tasks completed.";

        public const int ERROR_INVALID_VALUE_FOR_WORKFLOW_FIELD_CODE = 5031;
        public const string ERROR_INVALID_VALUE_FOR_WORKFLOW_FIELD_STRING = "Invalid value for a workflow field. Must be a single selection choice value.";
        public static readonly ErrorInfo ERROR_INVALID_VALUE_FOR_WORKFLOW_FIELD = new ErrorInfo { Code = Constants.ERROR_INVALID_VALUE_FOR_WORKFLOW_FIELD_CODE, Description = Constants.ERROR_INVALID_VALUE_FOR_WORKFLOW_FIELD_STRING };

        public const int ERROR_INVALID_VALUE_FOR_NEW_STEP_BY_STEP_WORKFLOW = 5032;
        public const string ERROR_INVALID_VALUE_FOR_NEW_STEP_BY_STEP_WORKFLOW_STRING = "Invalid value for a new step by step workflow. Should be first step in a workflow.";

        public const int ERROR_INVALID_VALUE_FOR_STEP_BY_STEP_WORKFLOW = 5033;
        public const string ERROR_INVALID_VALUE_FOR_STEP_BY_STEP_WORKFLOW_STRING = "Invalid value for a new step by step workflow. Should be next step in a workflow.";

        public const int ERROR_LIST_ENTRY_NOT_FOUND_CODE = 5034;
        public const string ERROR_LIST_ENTRY_NOT_FOUND_STRING = "List Entry is not found";
        public static readonly ErrorInfo ERROR_LIST_ENTRY_NOT_FOUND = new ErrorInfo { Code = ERROR_LIST_ENTRY_NOT_FOUND_CODE, Description = ERROR_LIST_ENTRY_NOT_FOUND_STRING };

        public const int AMBIGUOUS_REFERENCE_VALUE = 5035;
        public const string AMBIGUOUS_REFERENCE_VALUE_STRING = "Multiple entries with the same name exist in the lists referenced and the entry can not be resolved.";

        #endregion

        #region Query Builder

        public const int QUERY_INFO_IS_NOT_VALID = 5000;
        public const string QUERY_INFO_IS_NOT_VALID_STRING = "QueryInfo is not valid";

        #endregion

        #region Bulk editor

        #region error codes and messages

        public const int FILE_WAS_NOT_UPLOADED = 6000;
        public const string FILE_WAS_NOT_UPLOADED_STRING = "File was not uploaded";
        public const int BULK_FILE_EDITOR_FILE_NOT_FOUND = 6002;

        public const int RESOLVE_DUPLICATE_ENTRY_WAS_NOT_FOUND = 6003;
        public const string RESOLVE_DUPLICATE_ENTRY_WAS_NOT_FOUND_STRING = "Couldn't load selected entry";

        #endregion

        public const int BULK_EDITOR_MAX_PAGE_SIZE = 10000;

        #endregion

        #region Sync Service

        public const string OUTLOOK_SYNC_RECOVER_DELETED = "Item can not be deleted from outlook side, item state will be recoverd from latest DealCloud version. You can delete it from DealCloud using portal.";
        public const string OUTLOOK_SYNC_DELETE = "Item updated from outlook side was deleted on DealCloud, item will be deleted from outlook too.";
        public const string OUTLOOK_SYNC_RECOVER_UPDATED = "Item updated from outlook side was updated on DealCloud too, item state will be recovered from latest DealCloud version.";

        #endregion

        #region Matching Preferences

        public const int ERROR_DP_PREFERENCE_NOTFOUND = 9000;
        public const string ERROR_DP_PREFERENCE_NOTFOUND_STRING = "Preference not found.";

        public const int ERROR_DP_DATAPROVIDER_NOT_ENABLED = 9001;
        public const string ERROR_DP_DATAPROVIDER_NOT_ENABLED_STRING = "Data Provider is not enabled.";

        public const int ERROR_DP_DP_LIST_IS_NOT_VALID = 9002;
        public const string ERROR_DP_DP_LIST_IS_NOT_VALID_STRING = "Data Provider List is not valid.";

        public const int ERROR_DP_FIELDS_FROM_DIFFERENT_LISTS = 9003;
        public const string ERROR_DP_FIELDS_FROM_DIFFERENT_LISTS_STRING = "Fields from Data Provider side are from different Lists";

        public const int ERROR_DP_CLIENT_LIST_IS_NOT_VALID = 9004;
        public const string ERROR_DP_CLIENT_LIST_IS_NOT_VALID_STRING = "Client list is not valid.";

        public const int ERROR_DP_CLIENT_FIELDS_FROM_DIFFERENT_LISTS = 9005;
        public const string ERROR_DP_CLIENT_FIELDS_FROM_DIFFERENT_LISTS_STRING = "Fields from Client side are from different Lists";

        public const int ERROR_DP_WEIGHT_100 = 9006;
        public const string ERROR_DP_WEIGHT_100_STRING = "Sum of all Weight should be a 100.";

        public const int ERROR_DP_MATCHING_NOT_UNIQUE = 9007;
        public const string ERROR_DP_MATCHING_NOT_UNIQUE_STRING = "Provider List <-> Client List matching must be only one.";

        public const int ERROR_DP_FIELDTYPES_INVALID = 9008;
        public const string ERROR_DP_FIELDTYPES_INVALID_STRING = "Provider Field -> Client Field field types are not compatible.";

        public const int ERROR_DP_MAPPING_PAIR_NOT_VALID_ENTRIES = 9009;
        public const string ERROR_DP_MAPPING_PAIR_NOT_VALID_ENTRIES_STRING = "One or more entries/dataprovider entries are not valid.";

        #endregion


        #endregion

        #region Email wildcards

        public const string RECIPIENT_FIRST_NAME = "[Recipient.FirstName]";

        public const string RECIPIENT_LAST_NAME = "[Recipient.LastName]";

        public const string USER_FIRST_NAME = "[User.FirstName]";

        public const string USER_LAST_NAME = "[User.LastName]";

        public const string CLIENT_NAME = "[Client.Name]";

        public const string EMAIL_CONFIRMATION_LINK = "[Email.Confirmation]";

        public const string TWO_FACTOR_CODE = "[User.TwoFactorCode]";

        public const string PASSWORD_RESET_LINK = "[Password.Reset]";

        public const string PASSWORD_CREATE_LINK = "[Password.Create]";

        public const string NOTIFICATION_DETAILS = "[Notification.Details]";

        public const string NOTIFICATION_TITLE = "[Notification.Title]";

        public const string NOTIFICATION_SUMMARY = "[Notification.Summary]";

        public const string NOTIFICATION_FREQUENCY = "[Notification.Frequency]";

        public const string TEMPLATE_REPORT_ENTRY_NAME = "[TemplateReport.EntryName]";

        public const string TEMPLATE_REPORT_NAME = "[TemplateReport.Name]";

        public const string TEMPLATE_REPORT_DOCUMENT_LINK = "[TemplateReport.DocumentLink]";

        public const string DATE_WILDCARD = "[Date]";

        public const string DATE_WILDCARD_FORMAT = "MM-dd-yyyy";

        public const string WORKFLOW_ENTRY_NAME = "[Workflow.EntryName]";

        public const string WORKFLOW_CREATE_DATE = "[Workflow.CreateDate]";

        public const string WORKFLOW_DUE_DATE = "[Workflow.DueDate]";

        public const string WORKFLOW_CHOICE_FIELD_VALUE = "[Workflow.ChoiceFieldValue]";

        public const string ERROR_REPORT_SUBJECT = "[ErrorReport.Subject]";

        public const string ERROR_REPORT_BODY = "[ErrorReport.Body]";

        #endregion

        #region Mobile settings wildcards

        public const string MOBILE_ENTRY_NAME_WILDCARD = "[Mobile.EntryName]";

        public const string MOBILE_CREATED_DATE_WILDCARD = "[Mobile.CreatedDate]";


        #endregion

        #region Fields

        #region Field Names

        public const string CREATED_BY_FIELD_NAME = "CreatedBy";
        public const string CREATED_FIELD_NAME = "Created";
        public const string MODIFIED_BY_FIELD_NAME = "ModifiedBy";
        public const string MODIFIED_FIELD_NAME = "Modified";

        #endregion

        #region Tasks

        public const string DUE_DATE_FIELD_NAME = "DueDate";
        public const string ASSIGNED_TO_FIELD_NAME = "AssignedTo";

        #endregion

        #endregion

        #region Global Settings

        public const string PREPROCESSING_STORAGE_PREFIX = "PreProcessorWords";
        public const string WORDS_TO_IGNORE_STORAGE = "WordsToIgnoreStorage";
        public const string DEALCLOUD_UNIVERSITY_KEY = "DealCloudUniversityKey";
        public const string AFT_REQUEST_HEADER_NAME = "RequestVerificationToken"; //should match AFT_REQUEST_KEY in frontend constants
        public const string AFT_RESPONSE_HEADER_NAME = "X-AFT"; //should match AFT_HEADER_NAME in frontend constants
        public const string AFTC_RESPONSE_HEADER_NAME = "X-AFTC"; //should match AFTC_HEADER_NAME in frontend constants
        public const string ADDIN_VERSION_INFO = "AddinVersionInfo"; //addin version header

        public const int ADDIN_VERSION_LOGOUT = 7000;
        public const int ADDIN_VERSION_LOGIN = 7001;
        public const string ADDIN_VERSION_LOGOUT_STRING = "You have been logged out because there is a new version of the DealCloud Add In.  Please close and reopen [Outlook, Word or Excel] to update the DealCloud Add In to the latest version.";
        public const string ADDIN_VERSION_LOGIN_STRING = "There is a new version of the Add In. Please close and reopen [Outlook, Word or Excel] to update the DealCloud Add In to the latest version.";

        #endregion

        #region WebControls

        public const int DEFAULT_REPORT_HEIGTH = 800;

        public const int MAP_WIDGET_MAX_ENTRIES_COUNT = 100;

        public const string GOOGLE_MAP_CONFIG_KEY = "DealCloud.Web.GoogleMapApiKey";
        public const string GOOGLE_GEOCODING_CONFIG_KEY = "DealCloud.Web.GoogleGeocodingApiKey";

        #endregion

        #region Features toggle

        public const string DEALCLOUD_FEATURES_TOGGLE_CONFIG_KEY = "DealCloud.Web.FeaturesToggle";

        #endregion

        #region Data providers

        public const int SPS_DATAPROVIDER_ID = 2;

        public const int DATAFOX_DATAPROVIDER_ID = 1;

        #endregion
    }
}
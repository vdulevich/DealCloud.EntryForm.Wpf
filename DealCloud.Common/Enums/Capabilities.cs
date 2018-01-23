using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealCloud.Common.Enums
{
    /// <summary>
    /// Capabilities of the user in the system. Ids must be in sync with Capabilities table
    /// </summary>
    public enum Capabilities
    {
        /// <summary>
        /// default, means nothing
        /// </summary>
        None = 0,

        /// <summary>
        /// Provides the ability to read the data
        /// </summary>
        ReadData = 1,

        /// <summary>
        /// Allows user to export data in various formats
        /// </summary>
        ExportData = 2,

        /// <summary>
        /// Allows user to add or update data, additionally user might be able to delete data
        /// </summary>
        WriteData = 3,

        /// <summary>
        /// Allows user to delete data, which is allowed in WriteData
        /// </summary>
        DeleteData = 4,

        /// <summary>
        /// Allows user to create new objects
        /// </summary>
        CreateEntries = 5,

        /// <summary>
        /// Allows user to use excel add-in
        /// </summary>
        ExcelAddin = 6,

        /// <summary>
        /// Allows user to use Outlook add-in
        /// </summary>
        OutlookAddin = 7,

        /// <summary>
        /// Allows to manage users for the client
        /// </summary>
        UserManagement = 8,

        /// <summary>
        /// Allows administration of lists(entrylists) and fields
        /// </summary>
        Administration = 9,

        /// <summary>
        /// Allows to manage all notifications for the client
        /// </summary>
        NotificationManagement = 10,

        /// <summary>
        /// Allows user to use word add-in
        /// </summary>
        WordAddIn = 11,

        /// <summary>
        /// Allows access to Web Services
        /// </summary>
        AccessWebService = 12,

        /// <summary>
        /// Allows see data audit and restore|rollback changes
        /// </summary>
        DataAudit = 13,

        /// <summary>
        /// Allows see my views section
        /// </summary>
        MyViews = 14,

        /// <summary>
        /// Allows to see Merge Entries section
        /// </summary>
        MergeEntries = 15,

        /// <summary>
        /// allows to manage lists and fields
        /// </summary>
        ListManagement = 16,

        /// <summary>
        /// allows to manage Site Settings
        /// </summary>
        SiteSettings = 17,

        /// <summary>
        /// Allows to manage Entry Groups
        /// </summary>
        EntryGroups = 18,

        /// <summary>
        /// Allows to manage Template Reports
        /// </summary>
        TemplateReportManagement = 19,

        /// <summary>
        /// Allows to manage Email Templates
        /// </summary>
        EmailTemplates = 20,

        /// <summary>
        /// Allows to manage Workflows
        /// </summary>
        WorkflowManagement = 21,

        /// <summary>
        /// Allows to manage Public Views
        /// </summary>
        PublicViewManagement = 22,

        /// <summary>
        /// Alows to Manage MailChimp settings
        /// </summary>
        MailChimp = 23,

        /// <summary>
        /// Allows to magane portal views and reports
        /// </summary>
        DashboardConfiguration = 24,

        /// <summary>
        /// Allows to use Mobile App
        /// </summary>
        MobileApp = 25,

        /// <summary>
        /// Allows contacts sync in Outlook
        /// </summary>
        ContactSync = 26,

        /// <summary>
        /// Allows events sync in outlook
        /// </summary>
        EventSync = 27,

        /// <summary>
        /// Allows management of recurring tasks
        /// </summary>
        RecurringTasks = 28,

        /// <summary>
        /// Allows Running screens in data providers
        /// </summary>
        ScreeningPortal = 29,

        /// <summary>
        /// Allows creating links between dataprodider and client entries
        /// </summary>
        LinkManagement = 30,

        /// <summary>
        /// Allows management of Display Preferences
        /// </summary>
        DisplayPreferences = 31,

        /// <summary>
        /// Allows management of Matching Preferences
        /// </summary>
        MatchingPreferences = 32,


        /// <summary>
        /// Global Administration
        /// </summary>
        GlobalAdmin = 255
    }
}

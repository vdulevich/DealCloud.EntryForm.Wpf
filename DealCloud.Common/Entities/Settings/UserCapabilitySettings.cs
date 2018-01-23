using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealCloud.Common.Entities.Settings
{
    public class UserCapabilitySettings
    {
        public bool HasExportAccess { get; set; }
        public bool HasMyViewsAccess { get; set; }
        public bool HasPublicViewsAccess { get; set; }
        public bool HasDashboardConfigurationAccess { get; set; }
        public bool HasTemplateReportManagementAccess { get; set; }
        public bool HasUserManagementAccess { get; set; }
        public bool HasEmailTemplateManagementAccess { get; set; }
        public bool HasRecurringTaskManagementAccess { get; set; }
        public bool IsIdentityProviderEnabled { get; set; }
        public bool HasScreeningPortalAccess { get; set; }
        public bool HasLinkManagementAccess { get; set; }
    }
}

using System;

namespace DealCloud.Common.Entities.TemplateReports
{
    public class TemplateReport : TemplateReportBase
    {
        public TemplateProperties Properties { get; set; }

        public DateTime LastModified { get; set; }

        public int LastModifiedBy { get; set; }

        public DateTime Created { get; set; }

        public int CreatedBy { get; set; }

        public bool IsEnabled { get; set; }

        public bool HasLiveVersion { get; set; }
    }
}
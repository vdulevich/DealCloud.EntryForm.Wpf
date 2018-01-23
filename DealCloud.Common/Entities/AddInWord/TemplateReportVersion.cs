using System;
using DealCloud.Common.Enums;

namespace DealCloud.Common.Entities.AddInWord
{
    public class TemplateReportVersion
    {
        public int Id { get; set; }

        public int TemplateReportId { get; set; }

        public TemplateReportVersionType Version { get; set; }

        public DateTime Modified { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime Created { get; set; }

        public int CreatedBy { get; set; }

        public override string ToString()
        {
            return Version.ToString();
        }
    }
}

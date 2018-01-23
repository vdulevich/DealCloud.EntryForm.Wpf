using DealCloud.Common.Enums;

namespace DealCloud.Common.Entities.TemplateReports
{
    public class TemplateReportBase
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public TemplateReportType ReportType { get; set; }

        public int EntryListId { get; set; }

        public override string ToString()
        {
            return Name ?? base.ToString();
        }
    }
}
using DealCloud.Common.Entities.TemplateReports;

namespace DealCloud.Common.Entities.AddInWord
{
    public class RunTemplateReportRequest
    {
        public TemplateDocument TemplateDocument { get; set; }

        public int EntryId { get; set; }
    }
}

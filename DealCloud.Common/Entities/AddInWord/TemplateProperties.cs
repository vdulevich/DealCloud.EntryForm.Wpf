using System.Collections.Generic;
using DealCloud.Common.Enums;

namespace DealCloud.Common.Entities.TemplateReports
{
    public class TemplateProperties
    {
        public List<TemplateSchedule> Schedules { get; set; }

        public string DocumentName { get; set; }

        public DocumentFormat ReportFormat { get; set; }

        public ReportFileType? FileType { get; set; }

        public bool IsTagToEntry { get; set; }

        public List<CellValue> DocumentTemplateFields { get; set; }

        public HashSet<TemplateReportCreateDocumentEntryWhen> CreateDocumentEntryWhen { get; set; }

        public override string ToString()
        {
            return DocumentName ?? base.ToString();
        }
    }
}
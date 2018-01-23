using System.Collections.Generic;
using System.Linq;
using DealCloud.Common.Entities.Query;
using DealCloud.Common.Enums;

namespace DealCloud.Common.Entities.TemplateReports
{
    public class TemplateDocument : TemplateReportBase
    {
        public List<QueryInfo> Queries { get; set; }

        public byte[] Content { get; set; }

        public string ContentType { get; set; }

        public TemplateReportVersionType Version { get; set; }

        public override string ToString()
        {
            return $"{Version}: {base.ToString()}";
        }

        public TemplateDocument ShallowClone()
        {
            return new TemplateDocument()
            {
                Content = this.Content,
                ContentType = this.ContentType,
                Queries = this.Queries.Select(q => q.ShallowClone()).ToList(),
                Version = this.Version,
                Id = this.Id,
                Name = this.Name,
                ReportType = this.ReportType,
                EntryListId = this.EntryListId
            };
        }
    }
}
using DealCloud.Common.Entities.IncomingEmail;
using System.Collections.Generic;

namespace DealCloud.Common.Entities.AddInOutlook
{
    public class EmailDetailsRequest
    {
        public string MessageId { get; set; }

        public string SyncDcId { get; set; }

        public IEnumerable<string> Contacts { get; set; }

        public IEnumerable<string> ContactRef { get; set; }
    }
}

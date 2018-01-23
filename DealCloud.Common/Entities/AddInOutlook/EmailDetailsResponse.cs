using System.Collections.Generic;
using DealCloud.Common.Entities.IncomingEmail;
using System;

namespace DealCloud.Common.Entities.AddInOutlook
{
    public class EmailDetailsResponse
    {
        public int? Id { get; set; }

        public DateTime? Modified { get; set; }

        public Dictionary<int, object> ValuesByField { get; set; }

        public IEnumerable<ContactEntry> Contacts { get; set; }

        public IEnumerable<ContactEntry> Users { get; set; }
    }
}

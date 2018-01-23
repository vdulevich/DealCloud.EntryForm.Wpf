using System.Collections.Generic;
using DealCloud.Common.Entities.IncomingEmail;

namespace DealCloud.Common.Entities.AddInOutlook
{
    public class EventDetailsResponse
    {
        public int? Id { get; set; }

        public Dictionary<int, object> ValuesByField { get; set; }

        public List<ContactEntry> Contacts { get; set; }

        public List<ContactEntry> Users { get; set; }
    }
}

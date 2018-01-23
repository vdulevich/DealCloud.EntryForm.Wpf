using System.Collections.Generic;

namespace DealCloud.Common.Entities.AddInOutlook
{
    public class EventDetailsRequest
    {
        public int? Id { get; set; }

        public List<string> Contacts { get; set; }

        public List<string> ContactRef { get; set; }
    }
}

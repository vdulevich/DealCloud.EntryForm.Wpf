using System;
using System.Collections.Generic;

namespace DealCloud.Common.Entities.AddInOutlook
{
    public class SyncEventsRequest
    {
        public List<SycnRequestItem> Items { get; set; }

        public DateTime SyncStartUtc { get; set; }

        public DateTime SyncEndUtc { get; set; }
    }
}

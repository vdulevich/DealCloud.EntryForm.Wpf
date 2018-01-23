using System.Collections.Generic;

namespace DealCloud.Common.Entities.AddInOutlook
{
    public class SyncContactsRequest
    {
        public List<int> CreatedBy { get; set; }

        public List<int> Owners { get; set; }

        public List<int> CoOwners { get; set; }

        public List<SycnRequestItem> Items { get; set; }
    }
}

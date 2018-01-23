using System.Collections.Generic;

namespace DealCloud.Common.Entities.AddInOutlook
{
    public class SyncResponse
    {
        public List<SyncResponseItem> Updates { get; set; }

        public HashSet<int> Deletes { get; set; }

        public int TotalCount => (Updates?.Count + Deletes?.Count) ?? 0;

        public override string ToString()
        {
            return $"{Updates?.Count ?? 0} updates; {Deletes?.Count ?? 0} deletes";
        }
    }
}
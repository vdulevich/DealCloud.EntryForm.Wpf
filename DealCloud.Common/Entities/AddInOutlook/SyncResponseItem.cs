using System;
using System.Collections.Generic;
using DealCloud.Common.Entities.AddInCommon;

namespace DealCloud.Common.Entities.AddInOutlook
{
    public class SyncResponseItem
    {
        public Guid? RowId { get; set; }

        public int? DcId { get; set; }

        public Dictionary<SystemFieldTypes, object> Data { get; set; }

        public string Hash { get; set; }

        public SyncItemError Error { get; set; }

        public override string ToString()
        {
            return Error?.Description ?? $"{DcId}: {Hash}";
        }
    }
}
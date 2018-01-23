using System;
using System.Collections.Generic;
using DealCloud.Common.Entities.AddInCommon;
using DealCloud.Common.Enums;
using DealCloud.Common.Attributes;

namespace DealCloud.Common.Entities.AddInOutlook
{
    public class SycnRequestItem
    {
        public Guid? RowId { get; set; }

        public int? DcId { get; set; }

        public Dictionary<SystemFieldTypes, object> Data { get; set; }

        public Dictionary<int, object> DataRefs { get; set; }

        public string Hash { get; set; }

        public SycnRequestItemState State { get; set; }

        [Sanitize]
        public string EntryFormData { get; set; }

        public override string ToString()
        {
            return $"{State}: {DcId}";
        }
    }
}
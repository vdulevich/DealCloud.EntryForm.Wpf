using System.Collections.Generic;
using DealCloud.Common.Entities.AddInCommon;

namespace DealCloud.Common.Entities.AddInOutlook
{
    public class ContactDetailsResponse
    {
        public int? Id { get; set; }

        //public string Hash { get; set; }

        public NamedEntry Company { get; set; }

        //public Dictionary<SystemFieldTypes, object> Data { get; set; }
    }
}

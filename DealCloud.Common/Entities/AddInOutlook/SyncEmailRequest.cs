using System;
using System.Collections.Generic;
using DealCloud.Common.Attributes;
using DealCloud.Common.Entities.AddInCommon;

namespace DealCloud.Common.Entities.AddInOutlook
{
	public class SyncEmailAttachment
    {
		public string Name { get; set; }

		public byte[] Content { get; set; }
	}

	public class SyncEmailRequest
	{
        [Sanitize]
        public string MessageId { get; set; }

        public string SyncDcId { get; set; }

        [Sanitize]
        public string EntryFormData { get; set; }

        public Dictionary<SystemFieldTypes, object> OData { get; set; }

        public Dictionary<int, List<int>> ODataRefs { get; set; }

        public bool ValidateOnly { get; set; }

        public SyncEmailRequest()
		{
			ODataRefs = new Dictionary<int, List<int>>();
		}
	}
}

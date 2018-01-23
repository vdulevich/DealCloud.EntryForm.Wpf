using System;
using DealCloud.Common.Entities.Query;

namespace DealCloud.Common.Entities
{
    public class LibraryView : NamedEntry
    {
        public QueryInfo QueryInfo { get; set; }

        public string Description { get; set; }

        public bool IsPublic { get; set; }

        /// <summary>
        /// Modified date/time in UTC
        /// </summary>
        public DateTime Modified { get; set; }

        public int ModifiedBy { get; set; }

        /// <summary>
        /// Created date/time in UTC
        /// </summary>
        public DateTime Created { get; set; }

        public int CreatedBy { get; set; }
    }
}

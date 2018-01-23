using System.Collections.Generic;

namespace DealCloud.Common.Entities
{
    public class PagedResult<T>
    {
        /// <summary>
        ///     Total records
        /// </summary>
        public int TotalRecords { get; set; }

        /// <summary>
        ///     Rows for current page
        /// </summary>
        public List<T> Rows { get; set; }
    }
}
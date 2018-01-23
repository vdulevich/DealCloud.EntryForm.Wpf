using System.Collections.Generic;
using System.Linq;

namespace DealCloud.Common.Entities.Query.Filters
{
    /// <summary>
    /// resultset for EntriesMultiGet
    /// </summary>
    public class FilterResults
    {
        /// <summary>
        /// rowIds for current page
        /// </summary>
        public List<int> RowIds { get; set; }

        /// <summary>
        /// any relationships which we need to pull for a second call to get data
        /// key is EntryId from main RowIds, value is a list of related EntryId to Field
        /// </summary>
        public ILookup<long, int> RelatedEntries { get; set; }

        /// <summary>
        /// all rows if group is started on a previous page and finished on current
        /// </summary>
        public List<int> GroupRowsIds { get; set; }

        /// <summary>
        /// total rows for a query
        /// </summary>
        public int TotalRowsCount { get; set; }

        /// <summary>
        /// rows in the last group when grouping is on
        /// </summary>
        public int LastGroupItemsCount { get; set; }
    }
}

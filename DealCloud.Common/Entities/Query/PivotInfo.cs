using DealCloud.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DealCloud.Common.Entities.Query
{
    /// <summary>
    /// Pivot is a table which do group by 1 or fields (category and series)
    /// result columns are dynamic depending on data
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class PivotInfo : QueryInfo
    {
        /// <summary>
        /// Default constructor for deserialization
        /// </summary>
        public PivotInfo()
        {
            
        }

        public PivotInfo(QueryInfo src)
        {
            if (src == null) throw new ArgumentNullException(nameof(src));
            Id = src.Id;
            Name = src.Name;
            Columns = src.Columns;
            PrimaryFilters = src.PrimaryFilters;
            SecondaryFilters = src.SecondaryFilters;
            Grouping = src.Grouping;
            IterationEntryListIds = src.IterationEntryListIds;
            RelationshipFields = src.RelationshipFields;
            EntryDetailsRelationshipFields = src.EntryDetailsRelationshipFields;
            Paging = src.Paging;
            Sorting = src.Sorting;
        }

        /// <summary>
        /// category (rows) column ID (from queryInfo)
        /// </summary>
        public int CategoryColumnId { get; set; }

        /// <summary>
        /// type of Date aggreagtion by Category Column if this column is date
        /// </summary>
        public DateAggregationTypes CategoryDateAggregation { get; set; }

        /// <summary>
        /// Series (columns) column ID - optional
        /// </summary>
        public List<int> SeriesColumns { get; set; }

        /// <summary>
        /// type of Date aggreagtion by Series Column if this column is date
        /// </summary>
        public DateAggregationTypes SeriesDateAggregation { get; set; }

        /// <summary>
        /// Value column for pivot table (this is what will be sum/count
        /// </summary>
        public List<int> ValueColumns { get; set; } 

        /// <summary>
        /// type of aggreagtion for value
        /// </summary>
        public ColumnAggregation ValueAggregation { get; set; }

        /// <summary>
        /// If "Undefined" row/Column should be excluded
        /// </summary>
        public bool ExcludeBlanks { get; set; }

        public SortDirection CategoriesSort { get; set; }
        public SortDirection SeriesSort { get; set; }

        public int ShowLimit { get; set; } = -1;

        /// <summary>
        /// Determines whether to sort by sum/count totals or not
        /// </summary>
        public bool IsSortBySeriesTotal { get; set; }

        public SortDirection SeriesTotalSort { get; set; } = SortDirection.Descending;
    }
}

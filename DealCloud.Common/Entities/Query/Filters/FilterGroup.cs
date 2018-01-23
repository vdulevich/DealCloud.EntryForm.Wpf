using System.Collections.Generic;

namespace DealCloud.Common.Entities.Query.Filters
{
    public class FilterGroup : FilterTerm
    {
        public int GroupId { get; set; }

        public List<FilterTerm> Items { get; set; }

        public FilterTermType Operator { get; set; }
    }
}

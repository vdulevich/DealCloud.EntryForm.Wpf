namespace DealCloud.Common.Entities.Query
{
    /// <summary>
    ///     Info about sorting of the query
    /// </summary>
    public class SortInfo
    {
        /// <summary>
        ///     ColumnID or FieldId by which we are sorting or grouping
        /// </summary>
        public int ColumnId { get; set; }

        /// <summary>
        ///     Is Filter Descending
        /// </summary>
        public bool IsDescending { get; set; }

        public override int GetHashCode()
        {
            var result = 17;

            unchecked
            {
                result = result * 37 + ColumnId;
                result = result * 37 + IsDescending.GetHashCode();
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            var sort = obj as SortInfo;

            return sort != null && sort.ColumnId == ColumnId && sort.IsDescending == IsDescending;
        }
    }
}
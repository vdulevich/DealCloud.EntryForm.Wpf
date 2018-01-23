namespace DealCloud.Common.Entities.Query
{
    /// <summary>
    ///     Info about grouping of the query
    /// </summary>
    public class GroupInfo
    {
        /// <summary>
        ///     ColumnId or FieldId by which we are grouping
        /// </summary>
        public int ColumnId { get; set; }

        /// <summary>
        ///     Is Group sorting is Descending
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
            var group = obj as GroupInfo;

            return group != null && group.ColumnId == ColumnId && group.IsDescending == IsDescending;
        }
    }
}
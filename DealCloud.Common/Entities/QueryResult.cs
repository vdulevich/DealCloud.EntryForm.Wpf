namespace DealCloud.Common.Entities
{
    /// <summary>
    ///     Results of the query
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueryResult<T> : PagedResult<T>
    {
        /// <summary>
        ///     Column Metadata
        /// </summary>
        public ColumnMetadata Columns { get; set; }
    }
}

namespace DealCloud.Common.Entities.Query
{
    /// <summary>
    ///     Info about a paging for a query
    /// </summary>
    public class PageInfo
    {
        /// <summary>
        ///     Page number starting from 1
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        ///     Number of rows on a page
        /// </summary>
        public int PageSize { get; set; }

        public bool IsSinglePage { get; set; }

        public override int GetHashCode()
        {
            var result = 17;

            unchecked
            {
                result = result * 37 + PageNumber;
                result = result * 37 + PageSize;
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            var sort = obj as PageInfo;

            return sort != null && sort.PageNumber == PageNumber && sort.PageSize == PageSize;
        }
    }
}
using DealCloud.Server.Common.Enums;

namespace DealCloud.Common.Entities.Query.Filters
{
    //NOTE: Type is serealized as JSON to DB. Assembly should not be changed
    public class SmartFilter
    {
        /// <summary>
        /// Type of the SmartFilter (Date, User, ContextEntryId)
        /// </summary>
        public SmartFilterType Type { get; set; }

        public SmartFilterDateRange DateRangeType { get; set; }

        /// <summary>
        /// for Date type SmartFilter this is a Dynamic Date we are start counting from
        /// </summary>
        public DynamicDates FromValue { get; set; }

        /// <summary>
        /// for Date type SmartFilter this is operation for a Value offset from 
        /// </summary>
        public SmartFilterOperator Operator  { get; set; }
        /// <summary>
        /// value for the SmartFilter, might be UserID, ContextEntryId, or number of days 
        /// </summary>
        public int Value { get; set; }

        public override bool Equals(object obj)
        {
            var smartFilter = obj as SmartFilter;
            if (smartFilter == null)
            {
                return false;
            }
            return Type == smartFilter.Type && DateRangeType == smartFilter.DateRangeType && FromValue == smartFilter.FromValue && Operator == smartFilter.Operator && Value == smartFilter.Value;
        }

        public override int GetHashCode()
        {
            var result = 17;

            unchecked
            {
                result = result * 37 + Value;
                result = result * 37 + (int)Type;
                result = result * 37 + (int)DateRangeType;
                result = result * 37 + (int) Operator;
                result = result * 37 + (int) FromValue;
            }

            return result;
        }
    }

    public enum SmartFilterOperator
    {
        Substraction = 0,
        Addition = 1
    }

    public enum SmartFilterType
    {
        None = 0,
        /// <summary>
        /// when smart filter nee to resolve dates
        /// </summary>
        Date = 1,
        /// <summary>
        /// when we should resolve smart filter to ID of the currenly looged in user
        /// </summary>
        CurrentUser = 2,
        /// <summary>
        /// when we need to resolve smart filter to ID of the current Entry (like Entry Details page on a portal or entry report )
        /// </summary>
        ContextEntryId = 3
    }

    public enum SmartFilterDateRange
    {
        Whithin = 0,
        Exact = 1
    }
}

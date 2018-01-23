namespace DealCloud.Common.Entities.Query.Filters
{
    public class NumericFilter
    {
        public int FieldId { get; set; }
        public decimal? FromValue { get; set; }
        public decimal? ToValue { get; set; }
        public string CurrencyCode { get; set; }

        public int GroupId { get; set; }

        public bool IsNot { get; set; }
    }
}

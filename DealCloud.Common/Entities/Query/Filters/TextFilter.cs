namespace DealCloud.Common.Entities.Query.Filters
{
    public class TextFilter
    {
        public int FieldId { get; set; }

        public string Value { get; set; }

        public int GroupId { get; set; }

        public bool IsNot { get; set; }
    }
}

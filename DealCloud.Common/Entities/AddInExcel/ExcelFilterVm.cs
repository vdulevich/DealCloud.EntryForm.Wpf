using DealCloud.Common.Entities.Query;
using DealCloud.Common.Enums;
using DealCloud.Server.Common.Enums;

namespace DealCloud.Common.Entities.AddInExcel
{
    public class ExcelFilterVm
    {
        public FieldTypes FieldType { get; set; }
        public int SystemFieldType { get; set; }
        public int ColumnId { get; set; }
        public FilterOperation FilterOperation { get; set; }
        public FilterTermType FilterTermType { get; set; }
        public DynamicDates? DynamicDate { get; set; }
        public object Value { get; set; }
        public object ValueTo { get; set; }
        public bool CanBeSuppliedByUser { get; set; }
    }
}

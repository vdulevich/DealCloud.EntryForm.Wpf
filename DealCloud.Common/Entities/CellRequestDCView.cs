using DealCloud.Common.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace DealCloud.Common.Entities
{
    public class CellRequestDcView : CellRequestGet
    {
        public int QueryId { get; set; }

        public List<object> FilterParameters { get; set; }

        public override CellRequestGet Clone()
        {
            return new CellRequestDcView { ColumnId = this.ColumnId, RowId = this.RowId, QueryId = this.QueryId, FilterParameters = this.FilterParameters.ToList() };
        }

        public override bool Equals(object obj)
        {
            var y = obj as CellRequestDcView;
            if (y == null) return false;

            return QueryId == y.QueryId && IEnumerableExtensions.Equals(FilterParameters, y.FilterParameters);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var res = 17;
                res = res * 37 + QueryId;
                res = res * 37 + IEnumerableExtensions.GetHashCodeForList<object>(FilterParameters);
                return res;
            }
        }

        public override CellValue GetBlankCellValue()
        {
            return new CellValueDcView { QueryId = this.QueryId, FilterParameters = this.FilterParameters };
        }
    }
}

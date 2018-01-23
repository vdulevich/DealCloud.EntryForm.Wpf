using DealCloud.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealCloud.Common.Entities
{
    /// <summary>
    /// Request to get the data for the cell
    /// </summary>
    public class CellRequestGet : CellRequestBase
    {
        private static FuncEqualityComparer<CellRequestGet> _cellRequestComparer = new FuncEqualityComparer<CellRequestGet>((x, y) => x.EntryId == y.EntryId && x.FieldId == y.FieldId && (x.CurrencyCode ?? Constants.RC_CURRENCY) == (y.CurrencyCode ?? Constants.RC_CURRENCY), 
                                                                                                                            (x) => (((17 * 37) + x.EntryId) * 37 + x.FieldId) * 37 + (x.CurrencyCode ?? Constants.RC_CURRENCY).GetHashCode());

        public static IEqualityComparer<CellRequestGet> Comparer { get { return _cellRequestComparer; } }
        /// <summary>
        /// ID of column for multi-list grids
        /// </summary>
        public int ColumnId { get; set; }

        public int RowId { get; set; }

        public virtual CellRequestGet Clone()
        {
            return new CellRequestGet { ColumnId = this.ColumnId, RowId = this.RowId, EntryId = this.EntryId, FieldId = this.FieldId, CurrencyCode = this.CurrencyCode };
        }

        public override bool Equals(object obj)
        {
            return Comparer.Equals(this, obj as CellRequestGet);
        }

        public override int GetHashCode()
        {
            return Comparer.GetHashCode(this);
        }

        public override CellValue GetBlankCellValue()
        {
            return new CellValue { EntryId = this.EntryId, FieldId = this.FieldId, RowId = this.RowId, RequestedCurrencyCode = this.CurrencyCode };
        }
    }

    /// <summary>
    /// request for entry details via single select reference field. for ex. Company.PromaryContact.Email
    /// will only work when requests in one batch for 1 entry only
    /// </summary>
    public class CellRequestGetDetail : CellRequestGet
    {
        /// <summary>
        /// one entry only in the batch supported -> key is ReferenceFieldId + FieldId
        /// </summary>
        private static FuncEqualityComparer<CellRequestGetDetail> _cellRequestComparer = new FuncEqualityComparer<CellRequestGetDetail>((x, y) => (x.FieldId == y.FieldId) && (x.ReferenceFieldId == y.ReferenceFieldId) && (x.CurrencyCode ?? Constants.RC_CURRENCY) == (y.CurrencyCode ?? Constants.RC_CURRENCY),
                                                                                                                            (x) => (((17 * 37) + x.FieldId) * 37 + x.ReferenceFieldId) * 37 + (x.CurrencyCode ?? Constants.RC_CURRENCY).GetHashCode());

        public new static IEqualityComparer<CellRequestGetDetail> Comparer { get { return _cellRequestComparer; } }

        public int ReferenceFieldId { get; set; }

        public override bool Equals(object obj)
        {
            return Comparer.Equals(this, obj as CellRequestGetDetail);
        }

        public override int GetHashCode()
        {
            return Comparer.GetHashCode(this);
        }
    }
}

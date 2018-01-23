using DealCloud.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealCloud.Common.Entities
{
    /// <summary>
    /// request to store a value for a cell
    /// </summary>
    public class CellRequestStore : CellRequestBase
    {
        private static FuncEqualityComparer<CellRequestStore> _cellRequestComparer = new FuncEqualityComparer<CellRequestStore>((x, y) => x.EntryId == y.EntryId && x.FieldId == y.FieldId && (x.CurrencyCode ?? Constants.RC_CURRENCY) == (y.CurrencyCode ?? Constants.RC_CURRENCY),
                                                                                                                            (x) => (((17 * 37) + x.EntryId) * 37 + x.FieldId) * 37 + (x.CurrencyCode ?? Constants.RC_CURRENCY).GetHashCode());

        public static IEqualityComparer<CellRequestStore> Comparer { get { return _cellRequestComparer; } }

        /// <summary>
        /// value to store: can be decimal, int, IEnumerable&lt;int&gt;, string, byte[]
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// when near duplicates was resolved for the field set if to true, so it won't trigger validation errors anymore
        /// </summary>
        public bool IgnoreNearDups { get; set; }

        public override CellValue GetBlankCellValue()
        {
            return new CellValue { EntryId = this.EntryId, FieldId = this.FieldId, RequestedCurrencyCode = this.CurrencyCode, Value = this.Value };
        }
    }

    public class CellRequestStoreBinary : CellRequestStore
    {
        /// <summary>
        /// extension without . for binary file
        /// </summary>
        public string FormatType { get; set; }

        /// <summary>
        /// Content type (media type) for binary file
        /// </summary>
        public string ContentType { get; set; }
    }

}

using DealCloud.Common.Extensions;
using DealCloud.Common.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealCloud.Common.Entities
{
    /// <summary>
    /// Results of the CellRequest
    /// </summary>
    public class CellValue
    {
        private static FuncEqualityComparer<CellValue> _cellValueComparer = new FuncEqualityComparer<CellValue>((x, y) => x.EntryId == y.EntryId && x.FieldId == y.FieldId && (x.RequestedCurrencyCode ?? Constants.RC_CURRENCY) == (y.RequestedCurrencyCode ?? Constants.RC_CURRENCY), 
                                                                                                                (x) => (((17 * 37) + x.EntryId) * 37 + x.FieldId) * 37 + (x.RequestedCurrencyCode ?? Constants.RC_CURRENCY).GetHashCode());

        public static IEqualityComparer<CellValue> Comparer { get { return _cellValueComparer; } }

        /// <summary>
        /// Id of the Entry in CellRequestBase
        /// </summary>
        public int EntryId { get; set; }

        /// <summary>
        /// Id of the field
        /// </summary>
        public int FieldId { get; set; }

        /// <summary>
        /// this is Resolved EntryId - same as EntryId for existing entries, just created EntryId for new records 
        /// </summary>
        public int RowId { get; set; }

        /// <summary>
        /// Error for CellRequest if any
        /// </summary>
        public ErrorInfo Error { get; set; }

        public bool IsNoData { get; set; }

        public bool IsNew { get; set; }

        private object _value;
        public object Value
        {
            get { return _value; }
            set
            {
                if (value is DateTime)
                {
                    var dtValue = (DateTime)value;
                    if (dtValue.Kind == DateTimeKind.Unspecified)
                        value = DateTime.SpecifyKind(dtValue, DateTimeKind.Utc);
                }
                _value = value;
            }
        }

        public string CurrencyCode { get; set; }

        public string RequestedCurrencyCode { get; set; }

        public CellValue Clone()
        {
            return new CellValue
            {
                EntryId = this.EntryId,
                FieldId = this.FieldId,
                CurrencyCode = this.CurrencyCode,
                RequestedCurrencyCode = this.RequestedCurrencyCode,
                Error = this.Error,
                IsNew = this.IsNew,
                IsNoData = this.IsNoData,
                RowId = this.RowId,
                Value = this.Value
            };
        }

        public override bool Equals(object obj)
        {
            var cv = obj as CellValue;
            return Comparer.Equals(this, cv);
        }

        public override int GetHashCode()
        {
            return Comparer.GetHashCode(this);
        }
    }

    public class CellValueDcView : CellValue
    {
        public int QueryId { get; set; }

        public List<string> IterationListNames { get; set; }

        public List<object> FilterParameters { get; set; }

        public override bool Equals(object obj)
        {
            var y = obj as CellValueDcView;
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
    }
}

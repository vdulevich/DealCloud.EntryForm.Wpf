using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DealCloud.Common.Entities
{
    /// <summary>
    /// request for a cell in Entry List.
    /// </summary>
    public class CellRequestBase
    {
        /// <summary>
        /// Entry for which we want a CellValue
        /// </summary>
        public int EntryId { get; set; }

        /// <summary>
        /// Field for which we want a value
        /// </summary>
        public int FieldId { get; set; }

        /// <summary>
        /// result currency code
        /// </summary>
        public string CurrencyCode { get; set; }

        public virtual CellValue GetBlankCellValue()
        {
            return new CellValue { EntryId = this.EntryId, FieldId = this.FieldId, RequestedCurrencyCode = this.CurrencyCode };
        }

    }
}

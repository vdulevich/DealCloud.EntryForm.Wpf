using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DealCloud.Common.Serialization;

namespace DealCloud.Common.Entities
{
    /// <summary>
    /// Row result of generic (dynamic) table
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    [JsonConverter(typeof(DictionaryJsonConverter))]
    public class Row : Dictionary<int, object>
    {
        /// <summary>
        /// this is EntryId of the object in iteration
        /// </summary>
        public int RowId { get; set; }

        /// <summary>
        /// Key for a totals or subtotals row
        /// </summary>
        public object GroupKey { get; set; }

        /// <summary>
        /// true is this row is for a group subtotals
        /// </summary>
        public bool IsSubTotals { get; set; }

        /// <summary>
        /// true is this row is for a result Totals
        /// </summary>
        public bool IsTotals { get; set; }

        /// <summary>
        /// for subtotal rows - number of items in the group
        /// </summary>
        public int? GroupRowsCount { get; set; }

        /// <summary>
        /// true if user can edit row
        /// </summary>
        public bool NoEdit { get; set; }

        /// <summary>
        /// true if user can delete row
        /// </summary>
        public bool NoDel { get; set; }
    }
}

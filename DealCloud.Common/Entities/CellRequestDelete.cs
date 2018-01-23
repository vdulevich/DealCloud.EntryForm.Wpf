using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealCloud.Common.Entities
{
    /// <summary>
    /// Request to Delete Entry.
    /// Only EntryId and IsDelete fields are meaningful
    /// </summary>
    public class CellRequestDelete : CellRequestBase
    {
        public bool IsDelete { get; set; }
    }
}

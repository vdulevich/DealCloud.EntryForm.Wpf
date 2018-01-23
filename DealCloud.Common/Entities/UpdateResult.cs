using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealCloud.Common.Entities
{
    /// <summary>
    /// Result of add/update/delete operation for one or more records
    /// </summary>
    public class UpdateResult
    {
        /// <summary>
        /// Results of update for each row/record 
        /// </summary>
        public List<RowUpdateResult> RowResults { get; set; }

        /// <summary>
        /// true if there was any row which was updated after the row was retrieved.
        /// </summary>
        public bool HasModificationConflicts { get; set; }

        /// <summary>
        /// DateTime when operation completed
        /// </summary>
        public DateTime FinishTime { get; set; }
    }

    /// <summary>
    /// Add/Update/Delete result for a specific row
    /// </summary>
    public class RowUpdateResult
    {
        /// <summary>
        /// number of the row in the batch
        /// </summary>
        public int RowNumber { get; set; }

        /// <summary>
        /// Id for the Row/Record
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Error for the row/record if any
        /// </summary>
        public ErrorInfo Error { get; set; }
    }
}

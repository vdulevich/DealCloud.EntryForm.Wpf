using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DealCloud.Common.Entities.EntryForm
{
    /// <summary>
    /// information about entry form configuration
    /// </summary>
    public class EntryFormInfo : NamedEntry
    {
        private const int ID_FOR_ALL_VALUES = 0;

        /// <summary>
        /// tabs setup for this entry form
        /// if user select no tabs one default will be added, but should not be displayed in this case
        /// default entry tab should have id &lt;= 0
        /// </summary>
        public List<EntryFormTab> Tabs { get; set; } = Enumerable.Empty<EntryFormTab>().ToList();

        /// <summary>
        /// fields by which filter tabs will be filtered
        /// </summary>
        public List<int> FilterFieldsIds { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealCloud.Common.Entities.EntryForm
{

    /// <summary>
    /// definition of the tab for EntryForm
    /// </summary>
    public class EntryFormTab : NamedEntry
    {
        public static EntryFormTab Default => new EntryFormTab { Id = -1, Name = "Default" };

        /// <summary>
        /// fields assigned to this tab
        /// </summary>
        public List<int> Fields { get; set; }

        /// <summary>
        /// Choice/Reference/Boolean values to determine the tab visibility
        /// </summary>
        public Dictionary<int, HashSet<int>> FilterValues { get; set; }

        /// <summary>
        /// settings for cascading reference fields
        /// </summary>
        public Dictionary<int, List<int>> CascadingFieldIdsByParentId { get; set; } = new Dictionary<int, List<int>>();
    }
}

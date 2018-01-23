using DealCloud.Common.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DealCloud.Common.Entities.Lists
{
    /// <summary>
    /// single item in the choice field 
    /// </summary>
    public class ChoiceFieldValue : NamedEntry
    {
        public ChoiceFieldValue()
        {
            EntryListId = (int)EntryLists.ChoiceFieldValue;
        }

        public int? ParentID { get; set; }

        public int SeqNumber { get; set; }
        public bool IsAutoPdf { get; set; }
    }

    /// <summary>
    /// Set of choice field values for the Field
    /// </summary>
    public class ChoiceFieldValues : Dictionary<int, ChoiceFieldValue>
    {

        /// <summary>
        /// Values in the SeqNumber order 
        /// </summary>
        [JsonIgnore]
        public List<ChoiceFieldValue> ValuesList { get; }

        public ChoiceFieldValues()
        {
        }

        public ChoiceFieldValues(IEnumerable<ChoiceFieldValue> src)
        {
            if (src == null) throw new ArgumentNullException(nameof(src));
            ValuesList = src.ToList();
            foreach (var r in src)
            {
                this[r.Id] = r;
            }
        }
    }
}

using DealCloud.Common.Enums;

namespace DealCloud.Common.Entities.AddInCommon
{
    public class EntryList : NamedEntry
    {
        public string PluralName { get; set; }

        public EntryListTypes EntryListType { get; set; }

        public EntryListTypes EntryListSubType { get; set; }

        public int Count { get; set; }

        public bool IsAddEnabled { get; set; }
    }
}

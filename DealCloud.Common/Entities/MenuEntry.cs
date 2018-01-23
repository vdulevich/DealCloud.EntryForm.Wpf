using System;
using DealCloud.Common.Enums;
using Newtonsoft.Json;

namespace DealCloud.Common.Entities
{
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    public class MenuEntry : NamedEntry
    {
        public EntryListTypes ListType { get; set; }
        public int? GroupId { get; set; }
        public bool IsDisabled { get; set; }
        public string TooltipText { get; set; }
    }
}
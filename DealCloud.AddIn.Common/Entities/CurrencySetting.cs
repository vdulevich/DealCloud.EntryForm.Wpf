using DealCloud.Common.Entities;

namespace DealCloud.AddIn.Common.Entities
{
    public class CurrencySetting: NamedEntry
    {
        public bool IsAvailable { get; set; }

        public bool IsDefault { get; set; }

        public bool IsRemovable { get; set; }

        public string Code { get; set; }

        public string Symbol { get; set; }
    }
}

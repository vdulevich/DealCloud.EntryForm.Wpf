using DealCloud.Common.Attributes;

namespace DealCloud.Common.Entities.AddInOutlook
{
    public class ContactDetailsResquest
    {
        public int? DcId { get; set; }

        public string Hash { get; set; }

        [Sanitize]
        public string Company { get; set; }
    }
}

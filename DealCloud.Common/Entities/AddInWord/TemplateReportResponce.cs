using DealCloud.Common.Entities.AddInCommon;

namespace DealCloud.Common.Entities.AddInWord
{
    public class TemplateReportResponce
    {
        public UserInfo LockedBy { get; set; }

        public bool IsSessionValid { get; set; }
    }
}

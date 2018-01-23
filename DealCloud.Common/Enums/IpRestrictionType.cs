using System.ComponentModel;
using DealCloud.Common.Helpers;

namespace DealCloud.Common.Enums
{
    public enum IpRestrictionType
    {
        [MemberDescription("Apply to All")]
        All = 0,
        [MemberDescription("Apply to Mobile")]
        Mobile = 1,
        [MemberDescription("Apply to Web and Add Ins")]
        WebAndAddIns = 2
    }
}

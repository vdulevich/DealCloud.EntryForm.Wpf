using System.Runtime.Serialization;
using DealCloud.Common.Helpers;

namespace DealCloud.Common.Enums
{
    /// <summary>
    ///     Well known system object types that exist in the system.
    /// </summary>
    public enum EntryLists
    {
        [MemberDescription("None")]
        None = 0,

        [MemberDescription("User")]
        User = -1,

        [MemberDescription("Client")]
        Client = -2,

        [MemberDescription("Capability")]
        Capability = -3,

        [MemberDescription("User Group")]
        UserGroup = -4,

        [MemberDescription("Entry List")]
        EntryList = -5,

        [MemberDescription("Choice Value")]
        ChoiceFieldValue = -6,
    }
}
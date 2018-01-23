using System;

namespace DealCloud.Common.Enums
{
    [Flags]
    public enum RecurrenceType
    {
        Instant = 1,

        Daily = 2,

        Weekly = 4,

        Monthly = 8
    }
}
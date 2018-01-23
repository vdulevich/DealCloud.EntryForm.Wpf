using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealCloud.Server.Common.Enums
{
    public enum DynamicDates
    {
        None = 0,
        Today = 1,

        /** Virtual filters that exist only on UI**/
        TodayMinusXDays = -1,
        TodayPlusXDays = -2,
        NoData = -3,

        CurrentQuarterEnd = 2,
        LastQuarterEnd = 3,
        CurrentYearEnd = 4,
        LastYearEnd = 5,

        //dynamic dates for extra filtering ( between )
        YearToDate = 101,
        QuarterToDate = 102,
        MonthToDate = 103,
        LastTwelveMonth = 104
    }
}

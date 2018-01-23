using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealCloud.Common.Enums
{
    /// <summary>
    /// Capabilities of the user in the system. Ids must be in sync with Capabilities table
    /// </summary>
    public enum CapabilityCategories
    {
        /// <summary>
        /// default, means nothing
        /// </summary>
        None = 0,

        DataAccess = 1,

        AddIns = 2,

        Administration = 3,

        DataProviders = 4
    }
}

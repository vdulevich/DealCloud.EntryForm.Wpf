using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealCloud.Common.Entities
{
    /// <summary>
    /// Field value which may need to be drown as link on a portal 
    /// </summary>
    public class Reference : NamedEntry
    {
        /// <summary>
        /// Url for the link
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// true if Url is a link to File
        /// </summary>
        public bool? IsFile { get; set; }
    }
}

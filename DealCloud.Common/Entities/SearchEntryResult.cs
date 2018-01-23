using Newtonsoft.Json;
using System;

namespace DealCloud.Common.Entities
{
    /// <summary>
    /// results of full site search
    /// </summary>
    public class SearchEntryResult : NamedEntry
    {
        /// <summary>
        /// true if the URL is a link to File
        /// </summary>
        public bool? IsFile { get; set; }

        /// <summary>
        /// URL for entry if exist
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Quick View Report Tab for entry list if was configured
        /// </summary>
        public int? QuickViewReportTabId { get; set; }

        /// <summary>
        /// field1 matched the search if any
        /// </summary>
        public NamedEntry Field1 { get; set; }

        /// <summary>
        /// value for field1
        /// </summary>
        public object Value1 { get; set; }

        /// <summary>
        /// field2 which match the query
        /// </summary>
        public NamedEntry Field2 { get; set; }

        /// <summary>
        /// value for field2 
        /// </summary>
        public object Value2 { get; set; }

        /// <summary>
        /// total fields which had a match - need to display if it more than 2
        /// </summary>
        public int FieldMatchesCount { get; set; }

        /// <summary>
        /// Extra Fields like in lookups
        /// </summary>
        public object[] ExtraInfo { get; set; }

        /// <summary>
        /// date when this entry was created
        /// </summary>
        [JsonIgnore]
        public DateTime Created { get; set; } 
    }
}

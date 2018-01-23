using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DealCloud.Common.Entities.Settings
{
    public class GridSettings
    {
        public List<int> UserIds { get; set; }
        public int PageSize { get; set; } = int.MaxValue;
        public int PageIndex { get; set; } = 1;
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public bool IsAscending => SortOrder == "asc";
        public string Filter { get; set; } = string.Empty;
    }
}

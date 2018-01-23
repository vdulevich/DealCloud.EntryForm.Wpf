using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace DealCloud.AddIn.Common.Utils
{
    public class PageOffsetList : IListSource
    {
        private readonly int pageSize;

        private readonly int totalRecords;

        public bool ContainsListCollection { get; }

        public PageOffsetList(int total, int pageSize)
        {
            this.totalRecords = total;
            this.pageSize = pageSize;
        }

        public IList GetList()
        {
            var pageOffsets = new List<int>();
            for (int offset = 0; offset < totalRecords; offset += pageSize)
            {
                pageOffsets.Add(offset);
            }
            return pageOffsets;
        }
    }
}

using System;
using System.Collections.Generic;

namespace DealCloud.Common.Entities
{
    public class LibraryViewRequest : PagedRequest<LibraryViewSortBy>
    {
        public HashSet<int> PrimaryLists { get; set; }

        public HashSet<int> ModifiedByUserIds { get; set; }

        public HashSet<int> CreatedByUserIds { get; set; }

        public DateTime? ModifiedFromUtc { get; set; }

        public DateTime? ModifiedToUtc { get; set; }

        public DateTime? CreatedFromUtc { get; set; }

        public DateTime? CreatedToUtc { get; set; }

        public LibraryViewRequest(int pageSize = 10, int pageNumber = 1) : base(pageSize, pageNumber)
        {
        }
    }
}
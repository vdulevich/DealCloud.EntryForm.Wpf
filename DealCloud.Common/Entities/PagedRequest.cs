using System;

namespace DealCloud.Common.Entities
{
    public class PagedRequest
    {
        private int _pageNumber;

        private int _pageSize;

        public string Filter { get; set; }

        public int PageNumber
        {
            get { return _pageNumber; }
            set
            {
                if (value < 1) throw new ArgumentException($"{nameof(PageNumber)} can not be less 1.", nameof(value));

                _pageNumber = value;
            }
        }

        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                if (value < 1) throw new ArgumentException($"{nameof(PageSize)} can not be less 1.", nameof(value));

                _pageSize = value;
            }
        }

        public PagedRequest(int pageSize = 10, int pageNumber = 1)
        {
            PageNumber = pageNumber;
            _pageSize = pageSize;
        }

        public override string ToString()
        {
            return $"Page {PageNumber} with {PageSize} results. Filter: {Filter ?? "null"}";
        }
    }

    public class PagedRequest<TSort> : PagedRequest
    {
        public TSort SortBy { get; set; }

        public bool IsDescending { get; set; }

        public PagedRequest(int pageSize = 10, int pageNumber = 1) : base(pageSize, pageNumber)
        {
        }

        public override string ToString()
        {
            return $"{$"{base.ToString()} SortBy: {SortBy} "}{(IsDescending ? "desc" : "asc")}";
        }
    }
}
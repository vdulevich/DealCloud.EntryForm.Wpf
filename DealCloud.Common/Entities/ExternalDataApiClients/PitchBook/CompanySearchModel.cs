using System;
using System.Collections.Generic;
using DealCloud.Common.Entities.ExternalDataApiClients.Enums;

namespace DealCloud.Common.Entities.ExternalDataApiClients.PitchBook
{
    public class CompanySearchModel
    {
        private int _pageNumber;

        private int _pageSize;

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

        public Dictionary<PitchBookLookups, string> LookupsValues { get; set; }

        public string CompanyId { get; set; }

        public string City { get; set; }


        public string PostalCode { get; set; }

        public string Keywords { get; set; }

        public int? EnployeeCountFrom { get; set; }

        public int? EnployeeCountTo { get; set; }

        public int? TotalRaisedFrom { get; set; }

        public int? TotalRaisedTo { get; set; }

        // Amounts in millions
        public int? DealSizeFrom { get; set; }

        // Amounts in millions
        public int? DealSizeTo { get; set; }

        public DateTime? DateFoundedFrom { get; set; }

        public DateTime? DateFoundedTo { get; set; }

        public DateTime? DealDateFrom { get; set; }

        public DateTime? DealDateTo { get; set; }

        public CompanySearchModel()
        {
            PageNumber = 1;
            PageSize = 100;
        }
    }
}
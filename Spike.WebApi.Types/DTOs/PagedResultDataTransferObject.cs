using System.Collections.Generic;

namespace Spike.WebApi.Types.DTOs
{
    public class PagedResultDataTransferObject<T>
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalNumberOfPages { get; set; }

        public int TotalNumberOfRecords { get; set; }

        public string FirstPageUrl { get; set; }

        public string PreviousPageUrl { get; set; }

        public string NextPageUrl { get; set; }

        public string LastPageUrl { get; set; }

        public IList<T> Results { get; set; }
    }
}

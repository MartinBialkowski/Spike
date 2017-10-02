using System.Collections.Generic;

namespace SpikeWebAPI.DTOs
{
    public class PagedResultDataTransferObject<T>
    {
        /// <summary>
        /// The page number this page represents. 
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary> 
        /// The size of this page. 
        /// </summary> 
        public int PageSize { get; set; }

        /// <summary> 
        /// The total number of pages available. 
        /// </summary> 
        public int TotalNumberOfPages { get; set; }

        /// <summary> 
        /// The total number of records available. 
        /// </summary> 
        public int TotalNumberOfRecords { get; set; }

        /// <summary> 
        /// The URL to the first page
        /// </summary> 
        public string FirstPageUrl { get; set; }

        /// <summary> 
        /// The URL to the previous page
        /// </summary> 
        public string PreviousPageUrl { get; set; }

        /// <summary> 
        /// The URL to the next page 
        /// </summary> 
        public string NextPageUrl { get; set; }

        /// <summary> 
        /// The URL to the last page
        /// </summary> 
        public string LastPageUrl { get; set; }

        /// <summary> 
        /// The records this page represents. 
        /// </summary> 
        public IList<T> Results { get; set; }
    }
}

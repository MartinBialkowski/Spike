using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreSpike5.ConstraintsModels
{
    public class PagingModel : IPagingModel
    {
        public int PageNumber { get; set; }
        public int PageLimit { get; set; }
        public int Offset
        {
            get
            {
                return (PageNumber - 1) * PageLimit;
            }
        }

        public PagingModel()
        {
            PageNumber = 1;
            PageLimit = 50;
        }
        public PagingModel(int pageNumber, int pageLimit)
        {
            PageNumber = pageNumber;
            PageLimit = pageLimit;
        }
    }
}

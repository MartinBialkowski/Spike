﻿using System.ComponentModel.DataAnnotations;

namespace EFCoreSpike5.ConstraintsModels
{
    public class Paging
    {
        public int PageNumber { get; set; }
        public int PageLimit { get; set; }
        public int Offset { get; }

        public Paging()
        {
        }

        public Paging(int pageNumber, int pageLimit)
        {
            PageNumber = pageNumber;
            PageLimit = pageLimit;
        }
    }
}

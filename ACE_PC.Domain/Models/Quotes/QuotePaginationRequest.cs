using System;
using System.Collections.Generic;
using System.Text;

namespace ACE_PC.Domain.Models.Quotes
{
    public class QuotePaginationRequest
    {
        public int PageCount { get; set; } = 10;

        public int PageNumber { get; set; } = 1;

        //public int TotalPage { get; set; }

        //public int ItemCount { get; set; }
    }
}

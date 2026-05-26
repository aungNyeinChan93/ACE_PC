using System;
using System.Collections.Generic;
using System.Text;

namespace ACE_PC.Domain.Models.Quotes
{
    public class QuotesPaginationResult
    {
        public int PageNumber { get; set; }

        public int PageCount { get; set; }

        public int ItemCount { get; set; }

        public int TotalPage { get; set; }

    }
}

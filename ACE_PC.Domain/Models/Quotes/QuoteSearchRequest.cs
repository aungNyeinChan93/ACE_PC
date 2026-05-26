using System;
using System.Collections.Generic;
using System.Text;

namespace ACE_PC.Domain.Models.Quotes
{
    public class QuoteSearchRequest
    {
        public string QuoteTitle { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        public string AuthorName { get; set; } =string.Empty;

        public int AuthorId { get; set; }


    }
}

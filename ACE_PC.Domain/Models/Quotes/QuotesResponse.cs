using ACE_PC.Domain.Dtos.Quotes;
using ACE_PC.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACE_PC.Domain.Models.Quotes
{
    public class QuotesResponse
    {
        public List<QuotesDto>? Quotes { get; set; }

        public QuotesPaginationResult? PaginationResult { get; set; }
    }
}

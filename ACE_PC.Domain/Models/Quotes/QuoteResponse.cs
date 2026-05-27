using ACE_PC.Domain.Dtos.Quotes;
using ACE_PC.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ACE_PC.Domain.Models.Quotes
{
    public class QuoteResponse
    {
        public QuotesDto QuoteDto { get; set; } = new();
    }
}

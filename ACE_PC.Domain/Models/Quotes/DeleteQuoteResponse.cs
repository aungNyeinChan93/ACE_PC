using ACE_PC.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACE_PC.Domain.Models.Quotes
{
    public class DeleteQuoteResponse
    {
        public Quote Quote{ get; set; } = new Quote();
    }
}

using ACE_PC.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ACE_PC.Domain.Models.Quotes
{
    public class CreateQuoteRequest
    {

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public int UserId { get; set; }

        public int CategoryId { get; set; }


    }
}

using ACE_PC.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ACE_PC.Domain.Models.Quotes
{
    public class UpdateQuoteRequest
    {

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        //public User? User { get; set; }

        //public Category? Category { get; set; }

        //public List<Comment>? Comments { get; set; }
    }
}

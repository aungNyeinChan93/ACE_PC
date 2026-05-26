using System;
using System.Collections.Generic;
using System.Text;

namespace ACE_PC.Domain.Entity
{
    public class Like
    {

        public int QuoteId { get; set; }

        public Quote? Quote { get; set; }

        public int UserId { get; set; }

        public User? User { get; set; }
    }
}

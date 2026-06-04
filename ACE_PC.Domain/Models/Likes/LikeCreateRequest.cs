using System;
using System.Collections.Generic;
using System.Text;

namespace ACE_PC.Domain.Models.Likes
{
    public class LikeCreateRequest
    {
        public int UserId { get; set; }
        public int QuoteId { get; set; }
    }
}

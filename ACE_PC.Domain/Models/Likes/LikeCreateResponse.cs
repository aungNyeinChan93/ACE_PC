using ACE_PC.Domain.Dtos.Quotes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACE_PC.Domain.Models.Likes
{
    public class LikeCreateResponse
    {
        public LikeDto LikeDto { get; set; }

        public bool IsLike { get; set; } = false;
    }
}

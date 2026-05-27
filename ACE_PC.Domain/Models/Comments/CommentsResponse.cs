using ACE_PC.Domain.Dtos.Quotes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACE_PC.Domain.Models.Comments
{
    public class CommentsResponse
    {
        public List<CommentDto> Comments { get; set; }
    }
}

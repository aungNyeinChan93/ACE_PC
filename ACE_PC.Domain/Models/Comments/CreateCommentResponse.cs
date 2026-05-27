using ACE_PC.Domain.Dtos.Quotes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACE_PC.Domain.Models.Comments
{
    public class CreateCommentResponse
    {
        public CommentDto Comment { get; set; } = new();
    }
}

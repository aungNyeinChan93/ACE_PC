using System;
using System.Collections.Generic;
using System.Text;

namespace ACE_PC.Domain.Dtos.Quotes
{
    public class QuotesDto
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public string Title { get; set; }
        public string Author { get; set; }

        public int AuthorId { get; set; }
        public string Category { get; set; }

        public int CategoryId { get; set; }
        public List<LikeDto> Likes { get; set; } = new();
        public List<CommentDto> Comments { get; set; } = new();
    }

    public class LikeDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string UserName { get; set; }
    }

}

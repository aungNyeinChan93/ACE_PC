using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ACE_PC.Domain.Entity
{
    public class Comment
    {
        public int CommentId { get; set; }

        public string Body { get; set; } = string.Empty;

        [ForeignKey(nameof(Quote))]
        public int QuoteId { get; set; }

        public Quote? Quote { get; set; }


        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public User? User { get; set; }

    }
}

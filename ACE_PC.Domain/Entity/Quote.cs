using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ACE_PC.Domain.Entity
{
    public class Quote
    {
        public int QuoteId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public User? User { get; set; }

        public List<Like>? Likes { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        public List<Comment>? Comments { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;


    }
}



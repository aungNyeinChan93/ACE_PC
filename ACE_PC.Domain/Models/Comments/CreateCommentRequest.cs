using ACE_PC.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ACE_PC.Domain.Models.Comments
{
    public class CreateCommentRequest
    {

        [Required]
        public string Body { get; set; } = string.Empty;

        [Required]
        public int QuoteId { get; set; }

        [Required]
        public int UserId { get; set; }

    }
}

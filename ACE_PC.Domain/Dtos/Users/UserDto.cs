using ACE_PC.Domain.Dtos.Quotes;
using ACE_PC.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ACE_PC.Domain.Dtos.Users
{
    public class UserDto
    {
        public int UserId { get; set; }

        public string Name { get; set; } = string.Empty;

        //public string Password { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public Role? Role { get; set; } 

        public List<Quote>? Quotes { get; set; }

        public List<CommentDto>? Comments { get; set; }

        public List<Like>? Likes { get; set; }

        public List<Quote>? LikeQuotes { get; set; }

    }
}

using ACE_PC.Domain.Dtos.Quotes;
using ACE_PC.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace ACE_PC.Domain.Dtos.Users
{
    public class UserDto
    {
        public int UserId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public Role? Role { get; set; }

        //[JsonIgnore]
        public List<QuotesDto>? Quotes { get; set; }

        public List<CommentDto>? Comments { get; set; }

        public List<LikeDto>? Likes { get; set; }

        public List<Quote>? LikeQuotes { get; set; }

    }


    public class RoleDto
    {
        public int RoleId { get; set; }


        public string Name { get; set; } = string.Empty;

    }
}

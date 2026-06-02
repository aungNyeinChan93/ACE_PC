using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ACE_PC.Domain.Models.Users
{
    public class UserEditRequest
    {
        //[Required]
        public string Name { get; set; } = string.Empty;

        //[Required]
        public string Email { get; set; } = string.Empty;

        public int RoleId { get; set; }

        public string? Password { get; set; }
    }
}

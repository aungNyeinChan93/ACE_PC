using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ACE_PC.Domain.Models.Auth
{
    public class RegisterRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } =string.Empty;

        public int RoleId { get; set; } = 1;

        [Required]
        public string Password { get; set; } =string.Empty;
    }
}

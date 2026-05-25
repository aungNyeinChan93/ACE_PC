using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ACE_PC.Domain.Models.Auth
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; } = string.Empty;


        [Required]
        public string Password { get; set; } = string.Empty;
    }
}

using ACE_PC.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ACE_PC.Domain.Models.Auth
{
    public class LoginResponse
    {
        public User? User{ get; set; }

        public string Token { get; set; } = string.Empty;
    }
}

using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ACE_PC.Domain.Models.Users
{
    public class UserSearchRequest
    {
        public string Name { get; set; }  =string.Empty;

        public string Email { get; set; } = string.Empty;

    }
}

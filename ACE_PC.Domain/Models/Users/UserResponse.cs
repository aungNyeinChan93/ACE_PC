using ACE_PC.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACE_PC.Domain.Models.Users
{
    public class UserResponse
    {
        public User User { get; set; } = new User();
    }
}

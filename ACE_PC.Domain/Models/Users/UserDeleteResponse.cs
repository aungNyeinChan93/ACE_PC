using System;
using System.Collections.Generic;
using System.Text;

namespace ACE_PC.Domain.Models.Users
{
    public class UserDeleteResponse
    {
        public bool isDeleteSuccess { get; set; }

        public int UserId { get; set; }
    }
}
